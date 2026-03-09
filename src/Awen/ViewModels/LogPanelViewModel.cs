// -----------------------------------------------------------------------
// <copyright file="LogPanelViewModel.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Awen.ViewModels;

/// <summary>
/// ViewModel for the collapsible log panel.
/// </summary>
public sealed class LogPanelViewModel : INotifyPropertyChanged
{
    private readonly TimeProvider _timeProvider;
    private bool _isExpanded;

    /// <summary>
    /// Initializes a new instance of the <see cref="LogPanelViewModel"/> class.
    /// </summary>
    /// <param name="timeProvider">Time provider for timestamps. Uses <see cref="TimeProvider.System"/> if null.</param>
    public LogPanelViewModel(TimeProvider? timeProvider = null)
    {
        _timeProvider = timeProvider ?? TimeProvider.System;
    }

    /// <summary>
    /// Gets the collection of log entries.
    /// </summary>
    public ObservableCollection<LogEntry> Entries { get; } = [];

    /// <summary>
    /// Gets a command that toggles <see cref="IsExpanded"/>.
    /// </summary>
    public ICommand ToggleExpandedCommand => new RelayCommand(() => IsExpanded = !IsExpanded);

    /// <summary>
    /// Gets or sets a value indicating whether the log panel is expanded.
    /// </summary>
    public bool IsExpanded
    {
        get => _isExpanded;
        set
        {
            if (_isExpanded != value)
            {
                _isExpanded = value;
                OnPropertyChanged();
            }
        }
    }

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Adds a log entry.
    /// </summary>
    /// <param name="level">The severity level.</param>
    /// <param name="category">The source category.</param>
    /// <param name="message">The log message.</param>
    public void AddLog(LogLevel level, string category, string message)
    {
        Entries.Add(new LogEntry
        {
            Timestamp = _timeProvider.GetUtcNow(),
            Level = level,
            Category = category,
            Message = message,
        });

        if (level == LogLevel.Error)
        {
            IsExpanded = true;
        }
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
