// -----------------------------------------------------------------------
// <copyright file="Story.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Awen.Sdk;
using ExampleUI.Controls;

namespace ExampleUI.Stories.Indicators.Badge.Default;

/// <summary>
/// Story for the <see cref="Controls.Badge"/> control with text and variant properties.
/// Shared state backing both the preview control and property panel via binding.
/// </summary>
public sealed class Story : IStory<UserControl, UserControl>, INotifyPropertyChanged
{
    private string _text = "New";
    private BadgeVariant _variant = BadgeVariant.Primary;

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <inheritdoc/>
    string IStory<UserControl, UserControl>.Name => "Default";

    /// <inheritdoc/>
    string IStory<UserControl, UserControl>.Group => "Atoms/Indicators";

    /// <inheritdoc/>
    int IStory<UserControl, UserControl>.Order => 0;

    /// <inheritdoc/>
    string IStory<UserControl, UserControl>.Description =>
        "A status badge with configurable text and color variant.";

    /// <summary>
    /// Gets or sets the badge text.
    /// </summary>
    public string Text
    {
        get => _text;

        set
        {
            _text = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the badge variant.
    /// </summary>
    public BadgeVariant Variant
    {
        get => _variant;

        set
        {
            _variant = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc/>
    UserControl IStory<UserControl, UserControl>.CreateControl() => new Control();

    /// <inheritdoc/>
    UserControl IStory<UserControl, UserControl>.CreateProperties() => new Properties();

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
