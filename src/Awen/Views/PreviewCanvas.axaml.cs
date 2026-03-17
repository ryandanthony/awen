// -----------------------------------------------------------------------
// <copyright file="PreviewCanvas.axaml.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Awen.ViewModels;

namespace Awen.Views;

/// <summary>
/// Code-behind for the preview canvas that renders the selected story's component.
/// </summary>
public sealed partial class PreviewCanvas : UserControl
{
    private ResourceDictionary? _currentLibraryTheme;

    /// <summary>
    /// Initializes a new instance of the <see cref="PreviewCanvas"/> class.
    /// </summary>
    public PreviewCanvas()
    {
        InitializeComponent();
        PART_PreviewBorder.PropertyChanged += OnBorderSizeChanged;
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is PreviewViewModel vm)
        {
            vm.PropertyChanged += OnViewModelPropertyChanged;
            ApplyLibraryTheme(vm.LibraryTheme);
        }
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName?.Equals(nameof(PreviewViewModel.LibraryTheme), StringComparison.Ordinal) == true
            && sender is PreviewViewModel vm)
        {
            ApplyLibraryTheme(vm.LibraryTheme);
        }
    }

    private void ApplyLibraryTheme(ResourceDictionary? newTheme)
    {
        var merged = PART_ThemeScope.Resources.MergedDictionaries;

        // Add new before removing old to avoid flash of unstyled content
        if (newTheme is not null)
        {
            merged.Add(newTheme);
        }

        if (_currentLibraryTheme is not null)
        {
            merged.Remove(_currentLibraryTheme);
        }

        _currentLibraryTheme = newTheme;
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
