// -----------------------------------------------------------------------
// <copyright file="KnobValueType.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Awen.ViewModels;

/// <summary>
/// Identifies the type of editor control needed for a knob.
/// </summary>
public enum KnobValueType
{
    /// <summary>A text input knob.</summary>
    Text,

    /// <summary>A boolean toggle knob.</summary>
    Bool,

    /// <summary>An enum selection knob.</summary>
    Enum,

    /// <summary>A numeric input knob.</summary>
    Numeric,
}
