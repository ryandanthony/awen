// -----------------------------------------------------------------------
// <copyright file="PrimaryButtonStory.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Awen.Sdk;

namespace Awen.Tests.TestFixtures;

/// <summary>
/// A simple button story for testing.
/// Acts as the shared state (DataContext) for both control and properties.
/// </summary>
public sealed class PrimaryButtonStory : IStory<UserControl, UserControl>, INotifyPropertyChanged
{
    private string _label = "Click Me";
    private bool _isEnabled = true;

    /// <summary>
    /// Gets or sets the button label.
    /// </summary>
    public string Label
    {
        get => _label;
        set
        {
            _label = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the button is enabled.
    /// </summary>
    public bool IsButtonEnabled
    {
        get => _isEnabled;
        set
        {
            _isEnabled = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc/>
    public string Name => "Primary";

    /// <inheritdoc/>
    public string Group => "Atoms/Buttons";

    /// <inheritdoc/>
    public int Order => 1;

    /// <inheritdoc/>
    public string Description => "Standard primary action button.";

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <inheritdoc/>
    public UserControl CreateControl()
    {
        var button = new Button { Content = Label, IsEnabled = IsButtonEnabled };
        return new UserControl { Content = button, DataContext = this };
    }

    /// <inheritdoc/>
    public UserControl CreateProperties()
    {
        var panel = new StackPanel();
        panel.Children.Add(new TextBox { Text = Label });
        panel.Children.Add(new CheckBox { IsChecked = IsButtonEnabled });
        return new UserControl { Content = panel, DataContext = this };
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
