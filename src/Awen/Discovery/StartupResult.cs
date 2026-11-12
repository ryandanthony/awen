// -----------------------------------------------------------------------
// <copyright file="StartupResult.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Awen.Discovery;

/// <summary>
/// Outcome of running one story library's startup hook.
/// </summary>
/// <param name="LibraryName">The story library's display name.</param>
/// <param name="StartupTypeName">Full name of the startup hook type.</param>
/// <param name="Error">Why the hook failed, or <see langword="null"/> when it ran.</param>
/// <param name="Exception">The exception the hook threw, when there is one.</param>
internal sealed record StartupResult(string LibraryName, string StartupTypeName, string? Error, Exception? Exception)
{
    /// <summary>
    /// Gets a value indicating whether the hook ran without error.
    /// </summary>
    internal bool Success => Error is null;

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    internal static StartupResult Succeeded(string libraryName, string typeName) => new(libraryName, typeName, null, null);

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    internal static StartupResult Failed(string libraryName, string typeName, string error, Exception? exception) =>
        new(libraryName, typeName, error, exception);
}
