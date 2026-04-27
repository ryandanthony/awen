// -----------------------------------------------------------------------
// <copyright file="PreviewViewModel.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Styling;
using Awen.Configuration;
using Awen.Discovery;

namespace Awen.ViewModels;

/// <summary>
/// ViewModel for the preview canvas that renders the selected story's component.
/// </summary>
public sealed class PreviewViewModel : INotifyPropertyChanged
{
    private const string ResponsivePresetName = "Responsive";

    private readonly ViewportConfigStore _viewportConfigStore;
    private Dictionary<string, (double width, double height)> _presetDimensions = new(StringComparer.Ordinal)
    {
        [ResponsivePresetName] = (double.PositiveInfinity, double.PositiveInfinity),
    };

    private IReadOnlyList<StoryAssemblyInfo> _storyAssemblies = [];
    private LogPanelViewModel? _logPanel;
    private StoryDescriptor? _selectedStory;
    private Control? _previewContent;
    private string? _description;
    private string? _errorMessage;
    private bool _isDarkTheme;
    private bool _isLandscape;
    private ResourceDictionary? _libraryTheme;
    private double _viewportWidth = double.PositiveInfinity;
    private double _viewportHeight = double.PositiveInfinity;
    private double _actualViewportWidth;
    private double _actualViewportHeight;
    private IReadOnlyList<string> _viewportPresets = [ResponsivePresetName];
    private string _selectedPreset = ResponsivePresetName;
    private bool _suppressPresetChange;

    /// <summary>
    /// Initializes a new instance of the <see cref="PreviewViewModel"/> class.
    /// </summary>
    public PreviewViewModel(ViewportConfigStore? viewportConfigStore = null)
    {
        _viewportConfigStore = viewportConfigStore ?? new ViewportConfigStore();
        ReloadViewportConfiguration();
    }

    /// <summary>
    /// Gets the default user viewport config path.
    /// </summary>
    public string ViewportConfigPath => _viewportConfigStore.UserConfigPath;

    /// <summary>
    /// Gets the available viewport preset names.
    /// </summary>
    public IReadOnlyList<string> ViewportPresets
    {
        get => _viewportPresets;
        private set
        {
            _viewportPresets = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the log panel for reporting errors.
    /// </summary>
    public LogPanelViewModel? LogPanel
    {
        get => _logPanel;
        set
        {
            _logPanel = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the loaded story assemblies for library theme resolution.
    /// </summary>
    public IReadOnlyList<StoryAssemblyInfo> StoryAssemblies
    {
        get => _storyAssemblies;
        set
        {
            _storyAssemblies = value ?? [];
            OnPropertyChanged();
        }
    }

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

                // Reload library theme for new variant and re-render story
                if (_selectedStory is not null)
                {
                    LoadLibraryTheme(_selectedStory);
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
    /// Gets the library theme <see cref="ResourceDictionary"/> to merge into the preview scope.
    /// Loaded from the story assembly's embedded resources via <see cref="ThemeLoader"/>.
    /// </summary>
    public ResourceDictionary? LibraryTheme
    {
        get => _libraryTheme;
        private set
        {
            _libraryTheme = value;
            OnPropertyChanged();
        }
    }

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
    /// Gets or sets a value indicating whether the viewport is in landscape orientation.
    /// Only relevant when a fixed-size preset is active.
    /// </summary>
    public bool IsLandscape
    {
        get => _isLandscape;
        set
        {
            if (_isLandscape != value)
            {
                _isLandscape = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(OrientationLabel));
                ApplyPresetDimensions(_selectedPreset);
            }
        }
    }

    /// <summary>
    /// Gets a label describing the current orientation, used for display.
    /// </summary>
    public string OrientationLabel => _isLandscape ? "Landscape" : "Portrait";

    /// <summary>
    /// Gets a value indicating whether orientation toggle is applicable (non-Responsive preset).
    /// </summary>
    public bool CanToggleOrientation =>
        !string.Equals(_selectedPreset, ResponsivePresetName, StringComparison.Ordinal)
        && !string.Equals(_selectedPreset, "Custom", StringComparison.Ordinal);

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
                OnPropertyChanged(nameof(CanToggleOrientation));
                ApplyPresetDimensions(value);
            }
        }
    }

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Ensures user viewport config exists, then returns editable devices.
    /// </summary>
    /// <returns>Editable device list.</returns>
    public IReadOnlyList<ViewportDevice> LoadViewportDevicesForEdit()
    {
        _viewportConfigStore.EnsureUserConfigExists();
        var config = _viewportConfigStore.Load();
        return config.Devices
            .Select(device => device with { })
            .ToList();
    }

    /// <summary>
    /// Saves edited viewport devices and reloads presets.
    /// </summary>
    /// <param name="devices">Edited devices.</param>
    public void SaveViewportDevices(IEnumerable<ViewportDevice> devices)
    {
        ArgumentNullException.ThrowIfNull(devices);

        var sanitized = devices
            .Where(device =>
                !string.IsNullOrWhiteSpace(device.Name)
                && device.Width > 0
                && device.Height > 0)
            .DistinctBy(device => device.Name, StringComparer.Ordinal)
            .ToList();

        var config = new ViewportConfig
        {
            Devices = sanitized,
        };

        _viewportConfigStore.Write(config);
        ReloadViewportConfiguration();
    }

    /// <summary>
    /// Reloads viewport presets from user config file or embedded defaults.
    /// </summary>
    public void ReloadViewportConfiguration()
    {
        var config = _viewportConfigStore.Load();

        var presets = new List<string> { ResponsivePresetName };
        var dimensions = new Dictionary<string, (double width, double height)>(StringComparer.Ordinal)
        {
            [ResponsivePresetName] = (double.PositiveInfinity, double.PositiveInfinity),
        };

        foreach (var device in config.Devices.Where(device => device.Enabled))
        {
            if (string.IsNullOrWhiteSpace(device.Name) || device.Width <= 0 || device.Height <= 0)
            {
                continue;
            }

            if (dimensions.ContainsKey(device.Name))
            {
                continue;
            }

            dimensions[device.Name] = (device.Width, device.Height);
            presets.Add(device.Name);
        }

        _presetDimensions = dimensions;
        ViewportPresets = presets;

        UpdatePresetFromDimensions();
    }

    private void RenderStory(StoryDescriptor? descriptor)
    {
        ErrorMessage = null;

        if (descriptor is null)
        {
            PreviewContent = null;
            Description = null;
            LibraryTheme = null;
            return;
        }

        Description = descriptor.Description;
        LoadLibraryTheme(descriptor);

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
            LogPanel?.AddLog(LogLevel.Error, "Preview", $"CreateControl() failed for '{descriptor.Name}':\n{ex}");
        }
    }

