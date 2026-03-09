// -----------------------------------------------------------------------
// <copyright file="SearchBarStory.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Awen.Sdk;

namespace Awen.Tests.TestFixtures;

/// <summary>
/// A search bar molecule story for testing.
/// </summary>
public sealed class SearchBarStory : IStory<UserControl, UserControl>
{
    /// <inheritdoc/>
    public string Name => "Search Bar";

    /// <inheritdoc/>
    public string Group => "Molecules";

    /// <inheritdoc/>
    public int Order => 1;

    /// <inheritdoc/>
    public string Description => "Search bar with icon and input.";

    /// <inheritdoc/>
    public UserControl CreateControl() =>
        new() { Content = new StackPanel(), DataContext = this };

    /// <inheritdoc/>
    public UserControl CreateProperties() =>
        new() { DataContext = this };
}
