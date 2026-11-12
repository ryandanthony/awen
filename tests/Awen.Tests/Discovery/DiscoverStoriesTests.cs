// -----------------------------------------------------------------------
// <copyright file="DiscoverStoriesTests.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Awen.Discovery;
using Awen.Tests.TestFixtures;

namespace Awen.Tests.Discovery;

/// <summary>
/// Tests for story discovery inside a loaded assembly.
/// </summary>
public sealed class DiscoverStoriesTests
{
    private static readonly List<StoryDescriptor> Stories =
        AssemblyScanner.DiscoverStories(typeof(ThrowingConstructorStory).Assembly, "Test Lib");

    [Fact]
    public void ThrowingConstructor_DoesNotHideOtherStories()
    {
        Assert.Contains(Stories, s => string.Equals(s.Name, "Primary", StringComparison.Ordinal) && s.LoadError is null);
        Assert.Contains(Stories, s => string.Equals(s.Name, "Faulty", StringComparison.Ordinal) && s.LoadError is null);
    }

    [Fact]
    public void ThrowingConstructor_BecomesFaultedStory_InLoadErrorsGroup()
    {
        var faulted = Assert.Single(Stories, s => s.LoadError is not null);

        Assert.Equal(nameof(ThrowingConstructorStory), faulted.Name);
        Assert.Equal(AssemblyScanner.LoadErrorGroup, faulted.Group);
        Assert.IsType<InvalidOperationException>(faulted.LoadError);
        Assert.Equal("Story constructor failed.", faulted.LoadError!.Message);
        Assert.Throws<InvalidOperationException>(() => faulted.StoryInstance.CreateControl());
    }
}
