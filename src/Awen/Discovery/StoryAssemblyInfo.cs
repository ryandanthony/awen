// -----------------------------------------------------------------------
// <copyright file="StoryAssemblyInfo.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection;

namespace Awen.Discovery;

/// <summary>
/// Metadata extracted from a loaded assembly marked with <c>[AwenStoryAssembly]</c>.
/// </summary>
public sealed record StoryAssemblyInfo
{
    /// <summary>
    /// Gets the display name from the <c>[AwenStoryAssembly]</c> attribute.
    /// </summary>
    public required string LibraryName { get; init; }

    /// <summary>
    /// Gets the loaded .NET assembly reference.
    /// </summary>
    public required Assembly Assembly { get; init; }

    /// <summary>
    /// Gets the optional embedded resource path for a light theme AXAML dictionary.
    /// </summary>
    public string? LightThemeResourcePath { get; init; }

    /// <summary>
    /// Gets the optional embedded resource path for a dark theme AXAML dictionary.
    /// </summary>
    public string? DarkThemeResourcePath { get; init; }

    /// <summary>
    /// Gets the absolute path to the DLL on disk.
    /// </summary>
    public required string FilePath { get; init; }

    /// <summary>
    /// Gets all discovered stories from this assembly.
    /// </summary>
    public required IReadOnlyList<StoryDescriptor> Stories { get; init; }
}
