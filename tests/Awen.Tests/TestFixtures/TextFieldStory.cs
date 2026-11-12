// -----------------------------------------------------------------------
// <copyright file="TextFieldStory.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Awen.Sdk;

namespace Awen.Tests.TestFixtures;

/// <summary>
/// A text input story for testing.
/// </summary>
public sealed class TextFieldStory : IStory<UserControl, UserControl>
{
    /// <inheritdoc/>
    public string Name => "Text Field";

    /// <inheritdoc/>
    public string Group => "Atoms/Inputs";

    /// <inheritdoc/>
    public int Order => 1;

    /// <inheritdoc/>
    public string Description => "Standard text input field.";

    /// <inheritdoc/>
    public UserControl CreateControl() =>
#if AVALONIA11
        new() { Content = new TextBox { Watermark = "Enter text..." }, DataContext = this };
#else
        new() { Content = new TextBox { PlaceholderText = "Enter text..." }, DataContext = this };
#endif

    /// <inheritdoc/>
    public UserControl CreateProperties() =>
        new() { DataContext = this };
}
