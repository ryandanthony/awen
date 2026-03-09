// -----------------------------------------------------------------------
// <copyright file="NumericKnob.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Awen.Sdk.Knobs;

/// <summary>
/// A knob that edits a numeric (double) value via a NumericUpDown.
/// </summary>
public sealed class NumericKnob : IKnob
{
    private readonly Action<double> _onChange;
    private double _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="NumericKnob"/> class.
    /// </summary>
    /// <param name="label">Display label for the property editor.</param>
    /// <param name="initialValue">The initial numeric value.</param>
    /// <param name="onChange">Callback invoked when the value changes.</param>
    /// <param name="minimum">Minimum allowed value.</param>
    /// <param name="maximum">Maximum allowed value.</param>
    /// <param name="step">Increment step for the NumericUpDown control.</param>
    public NumericKnob(
        string label,
        double initialValue,
        Action<double> onChange,
        double minimum = double.MinValue,
        double maximum = double.MaxValue,
        double step = 1.0)
    {
        ArgumentNullException.ThrowIfNull(label);
        ArgumentNullException.ThrowIfNull(onChange);

        Label = label;
        _value = initialValue;
        _onChange = onChange;
        Minimum = minimum;
        Maximum = maximum;
        Step = step;
    }

    /// <inheritdoc/>
    public string Label { get; }

    /// <inheritdoc/>
    public object? Value => _value;

    /// <summary>
    /// Gets the minimum allowed value.
    /// </summary>
    public double Minimum { get; }

    /// <summary>
    /// Gets the maximum allowed value.
    /// </summary>
    public double Maximum { get; }

    /// <summary>
    /// Gets the increment step for the NumericUpDown control.
    /// </summary>
    public double Step { get; }

    /// <inheritdoc/>
    public void SetValue(object? value)
    {
        var numericValue = value switch
        {
            double d => d,
            int i => (double)i,
            float f => (double)f,
            decimal dec => (double)dec,
            _ => _value,
        };

        numericValue = Math.Clamp(numericValue, Minimum, Maximum);
        _value = numericValue;
        _onChange(numericValue);
    }
}
