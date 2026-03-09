// -----------------------------------------------------------------------
// <copyright file="AlertSeverity.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ExampleUI.Controls;

/// <summary>
/// Severity levels for the <see cref="AlertBanner"/> control.
/// </summary>
public enum AlertSeverity
{
    /// <summary>
    /// Informational alert.
    /// </summary>
    Info,

    /// <summary>
    /// Success alert.
    /// </summary>
    Success,

    /// <summary>
    /// Warning alert.
    /// </summary>
    Warning,

    /// <summary>
    /// Error alert.
    /// </summary>
    Error,
}
