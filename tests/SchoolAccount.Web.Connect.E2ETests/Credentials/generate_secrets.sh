#!/bin/bash
set -euo pipefail

# color constants
COLOR_BLUE='\033[0;34m'
COLOR_GREEN='\033[0;32m'
COLOR_YELLOW='\033[1;33m'
COLOR_CYAN='\033[0;36m'
COLOR_GRAY='\033[0;90m'
COLOR_RED='\033[0;31m'
COLOR_RESET='\033[0m'

print_color() {
  local color=$1
  shift
  printf "%b%s%b\n" "${color}" "$*" "${COLOR_RESET}"
}

print_color "$COLOR_BLUE" "================================"
print_color "$COLOR_BLUE" "User Secrets Generator"
print_color "$COLOR_BLUE" "================================"
echo
print_color "$COLOR_YELLOW" "This script will set values in .NET User Secrets"
print_color "$COLOR_YELLOW" "User Secrets will take precedence over environment variables and the test-run-config"
print_color "$COLOR_YELLOW" "Leave blank to skip a secret (it won't be added/updated)"
echo

# check dependencies
if ! command -v dotnet &>/dev/null; then
  print_color "$COLOR_RED" "Error: 'dotnet' is not installed or not in PATH"
  exit 1
fi

# get the project directory
script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
project_dir_rel="${script_dir}/.."

if [[ ! -d "$project_dir_rel" ]]; then
  print_color "$COLOR_RED" "Error: project directory not found: ${project_dir_rel}"
  exit 1
fi

project_dir="$(cd "$project_dir_rel" && pwd)"

print_color "$COLOR_GREEN" "Using project directory: ${project_dir}"
echo

# track results
secrets_set=0
secrets_skipped=0
last_input=""
declare -a set_keys=()
declare -a failed_keys=()

# prompts for input and optionally sets the secret
set_user_secret_if_provided() {
  local secret_key=$1
  local prompt_text=$2
  local is_secret=${3:-false}
  local is_bool=${4:-false}

  local user_input
  if [[ "$is_secret" == "true" ]]; then
    printf "%b%s (leave blank to skip): %b" "${COLOR_YELLOW}" "${prompt_text}" "${COLOR_RESET}"
    read -rs user_input
    echo
  else
    read -rp "${prompt_text} (leave blank to skip): " user_input
  fi

  last_input="$user_input"

  # skip if blank
  if [[ -z "$user_input" ]]; then
    printf "%b  - Skipped %s%b\n" "${COLOR_GRAY}" "${secret_key}" "${COLOR_RESET}"
    secrets_skipped=$((secrets_skipped + 1))
    return 1
  fi

  # validate boolean values
  if [[ "$is_bool" == "true" ]]; then
    if [[ "$user_input" != "true" && "$user_input" != "false" ]]; then
      print_color "$COLOR_RED" "  ✗ Invalid value for ${secret_key}: expected 'true' or 'false', got '${user_input}'"
      failed_keys+=("${secret_key}")
      return 1
    fi
  fi

  if dotnet user-secrets -p "${project_dir}" set "${secret_key}" "${user_input}" >/dev/null 2>&1; then
    print_color "$COLOR_GREEN" "  ✓ Set ${secret_key}"
    set_keys+=("${secret_key}")
    secrets_set=$((secrets_set + 1))
    return 0
  else
    print_color "$COLOR_RED" "  ✗ Failed to set ${secret_key}"
    failed_keys+=("${secret_key}")
    return 1
  fi
}

# connect configuration
echo
print_color "$COLOR_CYAN" "Connect Configuration:"
set_user_secret_if_provided "Connect:Email"    "Connect Email"    false false || true
set_user_secret_if_provided "Connect:Password" "Connect Password" true  false || true
set_user_secret_if_provided "Connect:Url"      "Connect URL"      false false || true

# manage configuration
echo
print_color "$COLOR_CYAN" "Manage Configuration:"
set_user_secret_if_provided "Manage:Email"    "Manage Email"    false false || true
set_user_secret_if_provided "Manage:Password" "Manage Password" true  false || true
set_user_secret_if_provided "Manage:Url"      "Manage URL"      false false || true

# screenshot configuration
echo
print_color "$COLOR_CYAN" "Screenshot Configuration:"
set_user_secret_if_provided "DisableScreenshots" "Disable Screenshots (true/false)" false true || true
if [[ "$last_input" == "false" ]]; then
  set_user_secret_if_provided "ScreenshotOnSuccess" "Screenshot On Success (true/false)" false true || true
fi

# video configuration
echo
print_color "$COLOR_CYAN" "Video Configuration:"
set_user_secret_if_provided "Video:Enabled"       "Video Enabled (true/false)"    false true || true
set_user_secret_if_provided "Video:SaveFailedOnly" "Save Failed Only (true/false)" false true || true

# logging configuration
echo
print_color "$COLOR_CYAN" "Logging Configuration:"
set_user_secret_if_provided "Logging:IncludeCSharp"      "Include C# Logging (true/false)"         false true || true
set_user_secret_if_provided "Logging:IncludeJavascript"  "Include Javascript Logging (true/false)"  false true || true

# database configuration
echo
print_color "$COLOR_CYAN" "Database Configuration:"
set_user_secret_if_provided "Database:TenantId"     "Azure Tenant ID"            false false || true
set_user_secret_if_provided "Database:ClientId"     "Azure App Client ID"        false false || true
set_user_secret_if_provided "Database:ClientSecret" "Azure App Client Secret"    true  false || true
set_user_secret_if_provided "Database:Server"       "Database Server"            false false || true
set_user_secret_if_provided "Database:DatabaseName" "Database Name"              false false || true

# summary
echo
print_color "$COLOR_YELLOW" "================================"
print_color "$COLOR_YELLOW" "Done!"
print_color "$COLOR_YELLOW" "================================"
echo
print_color "$COLOR_GREEN" "${secrets_set} secret(s) set, ${secrets_skipped} skipped"

if [[ "${#set_keys[@]}" -gt 0 ]]; then
  echo
  print_color "$COLOR_GREEN" "Set:"
  for k in "${set_keys[@]}"; do
    printf "%b  • %s%b\n" "${COLOR_GREEN}" "$k" "${COLOR_RESET}"
  done
fi

if [[ "${#failed_keys[@]}" -gt 0 ]]; then
  echo
  print_color "$COLOR_RED" "Failed / invalid:"
  for k in "${failed_keys[@]}"; do
    printf "%b  • %s%b\n" "${COLOR_RED}" "$k" "${COLOR_RESET}"
  done
fi

echo
echo "To view all secrets:"
print_color "$COLOR_BLUE" "  dotnet user-secrets -p \"${project_dir}\" list"
echo
echo "To remove a specific secret:"
print_color "$COLOR_BLUE" "  dotnet user-secrets remove -p \"${project_dir}\" \"SecretKey\""
echo
echo "To clear all secrets:"
print_color "$COLOR_BLUE" "  dotnet user-secrets -p \"${project_dir}\" clear"
echo