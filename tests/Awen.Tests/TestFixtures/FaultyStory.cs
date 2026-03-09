// -----------------------------------------------------------------------
// <copyright file="FaultyStory.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Awen.Sdk;

namespace Awen.Tests.TestFixtures;

/// <summary>
/// A test story whose CreateControl() method throws an exception.
/// </summary>
public sealed class FaultyStory : IStory<UserControl, UserControl>
{
    /// <inheritdoc/>
    public string Name => "Faulty";

    /// <inheritdoc/>
    public string Group => "Broken";

    /// <inheritdoc/>
    public int Order => 1;

    /// <inheritdoc/>
    public string Description => "This story throws on CreateControl().";

    /// <inheritdoc/>
    public UserControl CreateControl() => throw new InvalidOperationException("Simulated failure in CreateControl().");

    /// <inheritdoc/>
    public UserControl CreateProperties() => new() { DataContext = this };
}
