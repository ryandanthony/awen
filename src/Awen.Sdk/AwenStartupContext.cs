// -----------------------------------------------------------------------
// <copyright file="AwenStartupContext.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Media;

namespace Awen.Sdk;

/// <summary>
/// What a story library's <see cref="IAwenStartup"/> hook can configure. Created by the Awen host.
/// </summary>
public sealed class AwenStartupContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AwenStartupContext"/> class.
    /// </summary>
    /// <param name="libraryName">The story library's display name.</param>
    /// <param name="application">The running Avalonia application.</param>
    public AwenStartupContext(string libraryName, Application application)
    {
        ArgumentNullException.ThrowIfNull(libraryName);
        ArgumentNullException.ThrowIfNull(application);

        LibraryName = libraryName;
        Application = application;
    }

    /// <summary>
    /// Gets the display name of the story library being started, from <see cref="AwenStoryAssemblyAttribute.LibraryName"/>.
    /// </summary>
    public string LibraryName { get; }

    /// <summary>
    /// Gets the running Avalonia application. Use it to add application-wide
    /// <see cref="Application.Styles"/> or <see cref="Application.Resources"/>.
    /// These apply to the Awen window as well as the previews.
    /// </summary>
    public Application Application { get; }

    /// <summary>
    /// Gets the application's font manager. Register embedded fonts with
    /// <see cref="FontManager.AddFontCollection"/>, for example
    /// <c>context.Fonts.AddFontCollection(new EmbeddedFontCollection(new Uri("fonts:MyLib"), new Uri("avares://MyLib.Stories/Assets/Fonts")))</c>,
    /// then use <c>FontFamily="fonts:MyLib#Family Name"</c> in XAML.
    /// </summary>
    public FontManager Fonts => FontManager.Current;
}
