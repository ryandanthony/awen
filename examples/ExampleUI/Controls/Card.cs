// -----------------------------------------------------------------------
// <copyright file="Card.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace ExampleUI.Controls;

/// <summary>
/// A simple card container with title, subtitle, and content area.
/// Responds to light/dark theme changes.
/// </summary>
public partial class Card : UserControl
{
    private const double DefaultWidth = 320;
    private const double ShadowBlur = 8;
    private const double ShadowOffsetY = 2;
    private const double ShadowMargin = 10;

    /// <summary>
    /// Defines the <see cref="Title"/> styled property.
    /// </summary>
    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<Card, string>(nameof(Title), "Card Title");

    /// <summary>
    /// Defines the <see cref="Subtitle"/> styled property.
    /// </summary>
    public static readonly StyledProperty<string> SubtitleProperty =
        AvaloniaProperty.Register<Card, string>(nameof(Subtitle), string.Empty);

    /// <summary>
    /// Defines the <see cref="ShowShadow"/> styled property.
    /// </summary>
    public static readonly StyledProperty<bool> ShowShadowProperty =
        AvaloniaProperty.Register<Card, bool>(nameof(ShowShadow), true);

    /// <summary>
    /// Initializes a new instance of the <see cref="Card"/> class.
    /// </summary>
    public Card()
    {
        Width = DefaultWidth;
        InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the card title text.
    /// </summary>
    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the card subtitle text.
    /// </summary>
    public string Subtitle
    {
        get => GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the card shows an elevated shadow.
    /// </summary>
    public bool ShowShadow
    {
        get => GetValue(ShowShadowProperty);
        set => SetValue(ShowShadowProperty, value);
    }

    /// <summary>
    /// Gets the content area control for adding body content.
    /// </summary>
    public ContentControl ContentArea => PART_ContentArea;

    /// <inheritdoc/>
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        ApplyTheme();
    }

    /// <inheritdoc/>
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        ArgumentNullException.ThrowIfNull(change);
        base.OnPropertyChanged(change);

        if (change.Property == SubtitleProperty)
        {
            PART_Subtitle.IsVisible = !string.IsNullOrEmpty(Subtitle);
        }
        else if (change.Property == ShowShadowProperty)
        {
            ApplyShadow();
        }
        else if (string.Equals(change.Property.Name, "ActualThemeVariant", StringComparison.Ordinal))
        {
            ApplyTheme();
        }
        else
        {
            // Title is handled via AXAML binding.
        }
    }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        PART_Subtitle.IsVisible = !string.IsNullOrEmpty(Subtitle);
        ApplyShadow();
    }

    private void ApplyShadow()
    {
        var isDark = ActualThemeVariant == Avalonia.Styling.ThemeVariant.Dark;
        var shadowAlpha = isDark ? (byte)60 : (byte)40;

        PART_Border.BoxShadow = ShowShadow
            ? new BoxShadows(new BoxShadow { Blur = ShadowBlur, OffsetY = ShadowOffsetY, Color = Color.FromArgb(shadowAlpha, 0, 0, 0) })
            : default;

        PART_Border.Margin = ShowShadow
            ? new Thickness(ShadowMargin, ShadowMargin, ShadowMargin, ShadowMargin + ShadowOffsetY)
            : default;
    }

    private void ApplyTheme()
    {
        var isDark = ActualThemeVariant == Avalonia.Styling.ThemeVariant.Dark;

        PART_Border.Background = new SolidColorBrush(isDark ? Color.Parse("#2D2D2D") : Color.Parse("#FFFFFF"));
        PART_Border.BorderBrush = new SolidColorBrush(isDark ? Color.Parse("#3C3C3C") : Color.Parse("#E0E0E0"));

        var foreground = new SolidColorBrush(isDark ? Color.Parse("#E0E0E0") : Color.Parse("#1B1B1F"));
        PART_Title.Foreground = foreground;
        PART_Subtitle.Foreground = foreground;

        ApplyShadow();
    }
}
