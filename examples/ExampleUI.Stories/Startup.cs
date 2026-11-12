// -----------------------------------------------------------------------
// <copyright file="Startup.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Media.Fonts;
using Awen.Sdk;

namespace ExampleUI.Stories;

/// <summary>
/// Startup hook for the example story library, registered on <see cref="AwenStoryAssemblyAttribute.StartupType"/>.
/// Registers the embedded Noto Mono font so stories can use <c>FontFamily="fonts:ExampleUI#Noto Mono"</c>.
/// </summary>
public sealed class Startup : IAwenStartup
{
    /// <summary>
    /// Gets the font collection key used in XAML, as in <c>FontFamily="fonts:ExampleUI#Noto Mono"</c>.
    /// </summary>
    public static Uri FontCollectionKey { get; } = new("fonts:ExampleUI");

    /// <summary>
    /// Gets the embedded resource folder holding the font files (see the AvaloniaResource item in the csproj).
    /// </summary>
    public static Uri FontSource { get; } = new("avares://ExampleUI.Stories/Assets/Fonts");

    /// <inheritdoc/>
    public void OnStartup(AwenStartupContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        // The font manager owns the collection for the lifetime of the app.
#pragma warning disable CA2000
        context.Fonts.AddFontCollection(new EmbeddedFontCollection(FontCollectionKey, FontSource));
#pragma warning restore CA2000
    }
}
