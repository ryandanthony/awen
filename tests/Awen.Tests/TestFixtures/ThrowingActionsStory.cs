// -----------------------------------------------------------------------
// <copyright file="ThrowingActionsStory.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Awen.Sdk;

namespace Awen.Tests.TestFixtures;

/// <summary>
/// A story that loads fine but throws when the user acts on it: a synchronous click handler and an
/// async-void click handler that throws after an await.
/// </summary>
public sealed class ThrowingActionsStory : IStory<UserControl, UserControl>
{
    /// <summary>
    /// Gets the message thrown by the synchronous click handler.
    /// </summary>
    public static string ClickMessage => "Click handler failed.";

    /// <summary>
    /// Gets the message thrown by the async click handler.
    /// </summary>
    public static string AsyncMessage => "Async handler failed after await.";

    /// <inheritdoc/>
    public string Name => "Throwing Actions";

    /// <inheritdoc/>
    public string Group => "Broken";

    /// <inheritdoc/>
    public int Order => 2;

    /// <inheritdoc/>
    public string Description => "Buttons whose handlers throw.";

    /// <inheritdoc/>
    public UserControl CreateControl()
    {
        var click = new Button { Name = "ThrowButton", Content = "Throw", Width = 120, Height = 40 };
        click.Click += (_, _) => throw new InvalidOperationException(ClickMessage);

        var asyncClick = new Button { Name = "AsyncThrowButton", Content = "Async throw", Width = 120, Height = 40 };
        asyncClick.Click += OnAsyncClick;

        var panel = new StackPanel { Spacing = 8 };
        panel.Children.Add(click);
        panel.Children.Add(asyncClick);
        return new UserControl { Content = panel };
    }

    /// <inheritdoc/>
    public UserControl CreateProperties() => new() { Content = new TextBlock { Text = "props" } };

#pragma warning disable VSTHRD100 // async void is exactly what is under test.
    private static async void OnAsyncClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
#pragma warning restore VSTHRD100
    {
        await Task.Yield();
        throw new FormatException(AsyncMessage);
    }
}
