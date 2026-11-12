// -----------------------------------------------------------------------
// <copyright file="AwenStoryAssemblyAttribute.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Awen.Sdk;

/// <summary>
/// Marks an assembly as containing Awen stories. Discovered by the scanner via reflection.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
public sealed class AwenStoryAssemblyAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AwenStoryAssemblyAttribute"/> class.
    /// </summary>
    /// <param name="libraryName">Display name for this story library. Used as the top-level sidebar node.</param>
    public AwenStoryAssemblyAttribute(string libraryName)
    {
        LibraryName = libraryName;
    }

    /// <summary>
    /// Gets the display name for this story library. Used as the top-level sidebar node.
    /// </summary>
    public string LibraryName { get; }

    /// <summary>
    /// Gets or sets the optional embedded resource path to a light theme AXAML ResourceDictionary.
    /// Example: "MyLib.Themes.LightTheme.axaml".
    /// </summary>
    public string? LightThemeResourcePath { get; set; }

    /// <summary>
    /// Gets or sets the optional embedded resource path to a dark theme AXAML ResourceDictionary.
    /// Example: "MyLib.Themes.DarkTheme.axaml".
    /// </summary>
    public string? DarkThemeResourcePath { get; set; }

    /// <summary>
    /// Gets or sets an optional startup hook type that implements <see cref="IAwenStartup"/> and has a
    /// public parameterless constructor. Awen calls it once at startup, before any story is rendered,
    /// so the library can register fonts and do other app setup.
    /// Example: <c>StartupType = typeof(MyLibStartup)</c>.
    /// </summary>
    public Type? StartupType { get; set; }
}
