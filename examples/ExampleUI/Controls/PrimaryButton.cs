// -----------------------------------------------------------------------
// <copyright file="PrimaryButton.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace ExampleUI.Controls;

/// <summary>
/// A styled primary action button with configurable label and color.
/// </summary>
public partial class PrimaryButton : UserControl
{
    private const double DefaultFontSize = 14;
    private const double DisabledOpacity = 0.45;

    /// <summary>
    /// Defines the <see cref="Clicked"/> routed event.
    /// </summary>
    public static readonly RoutedEvent<RoutedEventArgs> ClickedEvent =
        RoutedEvent.Register<PrimaryButton, RoutedEventArgs>(nameof(Clicked), RoutingStrategies.Bubble);

    /// <summary>
    /// Defines the <see cref="Label"/> styled property.
    /// </summary>
    public static readonly StyledProperty<string> LabelProperty =
        AvaloniaProperty.Register<PrimaryButton, string>(nameof(Label), "Button");

    /// <summary>
    /// Defines the <see cref="ButtonColor"/> styled property.
    /// </summary>
    public static readonly StyledProperty<IBrush> ButtonColorProperty =
        AvaloniaProperty.Register<PrimaryButton, IBrush>(nameof(ButtonColor), Brushes.DodgerBlue);

    /// <summary>
    /// Defines the <see cref="ButtonFontSize"/> styled property.
    /// </summary>
    public static readonly StyledProperty<double> ButtonFontSizeProperty =
        AvaloniaProperty.Register<PrimaryButton, double>(nameof(ButtonFontSize), DefaultFontSize);

    /// <summary>
    /// Defines the <see cref="Command"/> styled property.
    /// </summary>
    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<PrimaryButton, ICommand?>(nameof(Command));

    /// <summary>
    /// Defines the <see cref="CommandParameter"/> styled property.
    /// </summary>
    public static readonly StyledProperty<object?> CommandParameterProperty =
        AvaloniaProperty.Register<PrimaryButton, object?>(nameof(CommandParameter));

    /// <summary>
    /// Initializes a new instance of the <see cref="PrimaryButton"/> class.
    /// </summary>
    public PrimaryButton()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Handles the inner button click and raises the <see cref="ClickedEvent"/> routed event.
    /// </summary>
    private void OnInnerButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        RaiseEvent(new RoutedEventArgs(ClickedEvent));
    }

    /// <summary>
    /// Raised when the button is clicked.
    /// </summary>
    public event EventHandler<RoutedEventArgs> Clicked
    {
        add => AddHandler(ClickedEvent, value);
        remove => RemoveHandler(ClickedEvent, value);
    }

    /// <summary>
    /// Gets or sets the button label text.
    /// </summary>
    public string Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    /// <summary>
    /// Gets or sets the button background color.
    /// </summary>
    public IBrush ButtonColor
    {
        get => GetValue(ButtonColorProperty);
        set => SetValue(ButtonColorProperty, value);
    }

    /// <summary>
    /// Gets or sets the button font size.
    /// </summary>
    public double ButtonFontSize
    {
        get => GetValue(ButtonFontSizeProperty);
        set => SetValue(ButtonFontSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the command to execute when the button is clicked.
    /// </summary>
    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the parameter to pass to the command.
    /// </summary>
    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    /// <inheritdoc/>
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        ArgumentNullException.ThrowIfNull(change);
        base.OnPropertyChanged(change);

        if (change.Property == ButtonColorProperty || change.Property == IsEnabledProperty)
        {
            ApplyEnabledState();
        }
        else
        {
            // No custom handling needed for other properties.
        }
    }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        ApplyEnabledState();
    }

    private void ApplyEnabledState()
    {
        PART_Button.Background = IsEnabled ? ButtonColor : Brushes.Gray;
        PART_Button.Opacity = IsEnabled ? 1.0 : DisabledOpacity;
        PART_Button.Cursor = IsEnabled
            ? new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Hand)
            : new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Arrow);
    }
}
