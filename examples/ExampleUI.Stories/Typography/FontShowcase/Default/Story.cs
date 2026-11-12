// -----------------------------------------------------------------------
// <copyright file="Story.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Awen.Sdk;

namespace ExampleUI.Stories.Typography.FontShowcase.Default;

/// <summary>
/// Shows text in the Noto Mono font that the library's <see cref="Startup"/> hook registers.
/// If the hook did not run, the text falls back to the default font.
/// </summary>
public sealed class Story : IStory<UserControl, UserControl>, INotifyPropertyChanged
{
    private string _sampleText = "The quick brown fox jumps over the lazy dog 0123456789";
    private double _sampleFontSize = 20;

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <inheritdoc/>
    string IStory<UserControl, UserControl>.Name => "Embedded Font";

    /// <inheritdoc/>
    string IStory<UserControl, UserControl>.Group => "Typography";

    /// <inheritdoc/>
    int IStory<UserControl, UserControl>.Order => 0;

    /// <inheritdoc/>
    string IStory<UserControl, UserControl>.Description =>
        "Uses fonts:ExampleUI#Noto Mono, an embedded font registered by the library's startup hook (IAwenStartup).";

    /// <summary>
    /// Gets or sets the sample text.
    /// </summary>
    public string SampleText
    {
        get => _sampleText;

        set
        {
            _sampleText = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the sample font size.
    /// </summary>
    public double SampleFontSize
    {
        get => _sampleFontSize;

        set
        {
            _sampleFontSize = value;
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
