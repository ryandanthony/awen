// -----------------------------------------------------------------------
// <copyright file="StoryErrorGuard.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Threading;

namespace Awen;

/// <summary>
/// Keeps Awen alive when story code throws on the UI thread after the story has loaded: click and command
/// handlers, async-void continuations, binding-driven code, and the story control's own measure, arrange or
/// render. Without this, any such exception reaches the dispatcher unhandled and the process exits.
/// </summary>
/// <remarks>
/// Every exception is marked handled and passed to <c>onError</c>, which removes the broken story from the
/// preview and shows the error. A layout pass that throws leaves Avalonia's layout manager believing a pass
/// is still queued, so nothing would ever be laid out again. After each error the guard forces one layout
/// pass on every window to get layout running again. If errors keep coming faster than the guard can
/// recover (the error is not in the removed story), it stops forcing layout so it cannot spin.
/// </remarks>
internal sealed class StoryErrorGuard : IDisposable
{
    private const int MaxRecoveriesPerWindow = 5;
    private static readonly TimeSpan RecoveryWindow = TimeSpan.FromSeconds(2);

    private readonly Dispatcher _dispatcher;
    private readonly Action<Exception> _onError;
    private readonly Func<IEnumerable<TopLevel>> _roots;
    private readonly Queue<long> _recentRecoveries = new();
    private bool _recoveryPending;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="StoryErrorGuard"/> class and starts handling exceptions.
    /// </summary>
    /// <param name="dispatcher">The UI dispatcher to guard.</param>
    /// <param name="onError">Called on the UI thread with each unhandled exception. Must not throw (it is guarded anyway).</param>
    /// <param name="roots">The windows to re-layout after an error.</param>
    internal StoryErrorGuard(Dispatcher dispatcher, Action<Exception> onError, Func<IEnumerable<TopLevel>> roots)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);
        ArgumentNullException.ThrowIfNull(onError);
        ArgumentNullException.ThrowIfNull(roots);

        _dispatcher = dispatcher;
        _onError = onError;
        _roots = roots;
        _dispatcher.UnhandledException += OnUnhandledException;
    }

    /// <summary>
    /// Gets the number of exceptions handled so far.
    /// </summary>
    internal int HandledCount { get; private set; }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _dispatcher.UnhandledException -= OnUnhandledException;
    }

    private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        e.Handled = true;
        HandledCount++;

#pragma warning disable CA1031 // The guard itself must never throw, or the app crashes after all.
        try
        {
            _onError(e.Exception);
        }
        catch (Exception reportError)
        {
            Console.Error.WriteLine($"awen: failed to report a story error: {reportError}");
            Console.Error.WriteLine($"awen: original error: {e.Exception}");
        }
#pragma warning restore CA1031

        ScheduleLayoutRecovery();
    }

    private void ScheduleLayoutRecovery()
    {
        if (_recoveryPending || _disposed)
        {
            return;
        }

        var now = Stopwatch.GetTimestamp();
        while (_recentRecoveries.Count > 0 && Stopwatch.GetElapsedTime(_recentRecoveries.Peek(), now) > RecoveryWindow)
        {
            _recentRecoveries.Dequeue();
        }

        if (_recentRecoveries.Count >= MaxRecoveriesPerWindow)
        {
            return; // Errors keep coming back after recovery, so they are not from the removed story.
        }

        _recentRecoveries.Enqueue(now);
        _recoveryPending = true;

        // Run after the error handler's changes (removed preview content) have been applied.
        _dispatcher.Post(RecoverLayout, DispatcherPriority.Background);
    }

    private void RecoverLayout()
    {
        _recoveryPending = false;
        if (_disposed)
        {
            return;
        }

        foreach (var root in _roots())
        {
            root.InvalidateMeasure();
            root.InvalidateArrange();

            // Runs a full pass directly, which also clears the layout manager's stuck "pass queued" flag.
            root.UpdateLayout();
            root.InvalidateVisual();

            // The failed frame never committed, so its pending commit request is stuck and nothing schedules
            // another render. Requesting a frame schedules one, which commits the error view to the screen.
            root.RequestAnimationFrame(static _ => { });
        }
    }
}
