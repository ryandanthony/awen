---
sidebar_position: 1
title: CLI Usage
---

# CLI Usage

Awen is launched from the command line and pointed at a directory containing compiled story assemblies.

## Synopsis

```
Awen --dir <path> [--watch] [--no-watch] [--theme <light|dark>] [--filter <pattern>]
```

## Options

| Option | Alias | Description | Default |
|--------|-------|-------------|---------|
| `--dir <path>` | `-d` | **Required.** Directory to scan for story assemblies. Must contain `.dll` files marked with `[AwenStoryAssembly]`. | — |
| `--watch` | `-w` | Enable hot-reload via FileSystemWatcher. Awen will detect rebuilt assemblies and refresh automatically. | `true` |
| `--no-watch` | — | Disable hot-reload. | `false` |
| `--theme <variant>` | `-t` | Initial theme variant. Accepts `light` or `dark`. | `light` |
| `--filter <pattern>` | `-f` | Filter stories by name or group path. | — |

## Examples

### Basic usage

```bash
Awen --dir ./MyLib.Stories/bin/Debug/net10.0/
```

### Dark theme, no hot-reload

```bash
Awen --dir ./MyLib.Stories/bin/Debug/net10.0/ --theme dark --no-watch
```

### Filter to a specific group

```bash
Awen --dir ./MyLib.Stories/bin/Debug/net10.0/ --filter "Atoms/Buttons"
```

## How Assembly Scanning Works

When Awen starts, it scans the `--dir` directory for `.dll` files. For each assembly, it:

1. Checks for the `[AwenStoryAssembly("Library Name")]` attribute
2. Discovers all types implementing `IStory<TControl, TStoryProperties>`
3. Builds a sidebar tree from each story's `Group` path
4. Optionally loads custom theme resources specified by `LightThemeResourcePath` / `DarkThemeResourcePath`

When `--watch` is enabled (the default), Awen uses a `FileSystemWatcher` to detect when assemblies are rebuilt. It automatically reloads stories without restarting the application.
