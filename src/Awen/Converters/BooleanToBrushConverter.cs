// -----------------------------------------------------------------------
// <copyright file="BooleanToBrushConverter.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Awen.Converters;

/// <summary>
/// Converts a boolean to a brush. Returns the TrueValue brush when true, FalseValue when false.
/// Use ConverterParameter to specify the true brush by resource key name.
/// </summary>
public sealed class BooleanToBrushConverter : IValueConverter
{
    /// <summary>
    /// Gets or sets the brush to use when the value is true.
    /// </summary>
    public IBrush? TrueBrush { get; set; }

    /// <summary>
    /// Gets or sets the brush to use when the value is false.
    /// </summary>
    public IBrush? FalseBrush { get; set; }

    /// <summary>
    /// Converts a boolean to a brush.
    /// </summary>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b)
        {
            return b ? TrueBrush : FalseBrush;
        }

        return FalseBrush;
    }

    /// <summary>
    /// Not supported.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
