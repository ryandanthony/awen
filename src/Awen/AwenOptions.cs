// -----------------------------------------------------------------------
// <copyright file="AwenOptions.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Awen;

/// <summary>
/// Parsed CLI arguments, immutable after construction. Single instance per process.
/// </summary>
public sealed record AwenOptions
{
    /// <summary>
    /// Gets the target directory to scan for story assemblies.
    /// </summary>
    public required DirectoryInfo Dir { get; init; }

    /// <summary>
    /// Gets a value indicating whether hot-reload is enabled (default: true).
    /// </summary>
    public bool Watch { get; init; } = true;

    /// <summary>
    /// Gets the initial theme variant name ("light" or "dark").
    /// </summary>
    public string? Theme { get; init; }

    /// <summary>
    /// Gets the initial story filter pattern.
    /// </summary>
    public string? Filter { get; init; }

    /// <summary>
    /// Gets the path to hot-reload state file for session restoration.
    /// </summary>
    public string? RestoreFile { get; init; }
}
