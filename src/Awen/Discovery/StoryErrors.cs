// -----------------------------------------------------------------------
// <copyright file="StoryErrors.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using System.Reflection;

namespace Awen.Discovery;

/// <summary>
/// Helpers for presenting exceptions thrown by story code.
/// </summary>
internal static class StoryErrors
{
    /// <summary>
    /// Strips reflection and aggregate wrappers so the story's own exception is reported.
    /// </summary>
    /// <param name="exception">The caught exception.</param>
    /// <returns>The innermost meaningful exception.</returns>
    internal static Exception Unwrap(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        var current = exception;
        while (Inner(current) is { } inner)
        {
            current = inner;
        }

        return current;
    }

    private static Exception? Inner(Exception exception) => exception switch
    {
        TargetInvocationException { InnerException: { } inner } => inner,
        TypeInitializationException { InnerException: { } inner } => inner,
        AggregateException { InnerExceptions.Count: 1 } aggregate => aggregate.InnerExceptions[0],
        _ => null,
    };

    /// <summary>
    /// Formats a one-line summary such as <c>InvalidOperationException: Something broke</c>.
    /// </summary>
    /// <param name="exception">The exception to describe.</param>
    /// <returns>The exception type name and message.</returns>
    internal static string Summarize(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);
        return string.Create(CultureInfo.InvariantCulture, $"{exception.GetType().Name}: {exception.Message}");
    }
}
