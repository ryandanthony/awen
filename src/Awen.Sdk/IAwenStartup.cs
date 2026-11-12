// -----------------------------------------------------------------------
// <copyright file="IAwenStartup.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Awen.Sdk;

/// <summary>
/// Optional startup hook for a story library. Register the implementing type with
/// <see cref="AwenStoryAssemblyAttribute.StartupType"/> and Awen calls <see cref="OnStartup"/> once,
/// on the UI thread, after the Avalonia application is set up and before any story is rendered.
/// Use it for app-level setup the library's own app normally does at startup, such as registering
/// embedded fonts or adding application styles and resources.
/// </summary>
/// <remarks>
/// The implementing type needs a public parameterless constructor. If the constructor or
/// <see cref="OnStartup"/> throws, Awen logs the error and keeps running.
/// </remarks>
public interface IAwenStartup
{
    /// <summary>
    /// Called once when Awen starts, before any story from this library is rendered.
    /// </summary>
    /// <param name="context">Access to the running application and its font manager.</param>
    void OnStartup(AwenStartupContext context);
}
