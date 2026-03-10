---
sidebar_position: 1
title: Overview
---

# User Interface

Awen provides a desktop application for browsing and interacting with your Avalonia component stories. The UI is organized into a three-panel layout with a toolbar and a collapsible log panel.

## Layout

```
┌──────────────────────────────────────────────────────────────┐
│  Toolbar            Viewport: [Responsive ▾]  Theme: [◉ Light] │
├──────────┬──────────────────────────────┬────────────────────┤
│          │                              │                    │
│ Sidebar  │     Preview Canvas           │  Properties Panel  │
│          │                              │                    │
│ ▸ Library│   ┌────────────────────┐     │  Label             │
│   ▸ Group│   │                    │     │  [Click Me      ]  │
│     Story│   │   Your Component   │     │                    │
│     Story│   │                    │     │  Enabled           │
│          │   └────────────────────┘     │  [◉ On]            │
│          │                              │                    │
│          │  Description text...         │                    │
├──────────┴──────────────────────────────┴────────────────────┤
│  ▸ Log Panel (collapsible)                                   │
└──────────────────────────────────────────────────────────────┘
```

### Panels

| Panel | Purpose |
|-------|---------|
| **Sidebar** | Hierarchical tree of all discovered stories, with search filtering |
| **Preview Canvas** | Live rendering of the selected story's control |
| **Properties Panel** | Interactive editors for the selected story's properties |
| **Toolbar** | Viewport presets and theme toggle |
| **Log Panel** | Discovery results, errors, and hot-reload events |

All panels are connected through data binding. Selecting a story in the sidebar updates the preview and properties panel simultaneously. Editing a property value in the properties panel updates the preview in real time.

## Launching the UI

```bash
Awen --dir <path-to-story-assemblies>
```

See [CLI Usage](../cli/usage) for the full list of options.
