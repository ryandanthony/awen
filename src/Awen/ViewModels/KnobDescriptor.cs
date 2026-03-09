// -----------------------------------------------------------------------
// <copyright file="KnobDescriptor.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Awen.Sdk;
using Awen.Sdk.Knobs;

namespace Awen.ViewModels;

/// <summary>
/// A typed wrapper around <see cref="IKnob"/> that adds editor metadata for the property panel.
/// </summary>
public sealed class KnobDescriptor : INotifyPropertyChanged
{
    private readonly IKnob _knob;
    private object? _currentValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="KnobDescriptor"/> class.
    /// </summary>
    /// <param name="knob">The underlying knob instance.</param>
    public KnobDescriptor(IKnob knob)
    {
        ArgumentNullException.ThrowIfNull(knob);

        _knob = knob;
        Label = knob.Label;
        ValueType = ResolveValueType(knob);

        // For enum knobs, store the matching reference from Options for proper ComboBox binding
        _currentValue = knob is EnumKnob enumKnob && knob.Value is Enum enumValue
            ? enumKnob.Options.FirstOrDefault(o => o.Equals(enumValue)) ?? knob.Value
            : knob.Value;
    }

    /// <summary>
    /// Gets the display label for this knob.
    /// </summary>
    public string Label { get; }

    /// <summary>
    /// Gets the value type for editor resolution.
    /// </summary>
    public KnobValueType ValueType { get; }

    /// <summary>
    /// Gets the underlying knob instance.
    /// </summary>
    public IKnob Knob => _knob;

    /// <summary>
    /// Gets or sets the current value. Setting dispatches to <see cref="IKnob.SetValue"/>.
    /// </summary>
    public object? CurrentValue
    {
        get => _currentValue;

        set
        {
            // For enum knobs, ensure we store the reference from Options for proper ComboBox binding
            var newValue = value;
            if (_knob is EnumKnob enumKnob && value is Enum enumValue)
            {
                newValue = enumKnob.Options.FirstOrDefault(o => o.Equals(enumValue)) ?? value;
            }

            if (Equals(_currentValue, newValue))
            {
                return;
            }

            _currentValue = newValue;
            _knob.SetValue(newValue);
            OnPropertyChanged();
            OnPropertyChanged(nameof(SelectedEnumIndex));
        }
    }

    /// <summary>
    /// Gets the enum options if this is an EnumKnob, otherwise null.
    /// </summary>
    public IReadOnlyList<Enum>? EnumOptions => (_knob as EnumKnob)?.Options;

    /// <summary>
    /// Gets or sets the boolean value for ToggleSwitch binding.
    /// Provides strongly-typed <see cref="bool"/> access for <see cref="Sdk.Knobs.BoolKnob"/>.
    /// </summary>
    public bool BoolValue
    {
        get => _currentValue is true;

        set
        {
            if (_currentValue is bool current && current == value)
            {
                return;
            }

            _currentValue = value;
            _knob.SetValue(value);
            OnPropertyChanged();
            OnPropertyChanged(nameof(CurrentValue));
        }
    }

    /// <summary>
    /// Gets or sets the selected enum index for ComboBox binding.
    /// </summary>
    public int SelectedEnumIndex
    {
        get
        {
            if (_knob is not EnumKnob enumKnob || _currentValue is not Enum enumValue)
            {
                return -1;
            }

            for (var i = 0; i < enumKnob.Options.Count; i++)
            {
                if (enumKnob.Options[i].Equals(enumValue))
                {
                    return i;
                }
            }

            return -1;
        }

        set
        {
            if (_knob is not EnumKnob enumKnob || value < 0 || value >= enumKnob.Options.Count)
            {
                return;
            }

            var newValue = enumKnob.Options[value];
            if (Equals(_currentValue, newValue))
            {
                return;
            }

            _currentValue = newValue;
            _knob.SetValue(newValue);
            OnPropertyChanged();
            OnPropertyChanged(nameof(CurrentValue));
        }
    }

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    private static KnobValueType ResolveValueType(IKnob knob)
    {
        return knob switch
        {
            TextKnob => KnobValueType.Text,
            BoolKnob => KnobValueType.Bool,
            EnumKnob => KnobValueType.Enum,
            NumericKnob => KnobValueType.Numeric,
            _ => KnobValueType.Text,
        };
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
