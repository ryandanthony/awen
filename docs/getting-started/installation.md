---
sidebar_position: 1
title: Installation
---

# Installation

Awen is distributed as a .NET tool and as standalone binaries. You can also build it from source.

## Choose the Right Awen for Your Avalonia Version

Awen loads your story assemblies into its own process, so they share Awen's copy of Avalonia. Your stories must use the **same Avalonia major version** as Awen:

| Your stories use | Use | .NET tool version | Release binaries |
|------------------|-----|-------------------|------------------|
| Avalonia 11 | Awen 1.x | `1.*` | `awen-<platform>` |
| Avalonia 12 | Awen 2.x | `2.*` | `awen-avalonia12-<platform>` |

The two lines are released together from the same source, with the same features. Only the major version differs.

If the versions don't match, Awen won't load the stories. It shows an error like this in the log panel and on the terminal:

```
ExampleUI.Stories.dll was built against Avalonia 12.1.6, but this Awen runs Avalonia 11.3.23.
Use Awen 2.x for Avalonia 12 stories, e.g. dotnet tool update awen --version "2.*"
```

`awen --help` also shows which Avalonia version the installed build runs.

## Install as a .NET Tool

Pin the major version, so `dotnet tool update` stays on the line that matches your Avalonia:

```bash
# Local tool (recommended, version tracked in .config/dotnet-tools.json)
dotnet new tool-manifest                   # once per repository
dotnet tool install awen --version "2.*"   # Avalonia 12
dotnet tool install awen --version "1.*"   # Avalonia 11 (instead of the line above)

# Global tool
dotnet tool install -g awen --version "2.*"
```

## Download Pre-Built Binaries

Get the latest release for your platform from [GitHub Releases](https://github.com/ryandanthony/awen/releases). Each version has two releases, `v1.N.P (Avalonia 11)` and `v2.N.P (Avalonia 12)`:

| Platform | Avalonia 11 (Awen 1.x) | Avalonia 12 (Awen 2.x) |
|----------|------------------------|------------------------|
| Linux x64 | `awen-linux-x64` | `awen-avalonia12-linux-x64` |
| Windows x64 | `awen-win-x64.exe` | `awen-avalonia12-win-x64.exe` |
| macOS x64 | `awen-osx-x64` | `awen-avalonia12-osx-x64` |
| macOS arm64 | `awen-osx-arm64` | `awen-avalonia12-osx-arm64` |

## Build From Source

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download) (or the version specified in `global.json`)

### Clone and Build

```bash
git clone https://github.com/ryandanthony/awen.git
cd awen
dotnet build                       # Avalonia 11 -> src/Awen/bin/Debug/net10.0/Awen
dotnet build -p:AvaloniaMajor=12   # Avalonia 12 -> src/Awen/bin/avalonia12/Debug/net10.0/Awen
```

Both flavors can be built side by side because the Avalonia 12 build uses separate `bin/avalonia12` and `obj/avalonia12` folders.

## SDK Package

To create stories for your Avalonia controls, add a reference to the **Awen.Sdk** NuGet package in your stories project. There is a single SDK package for both Avalonia versions. It depends on Avalonia `>= 11.3.23` and `< 13`, so your project's own Avalonia reference decides which version is used.

```xml
<PackageReference Include="Awen.Sdk" />
```

Or reference the project directly if building from source:

```xml
<ProjectReference Include="path/to/Awen.Sdk/Awen.Sdk.csproj" />
```
