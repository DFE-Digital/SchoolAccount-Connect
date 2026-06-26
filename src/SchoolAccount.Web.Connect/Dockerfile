# base — PRODUCTION runtime. No local/VPN root CA here: Container Apps talks to
# public endpoints whose CAs are already trusted. Keeping this cert-free is what
# lets CI (which has no ./certs/rootCA.crt) build the image.

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

RUN mkdir -p /home/app/.aspnet/DataProtection-Keys \
    && chown -R app:app /home/app/.aspnet
USER app

# =============================================================================
# build — restore + compile (SDK image). Unchanged from yours.
# =============================================================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG VERSION=0.0.0
ARG GIT_SHA=local
WORKDIR /build
COPY .nuget/NuGet.Config ./
COPY ["src/SchoolAccount.Application/SchoolAccount.Application.csproj", "src/SchoolAccount.Application/"]
COPY ["src/SchoolAccount.Domain/SchoolAccount.Domain.csproj", "src/SchoolAccount.Domain/"]
COPY ["src/SchoolAccount.Infrastructure/SchoolAccount.Infrastructure.csproj", "src/SchoolAccount.Infrastructure/"]
COPY ["src/SchoolAccount.Kernel/SchoolAccount.Kernel.csproj", "src/SchoolAccount.Kernel/"]
COPY ["src/SchoolAccount.Web.Connect/SchoolAccount.Web.Connect.csproj", "src/SchoolAccount.Web.Connect/"]
COPY ["integration/SchoolAccount.Integration.DfESignIn/SchoolAccount.Integration.DfESignIn.csproj", "integration/SchoolAccount.Integration.DfESignIn/"]
COPY ["Directory.Build.props", "Directory.Build.props"]
COPY ["Directory.Packages.props", "Directory.Packages.props"]
COPY ["global.json", "global.json"]
COPY [".editorconfig", ".editorconfig"]

RUN curl -L https://raw.githubusercontent.com/Microsoft/artifacts-credprovider/master/helpers/installcredprovider.sh | sh

RUN --mount=type=secret,id=personal_access_token \
    PAT=$(cat /run/secrets/personal_access_token) && \
    export VSS_NUGET_EXTERNAL_FEED_ENDPOINTS="{\"endpointCredentials\": [{\"endpoint\":\"https://dfe-gov-uk.pkgs.visualstudio.com/s268-schoolaccount/_packaging/SchoolAccount/nuget/v3/index.json\", \"username\":\"docker\", \"password\":\"${PAT}\"}]}" && \
    dotnet restore src/SchoolAccount.Web.Connect/SchoolAccount.Web.Connect.csproj --configfile NuGet.Config && \
    unset VSS_NUGET_EXTERNAL_FEED_ENDPOINTS

COPY ./src/ ./src/
COPY ./integration/ ./integration/

RUN dotnet build src/SchoolAccount.Web.Connect/SchoolAccount.Web.Connect.csproj --no-restore -c $BUILD_CONFIGURATION -o /app/build --verbosity quiet

# publish — produce the production artefacts.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish src/SchoolAccount.Web.Connect/SchoolAccount.Web.Connect.csproj --no-restore -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false -p:Version=$VERSION -p:SourceRevisionId=$GIT_SHA

# --- debug stage ---
FROM build AS debug
USER root

# Trust the local/VPN self-signed root CA so the running app's outbound calls work.
RUN apt-get update && apt-get install -y ca-certificates && rm -rf /var/lib/apt/lists/*
COPY ./certs/rootCA.crt /usr/local/share/ca-certificates/rootCA.crt
RUN update-ca-certificates

RUN dotnet build src/SchoolAccount.Web.Connect/SchoolAccount.Web.Connect.csproj \
    --no-restore -c Debug -o /app/debug

# Install the latest vsdbg debugger. Skip cert verification because dev machines
# are likely on a VPN that uses a self-signed certificate, which breaks the download.
RUN echo "check_certificate = off" > /etc/wgetrc \
    && echo "insecure" > /etc/curlrc \
    && curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg \
    && rm /etc/wgetrc /etc/curlrc

RUN mkdir -p /home/app/.aspnet/DataProtection-Keys \
    && chown -R app:app /home/app/.aspnet
USER app
WORKDIR /app/debug
ENTRYPOINT ["dotnet", "SchoolAccount.Web.Connect.dll"]

# --- publish stage ---
FROM base AS final-dev
WORKDIR /app
USER root
RUN apt-get update && apt-get install -y ca-certificates && rm -rf /var/lib/apt/lists/*
COPY ./certs/rootCA.crt /usr/local/share/ca-certificates/rootCA.crt
RUN update-ca-certificates
USER app
COPY --from=publish /app/publish/ .
ENTRYPOINT ["dotnet", "SchoolAccount.Web.Connect.dll"]

# --- final stage ---
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish/ .
ENTRYPOINT ["dotnet", "SchoolAccount.Web.Connect.dll"]