// -----------------------------------------------------------------------
// <copyright file="OwnModelStory.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Awen.Sdk;

namespace Awen.Tests.TestFixtures;

/// <summary>
/// A story that binds its control and properties to its own model instead of to the story. Several stories can
/// share one view this way, each with different data. With a null model it sets no DataContext at all.
/// </summary>
/// <param name="model">The story's model, or null to leave the DataContext unset.</param>
public sealed class OwnModelStory(object? model) : IStory<UserControl, UserControl>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OwnModelStory"/> class with no model, as discovery creates it.
    /// </summary>
    public OwnModelStory()
        : this(null)
    {
    }

    /// <summary>
    /// Gets the model the story binds its control and properties to, or null.
    /// </summary>
    public object? Model { get; } = model;

    /// <inheritdoc/>
    public string Name => "Own Model";

    /// <inheritdoc/>
    public string Group => "Screens";

    /// <inheritdoc/>
    public int Order => 1;

    /// <inheritdoc/>
    public string Description => "Binds to its own model.";

    /// <inheritdoc/>
    public UserControl CreateControl() => new() { DataContext = Model };

    /// <inheritdoc/>
    public UserControl CreateProperties() => new() { DataContext = Model };
}
