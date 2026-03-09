// -----------------------------------------------------------------------
// <copyright file="KnobShowcase.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace ExampleUI.Controls;

/// <summary>
/// A demo control that exercises every knob type: Text, Bool, Enum, and Numeric.
/// Each property visually reflects its knob value so testers can verify changes.
/// </summary>
public partial class KnobShowcase : UserControl
{
    private const double ItemHeight = 24;
    private const double MaxItems = 10;
    private const double LabelFontSize = 11;
    private const string AccentColor = "#6C63FF";
    private const string MutedColor = "#999999";
    private const string DefaultBorderColor = "#E0E0E0";

    /// <summary>
    /// Defines the <see cref="Heading"/> styled property.
    /// </summary>
    public static readonly StyledProperty<string> HeadingProperty =
        AvaloniaProperty.Register<KnobShowcase, string>(nameof(Heading), "Knob Showcase");

    /// <summary>
    /// Defines the <see cref="Description"/> styled property.
    /// </summary>
    public static readonly StyledProperty<string> DescriptionProperty =
        AvaloniaProperty.Register<KnobShowcase, string>(nameof(Description), "Every knob type in one control.");

    /// <summary>
    /// Defines the <see cref="IsHighlighted"/> styled property.
    /// </summary>
    public static readonly StyledProperty<bool> IsHighlightedProperty =
        AvaloniaProperty.Register<KnobShowcase, bool>(nameof(IsHighlighted), false);

    /// <summary>
    /// Defines the <see cref="ContentAlignment"/> styled property.
    /// </summary>
    public static readonly StyledProperty<ShowcaseAlignment> ContentAlignmentProperty =
        AvaloniaProperty.Register<KnobShowcase, ShowcaseAlignment>(nameof(ContentAlignment), ShowcaseAlignment.Left);

    /// <summary>
    /// Defines the <see cref="ItemCount"/> styled property.
    /// </summary>
    public static readonly StyledProperty<double> ItemCountProperty =
        AvaloniaProperty.Register<KnobShowcase, double>(nameof(ItemCount), 3);

    /// <summary>
    /// Initializes a new instance of the <see cref="KnobShowcase"/> class.
    /// </summary>
    public KnobShowcase()
    {
        InitializeComponent();
        UpdateHighlight();
        UpdateAlignment();
        UpdateItemCount();
    }

    /// <summary>
    /// Gets or sets the heading text (TextKnob).
    /// </summary>
    public string Heading
    {
        get => GetValue(HeadingProperty);
        set => SetValue(HeadingProperty, value);
    }

    /// <summary>
    /// Gets or sets the description text (TextKnob).
    /// </summary>
    public string Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the highlight border is shown (BoolKnob).
    /// </summary>
    public bool IsHighlighted
    {
        get => GetValue(IsHighlightedProperty);
        set => SetValue(IsHighlightedProperty, value);
    }

    /// <summary>
    /// Gets or sets the content alignment (EnumKnob).
    /// </summary>
    public ShowcaseAlignment ContentAlignment
    {
        get => GetValue(ContentAlignmentProperty);
        set => SetValue(ContentAlignmentProperty, value);
    }

    /// <summary>
    /// Gets or sets the item count (NumericKnob).
    /// </summary>
    public double ItemCount
    {
        get => GetValue(ItemCountProperty);
        set => SetValue(ItemCountProperty, value);
    }

    /// <inheritdoc/>
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        ArgumentNullException.ThrowIfNull(change);
        base.OnPropertyChanged(change);

        if (change.Property == IsHighlightedProperty)
        {
            UpdateHighlight();
        }
        else if (change.Property == ContentAlignmentProperty)
        {
            UpdateAlignment();
        }
        else if (change.Property == ItemCountProperty)
        {
            UpdateItemCount();
        }
        else
        {
            // Heading and Description are handled via AXAML bindings.
        }
    }

    private void UpdateHighlight()
    {
        var highlighted = IsHighlighted;
        PART_Border.BorderBrush = highlighted
            ? new SolidColorBrush(Color.Parse(AccentColor))
            : new SolidColorBrush(Color.Parse(DefaultBorderColor));
        PART_Border.BorderThickness = highlighted ? new Thickness(2) : new Thickness(1);

        PART_HighlightRow.Background = highlighted
            ? new SolidColorBrush(Color.Parse("#EDE7FF"))
            : new SolidColorBrush(Color.Parse("#F5F5F5"));

        PART_HighlightValue.Text = highlighted ? "ON" : "OFF";
        PART_HighlightValue.Foreground = highlighted
            ? new SolidColorBrush(Color.Parse(AccentColor))
            : new SolidColorBrush(Color.Parse(MutedColor));
    }

    private void UpdateAlignment()
    {
        PART_AlignmentValue.Text = ContentAlignment.ToString();
        PART_AlignmentValue.HorizontalAlignment = ContentAlignment switch
        {
            ShowcaseAlignment.Left => HorizontalAlignment.Left,
            ShowcaseAlignment.Center => HorizontalAlignment.Center,
            ShowcaseAlignment.Right => HorizontalAlignment.Right,
            _ => HorizontalAlignment.Left,
        };
    }

    private void UpdateItemCount()
    {
        var count = (int)Math.Max(0, Math.Min(MaxItems, ItemCount));
        PART_ItemCountLabel.Text = $"{count} item(s)";

        PART_ItemsPanel.Children.Clear();
        for (var i = 0; i < count; i++)
        {
            PART_ItemsPanel.Children.Add(new Border
            {
                Background = new SolidColorBrush(Color.Parse(AccentColor)),
                CornerRadius = new CornerRadius(4),
                Height = ItemHeight,
                Margin = new Thickness(0, 2),
                Padding = new Thickness(8, 2),
                Child = new TextBlock
                {
                    Text = $"Item {i + 1}",
                    FontSize = LabelFontSize,
                    Foreground = Brushes.White,
                    VerticalAlignment = VerticalAlignment.Center,
                },
            });
        }
    }
}
