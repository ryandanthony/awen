// -----------------------------------------------------------------------
// <copyright file="StoryDescriptor.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Awen.Sdk;

namespace Awen.Discovery;

/// <summary>
/// Metadata about a single story discovered via reflection.
/// Wraps the <see cref="IStory{TControl, TStoryProperties}"/> instance with additional computed properties.
/// </summary>
public sealed record StoryDescriptor
{
    /// <summary>
    /// Gets the library name from the parent <see cref="StoryAssemblyInfo"/>.
    /// </summary>
    public required string LibraryName { get; init; }

    /// <summary>
    /// Gets the story display name from <see cref="IStory{TControl, TStoryProperties}.Name"/>.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the hierarchical group path from <see cref="IStory{TControl, TStoryProperties}.Group"/>.
    /// </summary>
    public required string Group { get; init; }

    /// <summary>
    /// Gets the precomputed group segments (<see cref="Group"/> split by '/').
    /// </summary>
    public required IReadOnlyList<string> GroupSegments { get; init; }

    /// <summary>
    /// Gets the sort order among sibling stories.
    /// </summary>
    public required int Order { get; init; }

    /// <summary>
    /// Gets the human-readable description from <see cref="IStory{TControl, TStoryProperties}.Description"/>.
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Gets the instantiated story object used to call <c>CreateControl()</c> and <c>CreateProperties()</c>.
    /// </summary>
    public required IStory<Control, Control> StoryInstance { get; init; }

    /// <summary>
    /// Gets the unique identity string: <c>LibraryName/Group/Name</c>.
    /// Used for selection persistence across reloads.
    /// </summary>
    public required string Identity { get; init; }
}
