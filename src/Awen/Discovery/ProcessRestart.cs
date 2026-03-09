// -----------------------------------------------------------------------
// <copyright file="ProcessRestart.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics;

namespace Awen.Discovery;

/// <summary>
/// Handles process self-restart for hot-reload: serializes current state to a
/// temp JSON file, launches a new Awen process with <c>--restore</c>, and exits.
/// </summary>
public static class ProcessRestart
{
    /// <summary>
    /// Serializes the given state, launches a new process with the original
    /// arguments plus <c>--restore &lt;state-file&gt;</c>, then exits.
    /// </summary>
    /// <param name="state">The current session state to preserve.</param>
    /// <param name="originalArgs">The original CLI arguments.</param>
    public static void RestartWithState(HotReloadState state, string[] originalArgs)
    {
        ArgumentNullException.ThrowIfNull(state);
        ArgumentNullException.ThrowIfNull(originalArgs);

        var stateFile = Path.Combine(
            Path.GetTempPath(),
            $"awen-state-{Process.GetCurrentProcess().Id}.json");

        HotReloadState.WriteToFile(state, stateFile);

        var processPath = Environment.ProcessPath;
        if (string.IsNullOrEmpty(processPath))
        {
            return;
        }

        // Build args: original args + --restore <state-file>
        // First, strip any existing --restore argument
        var args = StripRestoreArg(originalArgs);
        var allArgs = new List<string>(args)
        {
            "--restore",
            stateFile,
        };

        var startInfo = new ProcessStartInfo
        {
            FileName = processPath,
            UseShellExecute = false,
        };

        foreach (var arg in allArgs)
        {
            startInfo.ArgumentList.Add(arg);
        }

#pragma warning disable CA1031 // Catch general exception for resilient restart
        try
        {
            Process.Start(startInfo);
        }
        catch (Exception)
        {
            // If we can't start the new process, don't exit the current one
            return;
        }
#pragma warning restore CA1031

#pragma warning disable S1147 // Process restart requires Environment.Exit
        Environment.Exit(0);
#pragma warning restore S1147
    }

    private static List<string> StripRestoreArg(string[] args)
    {
        var result = new List<string>();
        var skipNext = false;

        foreach (var arg in args)
        {
            if (skipNext)
            {
                skipNext = false;
                continue;
            }

            if (arg.Equals("--restore", StringComparison.Ordinal))
            {
                // Skip --restore and its value
                skipNext = true;
                continue;
            }

            result.Add(arg);
        }

        return result;
    }
}
