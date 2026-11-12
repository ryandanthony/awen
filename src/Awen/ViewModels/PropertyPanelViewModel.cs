// -----------------------------------------------------------------------
// <copyright file="PropertyPanelViewModel.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Awen.Discovery;

namespace Awen.ViewModels;

/// <summary>
/// ViewModel for the property panel that hosts the story's properties control.
/// </summary>
public sealed class PropertyPanelViewModel : INotifyPropertyChanged
{
    private Control? _propertiesContent;
    private bool _hasStorySelected;
    private string? _errorMessage;
    private LogPanelViewModel? _logPanel;

    /// <summary>
    /// Gets the properties control created by the current story's <c>CreateProperties()</c>.
    /// </summary>
    public Control? PropertiesContent
    {
        get => _propertiesContent;
        private set
        {
            _propertiesContent = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsEmpty));
        }
    }

    /// <summary>
    /// Gets a value indicating whether there is no properties control to display.
    /// </summary>
    public bool IsEmpty => _propertiesContent is null;

    /// <summary>
    /// Gets a value indicating whether a story is currently selected.
    /// </summary>
    public bool HasStorySelected
    {
        get => _hasStorySelected;
        private set
        {
            _hasStorySelected = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets the error message if <c>CreateProperties()</c> failed, or null when successful.
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

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Loads the properties panel from the given story descriptor.
    /// </summary>
    /// <param name="story">The selected story, or null to clear.</param>
    public void LoadStory(StoryDescriptor? story)
    {
        ErrorMessage = null;

        if (story is null)
        {
            PropertiesContent = null;
            HasStorySelected = false;
            return;
        }

        HasStorySelected = true;

        if (story.LoadError is not null)
        {
            PropertiesContent = null;
            ErrorMessage = "This story could not be loaded. See the preview for the error.";
            return;
        }

        try
        {
            var properties = story.StoryInstance.CreateProperties();
            properties.DataContext = story.StoryInstance;
            PropertiesContent = properties;
        }
#pragma warning disable CA1031 // Catch all for resilient properties rendering
        catch (Exception ex)
#pragma warning restore CA1031
        {
            var cause = StoryErrors.Unwrap(ex);
            PropertiesContent = null;
            ErrorMessage = $"CreateProperties() failed for '{story.Name}': {StoryErrors.Summarize(cause)}";
            LogPanel?.AddLog(LogLevel.Error, "Properties", $"CreateProperties() failed for '{story.Name}':\n{cause}");
        }
    }

    /// <summary>
    /// Removes the properties editor after the story failed while running, since it is bound to the same
    /// (possibly broken) story state. Retry in the preview reloads it.
    /// </summary>
    public void StopForError()
    {
        if (!HasStorySelected)
        {
            return;
        }

        PropertiesContent = null;
        ErrorMessage = "The story stopped after an error. Use Retry in the preview to reload it.";
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
