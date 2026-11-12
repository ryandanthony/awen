// -----------------------------------------------------------------------
// <copyright file="AvaloniaVersionMismatchException.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Awen.Discovery;

/// <summary>
/// A story assembly was built against a different Avalonia major version than the running Awen host.
/// </summary>
public sealed class AvaloniaVersionMismatchException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AvaloniaVersionMismatchException"/> class.
    /// </summary>
    public AvaloniaVersionMismatchException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AvaloniaVersionMismatchException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public AvaloniaVersionMismatchException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AvaloniaVersionMismatchException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public AvaloniaVersionMismatchException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
