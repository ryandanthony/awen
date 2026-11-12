---
sidebar_position: 4
title: Startup Hook
---

# Startup Hook

A story library can run its own setup code once when Awen starts. Use it for the app-level setup your real app does in `Program.cs` or `App.axaml`, so your controls look the same in Awen. Typical uses:

- registering embedded fonts
- adding application-wide styles or resources

The hook is optional. Libraries that don't need it don't change anything.

## Add a Hook

Implement `IAwenStartup` with a public parameterless constructor:

```csharp
using Avalonia.Media.Fonts;
using Awen.Sdk;

public sealed class MyLibStartup : IAwenStartup
{
    public void OnStartup(AwenStartupContext context)
    {
        // Fonts embedded as AvaloniaResource under Assets/Fonts
        context.Fonts.AddFontCollection(new EmbeddedFontCollection(
            new Uri("fonts:MyLib"),
            new Uri("avares://MyLib.Stories/Assets/Fonts")));
    }
}
```

Register it on the assembly attribute:

```csharp
[assembly: AwenStoryAssembly("My Library", StartupType = typeof(MyLibStartup))]
```

Then use the font in your stories' XAML:

```xml
<TextBlock FontFamily="fonts:MyLib#My Font Family" Text="Hello" />
```

## What the Context Gives You

| Member | Use |
|--------|-----|
| `LibraryName` | Your library's display name from `[AwenStoryAssembly]`. |
| `Application` | The running Avalonia `Application`. Add to `Application.Styles` or `Application.Resources`. |
| `Fonts` | The app's `FontManager`. Register font collections with `AddFontCollection`. |

Styles and resources you add to `Application` apply to the whole Awen window, not only the preview. Prefer keyed resources and styles scoped to your own controls. For theme colors, use `LightThemeResourcePath` and `DarkThemeResourcePath` (see [Assembly Registration](./assembly-registration)), which only apply inside the preview.

## When It Runs

- Once per Awen process, on the UI thread.
- After the Avalonia application is set up and all story assemblies are loaded.
- Before the main window is shown and before any story is rendered, including the story restored after a hot reload.

Hot reload restarts the Awen process, so the hook runs again with your new code.

## Errors

A startup hook can't crash Awen. If the type does not implement `IAwenStartup`, has no public parameterless constructor, or throws, Awen logs the error under **Startup** in the log panel and keeps running. Your stories still load, but whatever the hook would have set up (such as fonts) is missing.

A successful hook logs `Ran <type> for '<library>'`.

## Example

The `ExampleUI.Stories` sample registers an embedded Noto Mono font in `Startup.cs`, and the **Typography / Embedded Font** story uses it next to the default font for comparison.
