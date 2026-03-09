// -----------------------------------------------------------------------
// <copyright file="PreviewViewModelTests.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using Avalonia.Controls;
using Awen.Discovery;
using Awen.Sdk;
using Awen.Tests.TestFixtures;
using Awen.ViewModels;

namespace Awen.Tests.ViewModels;

/// <summary>
/// Tests for <see cref="PreviewViewModel"/>.
/// </summary>
public sealed class PreviewViewModelTests
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
    public void SelectingStory_Calls_Create_And_Populates_PreviewContent()
    {
        var vm = new PreviewViewModel();
        var descriptor = CreateDescriptor(new PrimaryButtonStory());

        vm.SelectedStory = descriptor;

        Assert.NotNull(vm.PreviewContent);
        Assert.IsType<UserControl>(vm.PreviewContent);
    }

    [Fact]
    public void SelectingStory_Populates_Description()
    {
        var vm = new PreviewViewModel();
        var descriptor = CreateDescriptor(new PrimaryButtonStory());

        vm.SelectedStory = descriptor;

        Assert.Equal("Standard primary action button.", vm.Description);
    }

    [Fact]
    public void SelectingDifferentStory_Replaces_PreviewContent()
    {
        var vm = new PreviewViewModel();

        vm.SelectedStory = CreateDescriptor(new PrimaryButtonStory());
        Assert.IsType<UserControl>(vm.PreviewContent);

        vm.SelectedStory = CreateDescriptor(new TextFieldStory());
        Assert.IsType<UserControl>(vm.PreviewContent);
    }

    [Fact]
    public void SelectingNull_Clears_PreviewContent()
    {
        var vm = new PreviewViewModel();
        vm.SelectedStory = CreateDescriptor(new PrimaryButtonStory());
        Assert.NotNull(vm.PreviewContent);

        vm.SelectedStory = null;

        Assert.Null(vm.PreviewContent);
        Assert.Null(vm.Description);
    }

    [Fact]
    public void Create_ThrowingException_Shows_ErrorMessage()
    {
        var vm = new PreviewViewModel();
        var descriptor = CreateDescriptor(new FaultyStory());

        vm.SelectedStory = descriptor;

        Assert.Null(vm.PreviewContent);
        Assert.NotNull(vm.ErrorMessage);
        Assert.Contains("CreateControl() failed", vm.ErrorMessage, StringComparison.Ordinal);
    }

    [Fact]
    public void SelectingStory_Clears_Previous_Error()
    {
        var vm = new PreviewViewModel();

        vm.SelectedStory = CreateDescriptor(new FaultyStory());
        Assert.NotNull(vm.ErrorMessage);

        vm.SelectedStory = CreateDescriptor(new PrimaryButtonStory());
        Assert.Null(vm.ErrorMessage);
        Assert.NotNull(vm.PreviewContent);
    }

    [Fact]
    public void HasError_Is_True_When_ErrorMessage_Set()
    {
        var vm = new PreviewViewModel();
        vm.SelectedStory = CreateDescriptor(new FaultyStory());

        Assert.True(vm.HasError);
    }

    [Fact]
    public void HasError_Is_False_When_No_Error()
    {
        var vm = new PreviewViewModel();
        vm.SelectedStory = CreateDescriptor(new PrimaryButtonStory());

        Assert.False(vm.HasError);
    }

    [Fact]
    public void PropertyChanged_Raised_For_SelectedStory()
    {
        var vm = new PreviewViewModel();
        var raised = new List<string>();
        vm.PropertyChanged += (_, e) => raised.Add(e.PropertyName!);

        vm.SelectedStory = CreateDescriptor(new PrimaryButtonStory());

        Assert.Contains(nameof(PreviewViewModel.SelectedStory), raised);
        Assert.Contains(nameof(PreviewViewModel.PreviewContent), raised);
        Assert.Contains(nameof(PreviewViewModel.Description), raised);
    }
}
