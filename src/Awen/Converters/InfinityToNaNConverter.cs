// -----------------------------------------------------------------------
// <copyright file="InfinityToNaNConverter.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using Avalonia.Data.Converters;

namespace Awen.Converters;

/// <summary>
/// Converts viewport dimension values for use in Avalonia Width/Height bindings.
/// Maps <see cref="double.PositiveInfinity"/> (unconstrained) to <see cref="double.NaN"/> (auto-size).
/// Passes through finite numbers unchanged.
/// </summary>
public sealed class InfinityToNaNConverter : IValueConverter
{
    /// <summary>
    /// Gets a shared singleton instance of the converter.
    /// </summary>
    public static InfinityToNaNConverter Instance { get; } = new();

    /// <summary>
    /// Converts a viewport dimension for binding to Width or Height.
    /// <see cref="double.PositiveInfinity"/> maps to <see cref="double.NaN"/> (auto-size).
    /// Finite values pass through unchanged (e.g., 375 for Phone width).
    /// </summary>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double d)
        {
            return double.IsPositiveInfinity(d) ? double.NaN : d;
        }

        return double.NaN;
    }

    /// <summary>
    /// Not used for viewport dimensions (one-way binding).
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
