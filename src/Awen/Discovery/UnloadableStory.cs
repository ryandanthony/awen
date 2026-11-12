// -----------------------------------------------------------------------
// <copyright file="UnloadableStory.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.ExceptionServices;
using Avalonia.Controls;
using Awen.Sdk;

namespace Awen.Discovery;

/// <summary>
/// Stands in for a story whose type could not be created, so it still appears in the sidebar.
/// Creating its control rethrows the original load error.
/// </summary>
internal sealed class UnloadableStory : IStory<Control, Control>
{
    private readonly Exception _error;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnloadableStory"/> class.
    /// </summary>
    /// <param name="error">Why the real story could not be created.</param>
    internal UnloadableStory(Exception error) => _error = error;

    /// <inheritdoc/>
    public string Name => "Unloadable story";

    /// <inheritdoc/>
    public string Group => AssemblyScanner.LoadErrorGroup;

    /// <inheritdoc/>
    public int Order => int.MaxValue;

    /// <inheritdoc/>
    public string Description => _error.Message;

    /// <inheritdoc/>
    public Control CreateControl()
    {
        ExceptionDispatchInfo.Capture(_error).Throw();
        return null!;
    }

    /// <inheritdoc/>
    public Control CreateProperties() => new Panel();
}
