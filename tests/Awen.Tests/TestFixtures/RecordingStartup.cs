// -----------------------------------------------------------------------
// <copyright file="RecordingStartup.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Awen.Sdk;

namespace Awen.Tests.TestFixtures;

/// <summary>
/// Startup hook that records the call on the application's resources, so each test can check its own app.
/// </summary>
public sealed class RecordingStartup : IAwenStartup
{
    /// <summary>
    /// Gets the resource key holding the library names this hook was called for.
    /// </summary>
    public static string CallsKey => "RecordingStartup.Calls";

    /// <summary>
    /// Returns the library names the hook was called for on <paramref name="application"/>.
    /// </summary>
    /// <param name="application">The application passed to the hook.</param>
    /// <returns>Library names, in call order.</returns>
    public static IReadOnlyList<string> CallsOn(Application application)
    {
        ArgumentNullException.ThrowIfNull(application);
        return application.Resources.TryGetValue(CallsKey, out var calls) && calls is List<string> list ? list : [];
    }

    /// <inheritdoc/>
    public void OnStartup(AwenStartupContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (!context.Application.Resources.TryGetValue(CallsKey, out var calls) || calls is not List<string> list)
        {
            list = [];
            context.Application.Resources[CallsKey] = list;
        }

        list.Add(context.LibraryName);
    }
}
