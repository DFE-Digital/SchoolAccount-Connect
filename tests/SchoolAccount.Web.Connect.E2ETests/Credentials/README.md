# Test Credentials Configuration

This directory contains scripts to help you set up test credentials for the Playwright test automation framework.

## Overview

You have two options for managing test credentials:

1. **Environment Variable Files** - Create shell scripts that set environment variables
2. **User Secrets** - Store sensitive credentials securely using .NET User Secrets

## Option 1: Environment Variable Files

Generate environment-specific credential files that can be sourced/executed to load test configuration.

### Windows (PowerShell)

```powershell
# Generate credentials file
.\generate_creds_file.ps1

# You'll be prompted for an environment name (e.g., dev, test, prod)
# This creates: credentials.{env}.ps1

# Load the credentials
. .\credentials.dev.ps1

# Run tests
dotnet test
```

### macOS/Linux (Bash)

```bash
# Generate credentials file
./generate_creds_file.sh

# You'll be prompted for an environment name (e.g., dev, test, prod)
# This creates: credentials.{env}.sh

# Load the credentials
source credentials.dev.sh

# Run tests
dotnet test
```

### What Gets Created

The generator will create a file like `credentials.dev.sh` or `credentials.dev.ps1` containing:

- Connect configuration (Email, Password, URL)
- Manage configuration (Email, Password, URL)
- Screenshot settings
- Video capture settings
- Logging configuration
- Database connection details

**Note:** Files matching `credentials.*.*` are automatically ignored by Git for security.

## Option 2: User Secrets (Recommended for Sensitive Data)

For sensitive credentials like passwords and API keys, use .NET User Secrets instead of environment variables.

### Prerequisites

⚠️ **Important:** Before using user secrets, ensure all projects share the same `UserSecretsId` in their `.csproj` files:

```xml
<PropertyGroup>
  <UserSecretsId>SECRETS_ID</UserSecretsId>
</PropertyGroup>
```

The following project must have a `UserSecretsId` set:
- `SchoolAccount.Web.Connect.E2ETests/PlaywrightTests.DfE.Tests.csproj`

If you need to initialize or change the `UserSecretsId`, run from the `SchoolAccount.Web.Connect.E2ETests` directory:

```bash
dotnet user-secrets init
```

This should not be required as projects will have a user-secrets value set as standard.

### Setup

```powershell
# From SchoolAccount.Web.Connect.E2ETests (initialize user secrets if not already done)
dotnet user-secrets init

# Run the secrets generator
cd Credentials
.\generate_secrets.ps1
```

### Benefits

✅ Secrets stored outside your project directory  
✅ Per-user, per-machine configuration  
✅ Never accidentally committed to Git  
✅ Works seamlessly with the configuration system  
✅ Cross-platform (Windows, macOS, Linux)  

### User Secrets vs Environment Variables

User secrets **take precedence** over environment variables in the configuration system, allowing you to:

- Keep non-sensitive defaults in environment variable files
- Override with secure values using user secrets
- Share environment files with your team while keeping secrets private

### Managing User Secrets

```bash
# View all secrets
dotnet user-secrets list

# Set a specific secret
dotnet user-secrets set "Database:ClientSecret" "your-secret-here"

# Remove a specific secret
dotnet user-secrets remove "Database:ClientSecret"

# Clear all secrets
dotnet user-secrets clear
```

## Configuration Priority

Values are loaded in this order (last wins):

1. **JSON config file** (`test-run-config.json`) - Base configuration
2. **Environment Variables** - Override JSON values
3. **User Secrets** - Override everything (highest priority)

This allows you to:
- Keep common settings in JSON
- Use environment variables for environment-specific settings
- Use user secrets for sensitive credentials

## Key Format

### Environment Variables

Use **double underscores** (`__`) for hierarchical keys:

```bash
export Connect__Email="user@example.com"
export Database__ClientSecret="secret"
```

### User Secrets

Use **colons** (`:`) for hierarchical keys:

```bash
dotnet user-secrets set "Connect:Email" "user@example.com"
dotnet user-secrets set "Database:ClientSecret" "secret"
```

## Security Best Practices

🔒 **Never commit credentials to source control**  
🔒 Use user secrets for passwords, API keys, and connection strings  
🔒 Use environment variable files for non-sensitive configuration  
🔒 All `credentials.*.*` files are automatically git-ignored  

## Quick Start

### First Time Setup

```bash
# 1. Generate environment-specific credentials (from this directory)
./generate_creds_file.sh  # or .ps1 on Windows
# Enter environment name: dev

# 2. Ensure UserSecretsId is set in PlaywrightTests.DfE.Tests.csproj (see Prerequisites above)

# 3. Initialize and set sensitive secrets (from SchoolAccount.Web.Connect.E2ETests)
cd ..
dotnet user-secrets init
cd Credentials
./generate_secrets.sh  # or .ps1 on Windows

# 4. Load credentials and run tests (from SchoolAccount.Web.Connect.E2ETests)
source Credentials/credentials.dev.sh  # or . .\Credentials\credentials.dev.ps1 on Windows
cd ..
dotnet test
```

### Daily Development

```bash
# Just source your credentials file and run tests
source playwright/credentials/credentials.dev.sh
dotnet test
```

User secrets are automatically loaded - no need to source them each time! 🎉

## Troubleshooting

### Secrets Not Loading

1. **Verify all projects share the same `UserSecretsId`** in their `.csproj` files:
   ```bash
   grep "UserSecretsId" ../*.csproj
   ```

2. Verify secrets are set: `dotnet user-secrets list`

3. Check you're in the correct project directory when setting secrets

4. Ensure the `Microsoft.Extensions.Configuration.UserSecrets` package is referenced in the project

### Environment Variables Not Working

1. Ensure you're **sourcing** the file, not executing it:
   - ✅ `source credentials.dev.sh` or `. credentials.dev.sh`
   - ❌ `./credentials.dev.sh` (won't persist variables)

2. Unset any empty environment variables that might override user secrets:
   ```bash
   unset Database__ClientSecret
   unset Connect__Password
   ```

## Files in This Directory

- `generate_creds_file.ps1` - PowerShell script to generate environment variable files
- `generate_creds_file.sh` - Bash script to generate environment variable files
- `generate_secrets.ps1` - PowerShell script to set user secrets
- `credentials.*.sh` - Generated bash credential files (git-ignored)
- `credentials.*.ps1` - Generated PowerShell credential files (git-ignored)
- `README.md` - This file

## Need Help?

If you encounter issues or have questions about credential management, please reach out to the test automation team.