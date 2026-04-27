// -----------------------------------------------------------------------
// <copyright file="ViewportEditorViewModel.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Awen.Configuration;

namespace Awen.ViewModels;

/// <summary>
/// ViewModel for editing viewport device configuration.
/// </summary>
public sealed class ViewportEditorViewModel : INotifyPropertyChanged
{
    private string _newDeviceName = string.Empty;
    private string _newDeviceWidth = string.Empty;
    private string _newDeviceHeight = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewportEditorViewModel"/> class.
    /// </summary>
    /// <param name="devices">Devices to edit.</param>
    public ViewportEditorViewModel(IEnumerable<ViewportDevice> devices)
    {
        ArgumentNullException.ThrowIfNull(devices);

        Devices = new ObservableCollection<ViewportEditableDevice>(
            devices.Select(d => new ViewportEditableDevice(d.Name, d.Width, d.Height, d.Enabled)));
    }

    /// <summary>
    /// Gets editable devices.
    /// </summary>
    public ObservableCollection<ViewportEditableDevice> Devices { get; }

    /// <summary>
    /// Gets or sets the new device name.
    /// </summary>
    public string NewDeviceName
    {
        get => _newDeviceName;
        set
        {
            _newDeviceName = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the new device width.
    /// </summary>
    public string NewDeviceWidth
    {
        get => _newDeviceWidth;
        set
        {
            _newDeviceWidth = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the new device height.
    /// </summary>
    public string NewDeviceHeight
    {
        get => _newDeviceHeight;
        set
        {
            _newDeviceHeight = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets validation error text for the add row.
    /// </summary>
    public string? AddError { get; private set; }

    /// <summary>
    /// Converts editor devices back into persisted device models.
    /// </summary>
    /// <returns>Persisted device models.</returns>
    public IReadOnlyList<ViewportDevice> ToDevices() => Devices
        .Select(d => new ViewportDevice
        {
            Name = d.Name,
            Width = d.Width,
            Height = d.Height,
            Enabled = d.Enabled,
        })
        .ToList();

    /// <summary>
    /// Adds a new device from the add row values.
    /// </summary>
    /// <returns>True if device was added; otherwise false.</returns>
    public bool TryAddDevice()
    {
        AddError = null;

        if (string.IsNullOrWhiteSpace(NewDeviceName))
        {
            AddError = "Name is required.";
            OnPropertyChanged(nameof(AddError));
            return false;
        }

        if (!double.TryParse(NewDeviceWidth, NumberStyles.Float, CultureInfo.InvariantCulture, out var width) || width <= 0)
        {
            AddError = "Width must be a positive number.";
            OnPropertyChanged(nameof(AddError));
            return false;
        }

        if (!double.TryParse(NewDeviceHeight, NumberStyles.Float, CultureInfo.InvariantCulture, out var height) || height <= 0)
        {
            AddError = "Height must be a positive number.";
            OnPropertyChanged(nameof(AddError));
            return false;
        }

        if (Devices.Any(d => d.Name.Equals(NewDeviceName, StringComparison.Ordinal)))
        {
            AddError = "A device with this name already exists.";
            OnPropertyChanged(nameof(AddError));
            return false;
        }

        Devices.Add(new ViewportEditableDevice(NewDeviceName.Trim(), width, height, enabled: true));

        NewDeviceName = string.Empty;
        NewDeviceWidth = string.Empty;
        NewDeviceHeight = string.Empty;
        OnPropertyChanged(nameof(AddError));

        return true;
    }

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
