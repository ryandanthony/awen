---
sidebar_position: 2
title: Sidebar
---

# Sidebar

The sidebar displays all discovered stories in a hierarchical tree and provides filtering to quickly find components.

## Tree Structure

Stories are organized into a tree based on three levels:

1. **Library** — the top-level node, derived from the `[AwenStoryAssembly("Library Name")]` attribute
2. **Group segments** — intermediate nodes created from the story's `Group` property, split by `/`
3. **Story** — leaf nodes showing the story's `Name`

For example, a story with `Group = "Atoms/Buttons"` and `Name = "Default"` in a library called `"Example UI"` produces:

```
▾ Example UI
  ▾ Atoms
    ▾ Buttons
      Default
```

Multiple stories can share the same group path. They appear as siblings, sorted by the `Order` property.

## Filtering

The filter text box at the top of the sidebar narrows the visible stories. Filtering matches against both the story name and group path (case-insensitive). Group and library nodes are automatically hidden when none of their children match the filter.

You can also set an initial filter from the command line:

```bash
Awen --dir ./stories --filter "Buttons"
```

## Selection

Clicking a story leaf node selects it. The selected story is passed to both the [Preview Canvas](preview-canvas) and the [Properties Panel](properties-panel), which update immediately.
