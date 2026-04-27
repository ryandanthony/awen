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

namespace ExampleUI.Stories.Feedback.AlertBanner.LongText;

/// <summary>
/// Story that demonstrates long-text wrapping behavior for <see cref="AlertBanner"/>.
/// </summary>
public sealed class Story : IStory<UserControl, UserControl>, INotifyPropertyChanged
{
    private string _message =
        "Deployment update: the nightly synchronization completed with warnings in the analytics pipeline, " +
        "and the operations team is reviewing the retry queue to confirm every delayed report has been " +
        "reprocessed successfully before the status page is marked green.";

    private AlertSeverity _severity = AlertSeverity.Warning;
    private bool _isDismissable;

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <inheritdoc/>
    string IStory<UserControl, UserControl>.Name => "Long Text";

    /// <inheritdoc/>
    string IStory<UserControl, UserControl>.Group => "Molecules/Feedback";

    /// <inheritdoc/>
    int IStory<UserControl, UserControl>.Order => 1;

    /// <inheritdoc/>
    string IStory<UserControl, UserControl>.Description =>
        "Long-message alert banner used to validate text wrapping in narrow preview widths.";

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
