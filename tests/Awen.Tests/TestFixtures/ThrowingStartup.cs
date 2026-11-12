// -----------------------------------------------------------------------
// <copyright file="ThrowingStartup.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Awen.Sdk;

namespace Awen.Tests.TestFixtures;

/// <summary>
/// Startup hook whose <see cref="OnStartup"/> throws.
/// </summary>
public sealed class ThrowingStartup : IAwenStartup
{
    /// <inheritdoc/>
    public void OnStartup(AwenStartupContext context) => throw new InvalidOperationException("Font file missing.");
}
