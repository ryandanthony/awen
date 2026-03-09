// -----------------------------------------------------------------------
// <copyright file="IKnob.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Awen.Sdk;

/// <summary>
/// An editable property that the host renders as an editor control.
/// </summary>
public interface IKnob
{
    /// <summary>
    /// Gets the display label for the property editor.
    /// </summary>
    string Label { get; }

    /// <summary>
    /// Gets the current value. The host reads this for initial editor state.
    /// </summary>
    object? Value { get; }

    /// <summary>
    /// Sets a new value and triggers the story to update.
    /// The host calls this when the user edits a knob.
    /// May throw — the host catches and reverts.
    /// </summary>
    /// <param name="value">The new value to set.</param>
    void SetValue(object? value);
}
