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

namespace ExampleUI.Stories.Testing.KnobShowcase.AllKnobs;

/// <summary>
/// Story for the <see cref="ExampleUI.Controls.KnobShowcase"/> control exercising every property type.
/// Shared state backing both the preview control and property panel via binding.
/// </summary>
public sealed class Story : IStory<UserControl, UserControl>, INotifyPropertyChanged
{
    private string _heading = "Knob Showcase";
    private string _knobDescription = "Every knob type in one control.";
    private bool _isHighlighted;
    private ShowcaseAlignment _alignment = ShowcaseAlignment.Left;
    private double _itemCount = 3;

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <inheritdoc/>
    string IStory<UserControl, UserControl>.Name => "All Knobs";

    /// <inheritdoc/>
    string IStory<UserControl, UserControl>.Group => "Testing";

    /// <inheritdoc/>
    int IStory<UserControl, UserControl>.Order => 0;

    /// <inheritdoc/>
    string IStory<UserControl, UserControl>.Description =>
        "Exercises every knob type: Text, Bool, Enum, and Numeric.";

    /// <summary>
    /// Gets or sets the showcase heading text.
    /// </summary>
    public string Heading
    {
        get => _heading;

        set
        {
            _heading = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the showcase description text.
    /// </summary>
    public string KnobDescription
    {
        get => _knobDescription;

        set
        {
            _knobDescription = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the highlight border is shown.
    /// </summary>
    public bool IsHighlighted
    {
        get => _isHighlighted;

        set
        {
            _isHighlighted = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the content alignment.
    /// </summary>
    public ShowcaseAlignment Alignment
    {
        get => _alignment;

        set
        {
            _alignment = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the item count.
    /// </summary>
    public double ItemCount
    {
        get => _itemCount;

        set
        {
            _itemCount = value;
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
