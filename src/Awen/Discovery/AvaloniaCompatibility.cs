// -----------------------------------------------------------------------
// <copyright file="AvaloniaCompatibility.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using Avalonia;

namespace Awen.Discovery;

/// <summary>
/// Story assemblies share the host's Avalonia, so they must be built against the same Avalonia major version.
/// Awen 1.x runs Avalonia 11, Awen 2.x runs Avalonia 12.
/// </summary>
public static class AvaloniaCompatibility
{
    /// <summary>
    /// Oldest Avalonia major version with an Awen host (Awen 1.x).
    /// </summary>
    private const int MinSupportedAvaloniaMajor = 11;

    /// <summary>
    /// Newest Avalonia major version with an Awen host (Awen 2.x).
    /// </summary>
    private const int MaxSupportedAvaloniaMajor = 12;

    /// <summary>
    /// Awen major = Avalonia major minus this offset (11 to 1, 12 to 2).
    /// </summary>
    private const int AwenMajorOffset = 10;

    /// <summary>
    /// Gets the Avalonia version this Awen host runs on.
    /// </summary>
    public static Version HostAvaloniaVersion { get; } =
        typeof(AvaloniaObject).Assembly.GetName().Version ?? new Version(0, 0, 0, 0);

    /// <summary>
    /// Gets the Awen major version that hosts the given Avalonia major version.
    /// </summary>
    /// <param name="avaloniaMajor">Avalonia major version.</param>
    /// <returns>The Awen major version, or <see langword="null"/> if that Avalonia version is not supported.</returns>
    public static int? AwenMajorFor(int avaloniaMajor) =>
        avaloniaMajor is >= MinSupportedAvaloniaMajor and <= MaxSupportedAvaloniaMajor
            ? avaloniaMajor - AwenMajorOffset
            : null;

    /// <summary>
    /// Describes why a story assembly cannot run in this host.
    /// </summary>
    /// <param name="fileName">Story assembly file name, for the message.</param>
    /// <param name="storyAvaloniaVersion">Avalonia version the story assembly was compiled against.</param>
    /// <param name="hostAvaloniaVersion">Avalonia version the host runs on.</param>
    /// <returns><see langword="null"/> when compatible; otherwise a message explaining the mismatch and how to fix it.</returns>
    public static string? GetMismatchMessage(string fileName, Version storyAvaloniaVersion, Version hostAvaloniaVersion)
    {
        ArgumentNullException.ThrowIfNull(storyAvaloniaVersion);
        ArgumentNullException.ThrowIfNull(hostAvaloniaVersion);

        if (storyAvaloniaVersion.Major == hostAvaloniaVersion.Major)
        {
            return null;
        }

        var story = Format(storyAvaloniaVersion);
        var host = Format(hostAvaloniaVersion);
        var advice = AwenMajorFor(storyAvaloniaVersion.Major) is { } awenMajor
            ? string.Create(
                CultureInfo.InvariantCulture,
                $"Use Awen {awenMajor}.x for Avalonia {storyAvaloniaVersion.Major} stories, e.g. dotnet tool update awen --version \"{awenMajor}.*\" (add -g for a global install).")
            : string.Create(
                CultureInfo.InvariantCulture,
                $"Avalonia {storyAvaloniaVersion.Major} is not supported. Awen 1.x supports Avalonia {MinSupportedAvaloniaMajor} and Awen 2.x supports Avalonia {MaxSupportedAvaloniaMajor}.");

        return string.Create(
            CultureInfo.InvariantCulture,
            $"{fileName} was built against Avalonia {story}, but this Awen runs Avalonia {host}. {advice}");
    }

    private static string Format(Version version) =>
        string.Create(CultureInfo.InvariantCulture, $"{version.Major}.{version.Minor}.{Math.Max(version.Build, 0)}");
}
