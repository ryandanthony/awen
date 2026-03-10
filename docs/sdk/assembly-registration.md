---
sidebar_position: 2
title: Assembly Registration
---

# Assembly Registration

Every story assembly must be registered with the `[AwenStoryAssembly]` attribute so Awen's scanner can discover it.

## The Attribute

```csharp
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
public sealed class AwenStoryAssemblyAttribute : Attribute
{
    public AwenStoryAssemblyAttribute(string libraryName);

    public string LibraryName { get; }
    public string? LightThemeResourcePath { get; set; }
    public string? DarkThemeResourcePath { get; set; }
}
```

## Basic Registration

Create an `AssemblyInfo.cs` file in your stories project:

```csharp
using Awen.Sdk;

[assembly: AwenStoryAssembly("My Component Library")]
```

The `libraryName` parameter becomes the top-level node in Awen's sidebar tree.

## Custom Themes

If your component library has its own theme resources, you can register them so Awen applies them when previewing your controls:

```csharp
using Awen.Sdk;

[assembly: AwenStoryAssembly(
    "My Component Library",
    LightThemeResourcePath = "MyLib.Themes.LightTheme.axaml",
    DarkThemeResourcePath = "MyLib.Themes.DarkTheme.axaml")]
```

The paths should be **embedded resource** paths within the story assembly. Awen loads these as `ResourceDictionary` instances and merges them into the application resources when the theme is active.

## Discovery Process

When Awen scans the `--dir` directory, it:

1. Loads each `.dll` file
2. Checks for `[AwenStoryAssembly]` on the assembly
3. If found, scans all exported types for `IStory<,>` implementations
4. Builds the sidebar tree from each story's `Group` property, nested under the `LibraryName`
