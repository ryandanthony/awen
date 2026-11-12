// -----------------------------------------------------------------------
// <copyright file="NoDefaultConstructorStartup.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Awen.Sdk;

namespace Awen.Tests.TestFixtures;

/// <summary>
/// Startup hook without a parameterless constructor, so the host cannot create it.
/// </summary>
public sealed class NoDefaultConstructorStartup : IAwenStartup
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NoDefaultConstructorStartup"/> class.
    /// </summary>
    /// <param name="name">Unused.</param>
    public NoDefaultConstructorStartup(string name) => Name = name;

    /// <summary>
    /// Gets the name passed to the constructor.
    /// </summary>
    public string Name { get; }

    /// <inheritdoc/>
    public void OnStartup(AwenStartupContext context)
    {
        // Never reached: the host cannot construct this type.
    }
}
