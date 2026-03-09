// -----------------------------------------------------------------------
// <copyright file="KnobValueTypeConverters.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using Avalonia.Data.Converters;

namespace Awen.ViewModels;

/// <summary>
/// Static value converters to map <see cref="KnobValueType"/> to visibility booleans.
/// </summary>
public static class KnobValueTypeConverters
{
    /// <summary>
    /// Gets a converter that returns true when the value is <see cref="KnobValueType.Text"/>.
    /// </summary>
    public static IValueConverter IsText { get; } = new KnobTypeConverter(KnobValueType.Text);

    /// <summary>
    /// Gets a converter that returns true when the value is <see cref="KnobValueType.Bool"/>.
    /// </summary>
    public static IValueConverter IsBool { get; } = new KnobTypeConverter(KnobValueType.Bool);

    /// <summary>
    /// Gets a converter that returns true when the value is <see cref="KnobValueType.Enum"/>.
    /// </summary>
    public static IValueConverter IsEnum { get; } = new KnobTypeConverter(KnobValueType.Enum);

    /// <summary>
    /// Gets a converter that returns true when the value is <see cref="KnobValueType.Numeric"/>.
    /// </summary>
    public static IValueConverter IsNumeric { get; } = new KnobTypeConverter(KnobValueType.Numeric);

    private sealed class KnobTypeConverter : IValueConverter
    {
        private readonly KnobValueType _targetType;

        public KnobTypeConverter(KnobValueType targetType)
        {
            _targetType = targetType;
        }

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is KnobValueType knobType && knobType == _targetType;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
