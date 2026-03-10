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

Download a pre-built binary from [GitHub Releases](https://github.com/ryandanthony/awen/releases), or build from source:

```bash
git clone https://github.com/ryandanthony/awen.git
cd awen
dotnet build
```

### 2. Add the SDK

Install the [Awen.Sdk](https://www.nuget.org/packages/Awen.Sdk) NuGet package in your stories project:

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

## License

[MIT](LICENSE) © Ryan Anthony
