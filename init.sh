#!/bin/bash

###############################################################################
# Guards
###############################################################################

script_root=$(dirname "${BASH_SOURCE[0]:-$0}")

###############################################################################
# Tools
###############################################################################

echo "Restoring dotnet tools..."
dotnet tool restore

echo "Installing Husky..."
dotnet husky install

###############################################################################
# Get OS Architecture
###############################################################################

arch=$(uname -m)
kernel=$(uname -s | tr '[:upper:]' '[:lower:]')

case "$kernel:$arch" in
    darwin:arm64)
        system="Apple Silicon"
        download_architecture="arm64"
        ;;
    darwin:x86_64)
        system="Intel Mac"
        download_architecture="amd64"
        ;;
    linux:x86_64)
        system="Linux x86_64"
        download_architecture="amd64"
        ;;
    linux:arm64 | linux:aarch64)
        system="Linux ARM64"
        download_architecture="arm64"
        ;;
    darwin:*)
        echo "Error: Unsupported architecture $arch on macOS" >&2
        return 1 2>/dev/null || exit 1
        ;;
    linux:*)
        echo "Error: Unsupported architecture $arch on Linux" >&2
        return 1 2>/dev/null || exit 1
        ;;
    *)
        echo "Error: Unsupported OS $kernel" >&2
        return 1 2>/dev/null || exit 1
        ;;
esac

# macOS uses zsh, Linux uses bash
if [ "$kernel" = "darwin" ]; then
    shell_profile="$HOME/.zshrc"
else
    shell_profile="$HOME/.bashrc"
fi

###############################################################################
# Get/Set Personal Access Token
###############################################################################

dev_pat=$DEV_PAT

# If missing, check shell profile
if [ -z "$dev_pat" ] && [ -f "$shell_profile" ]; then
    dev_pat=$(grep -oE 'DEV_PAT=[^"[:space:]]*' "$shell_profile" | sed 's/DEV_PAT=//' | head -1)
fi

# Prompt if still missing
if [ -z "$dev_pat" ]; then
    echo "Warning: Environment variable 'DEV_PAT' is not set."
    read -rp "Enter Personal Access Token: " dev_pat
    echo ""

    if [ -z "$dev_pat" ]; then
        echo "Error: No PAT provided. Exiting." >&2
        return 1 2>/dev/null || exit 1
    fi
    dev_pat=$(echo "$dev_pat" | xargs)
fi

# Persist to shell profile
if [ ! -f "$shell_profile" ]; then
    touch "$shell_profile"
    echo "Created $shell_profile"
fi

if ! grep -q "DEV_PAT=" "$shell_profile"; then
    echo "export DEV_PAT=$dev_pat" >> "$shell_profile"
    echo "Added DEV_PAT to $shell_profile."
else
    current=$(grep -oE 'DEV_PAT=[^"[:space:]]*' "$shell_profile" | sed 's/DEV_PAT=//' | head -1)
    if [ "$current" != "$dev_pat" ]; then
        if [ "$kernel" = "darwin" ]; then
            sed -i '' "s/DEV_PAT=.*/DEV_PAT=$dev_pat/" "$shell_profile"
        else
            sed -i "s/DEV_PAT=.*/DEV_PAT=$dev_pat/" "$shell_profile"
        fi
        echo "Updated DEV_PAT in $shell_profile."
    fi
fi

# Export into the current shell session
export DEV_PAT="$dev_pat"
echo "DEV_PAT set in current session."

###############################################################################
# Directory paths
###############################################################################

certs_directory="$script_root/certs"
mkcert_binary="$certs_directory/mkcert"

mkdir -p "$certs_directory"

###############################################################################
# Azure CLI
###############################################################################

