// -----------------------------------------------------------------------
// <copyright file="ThrowingLayoutStory.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Awen.Sdk;

namespace Awen.Tests.TestFixtures;

/// <summary>
/// A story whose control throws during layout (MeasureOverride), which happens inside Avalonia's render loop.
/// </summary>
public sealed class ThrowingLayoutStory : IStory<Control, Control>
{
    /// <summary>
    /// Gets the message thrown from MeasureOverride.
    /// </summary>
    public static string MeasureMessage => "MeasureOverride failed.";

    /// <inheritdoc/>
    public string Name => "Throwing Layout";

    /// <inheritdoc/>
    public string Group => "Broken";

    /// <inheritdoc/>
    public int Order => 3;

    /// <inheritdoc/>
    public string Description => "A control that throws while being measured.";

    /// <inheritdoc/>
    public Control CreateControl() => new ThrowingMeasureControl();

    /// <inheritdoc/>
    public Control CreateProperties() => new TextBlock { Text = "props" };

    private sealed class ThrowingMeasureControl : Control
    {
        protected override Size MeasureOverride(Size availableSize) =>
            throw new InvalidOperationException(MeasureMessage);
    }
}
