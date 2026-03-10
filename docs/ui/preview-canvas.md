---
sidebar_position: 3
title: Preview Canvas
---

# Preview Canvas

The preview canvas is the central panel where the selected story's control is rendered live. It displays the result of calling `CreateControl()` on the story instance.

## How It Works

When you select a story in the sidebar:

1. Awen calls `CreateControl()` on the story instance to get a fresh `UserControl`
2. The story instance is assigned as the control's `DataContext`
3. The control is rendered inside the preview canvas

If `CreateControl()` throws an exception, an error message is displayed instead of the control.

## Viewport Presets

The toolbar provides viewport presets to simulate different screen sizes:

| Preset | Width | Height |
|--------|-------|--------|
| **Responsive** | Fills available space | Fills available space |
| **Phone** | 375 px | 812 px |
| **Tablet** | 768 px | 1024 px |
| **Laptop** | 1366 px | 768 px |
| **Desktop** | 1920 px | 1080 px |
| **Widescreen** | 2560 px | 1440 px |

Select a preset from the dropdown in the toolbar. The current viewport dimensions are displayed next to the preset selector.

In **Responsive** mode, the preview canvas fills all available space and the displayed dimensions reflect the actual rendered size.

## Theme Switching

The toolbar includes a theme toggle to switch between **Light** and **Dark** themes. Toggling the theme:

- Updates the preview canvas theme scope so your control renders under the new theme
- Re-renders the current story to reflect theme changes immediately

You can also set the initial theme from the command line:

```bash
Awen --dir ./stories --theme dark
```

If your story assembly registers custom theme resources via `LightThemeResourcePath` / `DarkThemeResourcePath` on the `[AwenStoryAssembly]` attribute, those resources are merged into the application when the corresponding theme is active. See [Assembly Registration](../sdk/assembly-registration) for details.

## Story Description

Below the preview canvas, the story's `Description` property is displayed as informational text explaining what the story demonstrates.