if ! command -v az &>/dev/null; then
    echo "Azure CLI not found, installing..."
    if [ "$kernel" = "darwin" ]; then
        if ! command -v brew &>/dev/null; then
            echo "Error: Homebrew is not installed. Install it from https://brew.sh then re-run this script." >&2
            return 1 2>/dev/null || exit 1
        fi
        brew install azure-cli
    elif [ "$kernel" = "linux" ]; then
        sudo apt-get update && sudo apt-get install -y azure-cli
    fi
    echo "Azure CLI installed successfully."
else
    echo "Azure CLI already installed, skipping."
fi

###############################################################################
# Azure Login & Key Vault
###############################################################################

SUBSCRIPTION_ID="528c79a3-9423-4a8a-8b24-adb22c357fdb"

# Check if already logged in to the correct subscription
current_sub=$(az account show --query id -o tsv 2>/dev/null)

if [ "$current_sub" = "$SUBSCRIPTION_ID" ]; then
    echo "Already logged in to s268-schoolaccount-development, skipping login."
else
    echo "Logging in to Azure with device code..."
    az login --use-device-code --subscription $SUBSCRIPTION_ID --tenant "Educationgovuk.onmicrosoft.com"
fi

###############################################################################
# Download VPN Root CA from Key Vault
###############################################################################

vpn_root_ca="$certs_directory/dfe-vpn-root-ca.pem"

if [ ! -f "$vpn_root_ca" ]; then
    echo "Downloading DfE VPN root CA from Key Vault..."
    az keyvault secret show \
        --name dfe-vpn-root-ca \
        --vault-name s268d01kvs-sa-shared \
        --query value \
        -o tsv > "$vpn_root_ca"
    echo "VPN root CA saved to $vpn_root_ca"
else
    echo "VPN root CA already exists at $vpn_root_ca, skipping download."
fi


###############################################################################
# Download mkcert for creating SSL Certs
###############################################################################

filippo_url="https://dl.filippo.io/mkcert/latest?for="

if [ -f "$mkcert_binary" ]; then
    echo "mkcert binary already exists at $mkcert_binary, no need for download"
else
    echo "$mkcert_binary does not exist, starting download..."
    echo "System: $system, Kernel: $kernel, Arch: $download_architecture."
    url="$filippo_url$kernel/$download_architecture"
    echo "URL: $url"

    curl -s -L "$url" -o "$mkcert_binary"
    chmod +x "$mkcert_binary"
fi

###############################################################################
# Install Root Certificate Authority
###############################################################################

"$mkcert_binary" --install

###############################################################################
# Generate SSL certs
###############################################################################

schoolaccount_connect_cert="$certs_directory/connect.pem"
schoolaccount_connect_key="$certs_directory/connect-key.pem"
root_ca_cert="$certs_directory/rootCA.crt"

if [ ! -f "$schoolaccount_connect_cert" ]; then
    echo "Generating $schoolaccount_connect_cert certificate"
    "$mkcert_binary" -cert-file "$schoolaccount_connect_cert" -key-file "$schoolaccount_connect_key" localhost 127.0.0.1 schoolaccount-connect
else
    echo "$schoolaccount_connect_cert already exists"
fi

###############################################################################
# Copy root CA cert to certs directory
###############################################################################

if [ ! -f "$root_ca_cert" ]; then
    ca_root_location=$("$mkcert_binary" -CAROOT | tr -d '\n')
    ca_root_path="$ca_root_location/rootCA.pem"
    echo "Copying root CA certificate located at $ca_root_path to $root_ca_cert"
    cp "$ca_root_path" "$root_ca_cert"
else
    echo "$root_ca_cert already exists"
fi

###############################################################################
# Fix permissions for Docker (mkcert creates keys with 600; Docker needs 644)
###############################################################################

echo "Adjusting certificate permissions for Docker compatibility..."
[ -f "$schoolaccount_connect_cert" ] && chmod 644 "$schoolaccount_connect_cert"

###############################################################################
# Script finished
###############################################################################

echo ""
echo "Initialization script completed successfully."
echo ""

