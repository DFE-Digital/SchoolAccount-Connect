$ErrorActionPreference = "Stop"

function Write-ColorOutput {
    param([string]$ForegroundColor, [string]$Message)
    Write-Host $Message -ForegroundColor $ForegroundColor
}

Write-ColorOutput Blue "================================"
Write-ColorOutput Blue "User Secrets Generator"
Write-ColorOutput Blue "================================"
Write-Output ""
Write-ColorOutput Yellow "This script will set values in .NET User Secrets"
Write-ColorOutput Yellow "User Secrets will take precedence over environment variables and the test-run-config"
Write-ColorOutput Yellow "Leave blank to skip a secret (it won't be added/updated)"
Write-Output ""

# Check dependencies
if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    Write-ColorOutput Red "Error: 'dotnet' is not installed or not in PATH"
    exit 1
}

# Get the project directory
$projectDir = Join-Path $PSScriptRoot ".."

if (-not (Test-Path $projectDir)) {
    Write-ColorOutput Red "Error: project directory not found: $projectDir"
    exit 1
}

$projectDirRelative = Resolve-Path $projectDir -Relative

Write-ColorOutput Green "Using project directory: $projectDirRelative"
Write-Output ""

# Track results
$secretsSet = 0
$secretsSkipped = 0
$setKeys = [System.Collections.Generic.List[string]]::new()
$failedKeys = [System.Collections.Generic.List[string]]::new()

$script:lastInput = ""

function Set-UserSecretIfProvided {
    param(
        [string]$SecretKey,
        [string]$PromptText,
        [bool]$IsSecret = $false,
        [bool]$IsBool = $false
    )

    if ($IsSecret) {
        Write-Host "$PromptText (leave blank to skip): " -NoNewline -ForegroundColor Yellow
        $userInput = Read-Host
    } else {
        $userInput = Read-Host "$PromptText (leave blank to skip)"
    }

    $script:lastInput = $userInput

    if ([string]::IsNullOrWhiteSpace($userInput)) {
        Write-Host "  - Skipped $SecretKey" -ForegroundColor DarkGray
        $script:secretsSkipped++
        return $false
    }

    if ($IsBool -and $userInput -notin @("true", "false")) {
        Write-ColorOutput Red "  ✗ Invalid value for ${SecretKey}: expected 'true' or 'false', got '$userInput'"
        $script:failedKeys.Add($SecretKey)
        return $false
    }

    try {
        dotnet user-secrets -p $projectDirRelative set "$SecretKey" "$userInput" | Out-Null
        Write-ColorOutput Green "  ✓ Set $SecretKey"
        $script:setKeys.Add($SecretKey)
        $script:secretsSet++
        return $true
    } catch {
        Write-ColorOutput Red "  ✗ Failed to set $SecretKey"
        $script:failedKeys.Add($SecretKey)
        return $false
    }
}

# Connect Configuration
Write-Output ""
Write-ColorOutput Cyan "Connect Configuration:"
Set-UserSecretIfProvided "Connect:Email"    "Connect Email"    $false $false | Out-Null
Set-UserSecretIfProvided "Connect:Password" "Connect Password" $true  $false | Out-Null
Set-UserSecretIfProvided "Connect:Url"      "Connect URL"      $false $false | Out-Null

# Manage Configuration
Write-Output ""
Write-ColorOutput Cyan "Manage Configuration:"
Set-UserSecretIfProvided "Manage:Email"    "Manage Email"    $false $false | Out-Null
Set-UserSecretIfProvided "Manage:Password" "Manage Password" $true  $false | Out-Null
Set-UserSecretIfProvided "Manage:Url"      "Manage URL"      $false $false | Out-Null

# Screenshot Configuration
Write-Output ""
Write-ColorOutput Cyan "Screenshot Configuration:"
Set-UserSecretIfProvided "DisableScreenshots" "Disable Screenshots (true/false)" $false $true | Out-Null
if ($lastInput -eq "false") {
    Set-UserSecretIfProvided "ScreenshotOnSuccess" "Screenshot On Success (true/false)" $false $true | Out-Null
}

# Video Configuration
Write-Output ""
Write-ColorOutput Cyan "Video Configuration:"
Set-UserSecretIfProvided "Video:Enabled"       "Video Enabled (true/false)"    $false $true | Out-Null
Set-UserSecretIfProvided "Video:SaveFailedOnly" "Save Failed Only (true/false)" $false $true | Out-Null

# Logging Configuration
Write-Output ""
Write-ColorOutput Cyan "Logging Configuration:"
Set-UserSecretIfProvided "Logging:IncludeCSharp"     "Include C# Logging (true/false)"        $false $true | Out-Null
Set-UserSecretIfProvided "Logging:IncludeJavascript" "Include Javascript Logging (true/false)" $false $true | Out-Null

# Database Configuration
Write-Output ""
Write-ColorOutput Cyan "Database Configuration:"
Set-UserSecretIfProvided "Database:TenantId"     "Azure Tenant ID"         $false $false | Out-Null
Set-UserSecretIfProvided "Database:ClientId"     "Azure App Client ID"     $false $false | Out-Null
Set-UserSecretIfProvided "Database:ClientSecret" "Azure App Client Secret" $true  $false | Out-Null
Set-UserSecretIfProvided "Database:Server"       "Database Server"         $false $false | Out-Null
Set-UserSecretIfProvided "Database:DatabaseName" "Database Name"           $false $false | Out-Null

# Summary
Write-Output ""
Write-ColorOutput Yellow "================================"
Write-ColorOutput Yellow "User Secrets Updated!"
Write-ColorOutput Yellow "================================"
Write-Output ""
Write-ColorOutput Green "$secretsSet secret(s) set, $secretsSkipped skipped"

if ($setKeys.Count -gt 0) {
    Write-Output ""
    Write-ColorOutput Green "Set:"
    foreach ($k in $setKeys) {
        Write-Host "  • $k" -ForegroundColor Green
    }
}

if ($failedKeys.Count -gt 0) {
    Write-Output ""
    Write-ColorOutput Red "Failed / invalid:"
    foreach ($k in $failedKeys) {
        Write-Host "  • $k" -ForegroundColor Red
    }
}

Write-Output ""
Write-Output "To view all secrets:"
Write-ColorOutput Blue "  dotnet user-secrets -p $projectDirRelative list"
Write-Output ""
Write-Output "To remove a specific secret:"
Write-ColorOutput Blue "  dotnet user-secrets remove -p $projectDirRelative `"SecretKey`""
Write-Output ""
Write-Output "To clear all secrets:"
Write-ColorOutput Blue "  dotnet user-secrets -p $projectDirRelative clear"
Write-Output ""