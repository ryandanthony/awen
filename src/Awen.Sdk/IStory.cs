// -----------------------------------------------------------------------
// <copyright file="IStory.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;

namespace Awen.Sdk;

/// <summary>
/// Defines a single previewable component scenario (story).
/// The implementing class acts as the shared state (DataContext) for both
/// <typeparamref name="TControl"/> and <typeparamref name="TStoryProperties"/>.
/// </summary>
/// <typeparam name="TControl">
/// The wrapper <see cref="UserControl"/> that hosts the actual control being previewed.
/// Its DataContext should be bound to the story instance.
/// </typeparam>
/// <typeparam name="TStoryProperties">
/// The <see cref="UserControl"/> that provides property editors for tweaking the control.
/// Its DataContext should be bound to the story instance.
/// </typeparam>
public interface IStory<out TControl, out TStoryProperties>
    where TControl : Control
    where TStoryProperties : Control
{
    /// <summary>
    /// Gets the display name for this story (e.g., "Primary", "Disabled State").
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the hierarchical group path using "/" as separator (e.g., "Atoms/Buttons").
    /// Determines sidebar tree structure.
    /// </summary>
    string Group { get; }

    /// <summary>
    /// Gets the sort order among sibling stories in the same group.
    /// Lower values sort first. Default: 0.
    /// </summary>
    int Order { get; }

    /// <summary>
    /// Gets the human-readable description shown below the preview canvas.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Creates a fresh instance of the control wrapper for preview rendering.
    /// The returned control's DataContext should be set to this story instance.
    /// </summary>
    /// <returns>A new instance of the control wrapper.</returns>
    TControl CreateControl();

    /// <summary>
    /// Creates the properties panel for editing this story's state.
    /// The returned control's DataContext should be set to this story instance.
    /// </summary>
    /// <returns>A new instance of the properties editor, or an empty control if no properties are editable.</returns>
    TStoryProperties CreateProperties();
}
