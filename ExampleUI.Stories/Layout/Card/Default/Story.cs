// -----------------------------------------------------------------------
// <copyright file="Story.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Awen.Sdk;

namespace ExampleUI.Stories.Layout.Card.Default;

/// <summary>
/// Story for the <see cref="ExampleUI.Controls.Card"/> control.
/// Shared state backing both the preview control and property panel via binding.
/// </summary>
public sealed class Story : IStory<UserControl, UserControl>, INotifyPropertyChanged
{
    private string _title = "Recipe of the Day";
    private string _subtitle = "Spaghetti Carbonara";
    private bool _showShadow = true;
    private double _cardWidth = 320;

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <inheritdoc/>
    string IStory<UserControl, UserControl>.Name => "Default";

    /// <inheritdoc/>
    string IStory<UserControl, UserControl>.Group => "Molecules/Layout";

    /// <inheritdoc/>
    int IStory<UserControl, UserControl>.Order => 0;

    /// <inheritdoc/>
    string IStory<UserControl, UserControl>.Description =>
        "A card container with title, subtitle, body content, and optional shadow.";

    /// <summary>
    /// Gets or sets the card title.
    /// </summary>
    public string Title
    {
        get => _title;

        set
        {
            _title = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the card subtitle.
    /// </summary>
    public string Subtitle
    {
        get => _subtitle;

        set
        {
            _subtitle = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the card shows a shadow.
    /// </summary>
    public bool ShowShadow
    {
        get => _showShadow;

        set
        {
            _showShadow = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the card width.
    /// </summary>
    public double CardWidth
    {
        get => _cardWidth;

        set
        {
            _cardWidth = value;
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
