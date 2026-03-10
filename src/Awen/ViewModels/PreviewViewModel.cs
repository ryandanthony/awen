// -----------------------------------------------------------------------
// <copyright file="PreviewViewModel.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Styling;
using Awen.Discovery;

namespace Awen.ViewModels;

/// <summary>
/// ViewModel for the preview canvas that renders the selected story's component.
/// </summary>
public sealed class PreviewViewModel : INotifyPropertyChanged
{
    /// <summary>
    /// Available viewport preset names.
    /// </summary>
    public static readonly IReadOnlyList<string> ViewportPresets = ["Responsive", "Phone", "Tablet", "Laptop", "Desktop", "Widescreen"];

    private static readonly Dictionary<string, (double width, double height)> PresetDimensions = new(StringComparer.Ordinal)
    {
        ["Responsive"] = (double.PositiveInfinity, double.PositiveInfinity),
        ["Phone"] = (375, 812),
        ["Tablet"] = (768, 1024),
        ["Laptop"] = (1366, 768),
        ["Desktop"] = (1920, 1080),
        ["Widescreen"] = (2560, 1440),
    };

    private StoryDescriptor? _selectedStory;
    private Control? _previewContent;
    private string? _description;
    private string? _errorMessage;
    private bool _isDarkTheme;
    private double _viewportWidth = double.PositiveInfinity;
    private double _viewportHeight = double.PositiveInfinity;
    private double _actualViewportWidth;
    private double _actualViewportHeight;
    private string _selectedPreset = "Responsive";
    private bool _suppressPresetChange;

    /// <summary>
    /// Gets or sets a value indicating whether the dark theme is active.
    /// </summary>
    public bool IsDarkTheme
    {
        get => _isDarkTheme;
        set
        {
            if (_isDarkTheme != value)
            {
                _isDarkTheme = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ThemeVariant));

                // Re-render story under new theme
                if (_selectedStory is not null)
                {
                    RenderStory(_selectedStory);
                }
            }
        }
    }

    /// <summary>
    /// Gets the current theme variant for the preview scope.
    /// </summary>
    public ThemeVariant ThemeVariant => _isDarkTheme ? ThemeVariant.Dark : ThemeVariant.Light;

    /// <summary>
    /// Gets or sets the currently selected story to preview.
    /// Setting this triggers Create() and updates the preview content.
    /// </summary>
    public StoryDescriptor? SelectedStory
    {
        get => _selectedStory;
        set
        {
            if (_selectedStory != value)
            {
                _selectedStory = value;
                OnPropertyChanged();
                RenderStory(value);
            }
        }
    }

    /// <summary>
    /// Gets the rendered preview control, or null if no story is selected or an error occurred.
    /// </summary>
    public Control? PreviewContent
    {
        get => _previewContent;
        private set
        {
            _previewContent = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets the description of the currently selected story.
    /// </summary>
    public string? Description
    {
        get => _description;
        private set
        {
            _description = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets the error message if story rendering failed, or null when successful.
    /// </summary>
    public string? ErrorMessage
    {
        get => _errorMessage;
        private set
        {
            _errorMessage = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(HasError));
        }
    }

    /// <summary>
    /// Gets a value indicating whether there is an error to display.
    /// </summary>
    public bool HasError => _errorMessage is not null;

    /// <summary>
    /// Gets or sets the viewport width constraint. <see cref="double.PositiveInfinity"/> means unconstrained.
    /// </summary>
    public double ViewportWidth
    {
        get => _viewportWidth;
        set
        {
            if (!_viewportWidth.Equals(value))
            {
                _viewportWidth = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayViewportWidth));

                if (!_suppressPresetChange)
                {
                    UpdatePresetFromDimensions();
                }
            }
        }
    }

    /// <summary>
    /// Gets or sets the viewport height constraint. <see cref="double.PositiveInfinity"/> means unconstrained.
    /// </summary>
    public double ViewportHeight
    {
        get => _viewportHeight;
        set
        {
            if (!_viewportHeight.Equals(value))
            {
                _viewportHeight = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayViewportHeight));

                if (!_suppressPresetChange)
                {
                    UpdatePresetFromDimensions();
                }
            }
        }
    }

    /// <summary>
    /// Gets or sets the actual rendered width of the preview canvas.
    /// Updated by the view when the canvas is resized.
    /// </summary>
    public double ActualViewportWidth
    {
        get => _actualViewportWidth;
        set
        {
            if (!_actualViewportWidth.Equals(value))
            {
                _actualViewportWidth = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayViewportWidth));
            }
        }
    }

    /// <summary>
    /// Gets or sets the actual rendered height of the preview canvas.
    /// Updated by the view when the canvas is resized.
    /// </summary>
    public double ActualViewportHeight
    {
        get => _actualViewportHeight;
        set
        {
            if (!_actualViewportHeight.Equals(value))
            {
                _actualViewportHeight = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayViewportHeight));
            }
        }
    }

    /// <summary>
    /// Gets the width to display in the toolbar. Shows actual rendered size when unconstrained, otherwise the set value.
    /// </summary>
    public double DisplayViewportWidth =>
        double.IsPositiveInfinity(_viewportWidth) ? _actualViewportWidth : _viewportWidth;

    /// <summary>
    /// Gets the height to display in the toolbar. Shows actual rendered size when unconstrained, otherwise the set value.
    /// </summary>
    public double DisplayViewportHeight =>
        double.IsPositiveInfinity(_viewportHeight) ? _actualViewportHeight : _viewportHeight;

    /// <summary>
    /// Gets or sets the selected viewport preset name.
    /// </summary>
    public string SelectedPreset
    {
        get => _selectedPreset;
        set
        {
            if (!string.Equals(_selectedPreset, value, StringComparison.Ordinal))
            {
                _selectedPreset = value;
                OnPropertyChanged();
                ApplyPresetDimensions(value);
            }
        }
    }

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    private void RenderStory(StoryDescriptor? descriptor)
    {
        ErrorMessage = null;

        if (descriptor is null)
        {
            PreviewContent = null;
            Description = null;
            return;
        }

        Description = descriptor.Description;

        try
        {
            var control = descriptor.StoryInstance.CreateControl();
            control.DataContext = descriptor.StoryInstance;
            PreviewContent = control;
        }
#pragma warning disable CA1031 // Catch all for resilient preview rendering
        catch (Exception ex)
#pragma warning restore CA1031
        {
            PreviewContent = null;
            ErrorMessage = $"CreateControl() failed for '{descriptor.Name}': {ex.Message}";
        }
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void ApplyPresetDimensions(string preset)
    {
        if (PresetDimensions.TryGetValue(preset, out var dimensions))
        {
            _suppressPresetChange = true;
            ViewportWidth = dimensions.width;
            ViewportHeight = dimensions.height;
            _suppressPresetChange = false;
        }
    }

    private void UpdatePresetFromDimensions()
    {
        foreach (var (name, dimensions) in PresetDimensions)
        {
            if (_viewportWidth.Equals(dimensions.width) && _viewportHeight.Equals(dimensions.height))
            {
                if (!string.Equals(_selectedPreset, name, StringComparison.Ordinal))
                {
                    _selectedPreset = name;
                    OnPropertyChanged(nameof(SelectedPreset));
                }

                return;
            }
        }

        if (!string.Equals(_selectedPreset, "Custom", StringComparison.Ordinal))
        {
            _selectedPreset = "Custom";
            OnPropertyChanged(nameof(SelectedPreset));
        }
    }
}
