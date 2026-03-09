// -----------------------------------------------------------------------
// <copyright file="TextKnob.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Awen.Sdk.Knobs;

/// <summary>
/// A knob that edits a string value via a TextBox.
/// </summary>
public sealed class TextKnob : IKnob
{
    private readonly Action<string> _onChange;
    private string _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="TextKnob"/> class.
    /// </summary>
    /// <param name="label">Display label for the property editor.</param>
    /// <param name="initialValue">The initial string value.</param>
    /// <param name="onChange">Callback invoked when the value changes.</param>
    public TextKnob(string label, string initialValue, Action<string> onChange)
    {
        ArgumentNullException.ThrowIfNull(label);
        ArgumentNullException.ThrowIfNull(initialValue);
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
        var stringValue = value as string ?? string.Empty;
        _value = stringValue;
        _onChange(stringValue);
    }
}
