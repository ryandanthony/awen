// -----------------------------------------------------------------------
// <copyright file="HeadlessSessionFixture.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Headless;

namespace Awen.Tests;

/// <summary>
/// One headless Avalonia session (its own UI thread, with the real Awen <see cref="App"/>) shared by a test class.
/// Renders with Skia so tests can check real pixels in captured frames.
/// </summary>
public sealed class HeadlessSessionFixture : IDisposable
{
    /// <summary>
    /// Gets the session.
    /// </summary>
    public HeadlessUnitTestSession Session { get; } = HeadlessUnitTestSession.StartNew(typeof(HeadlessSessionFixture));

    /// <summary>
    /// App builder used by the headless session (found by name through reflection).
    /// </summary>
    /// <returns>The configured builder.</returns>
    public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>()
        .UseSkia()
        .UseHeadless(new AvaloniaHeadlessPlatformOptions { UseHeadlessDrawing = false });

    /// <inheritdoc/>
    public void Dispose() => Session.Dispose();
}
