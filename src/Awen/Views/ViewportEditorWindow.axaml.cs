// -----------------------------------------------------------------------
// <copyright file="ViewportEditorWindow.axaml.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Awen.ViewModels;

namespace Awen.Views;

/// <summary>
/// Dialog window for editing viewport devices.
/// </summary>
public sealed partial class ViewportEditorWindow : Window
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ViewportEditorWindow"/> class.
    /// </summary>
    public ViewportEditorWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewportEditorWindow"/> class.
    /// </summary>
    /// <param name="viewModel">Editor view model.</param>
    public ViewportEditorWindow(ViewportEditorViewModel viewModel)
        : this()
    {
        DataContext = viewModel;
    }

    private void OnAddDeviceClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is ViewportEditorViewModel vm)
        {
            vm.TryAddDevice();
        }
    }

    private void OnCancelClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close(false);
    }

    private void OnSaveClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close(true);
    }
}
