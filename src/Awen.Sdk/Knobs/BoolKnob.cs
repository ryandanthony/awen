// -----------------------------------------------------------------------
// <copyright file="BoolKnob.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Awen.Sdk.Knobs;

/// <summary>
/// A knob that edits a boolean value via a ToggleSwitch.
/// </summary>
public sealed class BoolKnob : IKnob
{
    private readonly Action<bool> _onChange;
    private bool _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="BoolKnob"/> class.
    /// </summary>
    /// <param name="label">Display label for the property editor.</param>
    /// <param name="initialValue">The initial boolean value.</param>
    /// <param name="onChange">Callback invoked when the value changes.</param>
    public BoolKnob(string label, bool initialValue, Action<bool> onChange)
    {
        ArgumentNullException.ThrowIfNull(label);
        ArgumentNullException.ThrowIfNull(onChange);

        Label = label;
        _value = initialValue;
        _onChange = onChange;
    }

    /// <inheritdoc/>
    public string Label { get; }

    /// <inheritdoc/>
    public object? Value => _value;

    /// <inheritdoc/>
    public void SetValue(object? value)
    {
        var boolValue = value is bool b && b;
        _value = boolValue;
        _onChange(boolValue);
    }
}
