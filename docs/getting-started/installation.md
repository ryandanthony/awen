---
sidebar_position: 1
title: Installation
---

# Installation

Awen is distributed as a standalone CLI tool. You can download pre-built binaries or build from source.

## Download Pre-Built Binaries

Grab the latest release for your platform from [GitHub Releases](https://github.com/ryandanthony/awen/releases):

| Platform | Artifact |
|----------|----------|
| Linux x64 | `awen-linux-x64-{version}` |
| Windows x64 | `awen-win-x64-{version}` |

## Build From Source

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download) (or the version specified in `global.json`)

### Clone and Build

```bash
git clone https://github.com/ryandanthony/awen.git
cd awen
dotnet build
```

The Awen CLI binary will be at `src/Awen/bin/Debug/net10.0/Awen`.

## SDK Package

To create stories for your Avalonia controls, add a reference to the **Awen.Sdk** NuGet package in your stories project:

```xml
<PackageReference Include="Awen.Sdk" />
```

Or reference the project directly if building from source:

```xml
<ProjectReference Include="path/to/Awen.Sdk/Awen.Sdk.csproj" />
```
