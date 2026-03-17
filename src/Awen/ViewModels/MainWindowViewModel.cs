// -----------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using Awen.Discovery;

namespace Awen.ViewModels;

/// <summary>
/// ViewModel for the main window, coordinating all panels.
/// </summary>
public sealed class MainWindowViewModel : INotifyPropertyChanged, IDisposable
{
    private const string HotReloadCategory = "HotReload";

    private readonly string[] _originalArgs;
    private DirectoryWatcher? _directoryWatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
    /// </summary>
    /// <param name="options">Parsed CLI options.</param>
    /// <param name="originalArgs">Original CLI arguments for process restart.</param>
    public MainWindowViewModel(AwenOptions options, string[]? originalArgs = null)
    {
        ArgumentNullException.ThrowIfNull(options);

        Options = options;
        _originalArgs = originalArgs ?? [];
        LogPanel = new LogPanelViewModel();

        var scanner = new AssemblyScanner();
        var (assemblies, loadErrors) = scanner.Scan(options.Dir);

        Registry = new StoryRegistry(assemblies, loadErrors);
        Sidebar = new SidebarViewModel(Registry, options.Filter);
        Preview = new PreviewViewModel
        {
            StoryAssemblies = assemblies,
            IsDarkTheme = options.Theme?.Equals("dark", StringComparison.OrdinalIgnoreCase) == true,
            LogPanel = LogPanel,
        };
        PropertyPanel = new PropertyPanelViewModel
        {
            LogPanel = LogPanel,
        };

        // Wire sidebar selection → preview + property panel
        Sidebar.PropertyChanged += OnSidebarPropertyChanged;

        LogDiscoveryResults(assemblies, loadErrors);
        RestoreState(options.RestoreFile);
        StartWatching(options);
    }

    /// <summary>
    /// Gets the parsed CLI options.
    /// </summary>
    public AwenOptions Options { get; }

    /// <summary>
    /// Gets the application version string from the assembly informational version.
    /// </summary>
    public static string Version { get; } = GetVersion();

    /// <summary>
    /// Gets the log panel view model.
    /// </summary>
    public LogPanelViewModel LogPanel { get; }

    /// <summary>
    /// Gets the story registry containing all discovered stories.
    /// </summary>
    public StoryRegistry Registry { get; }

    /// <summary>
    /// Gets the sidebar view model for story navigation.
    /// </summary>
    public SidebarViewModel Sidebar { get; }

    /// <summary>
    /// Gets the preview view model for rendering selected stories.
    /// </summary>
    public PreviewViewModel Preview { get; }

    /// <summary>
    /// Gets the property panel view model for story properties.
    /// </summary>
    public PropertyPanelViewModel PropertyPanel { get; }

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <inheritdoc/>
    public void Dispose()
    {
        _directoryWatcher?.Dispose();
        _directoryWatcher = null;
    }

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <param name="propertyName">The property name.</param>
    internal void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void LogDiscoveryResults(
        IReadOnlyList<StoryAssemblyInfo> assemblies,
        IReadOnlyList<LoadError> loadErrors)
    {
        foreach (var error in loadErrors)
        {
            LogPanel.AddLog(
                LogLevel.Error,
                "Discovery",
                $"Failed to load {error.FilePath}: {error.ErrorMessage}");
        }

        if (assemblies.Count == 0 && loadErrors.Count == 0)
        {
            LogPanel.AddLog(
                LogLevel.Warning,
                "Discovery",
                $"No story assemblies found in {Options.Dir.FullName}");
        }
    }

    private void RestoreState(string? restoreFile)
    {
        if (string.IsNullOrEmpty(restoreFile))
        {
            return;
        }

        var state = HotReloadState.ReadFromFile(restoreFile);
        if (state is null)
        {
            LogPanel.AddLog(LogLevel.Warning, HotReloadCategory, "Failed to read restore state file");
            return;
        }

        // Delete state file after reading
        DeleteStateFile(restoreFile);

        // Restore theme
        if (state.ThemeVariant is not null)
        {
            Preview.IsDarkTheme = state.ThemeVariant.Equals("dark", StringComparison.OrdinalIgnoreCase);
        }

        // Restore sidebar filter
        if (state.SidebarFilter is not null)
        {
            Sidebar.FilterText = state.SidebarFilter;
        }

        // Restore viewport dimensions
        if (state.ViewportWidth.HasValue)
        {
            Preview.ViewportWidth = state.ViewportWidth.Value;
        }

        if (state.ViewportHeight.HasValue)
        {
            Preview.ViewportHeight = state.ViewportHeight.Value;
        }

        // Restore selected story
        if (state.SelectedStoryIdentity is not null)
        {
            var story = Registry.FindByIdentity(state.SelectedStoryIdentity);
            if (story is not null)
            {
                Sidebar.SelectedStory = story;
            }
            else
            {
                LogPanel.AddLog(
                    LogLevel.Warning,
                    HotReloadCategory,
                    $"Story '{state.SelectedStoryIdentity}' no longer exists after reload");
            }
        }

        LogPanel.AddLog(LogLevel.Info, HotReloadCategory, "State restored");
    }

    private void StartWatching(AwenOptions options)
    {
        if (!options.Watch)
        {
            return;
        }

        _directoryWatcher = new DirectoryWatcher(options.Dir, OnDllChanged);
        LogPanel.AddLog(LogLevel.Info, HotReloadCategory, $"Watching {options.Dir.FullName} for DLL changes");
    }

    private void OnDllChanged(string changedPath)
    {
        LogPanel.AddLog(LogLevel.Info, HotReloadCategory, "DLL change detected, restarting...");

        var state = new HotReloadState
        {
            SelectedStoryIdentity = Sidebar.SelectedStory?.Identity,
            ThemeVariant = Preview.IsDarkTheme ? "dark" : "light",
            SidebarFilter = Sidebar.FilterText,
            ViewportWidth = double.IsPositiveInfinity(Preview.ViewportWidth) ? null : Preview.ViewportWidth,
            ViewportHeight = double.IsPositiveInfinity(Preview.ViewportHeight) ? null : Preview.ViewportHeight,
        };

        ProcessRestart.RestartWithState(state, _originalArgs);
    }

    private void OnSidebarPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName?.Equals(nameof(SidebarViewModel.SelectedStory), StringComparison.Ordinal) == true)
        {
            Preview.SelectedStory = Sidebar.SelectedStory;
            PropertyPanel.LoadStory(Sidebar.SelectedStory);
        }
    }

    private static string GetVersion()
    {
        var attr = typeof(MainWindowViewModel).Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        return attr?.InformationalVersion ?? "dev";
    }

    private static void DeleteStateFile(string filePath)
    {
#pragma warning disable CA1031 // Best-effort cleanup of temp state file
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        catch (Exception)
        {
            // Ignore — temp file cleanup is best-effort
        }
#pragma warning restore CA1031
    }
}
