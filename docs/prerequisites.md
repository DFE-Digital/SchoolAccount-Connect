# Prerequisites

## Windows

| Software          | Install command                                  |
|-------------------|--------------------------------------------------|
| PowerShell Core   | `winget install -e --id Microsoft.PowerShell`    |
| Docker Desktop    | `winget install -e --id Docker.DockerDesktop`    |
| .NET 10 SDK       | `winget install -e --id Microsoft.DotNet.SDK.10` |

> ⚠️ The init script requires **PowerShell Core** (`pwsh`), not Windows PowerShell. If you encounter execution policy errors, run the following from an elevated PowerShell terminal:
> ```powershell
> Set-ExecutionPolicy -Scope CurrentUser -ExecutionPolicy RemoteSigned
> ```

## macOS

| Software       | Install command                                                                                       |
|----------------|-------------------------------------------------------------------------------------------------------|
| Homebrew       | `/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"`     |
| Docker Desktop | Follow the [official macOS install guide](https://docs.docker.com/desktop/setup/install/mac-install/) |
| .NET 10 SDK    | `brew install dotnet@10`                                                                              |

> Homebrew is required by the init script to install the Azure CLI if it is not already present.

## IDE

Install one of the following:

| IDE             | Download                                          |
|-----------------|---------------------------------------------------|
| JetBrains Rider | https://www.jetbrains.com/rider/download/          |
| Visual Studio Code | https://code.visualstudio.com/download         |

## Azure Access

The init script logs in to Azure to download certificates from Key Vault. You will need:

- An account with access to the **s268-schoolaccount-development** Azure subscription
- A **Personal Access Token (PAT)** for authenticating with Azure DevOps — see [Personal Access Token](personal-access-token.md)

> The Azure CLI will be installed automatically by the init script if it is not already present on your machine.

## Related Repositories

The following repositories must be checked out alongside this one. If you do not have them, run the clone script from [SchoolAccount-LocalDevTools](https://dfe-gov-uk.visualstudio.com/s268-schoolaccount/_git/SchoolAccount-LocalDevTools?path=/scripts):

- [SchoolAccount-Database](https://dfe-gov-uk.visualstudio.com/s268-schoolaccount/_git/SchoolAccount-Database)
- [SchoolAccount-Frontend](https://dfe-gov-uk.visualstudio.com/s268-schoolaccount/_git/SchoolAccount-Frontend)
- [TSI-Backend](https://dfe-gov-uk.visualstudio.com/s268-schoolaccount/_git/TSI-Backend)
- [TSI-DomainService](https://dfe-gov-uk.visualstudio.com/s268-schoolaccount/_git/TSI-DomainService)
- [TSI-Frontend](https://dfe-gov-uk.visualstudio.com/s268-schoolaccount/_git/TSI-Frontend)
