// -----------------------------------------------------------------------
// <copyright file="InfinityToSymbolConverter.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using Avalonia.Data.Converters;

namespace Awen.Converters;

/// <summary>
/// Converts <see cref="double.PositiveInfinity"/> to the ∞ symbol for display.
/// </summary>
public sealed class InfinityToSymbolConverter : IValueConverter
{
    /// <summary>
    /// Gets a shared singleton instance of the converter.
    /// </summary>
    public static InfinityToSymbolConverter Instance { get; } = new();

    /// <summary>
    /// Converts a <c>double</c> to a formatted string.
    /// <see cref="double.PositiveInfinity"/> maps to "∞".
    /// </summary>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double d)
        {
            return double.IsPositiveInfinity(d) ? "∞" : d.ToString("F0", culture);
        }

        return string.Empty;
    }

    /// <summary>
    /// Not used for this converter.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
