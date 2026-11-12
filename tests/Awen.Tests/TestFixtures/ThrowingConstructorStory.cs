// -----------------------------------------------------------------------
// <copyright file="ThrowingConstructorStory.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Awen.Sdk;

namespace Awen.Tests.TestFixtures;

/// <summary>
/// A test story whose constructor throws, so it can never be created.
/// </summary>
public sealed class ThrowingConstructorStory : IStory<UserControl, UserControl>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ThrowingConstructorStory"/> class.
    /// </summary>
    public ThrowingConstructorStory() => throw new InvalidOperationException("Story constructor failed.");

    /// <inheritdoc/>
    public string Name => "Never";

    /// <inheritdoc/>
    public string Group => "Broken";

    /// <inheritdoc/>
    public int Order => 0;

    /// <inheritdoc/>
    public string Description => "Never created.";

    /// <inheritdoc/>
    public UserControl CreateControl() => new();

    /// <inheritdoc/>
    public UserControl CreateProperties() => new();
}
