---
sidebar_position: 5
title: Log Panel
---

# Log Panel

The log panel is a collapsible section at the bottom of the Awen window that displays diagnostic information.

## What Gets Logged

- **Discovery results** — how many assemblies and stories were found at startup
- **Load errors** — assemblies that failed to load, with error details
- **Hot-reload events** — when a DLL change is detected and stories are reloaded

## Behavior

- The log panel starts collapsed by default
- It **expands automatically** when an error-level event is logged
- Click the panel header to toggle it open or closed

## Log Levels

| Level | Description |
|-------|-------------|
| **Info** | Informational messages (discovery counts, reload events) |
| **Warning** | Non-fatal issues that may need attention |
| **Error** | Failures that prevent stories from loading or rendering |

Each log entry includes a UTC timestamp, category, and message.
