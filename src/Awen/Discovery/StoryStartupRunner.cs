// -----------------------------------------------------------------------
// <copyright file="StoryStartupRunner.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection;
using Avalonia;
using Awen.Sdk;

namespace Awen.Discovery;

/// <summary>
/// Runs each story library's optional <see cref="IAwenStartup"/> hook once. A bad or throwing hook is
/// reported and skipped, so one library can never stop Awen from starting.
/// </summary>
internal static class StoryStartupRunner
{
    /// <summary>
    /// Runs the startup hook of every assembly that declares one, in scan order.
    /// </summary>
    /// <param name="assemblies">Loaded story assemblies.</param>
    /// <param name="application">The running application passed to each hook.</param>
    /// <returns>One result per assembly that declares a startup hook.</returns>
    internal static IReadOnlyList<StartupResult> Run(IReadOnlyList<StoryAssemblyInfo> assemblies, Application application)
    {
        ArgumentNullException.ThrowIfNull(assemblies);
        ArgumentNullException.ThrowIfNull(application);

        var results = new List<StartupResult>();
        foreach (var assembly in assemblies)
        {
            if (assembly.StartupType is { } startupType)
            {
                results.Add(RunOne(assembly.LibraryName, startupType, application));
            }
        }

        return results;
    }

    private static StartupResult RunOne(string libraryName, Type startupType, Application application)
    {
        var typeName = startupType.FullName ?? startupType.Name;

        if (!typeof(IAwenStartup).IsAssignableFrom(startupType))
        {
            return StartupResult.Failed(libraryName, typeName, $"{typeName} does not implement {nameof(IAwenStartup)}.", exception: null);
        }

        if (startupType.IsAbstract || startupType.GetConstructor(Type.EmptyTypes) is null)
        {
            return StartupResult.Failed(libraryName, typeName, $"{typeName} needs to be a concrete class with a public parameterless constructor.", exception: null);
        }

        try
        {
            var hook = (IAwenStartup)Activator.CreateInstance(startupType)!;
            hook.OnStartup(new AwenStartupContext(libraryName, application));
            return StartupResult.Succeeded(libraryName, typeName);
        }
#pragma warning disable CA1031 // A story library's startup code must never take the host down.
        catch (Exception ex)
#pragma warning restore CA1031
        {
            var cause = ex is TargetInvocationException { InnerException: { } inner } ? inner : ex;
            return StartupResult.Failed(libraryName, typeName, $"{typeName} threw {cause.GetType().Name}: {cause.Message}", cause);
        }
    }
}
