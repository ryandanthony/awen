// -----------------------------------------------------------------------
// <copyright file="DisabledButtonStory.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Awen.Sdk;

namespace Awen.Tests.TestFixtures;

/// <summary>
/// A disabled button story for testing.
/// </summary>
public sealed class DisabledButtonStory : IStory<UserControl, UserControl>
{
    /// <inheritdoc/>
    public string Name => "Disabled";

    /// <inheritdoc/>
    public string Group => "Atoms/Buttons";

    /// <inheritdoc/>
    public int Order => 2;

    /// <inheritdoc/>
    public string Description => "Disabled state button.";

    /// <inheritdoc/>
    public UserControl CreateControl() =>
        new() { Content = new Button { Content = "Disabled", IsEnabled = false }, DataContext = this };

    /// <inheritdoc/>
    public UserControl CreateProperties() =>
        new() { DataContext = this };
}
