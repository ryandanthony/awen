// -----------------------------------------------------------------------
// <copyright file="StoryAssemblyLoadContext.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection;
using System.Runtime.Loader;

namespace Awen.Discovery;

/// <summary>
/// Non-collectible AssemblyLoadContext with shared-assembly fallthrough for Avalonia, Awen.Sdk, and System assemblies.
/// </summary>
public sealed class StoryAssemblyLoadContext : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver _resolver;

    /// <summary>
    /// Initializes a new instance of the <see cref="StoryAssemblyLoadContext"/> class.
    /// </summary>
    /// <param name="assemblyPath">Path to the root assembly to resolve dependencies for.</param>
    public StoryAssemblyLoadContext(string assemblyPath)
        : base(isCollectible: false)
    {
        _resolver = new AssemblyDependencyResolver(assemblyPath);
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

    private static bool IsSharedAssembly(AssemblyName assemblyName)
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
}
