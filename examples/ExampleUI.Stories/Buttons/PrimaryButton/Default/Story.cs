// -----------------------------------------------------------------------
// <copyright file="Story.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Awen.Sdk;

namespace ExampleUI.Stories.Buttons.PrimaryButton.Default;

/// <summary>
/// Default story for the <see cref="ExampleUI.Controls.PrimaryButton"/> control.
/// Shared state backing both the preview control and property panel via binding.
/// </summary>
public sealed class Story : IStory<UserControl, UserControl>, INotifyPropertyChanged
{
    private readonly TimeProvider _timeProvider = TimeProvider.System;
    private string _label = "Click Me";
    private bool _isButtonEnabled = true;
    private double _buttonFontSize = 14;
    private string _clickLog = string.Empty;

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <inheritdoc/>
    string IStory<UserControl, UserControl>.Name => "Default";

    /// <inheritdoc/>
    string IStory<UserControl, UserControl>.Group => "Atoms/Buttons";

    /// <inheritdoc/>
    int IStory<UserControl, UserControl>.Order => 0;

    /// <inheritdoc/>
    string IStory<UserControl, UserControl>.Description =>
        "A styled primary action button with configurable label, color, and size.";

    /// <summary>
    /// Gets or sets the button label text.
    /// </summary>
    public string Label
    {
        get => _label;

        set
        {
            _label = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the button is enabled.
    /// </summary>
    public bool IsButtonEnabled
    {
        get => _isButtonEnabled;

        set
        {
            _isButtonEnabled = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the button font size.
    /// </summary>
    public double ButtonFontSize
    {
        get => _buttonFontSize;

        set
        {
            _buttonFontSize = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the click log text.
    /// </summary>
    public string ClickLog
    {
        get => _clickLog;

        set
        {
            _clickLog = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Records a click event with the current timestamp.
    /// </summary>
    public void RecordClick()
    {
        var timestamp = _timeProvider.GetUtcNow().ToString("o", System.Globalization.CultureInfo.InvariantCulture);
        ClickLog = string.IsNullOrEmpty(ClickLog)
            ? $"Clicked at {timestamp}"
            : $"{ClickLog}\nClicked at {timestamp}";
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
