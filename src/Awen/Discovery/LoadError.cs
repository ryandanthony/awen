// -----------------------------------------------------------------------
// <copyright file="LoadError.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Awen.Discovery;

/// <summary>
/// Captures an assembly load failure for the log panel.
/// </summary>
public sealed record LoadError
{
    /// <summary>
    /// Gets the path to the DLL that failed to load.
    /// </summary>
    public required string FilePath { get; init; }

    /// <summary>
    /// Gets the exception message.
    /// </summary>
    public required string ErrorMessage { get; init; }

    /// <summary>
    /// Gets the full exception for logging.
    /// </summary>
    public required Exception Exception { get; init; }

    /// <summary>
    /// Gets when the error occurred.
    /// </summary>
    public required DateTimeOffset Timestamp { get; init; }
}
