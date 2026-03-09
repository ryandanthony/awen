// -----------------------------------------------------------------------
// <copyright file="BadgeVariant.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ExampleUI.Controls;

/// <summary>
/// Visual variant for the <see cref="Badge"/> control.
/// </summary>
public enum BadgeVariant
{
    /// <summary>Blue/informational.</summary>
    Primary,

    /// <summary>Green/success.</summary>
    Success,

    /// <summary>Orange/warning.</summary>
    Warning,

    /// <summary>Red/error or destructive.</summary>
    Danger,
}
