// -----------------------------------------------------------------------
// <copyright file="SidebarViewModelTests.cs" company="Ryan Anthony">
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
/// Tests for <see cref="SidebarViewModel"/>.
/// </summary>
public sealed class SidebarViewModelTests
{
    private static StoryRegistry CreateTestRegistry()
    {
        var stories = new List<StoryDescriptor>
        {
            CreateDescriptor("Test Lib", new PrimaryButtonStory()),
            CreateDescriptor("Test Lib", new DisabledButtonStory()),
            CreateDescriptor("Test Lib", new TextFieldStory()),
            CreateDescriptor("Test Lib", new SearchBarStory()),
        };

        var assemblyInfo = new StoryAssemblyInfo
        {
            LibraryName = "Test Lib",
            Assembly = typeof(PrimaryButtonStory).Assembly,
            FilePath = "/test/assembly.dll",
            Stories = stories,
        };

        return new StoryRegistry([assemblyInfo], []);
    }

    private static StoryDescriptor CreateDescriptor(string libraryName, IStory<Control, Control> story)
    {
        var group = story.Group ?? string.Empty;
        return new StoryDescriptor
        {
            LibraryName = libraryName,
            Name = story.Name,
            Group = group,
            GroupSegments = group.Split('/', StringSplitOptions.RemoveEmptyEntries),
            Order = story.Order,
            Description = story.Description,
            StoryInstance = story,
            Identity = $"{libraryName}/{group}/{story.Name}",
        };
    }

    [Fact]
    public void TreeNodes_Populated_From_Registry()
    {
        var registry = CreateTestRegistry();
        var vm = new SidebarViewModel(registry);

        Assert.Single(vm.TreeNodes); // One library
        Assert.Equal("Test Lib", vm.TreeNodes[0].Label);
    }

    [Fact]
    public void FilterText_Narrows_Visible_Stories()
    {
        var registry = CreateTestRegistry();
        var vm = new SidebarViewModel(registry)
        {
            FilterText = "primary",
        };

        // When filter is set, filtered stories should only contain matches
        Assert.Single(vm.FilteredStories);
        Assert.Equal("Primary", vm.FilteredStories[0].Name);
    }

    [Fact]
    public void Empty_FilterText_Shows_All_Stories()
    {
        var registry = CreateTestRegistry();
        var vm = new SidebarViewModel(registry)
        {
            FilterText = string.Empty,
        };

        Assert.Equal(4, vm.FilteredStories.Count);
    }

    [Fact]
    public void EmptyRegistry_Shows_EmptyState()
    {
        var registry = new StoryRegistry([], []);
        var vm = new SidebarViewModel(registry);

        Assert.Empty(vm.TreeNodes);
        Assert.True(vm.IsEmpty);
    }

    [Fact]
    public void SelectedStory_Can_Be_Set()
    {
        var registry = CreateTestRegistry();
        var vm = new SidebarViewModel(registry);

        vm.SelectedStory = registry.AllStories[0];

        Assert.NotNull(vm.SelectedStory);
        Assert.Equal("Primary", vm.SelectedStory.Name);
    }

    [Fact]
    public void InitialFilter_Applied_On_Construction()
    {
        var registry = CreateTestRegistry();
        var vm = new SidebarViewModel(registry, initialFilter: "molecules");

        Assert.Single(vm.FilteredStories);
        Assert.Equal("Search Bar", vm.FilteredStories[0].Name);
    }

    [Fact]
    public void PropertyChanged_Raised_On_FilterText_Change()
    {
        var registry = CreateTestRegistry();
        var vm = new SidebarViewModel(registry);
        var changedProperties = new List<string>();
        vm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is not null)
            {
                changedProperties.Add(e.PropertyName);
            }
        };

        vm.FilterText = "button";

        Assert.Contains(nameof(SidebarViewModel.FilterText), changedProperties);
    }
}
