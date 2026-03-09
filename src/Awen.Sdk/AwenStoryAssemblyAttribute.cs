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
}
