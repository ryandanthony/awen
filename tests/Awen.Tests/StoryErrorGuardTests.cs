// -----------------------------------------------------------------------
// <copyright file="StoryErrorGuardTests.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Awen.Discovery;
using Awen.Sdk;
using Awen.Tests.TestFixtures;
using Awen.ViewModels;
using Awen.Views;

namespace Awen.Tests;

/// <summary>
/// End-to-end tests on a headless Avalonia app with the real <see cref="MainWindow"/>: story code that throws
/// while running must not crash Awen, must show the error in red in the preview area, and Awen must keep working.
/// </summary>
[Collection("Headless Avalonia")]
public sealed class StoryErrorGuardTests
{
    private static readonly Color ErrorRed = Color.Parse("#FF6B6B");

    private readonly HeadlessUnitTestSession _session;

    /// <summary>
    /// Initializes a new instance of the <see cref="StoryErrorGuardTests"/> class.
    /// </summary>
    /// <param name="session">Shared headless session.</param>
    public StoryErrorGuardTests(HeadlessSessionFixture session)
    {
        ArgumentNullException.ThrowIfNull(session);
        _session = session.Session;
    }

    [Fact]
    public Task ClickHandlerThrows_ShowsRedError_AndAppKeepsRunning() => Run(app =>
    {
        app.Select(new ThrowingActionsStory());

        app.Click("ThrowButton");

        Assert.Equal(1, app.Guard.HandledCount);
        app.AssertRedError("InvalidOperationException: " + ThrowingActionsStory.ClickMessage);
        Assert.True(app.ViewModel.PropertyPanel.HasError);
        Assert.Contains(app.ViewModel.LogPanel.Entries, e => e.Level == LogLevel.Error
            && e.Message.Contains(ThrowingActionsStory.ClickMessage, StringComparison.Ordinal));
    });

    [Fact]
    public Task AsyncHandlerThrowsAfterAwait_ShowsRedError() => Run(app =>
    {
        app.Select(new ThrowingActionsStory());

        app.Click("AsyncThrowButton");

        Assert.Equal(1, app.Guard.HandledCount);
        app.AssertRedError("FormatException: " + ThrowingActionsStory.AsyncMessage);
    });

    [Fact]
    public Task LayoutThrows_ShowsRedError_AndLayoutRecovers() => Run(app =>
    {
        app.Select(new ThrowingLayoutStory());

        Assert.True(app.Guard.HandledCount >= 1);
        app.AssertRedError("InvalidOperationException: " + ThrowingLayoutStory.MeasureMessage);
        Assert.True(app.RenderedFrameHasErrorRed(), "The error is not on screen: the window kept showing a stale frame.");

        // Layout still works: switching to a good story lays it out with a real size.
        app.Select(new PrimaryButtonStory());
        Assert.False(app.ViewModel.Preview.HasError);
        var content = Assert.IsAssignableFrom<Control>(app.ViewModel.Preview.PreviewContent);
        Assert.True(content.Bounds.Width > 0 && content.Bounds.Height > 0, $"Story was not laid out: {content.Bounds}");
    });

    [Fact]
    public Task Retry_ReloadsStory_AndProperties() => Run(app =>
    {
        app.Select(new ThrowingActionsStory());
        app.Click("ThrowButton");
        Assert.True(app.ViewModel.Preview.HasError);

        app.FindButton("PART_RetryButton").Command!.Execute(null);
        app.Pump();

        Assert.False(app.ViewModel.Preview.HasError);
        Assert.False(app.FindNamed("PART_ErrorView").IsVisible);
        Assert.NotNull(app.ViewModel.Preview.PreviewContent);
        Assert.NotNull(app.ViewModel.PropertyPanel.PropertiesContent);
        Assert.False(app.ViewModel.PropertyPanel.HasError);

        // The reloaded story still reports errors, and the app survives a second one.
        app.Click("ThrowButton");
        Assert.Equal(2, app.Guard.HandledCount);
        Assert.True(app.ViewModel.Preview.HasError);
    });

    [Fact]
    public Task CreateControlThrows_ShowsRedError_WithoutGuard() => Run(app =>
    {
        app.Select(new FaultyStory());

        Assert.Equal(0, app.Guard.HandledCount);
        app.AssertRedError("InvalidOperationException: Simulated failure in CreateControl().");
    });

    [Fact]
    public Task ErrorReporterThrows_ExceptionIsStillHandled() => Run(_ =>
    {
        using var guard = new StoryErrorGuard(
            Dispatcher.UIThread,
            _ => throw new InvalidOperationException("reporter broke"),
            () => []);

        Dispatcher.UIThread.Post(() => throw new InvalidOperationException("story broke"));
        Dispatcher.UIThread.RunJobs();

        Assert.Equal(1, guard.HandledCount);
    });

