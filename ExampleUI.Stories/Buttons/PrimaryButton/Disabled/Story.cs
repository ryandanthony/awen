// -----------------------------------------------------------------------
// <copyright file="Story.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Awen.Sdk;

namespace ExampleUI.Stories.Buttons.PrimaryButton.Disabled;

/// <summary>
/// Story showing a disabled <see cref="ExampleUI.Controls.PrimaryButton"/>.
/// </summary>
public sealed class Story : IStory<UserControl, UserControl>
{
    /// <inheritdoc/>
    string IStory<UserControl, UserControl>.Name => "Disabled";

    /// <inheritdoc/>
    string IStory<UserControl, UserControl>.Group => "Atoms/Buttons";

    /// <inheritdoc/>
    int IStory<UserControl, UserControl>.Order => 1;

    /// <inheritdoc/>
    string IStory<UserControl, UserControl>.Description =>
        "A primary button in its disabled state with muted styling.";

    /// <inheritdoc/>
    UserControl IStory<UserControl, UserControl>.CreateControl() => new Control();

    /// <inheritdoc/>
    UserControl IStory<UserControl, UserControl>.CreateProperties() => new Properties();
}
