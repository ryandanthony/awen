// -----------------------------------------------------------------------
// <copyright file="Story.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Awen.Sdk;

namespace ExampleUI.Stories.Themes.ThemeDemo.Default;

/// <summary>
/// Story for the <see cref="ExampleUI.Controls.ThemeDemo"/> control.
/// Shared state backing both the preview control and property panel via binding.
/// </summary>
public sealed class Story : IStory<UserControl, UserControl>, INotifyPropertyChanged
{
    private string _title = "Theme Preview";
    private string _message = "This control demonstrates the current theme.";

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <inheritdoc/>
    string IStory<UserControl, UserControl>.Name => "Default";

    /// <inheritdoc/>
    string IStory<UserControl, UserControl>.Group => "Themes";

    /// <inheritdoc/>
    int IStory<UserControl, UserControl>.Order => 0;

    /// <inheritdoc/>
    string IStory<UserControl, UserControl>.Description =>
        "Demonstrates the current Fluent theme with title and message.";

    /// <summary>
    /// Gets or sets the demo title.
    /// </summary>
    public string Title
    {
        get => _title;

        set
        {
            _title = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the demo message.
    /// </summary>
    public string Message
    {
        get => _message;

        set
        {
            _message = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc/>
    UserControl IStory<UserControl, UserControl>.CreateControl() => new Control();

    /// <inheritdoc/>
    UserControl IStory<UserControl, UserControl>.CreateProperties() => new Properties();

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
