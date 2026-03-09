// -----------------------------------------------------------------------
// <copyright file="DirectoryWatcherTests.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using Awen.Discovery;

namespace Awen.Tests.Discovery;

/// <summary>
/// Tests for <see cref="DirectoryWatcher"/>.
/// </summary>
public sealed class DirectoryWatcherTests : IDisposable
{
    private readonly DirectoryInfo _watchDir;

    /// <summary>
    /// Initializes a new instance of the <see cref="DirectoryWatcherTests"/> class.
    /// </summary>
    public DirectoryWatcherTests()
    {
        _watchDir = Directory.CreateDirectory(
            Path.Combine(AppContext.BaseDirectory, "watcher-test-" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)[..8]));
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_watchDir.Exists)
        {
            _watchDir.Delete(recursive: true);
        }
    }

    [Fact]
    public void Constructor_NullDirectory_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new DirectoryWatcher(null!, _ => { }));
    }

    [Fact]
    public void Constructor_NullCallback_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new DirectoryWatcher(_watchDir, null!));
    }

    [Fact]
    public async Task DllChange_TriggersCallback_AfterDebounce()
    {
        var triggered = new TaskCompletionSource<string>();

        using var watcher = new DirectoryWatcher(_watchDir, path =>
        {
            triggered.TrySetResult(path);
        });

        // Write a DLL file after watcher is active
        var dllPath = Path.Combine(_watchDir.FullName, "test.dll");
        await File.WriteAllBytesAsync(dllPath, [0x00, 0x01]);

        // Wait for debounce (500ms) + margin
        var result = await Task.WhenAny(triggered.Task, Task.Delay(3000));
        Assert.Equal(triggered.Task, result);
    }

    [Fact]
    public async Task NonDllFile_DoesNotTriggerCallback()
    {
        var triggered = new TaskCompletionSource<string>();

        using var watcher = new DirectoryWatcher(_watchDir, path =>
        {
            triggered.TrySetResult(path);
        });

        // Write a non-DLL file
        await File.WriteAllTextAsync(Path.Combine(_watchDir.FullName, "readme.txt"), "hello");

        // Wait enough time for debounce — should NOT trigger
        var result = await Task.WhenAny(triggered.Task, Task.Delay(1500));
        Assert.NotEqual(triggered.Task, result);
    }

    [Fact]
    public async Task RapidDllChanges_Debounced_SingleCallback()
    {
        var callbackCount = 0;
        var triggered = new TaskCompletionSource<bool>();

        using var watcher = new DirectoryWatcher(_watchDir, _ =>
        {
            Interlocked.Increment(ref callbackCount);
            triggered.TrySetResult(true);
        });

        // Rapid-fire DLL changes
        var dllPath = Path.Combine(_watchDir.FullName, "lib.dll");
        for (int i = 0; i < 5; i++)
        {
            await File.WriteAllBytesAsync(dllPath, [(byte)i]);
            await Task.Delay(50);
        }

        // Wait for debounce to settle
        await Task.WhenAny(triggered.Task, Task.Delay(3000));

        // Should collapse into 1 (or very few) callbacks
        Assert.InRange(callbackCount, 1, 2);
    }

    [Fact]
    public async Task Dispose_StopsWatching()
    {
        var triggered = new TaskCompletionSource<string>();

        var watcher = new DirectoryWatcher(_watchDir, path =>
        {
            triggered.TrySetResult(path);
        });

        watcher.Dispose();

        // Write a DLL after dispose
        await File.WriteAllBytesAsync(Path.Combine(_watchDir.FullName, "after.dll"), [0x00]);

        // Should not trigger
        var result = await Task.WhenAny(triggered.Task, Task.Delay(1000));
        Assert.NotEqual(triggered.Task, result);
    }

    [Fact]
    public void Dispose_MultipleCallsDoNotThrow()
    {
        var watcher = new DirectoryWatcher(_watchDir, _ => { });

        watcher.Dispose();
        var exception = Record.Exception(() => watcher.Dispose());
        Assert.Null(exception);
    }
}
