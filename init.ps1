#!/usr/bin/env pwsh
#Requires -Version 7.0

$ErrorActionPreference = 'Stop'

###############################################################################
# Guards
###############################################################################

$ScriptRoot = $PSScriptRoot

if ($PSVersionTable.PSEdition -ne 'Core')
{
    Write-Error "This script requires PowerShell Core (pwsh). You are running Windows PowerShell."
    exit 1
}

###############################################################################
# Tools
###############################################################################

Write-Host "Restoring dotnet tools..." -ForegroundColor Cyan
dotnet tool restore

Write-Host "Installing Husky..." -ForegroundColor Cyan
dotnet husky install

###############################################################################
# Get OS Architecture
###############################################################################

$Arch = [System.Runtime.InteropServices.RuntimeInformation]::OSArchitecture

if ($IsMacOS -and $Arch -eq 'Arm64')
{
    $System = 'Apple Silicon'
    $DownloadArchitecture = 'arm64'
    $Kernel = 'darwin'
}
elseif ($IsMacOS -and $Arch -eq 'X64')
{
    $System = 'Intel Mac'
    $DownloadArchitecture = 'amd64'
    $Kernel = 'darwin'
}
elseif ($IsLinux -and $Arch -eq 'X64')
{
    $System = 'Linux'
    $DownloadArchitecture = 'amd64'
    $Kernel = 'linux'
}
elseif ($IsWindows -and $Arch -eq 'X64')
{
    $System = 'Windows'
    $DownloadArchitecture = 'amd64'
    $Kernel = 'windows'
}
elseif ($IsWindows -and $Arch -eq 'Arm64')
{
    $System = 'Windows'
    $DownloadArchitecture = 'arm64'
    $Kernel = 'windows'
}
else
{
    Write-Output "$System/$Arch"
    Write-Error "OS/Architecture not supported"
    exit 0
}

###############################################################################
# Get/Set Personal Access Token
###############################################################################

$DevPat = $env:DEV_PAT

