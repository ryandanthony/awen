set shell := ["bash", "-eu", "-o", "pipefail", "-c"]

# Show all available recipes.
default:
  just --list

# Restore dependencies for the whole solution.
restore:
  dotnet restore

# Build all projects in Debug mode.
build:
  dotnet build --configuration Debug

# Build all projects in Release mode.
build-release:
  dotnet build --configuration Release

# Run all tests in Release mode.
test:
  dotnet test --configuration Release

# Run only SDK tests.
test-sdk:
  dotnet test tests/Awen.Sdk.Tests/Awen.Sdk.Tests.csproj --configuration Release

# Run only app tests.
test-app:
  dotnet test tests/Awen.Tests/Awen.Tests.csproj --configuration Release

# Build example stories and launch Awen against them.
app:
  dotnet build examples/ExampleUI.Stories/ExampleUI.Stories.csproj
  dotnet run --project src/Awen -- --dir examples/ExampleUI.Stories/bin/Debug/net10.0/

# Publish self-contained linux binary.
publish-linux:
  dotnet publish src/Awen/Awen.csproj \
    --configuration Release \
    --runtime linux-x64 \
    --self-contained \
    --output ./publish/linux-x64 \
    /p:PublishSingleFile=true

# Publish self-contained windows binary.
publish-win:
  dotnet publish src/Awen/Awen.csproj \
    --configuration Release \
    --runtime win-x64 \
    --self-contained \
    --output ./publish/win-x64 \
    /p:PublishSingleFile=true

# Install docs dependencies.
docs-install:
  cd website && npm ci

# Run docs site locally.
docs-dev:
  cd website && npm start

# Build docs site for production.
docs-build:
  cd website && npm run build
