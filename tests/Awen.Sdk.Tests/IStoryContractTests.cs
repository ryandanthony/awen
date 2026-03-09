// -----------------------------------------------------------------------
// <copyright file="IStoryContractTests.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Awen.Sdk;

namespace Awen.Sdk.Tests;

/// <summary>
/// Contract tests validating basic <see cref="IStory{TControl, TStoryProperties}"/> requirements.
/// </summary>
public sealed class IStoryContractTests
{
    [Fact]
    public void Name_IsNotNullOrEmpty()
    {
        var story = new SampleStory();

        Assert.False(string.IsNullOrEmpty(story.Name));
    }

    [Fact]
    public void Group_IsNotNullOrEmpty()
    {
        var story = new SampleStory();

        Assert.False(string.IsNullOrEmpty(story.Group));
    }

    [Fact]
    public void CreateControl_ReturnsNonNull()
    {
        var story = new SampleStory();

        var control = story.CreateControl();

        Assert.NotNull(control);
    }

    [Fact]
    public void CreateProperties_ReturnsNonNull()
    {
        var story = new SampleStory();

        var properties = story.CreateProperties();

        Assert.NotNull(properties);
    }

    [Fact]
    public void Order_DefaultsToZero()
    {
        var story = new SampleStory();

        Assert.Equal(0, story.Order);
    }

    [Fact]
    public void Description_IsNotNull()
    {
        var story = new SampleStory();

        Assert.NotNull(story.Description);
    }

    [Fact]
    public void CreateControl_SetsDataContext_ToStory()
    {
        var story = new SampleStory();

        var control = story.CreateControl();

        Assert.Same(story, control.DataContext);
    }

    [Fact]
    public void CreateProperties_SetsDataContext_ToStory()
    {
        var story = new SampleStory();

        var properties = story.CreateProperties();

        Assert.Same(story, properties.DataContext);
    }

    /// <summary>
    /// Minimal IStory implementation for contract testing.
    /// </summary>
    private sealed class SampleStory : IStory<UserControl, UserControl>
    {
        public string Name => "Sample";

        public string Group => "Tests/Contract";

        public int Order => 0;

        public string Description => "A sample story for contract validation.";

        public UserControl CreateControl() =>
            new() { Content = new TextBlock { Text = "Hello" }, DataContext = this };

        public UserControl CreateProperties() =>
            new() { DataContext = this };
    }
}
