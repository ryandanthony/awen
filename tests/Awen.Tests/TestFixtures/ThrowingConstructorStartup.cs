// -----------------------------------------------------------------------
// <copyright file="ThrowingConstructorStartup.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Awen.Sdk;

namespace Awen.Tests.TestFixtures;

/// <summary>
/// Startup hook whose constructor throws.
/// </summary>
public sealed class ThrowingConstructorStartup : IAwenStartup
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ThrowingConstructorStartup"/> class.
    /// </summary>
    public ThrowingConstructorStartup() => throw new InvalidOperationException("Constructor failed.");

    /// <inheritdoc/>
    public void OnStartup(AwenStartupContext context)
    {
        // Never reached: the constructor throws.
    }
}
