// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.CommandLine;
using Avalonia;

namespace Awen;

/// <summary>
/// CLI entry point for Awen — Storybook for Avalonia.
/// </summary>
public static class Program
{
    /// <summary>
    /// Application entry point.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    /// <returns>Exit code.</returns>
    public static int Main(string[] args)
    {
        var rootCommand = BuildRootCommand(args);
        return rootCommand.Parse(args).Invoke();
    }

    internal static RootCommand BuildRootCommand(string[] originalArgs)
    {
        var (dirOption, watchOption, noWatchOption, themeOption, filterOption, restoreOption) = CreateOptions();

        var rootCommand = new RootCommand("Awen — Storybook for Avalonia");
        rootCommand.Options.Add(dirOption);
        rootCommand.Options.Add(watchOption);
        rootCommand.Options.Add(noWatchOption);
        rootCommand.Options.Add(themeOption);
        rootCommand.Options.Add(filterOption);
        rootCommand.Options.Add(restoreOption);

        rootCommand.SetAction(parseResult =>
        {
            var noWatch = parseResult.GetValue(noWatchOption);
            var watch = !noWatch && parseResult.GetValue(watchOption);

            var options = new AwenOptions
            {
                Dir = parseResult.GetValue(dirOption)!,
                Watch = watch,
                Theme = parseResult.GetValue(themeOption),
                Filter = parseResult.GetValue(filterOption),
                RestoreFile = parseResult.GetValue(restoreOption),
            };

            BuildAvaloniaApp(options, originalArgs).StartWithClassicDesktopLifetime(originalArgs);
        });

        return rootCommand;
    }

    private static (Option<DirectoryInfo> dir, Option<bool> watch, Option<bool> noWatch, Option<string> theme, Option<string?> filter, Option<string?> restore) CreateOptions()
    {
        var dirOption = new Option<DirectoryInfo>("--dir", "-d")
        {
            Description = "Directory to scan for story assemblies",
            Required = true,
        };
        dirOption.AcceptExistingOnly();

        var watchOption = new Option<bool>("--watch", "-w")
        {
            Description = "Enable hot-reload via FileSystemWatcher",
            DefaultValueFactory = _ => true,
        };

        var noWatchOption = new Option<bool>("--no-watch")
        {
            Description = "Disable hot-reload",
        };

        var themeOption = new Option<string>("--theme", "-t")
        {
            Description = "Initial theme variant (light or dark)",
            DefaultValueFactory = _ => "light",
        };
        themeOption.AcceptOnlyFromAmong("light", "dark");

        var filterOption = new Option<string?>("--filter", "-f")
        {
            Description = "Filter stories by name or group path",
        };

        var restoreOption = new Option<string?>("--restore")
        {
            Description = "Internal: path to a hot-reload state JSON file",
            Hidden = true,
        };

        return (dirOption, watchOption, noWatchOption, themeOption, filterOption, restoreOption);
    }

    private static AppBuilder BuildAvaloniaApp(AwenOptions options, string[] originalArgs)
    {
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .AfterSetup(builder =>
            {
                if (builder.Instance is App app)
                {
                    app.Options = options;
                    app.OriginalArgs = originalArgs;
                }
            });
    }
}
