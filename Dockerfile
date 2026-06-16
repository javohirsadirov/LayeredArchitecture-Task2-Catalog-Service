FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
# Pulls the .NET 9 ASP.NET runtime image (lightweight, no SDK) and names this stage "base".
# This will be used as the final runtime image.

WORKDIR /app
# Sets /app as the working directory inside the "base" stage container.

EXPOSE 8080
# Documents that the container listens on port 8080 at runtime (ASP.NET default in .NET 8+).

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
# Starts a new stage using the full .NET 9 SDK image (includes compiler, NuGet, etc.)
# Named "build" — used only for compiling, not shipped in the final image.

WORKDIR /src
# Sets /src as the working directory for the build stage.

COPY Directory.Build.props ./
# Copies the shared MSBuild properties file (analyzers, TreatWarningsAsErrors, stylecop settings).

COPY stylecop.json ./
# Copies the StyleCop configuration file (referenced by Directory.Build.props).

COPY *.sln ./
# Copies the solution file so dotnet restore can resolve project references.

COPY LayeredArchitecture-Task2-Catalog-Service/*.csproj LayeredArchitecture-Task2-Catalog-Service/
COPY LayeredArchitecture-Task2-Catalog-Service.Business/*.csproj LayeredArchitecture-Task2-Catalog-Service.Business/
COPY LayeredArchitecture-Task2-Catalog-Service.Repository/*.csproj LayeredArchitecture-Task2-Catalog-Service.Repository/
COPY LayeredArchitecture-Task2-Catalog-Service.MessageQueue/*.csproj LayeredArchitecture-Task2-Catalog-Service.MessageQueue/
COPY LayeredArchitecture-Task2-Catalog-Service.Dtos/*.csproj LayeredArchitecture-Task2-Catalog-Service.Dtos/
COPY LayeredArchitecture-Task2-Catalog-Service.Tests/*.csproj LayeredArchitecture-Task2-Catalog-Service.Tests/
# Copies only the .csproj files from each project into matching directories.
# This is done separately from source code so that "dotnet restore" can be cached
# by Docker layer caching — as long as .csproj files don't change, restore is skipped on rebuild.

RUN dotnet restore LayeredArchitecture-Task2-Catalog-Service/LayeredArchitecture-Task2-Catalog-Service.API.csproj
# Restores NuGet packages for the API project (and its transitive dependencies).

COPY . .
# Copies the entire source code into the container. This happens after restore
# so that code changes don't invalidate the cached restore layer.

RUN dotnet publish LayeredArchitecture-Task2-Catalog-Service/LayeredArchitecture-Task2-Catalog-Service.API.csproj -c Release -o /app/publish --no-restore
# Builds and publishes the API project in Release configuration to /app/publish.
# --no-restore skips restore since it was already done above.

FROM base AS final
# Starts the final stage from the "base" image (the lightweight ASP.NET runtime).
# The SDK and all build artifacts are discarded — only the published output is kept.

WORKDIR /app
# Sets /app as the working directory in the final image.

COPY --from=build /app/publish .
# Copies the published output from the "build" stage into the final image.

ENTRYPOINT ["dotnet", "LayeredArchitecture-Task2-Catalog-Service.API.dll"]
# Sets the command that runs when the container starts — launches the API.