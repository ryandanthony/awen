// -----------------------------------------------------------------------
// <copyright file="PreviewCanvas.axaml.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Awen.ViewModels;

namespace Awen.Views;

/// <summary>
/// Code-behind for the preview canvas that renders the selected story's component.
/// </summary>
public sealed partial class PreviewCanvas : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PreviewCanvas"/> class.
    /// </summary>
    public PreviewCanvas()
    {
        InitializeComponent();
        PART_PreviewBorder.PropertyChanged += OnBorderSizeChanged;
    }

    private void OnBorderSizeChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == BoundsProperty && DataContext is PreviewViewModel vm)
        {
            var bounds = PART_PreviewBorder.Bounds;
            vm.ActualViewportWidth = bounds.Width;
            vm.ActualViewportHeight = bounds.Height;
        }
    }
}
