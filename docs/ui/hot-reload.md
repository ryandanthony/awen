---
sidebar_position: 6
title: Hot Reload
---

# Hot Reload

Awen watches your story assembly directory for changes and automatically reloads stories when DLLs are rebuilt.

## How It Works

When `--watch` is enabled (the default), Awen uses two mechanisms to detect changes:

1. **FileSystemWatcher** — monitors the `--dir` directory for `*.dll` file changes (Created, Changed, Renamed events)
2. **Polling fallback** — a 5-second interval poll that compares file write timestamps, catching changes that FileSystemWatcher may miss on some platforms

When a change is detected, Awen applies a **500ms debounce window** to batch rapid successive events (common during a build), then triggers a reload.

## Reload Process

On reload, Awen:

1. Saves the current UI state (selected story, filter text)
2. Restarts the application process with a `--restore` flag pointing to a state file
3. Rescans the assembly directory for updated stories
4. Restores the previous selection and filter

This ensures a clean reload without stale type references from previously loaded assemblies.

## Controlling Hot Reload

| Option | Effect |
|--------|--------|
| `--watch` (default) | Enable hot-reload |
| `--no-watch` | Disable hot-reload entirely |

```bash
# Hot-reload enabled (default)
Awen --dir ./stories

# Hot-reload disabled
Awen --dir ./stories --no-watch
```

## Workflow

A typical development workflow:

1. Launch Awen pointing at your stories output directory
2. Edit your control or story code in your IDE
3. Rebuild (e.g., `dotnet build`)
4. Awen detects the new DLLs and reloads automatically
5. Your changes appear in the preview without restarting Awen
