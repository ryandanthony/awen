// -----------------------------------------------------------------------
// <copyright file="StoryAssemblyLoadContext.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Loader;

namespace Awen.Discovery;

/// <summary>
/// Non-collectible AssemblyLoadContext with shared-assembly fallthrough for Avalonia, Awen.Sdk, and System assemblies.
/// </summary>
/// <remarks>
/// Shared assemblies are loaded once, into the default context, so story code and the host agree on
/// Avalonia types. A story can depend on a shared-named assembly the host does not ship (for example
/// <c>Avalonia.Fonts.Inter</c>). The default context cannot find those on its own, and Avalonia resolves
/// <c>avares://</c> URIs with <see cref="Assembly.Load(AssemblyName)"/> from the default context. So
/// missing shared assemblies are resolved from the story directories and loaded into the default context.
/// </remarks>
public sealed class StoryAssemblyLoadContext : AssemblyLoadContext
{
    private static readonly ConcurrentQueue<AssemblyDependencyResolver> StoryResolvers = new();
    private static readonly Lock DefaultResolvingLock = new();
    private static bool _defaultResolvingRegistered;

    private readonly AssemblyDependencyResolver _resolver;

    /// <summary>
    /// Initializes a new instance of the <see cref="StoryAssemblyLoadContext"/> class.
    /// </summary>
    /// <param name="assemblyPath">Path to the root assembly to resolve dependencies for.</param>
    public StoryAssemblyLoadContext(string assemblyPath)
        : base(isCollectible: false)
    {
        _resolver = new AssemblyDependencyResolver(assemblyPath);
        StoryResolvers.Enqueue(_resolver);
        EnsureDefaultResolvingRegistered();
        Resolving += (_, assemblyName) => ResolveSharedFromStories(Default, assemblyName);
    }

    /// <summary>
    /// Returns whether <paramref name="assemblyName"/> is shared with the host (loaded into the default context).
    /// </summary>
    /// <param name="assemblyName">The assembly name to check.</param>
    /// <returns><see langword="true"/> for Avalonia, Awen.Sdk, System, Microsoft, and netstandard assemblies.</returns>
    internal static bool IsSharedAssembly(AssemblyName assemblyName)
    {
        var name = assemblyName.Name;
        if (name is null)
        {
            return false;
        }

        if (name.StartsWith("Avalonia", StringComparison.Ordinal)
            || name.StartsWith("Awen.Sdk", StringComparison.Ordinal))
        {
            return true;
        }

        return name.StartsWith("System", StringComparison.Ordinal)
            || name.StartsWith("Microsoft", StringComparison.Ordinal)
            || name.StartsWith("netstandard", StringComparison.Ordinal);
    }

    /// <inheritdoc/>
    protected override Assembly? Load(AssemblyName assemblyName)
    {
        ArgumentNullException.ThrowIfNull(assemblyName);

        if (IsSharedAssembly(assemblyName))
        {
            return null; // Falls through to Default ALC
        }

        var path = _resolver.ResolveAssemblyToPath(assemblyName);
        return path is not null ? LoadFromAssemblyPath(path) : null;
    }

    private static void EnsureDefaultResolvingRegistered()
    {
        lock (DefaultResolvingLock)
        {
            if (_defaultResolvingRegistered)
            {
                return;
            }

            Default.Resolving += ResolveSharedFromStories;
            _defaultResolvingRegistered = true;
        }
    }

    /// <summary>
    /// Loads a shared assembly the host does not ship from a story directory, into the default context.
    /// Non-shared assemblies are left to their story context, so their types are never duplicated.
    /// </summary>
    private static Assembly? ResolveSharedFromStories(AssemblyLoadContext context, AssemblyName assemblyName)
    {
        if (!IsSharedAssembly(assemblyName))
        {
            return null;
        }

        foreach (var resolver in StoryResolvers)
        {
            var path = resolver.ResolveAssemblyToPath(assemblyName);
            if (path is not null)
            {
                return Default.LoadFromAssemblyPath(path);
            }
        }

        return null;
    }
}
