// -----------------------------------------------------------------------
// <copyright file="AlertBanner.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace ExampleUI.Controls;

/// <summary>
/// An alert banner that displays a message with an icon and configurable severity.
/// </summary>
public partial class AlertBanner : UserControl
{
    /// <summary>
    /// Defines the <see cref="Message"/> styled property.
    /// </summary>
    public static readonly StyledProperty<string> MessageProperty =
        AvaloniaProperty.Register<AlertBanner, string>(nameof(Message), "Alert message");

    /// <summary>
    /// Defines the <see cref="Severity"/> styled property.
    /// </summary>
    public static readonly StyledProperty<AlertSeverity> SeverityProperty =
        AvaloniaProperty.Register<AlertBanner, AlertSeverity>(nameof(Severity), AlertSeverity.Info);

    /// <summary>
    /// Defines the <see cref="IsDismissable"/> styled property.
    /// </summary>
    public static readonly StyledProperty<bool> IsDismissableProperty =
        AvaloniaProperty.Register<AlertBanner, bool>(nameof(IsDismissable), false);

    /// <summary>
    /// Initializes a new instance of the <see cref="AlertBanner"/> class.
    /// </summary>
    public AlertBanner()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Dismisses the alert banner by hiding it.
    /// </summary>
    public void Dismiss()
    {
        IsVisible = false;
    }

    /// <summary>
    /// Gets or sets the alert message text.
    /// </summary>
    public string Message
    {
        get => GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    /// <summary>
    /// Gets or sets the severity level of the alert.
    /// </summary>
    public AlertSeverity Severity
    {
        get => GetValue(SeverityProperty);
        set => SetValue(SeverityProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the alert can be dismissed.
    /// </summary>
    public bool IsDismissable
    {
        get => GetValue(IsDismissableProperty);
        set => SetValue(IsDismissableProperty, value);
    }

    /// <inheritdoc/>
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        ArgumentNullException.ThrowIfNull(change);
        base.OnPropertyChanged(change);

        if (change.Property == SeverityProperty)
        {
            ApplySeverity();
        }
        else
        {
            // Message and IsDismissable are handled via AXAML bindings.
        }
    }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        ApplySeverity();
    }

    private void ApplySeverity()
    {
        switch (Severity)
        {
            default:
            case AlertSeverity.Info:
            {
                PART_Border.Background = new SolidColorBrush(Color.FromRgb(219, 234, 254));
                PART_Border.BorderBrush = new SolidColorBrush(Color.FromRgb(96, 165, 250));
                PART_Icon.Text = "\u2139";
                PART_Icon.Foreground = new SolidColorBrush(Color.FromRgb(37, 99, 235));
                PART_Message.Foreground = new SolidColorBrush(Color.FromRgb(30, 64, 175));
                break;
            }

            case AlertSeverity.Success:
            {
                PART_Border.Background = new SolidColorBrush(Color.FromRgb(220, 252, 231));
                PART_Border.BorderBrush = new SolidColorBrush(Color.FromRgb(74, 222, 128));
                PART_Icon.Text = "\u2714";
                PART_Icon.Foreground = new SolidColorBrush(Color.FromRgb(22, 163, 74));
                PART_Message.Foreground = new SolidColorBrush(Color.FromRgb(21, 128, 61));
                break;
            }

            case AlertSeverity.Warning:
            {
                PART_Border.Background = new SolidColorBrush(Color.FromRgb(254, 249, 195));
                PART_Border.BorderBrush = new SolidColorBrush(Color.FromRgb(250, 204, 21));
                PART_Icon.Text = "\u26A0";
                PART_Icon.Foreground = new SolidColorBrush(Color.FromRgb(202, 138, 4));
                PART_Message.Foreground = new SolidColorBrush(Color.FromRgb(133, 77, 14));
                break;
            }

            case AlertSeverity.Error:
            {
                PART_Border.Background = new SolidColorBrush(Color.FromRgb(254, 226, 226));
                PART_Border.BorderBrush = new SolidColorBrush(Color.FromRgb(248, 113, 113));
                PART_Icon.Text = "\u2716";
                PART_Icon.Foreground = new SolidColorBrush(Color.FromRgb(220, 38, 38));
                PART_Message.Foreground = new SolidColorBrush(Color.FromRgb(153, 27, 27));
                break;
            }
        }
    }
}
