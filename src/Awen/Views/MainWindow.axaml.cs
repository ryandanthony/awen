// -----------------------------------------------------------------------
// <copyright file="MainWindow.axaml.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Awen.ViewModels;

namespace Awen.Views;

/// <summary>
/// Main application window with 3-panel layout and collapsible log panel.
/// </summary>
public sealed partial class MainWindow : Window
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
    }

#pragma warning disable VSTHRD100 // UI event handlers in Avalonia require async void signature.
    private async void OnEditViewportClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is not MainWindowViewModel vm)
        {
            return;
        }

        var editableDevices = vm.Preview.LoadViewportDevicesForEdit();
        var dialogVm = new ViewportEditorViewModel(editableDevices);
        var dialog = new ViewportEditorWindow(dialogVm);

        var result = await dialog.ShowDialog<bool?>(this);
        if (result == true)
        {
            vm.Preview.SaveViewportDevices(dialogVm.ToDevices());
        }
    }
#pragma warning restore VSTHRD100

    private void OnPortraitClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.Preview.IsLandscape = false;
        }
    }

    private void OnLandscapeClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.Preview.IsLandscape = true;
        }
    }
}
