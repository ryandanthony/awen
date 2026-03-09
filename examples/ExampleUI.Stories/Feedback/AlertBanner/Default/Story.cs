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

namespace ExampleUI.Stories.Feedback.AlertBanner.Default;

/// <summary>
/// Story for the <see cref="Controls.AlertBanner"/> control.
/// Shared state backing both the preview control and property panel via binding.
/// </summary>
public sealed class Story : IStory<UserControl, UserControl>, INotifyPropertyChanged
{
    private string _message = "This is an informational message.";
    private AlertSeverity _severity = AlertSeverity.Info;
    private bool _isDismissable;

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <inheritdoc/>
    string IStory<UserControl, UserControl>.Name => "Default";

    /// <inheritdoc/>
    string IStory<UserControl, UserControl>.Group => "Molecules/Feedback";

    /// <inheritdoc/>
    int IStory<UserControl, UserControl>.Order => 0;

    /// <inheritdoc/>
    string IStory<UserControl, UserControl>.Description =>
        "An alert banner with configurable message, severity level, and dismissability.";

    /// <summary>
    /// Gets or sets the alert message text.
    /// </summary>
    public string Message
    {
        get => _message;

        set
        {
            _message = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the severity level.
    /// </summary>
    public AlertSeverity Severity
    {
        get => _severity;

        set
        {
            _severity = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the alert is dismissable.
    /// </summary>
    public bool IsDismissable
    {
        get => _isDismissable;

        set
        {
            _isDismissable = value;
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
