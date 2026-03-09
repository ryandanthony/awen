// -----------------------------------------------------------------------
// <copyright file="ThemeLoader.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection;
using Avalonia.Markup.Xaml;

namespace Awen.Discovery;

/// <summary>
/// Loads custom theme ResourceDictionary from embedded assembly resources.
/// </summary>
public static class ThemeLoader
{
    /// <summary>
    /// Loads a theme ResourceDictionary from an assembly's embedded resource.
    /// </summary>
    /// <param name="assembly">The assembly containing the resource.</param>
    /// <param name="resourcePath">The manifest resource stream name.</param>
    /// <returns>The loaded ResourceDictionary, or null if loading failed.</returns>
    public static Avalonia.Controls.ResourceDictionary? LoadFromAssembly(Assembly assembly, string? resourcePath)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        if (string.IsNullOrEmpty(resourcePath))
        {
            return null;
        }

        using var stream = assembly.GetManifestResourceStream(resourcePath);
        if (stream is null)
        {
            return null;
        }

#pragma warning disable CA1031 // Catch all for resilient theme loading
        try
        {
            return AvaloniaRuntimeXamlLoader.Load(stream) as Avalonia.Controls.ResourceDictionary;
        }
        catch (Exception)
        {
            return null;
        }
#pragma warning restore CA1031
    }
}
