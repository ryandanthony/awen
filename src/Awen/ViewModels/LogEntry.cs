// -----------------------------------------------------------------------
// <copyright file="LogEntry.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Awen.ViewModels;

/// <summary>
/// A single log message displayed in the in-app log panel.
/// </summary>
public sealed record LogEntry
{
    /// <summary>
    /// Gets when the event occurred.
    /// </summary>
    public required DateTimeOffset Timestamp { get; init; }

    /// <summary>
    /// Gets the log severity level.
    /// </summary>
    public required LogLevel Level { get; init; }

    /// <summary>
    /// Gets the source category (e.g., "Discovery", "HotReload", "Preview").
    /// </summary>
    public required string Category { get; init; }

    /// <summary>
    /// Gets the human-readable log message.
    /// </summary>
    public required string Message { get; init; }
}
