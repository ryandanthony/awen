// -----------------------------------------------------------------------
// <copyright file="EnumKnob.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Awen.Sdk.Knobs;

/// <summary>
/// A knob that edits an enum value via a ComboBox.
/// </summary>
public sealed class EnumKnob : IKnob
{
    private readonly Action<Enum> _onChange;
    private Enum _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnumKnob"/> class.
    /// </summary>
    /// <param name="label">Display label for the property editor.</param>
    /// <param name="initialValue">The initial enum value.</param>
    /// <param name="onChange">Callback invoked when the value changes.</param>
    public EnumKnob(string label, Enum initialValue, Action<Enum> onChange)
    {
        ArgumentNullException.ThrowIfNull(label);
        ArgumentNullException.ThrowIfNull(initialValue);
        ArgumentNullException.ThrowIfNull(onChange);

        Label = label;
        _value = initialValue;
        _onChange = onChange;
        Options = Enum.GetValues(initialValue.GetType()).Cast<Enum>().ToList().AsReadOnly();
    }

    /// <inheritdoc/>
    public string Label { get; }

    /// <inheritdoc/>
    public object? Value => _value;

    /// <summary>
    /// Gets all possible enum values. Used to populate the ComboBox.
    /// </summary>
    public IReadOnlyList<Enum> Options { get; }

    /// <inheritdoc/>
    public void SetValue(object? value)
    {
        if (value is Enum enumValue)
        {
            _value = enumValue;
            _onChange(enumValue);
        }
    }
}
