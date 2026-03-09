// -----------------------------------------------------------------------
// <copyright file="ThemeDemo.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace ExampleUI.Controls;

/// <summary>
/// A demo control that showcases theme-reactive styling.
/// Responds to light/dark theme changes via ActualThemeVariant.
/// </summary>
public partial class ThemeDemo : UserControl
{
    /// <summary>
    /// Defines the <see cref="Title"/> styled property.
    /// </summary>
    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<ThemeDemo, string>(nameof(Title), "Theme Demo");

    /// <summary>
    /// Defines the <see cref="Message"/> styled property.
    /// </summary>
    public static readonly StyledProperty<string> MessageProperty =
        AvaloniaProperty.Register<ThemeDemo, string>(nameof(Message), "This panel responds to theme changes.");

    /// <summary>
    /// Initializes a new instance of the <see cref="ThemeDemo"/> class.
    /// </summary>
    public ThemeDemo()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the title text.
    /// </summary>
    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the message text.
    /// </summary>
    public string Message
    {
        get => GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    /// <inheritdoc/>
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        ApplyTheme();
    }

    /// <inheritdoc/>
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change is not null &&
            string.Equals(change.Property.Name, "ActualThemeVariant", StringComparison.Ordinal))
        {
            ApplyTheme();
        }
    }

    private void ApplyTheme()
    {
        var isDark = ActualThemeVariant == Avalonia.Styling.ThemeVariant.Dark;

        PART_Border.Background = new SolidColorBrush(isDark ? Color.Parse("#1E1E1E") : Color.Parse("#FFFFFF"));
        PART_Border.BorderBrush = new SolidColorBrush(isDark ? Color.Parse("#3C3C3C") : Color.Parse("#E0E0E0"));

        var foreground = new SolidColorBrush(isDark ? Color.Parse("#E0E0E0") : Color.Parse("#1B1B1F"));
        PART_Title.Foreground = foreground;
        PART_Message.Foreground = foreground;
        PART_SurfaceLabel.Foreground = foreground;

        PART_SampleSurface.Background = new SolidColorBrush(isDark ? Color.Parse("#2D2D2D") : Color.Parse("#F5F5F5"));
        PART_SampleText.Foreground = foreground;
    }
}
