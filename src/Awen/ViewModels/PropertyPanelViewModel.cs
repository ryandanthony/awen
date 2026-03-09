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
            PropertiesContent = null;
            ErrorMessage = $"CreateProperties() failed for '{story.Name}': {ex.Message}";
        }
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