    private Task Run(Action<TestApp> test) => _session.Dispatch(
        () =>
        {
            using var app = new TestApp();
            test(app);
        },
        CancellationToken.None);

    private sealed class TestApp : IDisposable
    {
        private readonly DirectoryInfo _emptyDir;

        public TestApp()
        {
            _emptyDir = Directory.CreateDirectory(Path.Combine(
                Path.GetTempPath(),
                "awen-guard-" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)[..8]));

            ViewModel = new MainWindowViewModel(new AwenOptions { Dir = _emptyDir, Watch = false });
            Window = new MainWindow { DataContext = ViewModel, Width = 1400, Height = 900 };
            Guard = new StoryErrorGuard(Dispatcher.UIThread, ViewModel.HandleStoryException, () => [Window]);
            Window.Show();
            Pump();
        }

        public MainWindowViewModel ViewModel { get; }

        public MainWindow Window { get; }

        public StoryErrorGuard Guard { get; }

        public void Select(IStory<Control, Control> story)
        {
            ViewModel.Sidebar.SelectedStory = new StoryDescriptor
            {
                LibraryName = "Test Lib",
                Name = story.Name,
                Group = story.Group,
                GroupSegments = story.Group.Split('/'),
                Order = story.Order,
                Description = story.Description,
                StoryInstance = story,
                Identity = "Test Lib/" + story.Group + "/" + story.Name,
            };
            Pump();
        }

        public void Click(string buttonName)
        {
            var button = FindButton(buttonName);
            var center = button.TranslatePoint(new Point(button.Bounds.Width / 2, button.Bounds.Height / 2), Window)
                ?? throw new InvalidOperationException($"{buttonName} is not in the window.");

            Window.MouseMove(center);
            Window.MouseDown(center, MouseButton.Left);
            Window.MouseUp(center, MouseButton.Left);
            Pump();
        }

        public Control FindNamed(string name) =>
            Window.GetVisualDescendants().OfType<Control>().FirstOrDefault(c => string.Equals(c.Name, name, StringComparison.Ordinal))
            ?? throw new InvalidOperationException($"No control named {name} in the window.");

        public Button FindButton(string name) => Assert.IsType<Button>(FindNamed(name));

        public void AssertRedError(string expectedMessage)
        {
            Assert.True(ViewModel.Preview.HasError);
            Assert.Null(ViewModel.Preview.PreviewContent);
            Assert.Equal(expectedMessage, ViewModel.Preview.ErrorMessage);

            var errorView = FindNamed("PART_ErrorView");
            Assert.True(errorView.IsEffectivelyVisible);
            Assert.True(errorView.Bounds.Width > 0 && errorView.Bounds.Height > 0, $"Error view was not laid out: {errorView.Bounds}");

            var message = Assert.IsType<SelectableTextBlock>(FindNamed("PART_ErrorMessage"));
            Assert.Equal(expectedMessage, message.Text);
            var brush = Assert.IsAssignableFrom<ISolidColorBrush>(message.Foreground);
            Assert.Equal(ErrorRed, brush.Color);
        }

        /// <summary>
        /// Checks the last frame actually drawn to the window contains the error red, so the error view is on
        /// screen and not only in the visual tree.
        /// </summary>
        public bool RenderedFrameHasErrorRed()
        {
            using var frame = Window.CaptureRenderedFrame()
                ?? throw new InvalidOperationException("No frame was rendered.");

            var size = frame.PixelSize;
            var stride = size.Width * 4;
            var pixels = new byte[stride * size.Height];
            var handle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
            try
            {
                frame.CopyPixels(new PixelRect(size), handle.AddrOfPinnedObject(), pixels.Length, stride);
            }
            finally
            {
                handle.Free();
            }

            var bgra = frame.Format == PixelFormat.Bgra8888;
            for (var i = 0; i + 3 < pixels.Length; i += 4)
            {
                var (r, g, b) = bgra ? (pixels[i + 2], pixels[i + 1], pixels[i]) : (pixels[i], pixels[i + 1], pixels[i + 2]);
                if (r == ErrorRed.R && g == ErrorRed.G && b == ErrorRed.B)
                {
                    return true;
                }
            }

            return false;
        }

        public void Pump()
        {
            for (var i = 0; i < 5; i++)
            {
                Dispatcher.UIThread.RunJobs();
                AvaloniaHeadlessPlatform.ForceRenderTimerTick();
            }

            Dispatcher.UIThread.RunJobs();
        }

        public void Dispose()
        {
            Guard.Dispose();
            Window.Close();
            ViewModel.Dispose();
            _emptyDir.Delete(recursive: true);
        }
    }
}
