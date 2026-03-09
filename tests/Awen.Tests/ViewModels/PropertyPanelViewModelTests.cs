// -----------------------------------------------------------------------
// <copyright file="PropertyPanelViewModelTests.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Awen.Discovery;
using Awen.Sdk;
using Awen.Tests.TestFixtures;
using Awen.ViewModels;

namespace Awen.Tests.ViewModels;

/// <summary>
/// Tests for <see cref="PropertyPanelViewModel"/>.
/// </summary>
public sealed class PropertyPanelViewModelTests
{
    private static StoryDescriptor CreateDescriptor(IStory<Control, Control> story)
    {
        var group = story.Group ?? string.Empty;
        return new StoryDescriptor
        {
            LibraryName = "Test Lib",
            Name = story.Name,
            Group = group,
            GroupSegments = group.Split('/', StringSplitOptions.RemoveEmptyEntries),
            Order = story.Order,
            Description = story.Description,
            StoryInstance = story,
            Identity = $"Test Lib/{group}/{story.Name}",
        };
    }

    [Fact]
    public void LoadStory_Populates_PropertiesContent()
    {
        var vm = new PropertyPanelViewModel();
        var descriptor = CreateDescriptor(new PrimaryButtonStory());

        vm.LoadStory(descriptor);

        Assert.NotNull(vm.PropertiesContent);
        Assert.IsType<UserControl>(vm.PropertiesContent);
    }

    [Fact]
    public void LoadStory_WithNoProperties_ShowsEmptyControl()
    {
        var vm = new PropertyPanelViewModel();
        var descriptor = CreateDescriptor(new DisabledButtonStory());

        vm.LoadStory(descriptor);

        Assert.NotNull(vm.PropertiesContent);
        Assert.False(vm.IsEmpty);
    }

    [Fact]
    public void LoadStory_Null_Clears_PropertiesContent()
    {
        var vm = new PropertyPanelViewModel();
        vm.LoadStory(CreateDescriptor(new PrimaryButtonStory()));
        Assert.NotNull(vm.PropertiesContent);

        vm.LoadStory(null);

        Assert.Null(vm.PropertiesContent);
        Assert.True(vm.IsEmpty);
    }

    [Fact]
    public void LoadStory_Replacing_Replaces_PropertiesContent()
    {
        var vm = new PropertyPanelViewModel();
        vm.LoadStory(CreateDescriptor(new PrimaryButtonStory()));
        var first = vm.PropertiesContent;

        vm.LoadStory(CreateDescriptor(new DisabledButtonStory()));
        var second = vm.PropertiesContent;

        Assert.NotSame(first, second);
    }

    [Fact]
    public void LoadStory_SetsHasStorySelected()
    {
        var vm = new PropertyPanelViewModel();

        vm.LoadStory(CreateDescriptor(new PrimaryButtonStory()));

        Assert.True(vm.HasStorySelected);
    }

    [Fact]
    public void LoadStory_Null_ClearsHasStorySelected()
    {
        var vm = new PropertyPanelViewModel();
        vm.LoadStory(CreateDescriptor(new PrimaryButtonStory()));

        vm.LoadStory(null);

        Assert.False(vm.HasStorySelected);
    }

    [Fact]
    public void LoadStory_PropertiesContent_HasStoryAsDataContext()
    {
        var story = new PrimaryButtonStory();
        var vm = new PropertyPanelViewModel();

        vm.LoadStory(CreateDescriptor(story));

        Assert.Same(story, vm.PropertiesContent?.DataContext);
    }

    [Fact]
    public void PropertyChanged_Raised_OnLoadStory()
    {
        var vm = new PropertyPanelViewModel();
        var raised = new List<string>();
        vm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is not null)
            {
                raised.Add(e.PropertyName);
            }
        };

        vm.LoadStory(CreateDescriptor(new PrimaryButtonStory()));

        Assert.Contains(nameof(PropertyPanelViewModel.PropertiesContent), raised);
        Assert.Contains(nameof(PropertyPanelViewModel.HasStorySelected), raised);
    }
}
