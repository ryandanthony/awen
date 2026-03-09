// -----------------------------------------------------------------------
// <copyright file="DirectoryWatcher.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Awen.Discovery;

/// <summary>
/// Watches a directory for <c>*.dll</c> changes using <see cref="FileSystemWatcher"/>
/// with a 500ms sliding-window debounce and a 5-second polling fallback.
/// Calls the provided callback when a DLL change event settles.
/// Implements <see cref="IDisposable"/> for cleanup.
/// </summary>
public sealed class DirectoryWatcher : IDisposable
{
    private static readonly TimeSpan DebounceDelay = TimeSpan.FromMilliseconds(500);
    private static readonly TimeSpan PollInterval = TimeSpan.FromSeconds(5);

    private readonly Action<string> _onDllChanged;
    private readonly FileSystemWatcher _fileSystemWatcher;
    private readonly string _watchDirectory;
    private readonly Lock _lock = new();
    private readonly Timer _pollTimer;
    private Dictionary<string, DateTime> _lastWriteTimes;
    private CancellationTokenSource? _debounceCts;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="DirectoryWatcher"/> class.
    /// </summary>
    /// <param name="directory">The directory to watch for *.dll changes.</param>
    /// <param name="onDllChanged">Callback invoked after debounce with the changed file path.</param>
    public DirectoryWatcher(DirectoryInfo directory, Action<string> onDllChanged)
    {
        ArgumentNullException.ThrowIfNull(directory);
        ArgumentNullException.ThrowIfNull(onDllChanged);

        _onDllChanged = onDllChanged;
        _watchDirectory = directory.FullName;
        _lastWriteTimes = SnapshotDllWriteTimes(_watchDirectory);

        const int bufferSize = 32768; // 32 KB (default 8 KB)

        _fileSystemWatcher = new FileSystemWatcher(directory.FullName, "*.dll")
        {
            NotifyFilter = NotifyFilters.LastWrite
                         | NotifyFilters.CreationTime
                         | NotifyFilters.FileName,
            InternalBufferSize = bufferSize,
            EnableRaisingEvents = true,
        };

        _fileSystemWatcher.Changed += OnFileEvent;
        _fileSystemWatcher.Created += OnFileEvent;
        _fileSystemWatcher.Renamed += OnFileRenamed;
        _fileSystemWatcher.Error += OnWatcherError;

        // 5-second polling fallback for platforms where FSW may miss events (e.g., macOS)
        _pollTimer = new Timer(OnPollTick, null, PollInterval, PollInterval);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _fileSystemWatcher.EnableRaisingEvents = false;
        _fileSystemWatcher.Dispose();
        _pollTimer.Dispose();

        lock (_lock)
        {
            _debounceCts?.Cancel();
            _debounceCts?.Dispose();
            _debounceCts = null;
        }
    }

    private void OnFileEvent(object sender, FileSystemEventArgs e)
    {
        ScheduleDebounce(e.FullPath);
    }

    private void OnFileRenamed(object sender, RenamedEventArgs e)
    {
        ScheduleDebounce(e.FullPath);
    }

    private void OnWatcherError(object sender, ErrorEventArgs e)
    {
        // Treat FSW error (buffer overflow, etc.) as "something changed, reload everything"
        ScheduleDebounce(string.Empty);
    }

    private void ScheduleDebounce(string changedPath)
    {
        lock (_lock)
        {
            if (_disposed)
            {
                return;
            }

            _debounceCts?.Cancel();
            _debounceCts?.Dispose();
            _debounceCts = new CancellationTokenSource();
            var token = _debounceCts.Token;

            _ = Task.Delay(DebounceDelay, token)
                .ContinueWith(
                    _ => _onDllChanged(changedPath),
                    token,
                    TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.Default);
        }
    }

    private void OnPollTick(object? state)
    {
        if (_disposed)
        {
            return;
        }

        var current = SnapshotDllWriteTimes(_watchDirectory);
        var changed = FindChangedDll(current);

        if (changed is not null)
        {
            _lastWriteTimes = current;
            ScheduleDebounce(changed);
        }
        else
        {
            _lastWriteTimes = current;
        }
    }

    private string? FindChangedDll(Dictionary<string, DateTime> current)
    {
        // Check for modified or new files
        return current
            .Where(entry =>
                !_lastWriteTimes.TryGetValue(entry.Key, out var previous)
                || previous != entry.Value)
            .Select(entry => entry.Key)
            .FirstOrDefault();
    }

    private static Dictionary<string, DateTime> SnapshotDllWriteTimes(string directory)
    {
        var snapshot = new Dictionary<string, DateTime>(StringComparer.Ordinal);

#pragma warning disable CA1031 // Directory may not exist or be inaccessible
        try
        {
            foreach (var file in Directory.GetFiles(directory, "*.dll"))
            {
                snapshot[file] = File.GetLastWriteTimeUtc(file);
            }
        }
        catch (Exception)
        {
            // Directory deleted or inaccessible — return empty snapshot
        }
#pragma warning restore CA1031

        return snapshot;
    }
}
