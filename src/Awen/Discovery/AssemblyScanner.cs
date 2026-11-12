// -----------------------------------------------------------------------
// <copyright file="AssemblyScanner.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection;
using Avalonia.Controls;
using Awen.Sdk;

namespace Awen.Discovery;

/// <summary>
/// Scans a directory for .NET assemblies marked with <see cref="AwenStoryAssemblyAttribute"/>
/// and discovers <see cref="IStory{T}"/> implementations.
/// </summary>
public sealed class AssemblyScanner
{
    private readonly TimeProvider _timeProvider;
    private readonly Version _hostAvaloniaVersion;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyScanner"/> class.
    /// </summary>
    /// <param name="timeProvider">Optional time provider for timestamps. Defaults to <see cref="TimeProvider.System"/>.</param>
    public AssemblyScanner(TimeProvider? timeProvider = null)
        : this(timeProvider, AvaloniaCompatibility.HostAvaloniaVersion)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyScanner"/> class with an explicit host Avalonia version.
    /// </summary>
    /// <param name="timeProvider">Optional time provider for timestamps.</param>
    /// <param name="hostAvaloniaVersion">Avalonia version the host runs on; story assemblies must match its major version.</param>
    internal AssemblyScanner(TimeProvider? timeProvider, Version hostAvaloniaVersion)
    {
        _timeProvider = timeProvider ?? TimeProvider.System;
        _hostAvaloniaVersion = hostAvaloniaVersion;
    }

    /// <summary>
    /// Scans the given directory for story assemblies.
    /// </summary>
    /// <param name="directory">The directory to scan for *.dll files.</param>
    /// <returns>A tuple of successfully loaded assemblies and load errors.</returns>
    public (IReadOnlyList<StoryAssemblyInfo> assemblies, IReadOnlyList<LoadError> errors) Scan(DirectoryInfo directory)
    {
        ArgumentNullException.ThrowIfNull(directory);

        var assemblies = new List<StoryAssemblyInfo>();
        var errors = new List<LoadError>();

        var dllFiles = directory.GetFiles("*.dll", SearchOption.TopDirectoryOnly);

#pragma warning disable S3267 // Loop body contains try/catch — not simplifiable to LINQ
        foreach (var dll in dllFiles)
#pragma warning restore S3267
        {
            try
            {
                var metadata = StoryAssemblyMetadata.Read(dll.FullName);
                if (metadata is null || !metadata.IsStoryAssembly)
                {
                    continue; // Native library or not a story assembly. Skip without loading it.
                }

                ThrowIfAvaloniaMismatch(dll.Name, metadata.AvaloniaVersion);

                var context = new StoryAssemblyLoadContext(dll.FullName);
                var assembly = context.LoadFromAssemblyPath(dll.FullName);
                var attribute = assembly.GetCustomAttribute<AwenStoryAssemblyAttribute>();

                if (attribute is null)
                {
                    continue; // Not a story assembly — skip silently
                }

                var stories = DiscoverStories(assembly, attribute.LibraryName);

                assemblies.Add(new StoryAssemblyInfo
                {
                    LibraryName = attribute.LibraryName,
                    Assembly = assembly,
                    LightThemeResourcePath = attribute.LightThemeResourcePath,
                    DarkThemeResourcePath = attribute.DarkThemeResourcePath,
                    StartupType = attribute.StartupType,
                    FilePath = dll.FullName,
                    Stories = stories,
                });
            }
#pragma warning disable CA1031 // Catch all exceptions for resilient scanning
            catch (Exception ex)
#pragma warning restore CA1031
            {
                errors.Add(new LoadError
                {
                    FilePath = dll.FullName,
                    ErrorMessage = ex.Message,
                    Exception = ex,
                    Timestamp = _timeProvider.GetUtcNow(),
                });
            }
        }

        return (assemblies, errors);
    }

    private void ThrowIfAvaloniaMismatch(string fileName, Version? storyAvaloniaVersion)
    {
        if (storyAvaloniaVersion is null)
        {
            return;
        }

        var message = AvaloniaCompatibility.GetMismatchMessage(fileName, storyAvaloniaVersion, _hostAvaloniaVersion);
        if (message is not null)
        {
            throw new AvaloniaVersionMismatchException(message);
        }
    }

    private static List<StoryDescriptor> DiscoverStories(Assembly assembly, string libraryName)
    {
        var stories = new List<StoryDescriptor>();

        foreach (var type in assembly.GetExportedTypes())
        {
            var storyInterface = Array.Find(
                type.GetInterfaces(),
                i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IStory<,>));

            if (storyInterface is null || type.IsAbstract || type.IsInterface)
            {
                continue;
            }

            if (Activator.CreateInstance(type) is IStory<Control, Control> story)
            {
                stories.Add(BuildDescriptor(libraryName, story));
            }
        }

        return stories;
    }

    private static StoryDescriptor BuildDescriptor(string libraryName, IStory<Control, Control> story)
    {
        var group = story.Group ?? string.Empty;
        return new StoryDescriptor
        {
            LibraryName = libraryName,
            Name = story.Name,
            Group = group,
            GroupSegments = group.Split('/', StringSplitOptions.RemoveEmptyEntries),
            Order = story.Order,
            Description = story.Description,
            StoryInstance = story,
            Identity = $"{libraryName}/{group}/{story.Name}",
        };
    }
}
