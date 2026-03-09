// -----------------------------------------------------------------------
// <copyright file="Badge.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace ExampleUI.Controls;

/// <summary>
/// A small status/count badge, typically overlaid on another element.
/// </summary>
public partial class Badge : UserControl
{
    /// <summary>
    /// Defines the <see cref="Text"/> styled property.
    /// </summary>
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<Badge, string>(nameof(Text), "0");

    /// <summary>
    /// Defines the <see cref="Variant"/> styled property.
    /// </summary>
    public static readonly StyledProperty<BadgeVariant> VariantProperty =
        AvaloniaProperty.Register<Badge, BadgeVariant>(nameof(Variant), BadgeVariant.Primary);

    /// <summary>
    /// Initializes a new instance of the <see cref="Badge"/> class.
    /// </summary>
    public Badge()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the badge text content.
    /// </summary>
    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// Gets or sets the visual variant (color scheme).
    /// </summary>
    public BadgeVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    /// <inheritdoc/>
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        ArgumentNullException.ThrowIfNull(change);
        base.OnPropertyChanged(change);

        if (change.Property == VariantProperty)
        {
            ApplyVariant();
        }
        else
        {
            // Text is handled via AXAML binding.
        }
    }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        ApplyVariant();
    }

    private void ApplyVariant()
    {
        (PART_Border.Background, PART_Text.Foreground) = Variant switch
        {
            BadgeVariant.Primary => (Brushes.DodgerBlue, Brushes.White),
            BadgeVariant.Success => (Brushes.ForestGreen, Brushes.White),
            BadgeVariant.Warning => (Brushes.Orange, Brushes.Black),
            BadgeVariant.Danger => (Brushes.Crimson, Brushes.White),
            _ => (Brushes.Gray, Brushes.White),
        };
    }
}
