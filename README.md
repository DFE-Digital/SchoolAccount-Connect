# Introduction
SchoolAccount-Connect is an ASP.NET Core web application that provides the public-facing frontend for the DfE School Account service. It handles user authentication via DfE Sign In and acts as the primary entry point for schools interacting with the platform.

# Getting Started
Follow these steps to start the web application locally.

1. Install prerequisites — see [Prerequisites](docs/prerequisites.md) for detailed instructions:
   - Docker Desktop
   - Rider or Visual Studio Code
   - .NET 10 SDK
   - Related repositories checked out alongside this one

2. Run the appropriate initialization script for your OS. This installs dotnet tools, generates SSL certificates, downloads the DfE VPN root CA, and sets up your `DEV_PAT`:

    | macOS / Linux | Windows PowerShell |
    |---------------|--------------------|
    | `./init.sh`   | `.\init.ps1`       |

    > You will be prompted for a Personal Access Token (PAT) if `DEV_PAT` is not already set in your environment. 
      The script will persist it to your shell profile for future sessions. 
      See [Personal Access Token](docs/personal-access-token.md) for instructions on generating one.

3. Starting the app in Rider:
   - Open the project in Rider
   - Use Rider's run configurations from the toolbar

     | Run Configuration                                         | Type     | Outcome                                                                                                                                           |
     |-----------------------------------------------------------|----------|---------------------------------------------------------------------------------------------------------------------------------------------------|
     | ![img.png](docs/images/connect-compose-compound.png)      | Compound | Starts the Connect app and its direct dependencies (database, migrations, app-config-emulator, seq) via Docker Compose and launches a browser     |
     | ![img.png](docs/images/connect-compose-full-compound.png) | Compound | Starts all services including TSI Frontend, TSI Backend, TSI Domain Service, and SchoolAccount Frontend via Docker Compose and launches a browser |
     | ![img.png](docs/images/connect-dotnet.png)                | Dotnet   | Runs the app directly using `dotnet run` against an `https` launch profile, no dependencies are started                                           |

4. Once running, the application is available at `https://localhost:7034/`

5. Debugging guidance:
   - Set breakpoints in your C# files under `src/SchoolAccount.Web.Connect` any of the above run configurations will 
     automatically start the app with debugging enabled.
   - Structured logs are available in Seq at `http://localhost:5341` when running via Compound run configurations.

# Build and Test
Use the .NET CLI to build or test the application.

- To build locally:
  ```bash
  dotnet build
  ```

- To run all tests:
  ```bash
  dotnet test
  ```

- To run a specific test project:
  ```bash
  dotnet test tests/SchoolAccount.Application.UnitTests
  ```