# If missing, check persistent storage
if ( [string]::IsNullOrWhiteSpace($DevPat))
{
    if ($IsWindows)
    {
        $DevPat = [Environment]::GetEnvironmentVariable("DEV_PAT", "User")
    }
    else
    {
        # Linux/macOS: Check PowerShell Profile
        if (Test-Path $PROFILE)
        {
            $ProfileContent = Get-Content $PROFILE -Raw -ErrorAction SilentlyContinue
            # Regex to find: $env:DEV_PAT = '...'
            if ($ProfileContent -match "(?m)^\s*`$env:DEV_PAT\s*=\s*['`"]([^'`"]+)['`"]")
            {
                $DevPat = $Matches[1]
            }
        }
    }
}

# Prompt if still missing
if ( [string]::IsNullOrWhiteSpace($DevPat))
{
    Write-Warning "Environment variable 'DEV_PAT' is not set."
    Write-Output "Please enter a Personal Access Token."

    $DevPat = Read-Host -Prompt "Enter Personal Access Token"

    if ( [string]::IsNullOrWhiteSpace($DevPat))
    {
        Write-Error "No PAT provided. Exiting."
        exit 1
    }
    $DevPat = $DevPat.Trim()

    # Set in current session immediately
    $env:DEV_PAT = $DevPat
}

# Add or update persistent storage
if ($IsWindows)
{
    $RegPat = [Environment]::GetEnvironmentVariable("DEV_PAT", "User")
    if ($RegPat -ne $DevPat)
    {
        [Environment]::SetEnvironmentVariable("DEV_PAT", $DevPat, "User")
        Write-Output "Updated DEV_PAT in User Registry."
    }
}
else
{
    # Linux/macOS: Ensure it is in the profile
    if (-not (Test-Path $PROFILE))
    {
        New-Item -Path $PROFILE -ItemType File -Force | Out-Null
    }

    $ProfileContent = Get-Content $PROFILE -Raw -ErrorAction SilentlyContinue

    # If the file does NOT contain the variable definition, append it
    if ($null -eq $ProfileContent -or $ProfileContent -notmatch "env:DEV_PAT")
    {
        Add-Content -Path $PROFILE -Value "`n`$env:DEV_PAT = '$DevPat'"
        Write-Warning "Added DEV_PAT to your PowerShell profile ($PROFILE)."
    }
}

###############################################################################
# Directory paths
###############################################################################

$CertsDirectory = Join-Path $ScriptRoot './certs'

if (-not (Test-Path -Path $CertsDirectory))
{
    New-Item -ItemType Directory -Path $CertsDirectory | Out-Null
}

###############################################################################
# Azure CLI
###############################################################################

if (-not (Get-Command az -ErrorAction SilentlyContinue))
{
    Write-Host "Azure CLI not found, installing..." -ForegroundColor Cyan
    if ($IsMacOS)
    {
        if (-not (Get-Command brew -ErrorAction SilentlyContinue))
        {
            Write-Error "Homebrew is not installed. Install it from https://brew.sh then re-run this script."
            exit 1
        }
        brew install azure-cli
    }
    elseif ($IsLinux)
    {
        sudo apt-get update
        sudo apt-get install -y azure-cli
    }
    elseif ($IsWindows)
    {
        winget install --id Microsoft.AzureCLI --silent --accept-source-agreements --accept-package-agreements

        # Refresh the current session PATH so az is available immediately after install.
        $env:Path = [Environment]::GetEnvironmentVariable('Path', 'Machine') + ';' + [Environment]::GetEnvironmentVariable('Path', 'User')

        if (-not (Get-Command az -ErrorAction SilentlyContinue))
        {
            throw 'Azure CLI was installed but is not available on PATH in the current session.'
        }
    }
    Write-Host "Azure CLI installed successfully." -ForegroundColor Green
}
else
{
    Write-Output "Azure CLI already installed, skipping."
}

###############################################################################
# Azure Login & Key Vault
###############################################################################

$SubscriptionId = "528c79a3-9423-4a8a-8b24-adb22c357fdb"

# Check if already logged in to the correct subscription
$CurrentSub = az account show --query id -o tsv 2>$null

if ($CurrentSub -eq $SubscriptionId)
{
    Write-Output "Already logged in to s268-schoolaccount-development, skipping login."
}
else
{
    Write-Host "Logging in to Azure with device code..." -ForegroundColor Cyan
    az login --use-device-code --subscription $SubscriptionId --tenant "Educationgovuk.onmicrosoft.com"
}

###############################################################################
# Download VPN Root CA from Key Vault
###############################################################################

$VpnRootCa = Join-Path $CertsDirectory 'dfe-vpn-root-ca.pem'

if (-not (Test-Path -Path $VpnRootCa))
{
    Write-Host "Downloading DfE VPN root CA from Key Vault..." -ForegroundColor Cyan
    az keyvault secret show `
        --name dfe-vpn-root-ca `
        --vault-name s268d01kvs-sa-shared `
        --query value `
        -o tsv | Out-File -FilePath $VpnRootCa -Encoding utf8 -NoNewline
    Write-Output "VPN root CA saved to $VpnRootCa"
}
else
{
    Write-Output "VPN root CA already exists at $VpnRootCa, skipping download."
}

###############################################################################
# Download mkcert for creating SSL Certs
###############################################################################
$FilippoUrl = "https://dl.filippo.io/mkcert/latest?for="
$MkcertBinary = Join-Path $CertsDirectory $(if ($IsWindows) { 'mkcert.exe' } else { 'mkcert' })

if (Test-Path -Path $MkcertBinary)
{
    Write-Output "mkcert binary already exists at $MkcertBinary no need for download"
}
else
{
    Write-Output "$MkcertBinary does not exist, starting download..."
    Write-Output "System: $System, Kernel: $Kernel, Arch: $DownloadArchitecture."
    Write-Output "URL: $FilippoUrl$Kernel/$DownloadArchitecture"

    $ProgressPreference = 'SilentlyContinue'
    Invoke-WebRequest -Uri "$FilippoUrl$Kernel/$DownloadArchitecture" -OutFile $MkcertBinary

    if (-not $IsWindows)
    {
        chmod +x $MkcertBinary
    }
}

###############################################################################
# Install Root Certificate Authority
###############################################################################

& $MkcertBinary --install

###############################################################################
# Generate SSL certs
###############################################################################

$SchoolAccountConnectCert = Join-Path $CertsDirectory 'connect.pem'
$SchoolAccountConnectKey = Join-Path $CertsDirectory 'connect-key.pem'

if (-not (Test-Path -Path $SchoolAccountConnectCert))
{
    Write-Output "Generating $SchoolAccountConnectCert certificate"
    & $MkcertBinary -cert-file $SchoolAccountConnectCert -key-file $SchoolAccountConnectKey localhost 127.0.0.1 schoolaccount-connect
}
else
{
    Write-Output "$SchoolAccountConnectCert already exists"
}

###############################################################################
# Copy root CA cert to certs directory
###############################################################################

$RootCaCert = Join-Path $CertsDirectory 'rootCA.crt'

if (-not (Test-Path -Path $RootCaCert))
{
    $CaRootLocation = (& $MkcertBinary -CAROOT).Trim()
    $CaRootPath = Join-Path $CaRootLocation 'rootCA.pem'
    Write-Output "Copying root CA certificate located at $CaRootPath to $RootCaCert"
    Copy-Item -Path $CaRootPath -Destination $RootCaCert -Force
}
else
{
    Write-Output "$RootCaCert already exists"
}

###############################################################################
# Fix permissions for Docker (mkcert creates keys with 600; Docker needs 644)
###############################################################################

if (-not $IsWindows)
{
    Write-Output "Adjusting certificate permissions for Docker compatibility..."
    if (Test-Path $SchoolAccountConnectCert)
    {
        chmod 644 $SchoolAccountConnectCert
    }
}

###############################################################################
# Script finished
###############################################################################
Write-Host ""
Write-Host "Initialization script completed successfully." -ForegroundColor Green
Write-Host ""