    private void LoadLibraryTheme(StoryDescriptor descriptor)
    {
        var assemblyInfo = _storyAssemblies.FirstOrDefault(
            a => a.LibraryName.Equals(descriptor.LibraryName, StringComparison.Ordinal));

        if (assemblyInfo is null)
        {
            LibraryTheme = null;
            return;
        }

        var resourcePath = _isDarkTheme
            ? assemblyInfo.DarkThemeResourcePath
            : assemblyInfo.LightThemeResourcePath;

        try
        {
            LibraryTheme = ThemeLoader.LoadFromAssembly(assemblyInfo.Assembly, resourcePath);
        }
#pragma warning disable CA1031 // Catch all for resilient theme loading
        catch (Exception ex)
#pragma warning restore CA1031
        {
            LibraryTheme = null;
            LogPanel?.AddLog(LogLevel.Error, "Theme", $"LoadLibraryTheme() failed for '{descriptor.LibraryName}':\n{ex}");
        }
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void ApplyPresetDimensions(string preset)
    {
        if (_presetDimensions.TryGetValue(preset, out var dimensions))
        {
            _suppressPresetChange = true;
            if (_isLandscape && !double.IsPositiveInfinity(dimensions.width))
            {
                ViewportWidth = Math.Max(dimensions.width, dimensions.height);
                ViewportHeight = Math.Min(dimensions.width, dimensions.height);
            }
            else
            {
                ViewportWidth = double.IsPositiveInfinity(dimensions.width)
                    ? dimensions.width
                    : Math.Min(dimensions.width, dimensions.height);
                ViewportHeight = double.IsPositiveInfinity(dimensions.height)
                    ? dimensions.height
                    : Math.Max(dimensions.width, dimensions.height);
            }

            _suppressPresetChange = false;
        }
    }

    private void UpdatePresetFromDimensions()
    {
        foreach (var (name, dimensions) in _presetDimensions)
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
            OnPropertyChanged(nameof(CanToggleOrientation));
        }
    }
}
