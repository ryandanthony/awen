// -----------------------------------------------------------------------
// <copyright file="InfinityToNullConverter.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using Avalonia.Data.Converters;

namespace Awen.Converters;

/// <summary>
/// Converts between <see cref="double.PositiveInfinity"/> (unconstrained viewport)
/// and <c>null</c> (empty NumericUpDown). Handles the <c>double</c> ↔ <c>decimal?</c>
/// mismatch between the ViewModel and Avalonia's <c>NumericUpDown.Value</c>.
/// </summary>
public sealed class InfinityToNullConverter : IValueConverter
{
    /// <summary>
    /// Gets a shared singleton instance of the converter.
    /// </summary>
    public static InfinityToNullConverter Instance { get; } = new();

    /// <summary>
    /// Converts a <c>double</c> to <c>decimal?</c> for the NumericUpDown.
    /// <see cref="double.PositiveInfinity"/> maps to <c>null</c> (empty field).
    /// </summary>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double d)
        {
            return double.IsPositiveInfinity(d) ? null : (decimal?)((decimal)d);
        }

        return null;
    }

    /// <summary>
    /// Converts a <c>decimal?</c> from the NumericUpDown back to <c>double</c>.
    /// <c>null</c> maps to <see cref="double.PositiveInfinity"/> (unconstrained).
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is decimal m)
        {
            return (double)m;
        }

        return double.PositiveInfinity;
    }
}
