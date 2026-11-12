# Awen

[![Build](https://github.com/ryandanthony/awen/actions/workflows/build.yml/badge.svg)](https://github.com/ryandanthony/awen/actions/workflows/build.yml)
[![NuGet](https://img.shields.io/nuget/v/Awen.Sdk)](https://www.nuget.org/packages/Awen.Sdk)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

**Storybook-like component explorer for [Avalonia UI](https://avaloniaui.net/).** Develop, preview, and interact with your Avalonia controls in isolation.



## Features

- **Live preview** — render controls in an interactive canvas with viewport presets and theme switching
- **Hot reload** — automatically reload stories when assemblies are rebuilt
- **Sidebar navigation** — browse stories by category with search filtering
- **Properties panel** — interactively edit story state at runtime
- **Log panel** — diagnostic output and error reporting
- **Cross-platform** — runs on Linux and Windows

## Quick Start

### 1. Install Awen

Pick the Awen major version that matches the Avalonia version of your stories:

| Your stories use | Install | Release binaries |
|---|---|---|
| Avalonia 11 | Awen **1.x** | `awen-<platform>` |
| Avalonia 12 | Awen **2.x** | `awen-avalonia12-<platform>` |

As a .NET tool (pin the major in your tool manifest so updates stay on the right line):

```bash
dotnet new tool-manifest            # once per repo
dotnet tool install awen --version "2.*"   # Avalonia 12
# dotnet tool install awen --version "1.*" # Avalonia 11
```

Or download a pre-built binary from [GitHub Releases](https://github.com/ryandanthony/awen/releases), or build from source:

```bash
git clone https://github.com/ryandanthony/awen.git
cd awen
dotnet build                        # Awen 1.x, Avalonia 11
dotnet build -p:AvaloniaMajor=12    # Awen 2.x, Avalonia 12
```

If the versions don't match, Awen refuses to load the stories and tells you which Awen to install.

### 2. Add the SDK

Install the [Awen.Sdk](https://www.nuget.org/packages/Awen.Sdk) NuGet package in your stories project. The same SDK package works with Avalonia 11 and Avalonia 12:

```bash
dotnet add package Awen.Sdk
```

### 3. Register Your Assembly

Create an `AssemblyInfo.cs` in your stories project:

```csharp
using Awen.Sdk;

[assembly: AwenStoryAssembly("My Controls")]
```

### 4. Write a Story

Implement the `IStory<TControl, TStoryProperties>` interface to define a previewable component scenario:

```csharp
public class PrimaryButtonStory : IStory<PrimaryButtonPreview, PrimaryButtonProperties>
{
    public string Name => "Primary Button";
    public string Group => "Buttons";
    // ...
}
```

### 5. Run Awen

```bash
Awen --dir ./MyControls.Stories/bin/Debug/net10.0/
```

## CLI Usage

```
Awen --dir <path> [--watch] [--no-watch] [--theme <light|dark>] [--filter <pattern>]
```

| Option | Description |
|--------|-------------|
| `--dir <path>` | **Required.** Directory containing compiled story assemblies. |
| `--watch` | Enable hot-reload (default: `true`). |
| `--no-watch` | Disable hot-reload. |
| `--theme <variant>` | Initial theme — `light` or `dark`. |
| `--filter <pattern>` | Filter stories by name or group path. |

## Documentation

Full documentation is available at the [Awen docs site](https://ryandanthony.github.io/awen/).

## Project Structure

```
src/
  Awen/          # CLI application and UI
  Awen.Sdk/      # Lightweight SDK for authoring stories
examples/
  ExampleUI/           # Sample Avalonia controls
  ExampleUI.Stories/   # Sample stories
tests/
  Awen.Tests/          # Application tests
  Awen.Sdk.Tests/      # SDK tests
docs/                  # Documentation source
website/               # Docusaurus site
```

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

## Avalonia 11 and 12

One codebase builds two hosts. Story assemblies share the host's Avalonia, so they must use the same major version:

- `Awen.Sdk` is compiled against Avalonia 11.3 and depends on Avalonia `[11.3.23, 13.0.0)`, so it works with both.
- The `Awen` host is built per flavor: `-p:AvaloniaMajor=11` (default) gives Awen 1.x, `-p:AvaloniaMajor=12` gives Awen 2.x. The Avalonia 12 build goes to `bin/avalonia12/` and `obj/avalonia12/`.
- Version-specific code uses `#if AVALONIA11` / `#if AVALONIA12`.
- Versioning: GitVersion produces the shared `Minor.Patch` from `v1.*` tags. The Avalonia 12 host swaps in major `2`, so one commit ships as `1.N.P` and `2.N.P`. CI builds, tests and releases both.

## License

[MIT](LICENSE) © Ryan Anthony
