// -----------------------------------------------------------------------
// <copyright file="ViewportEditableDevice.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Awen.ViewModels;

/// <summary>
/// Editable viewport device row.
/// </summary>
public sealed class ViewportEditableDevice : INotifyPropertyChanged
{
    private bool _enabled;

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewportEditableDevice"/> class.
    /// </summary>
    /// <param name="name">Device name.</param>
    /// <param name="width">Device width.</param>
    /// <param name="height">Device height.</param>
    /// <param name="enabled">Whether the device is enabled.</param>
    public ViewportEditableDevice(string name, double width, double height, bool enabled)
    {
        Name = name;
        Width = width;
        Height = height;
        _enabled = enabled;
    }

    /// <summary>
    /// Gets the device name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the device width.
    /// </summary>
    public double Width { get; }

    /// <summary>
    /// Gets the device height.
    /// </summary>
    public double Height { get; }

    /// <summary>
    /// Gets or sets a value indicating whether this device is enabled.
    /// </summary>
    public bool Enabled
    {
        get => _enabled;
        set
        {
            if (_enabled != value)
            {
                _enabled = value;
                OnPropertyChanged();
            }
        }
    }

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
