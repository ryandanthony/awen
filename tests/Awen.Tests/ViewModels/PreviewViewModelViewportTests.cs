// -----------------------------------------------------------------------
// <copyright file="PreviewViewModelViewportTests.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Awen.ViewModels;

namespace Awen.Tests.ViewModels;

/// <summary>
/// Tests for viewport constraint logic in <see cref="PreviewViewModel"/>.
/// </summary>
public sealed class PreviewViewModelViewportTests
{
    [Fact]
    public void DefaultPreset_IsResponsive_Unconstrained()
    {
        var vm = new PreviewViewModel();

        Assert.Equal("Responsive", vm.SelectedPreset);
        Assert.Equal(double.PositiveInfinity, vm.ViewportWidth);
        Assert.Equal(double.PositiveInfinity, vm.ViewportHeight);
    }

    [Fact]
    public void SelectIPhone14ProMaxPreset_AppliesDimensions()
    {
        var vm = new PreviewViewModel();

        vm.SelectedPreset = "iPhone 14 Pro Max";

        Assert.Equal(430, vm.ViewportWidth);
        Assert.Equal(932, vm.ViewportHeight);
    }

    [Fact]
    public void SelectPixel8ProPreset_AppliesDimensions()
    {
        var vm = new PreviewViewModel();

        vm.SelectedPreset = "Pixel 8 Pro";

        Assert.Equal(412, vm.ViewportWidth);
        Assert.Equal(915, vm.ViewportHeight);
    }

    [Fact]
    public void SelectTabletPreset_AppliesDimensions()
    {
        var vm = new PreviewViewModel();

        vm.SelectedPreset = "iPad Mini";

        Assert.Equal(744, vm.ViewportWidth);
        Assert.Equal(1133, vm.ViewportHeight);
    }

    [Fact]
    public void SelectLaptopPreset_AppliesDimensions()
    {
        var vm = new PreviewViewModel
        {
            IsLandscape = true,
        };

        vm.SelectedPreset = "HD 1366x768";

        Assert.Equal(1366, vm.ViewportWidth);
        Assert.Equal(768, vm.ViewportHeight);
    }

    [Fact]
    public void SelectDesktopPreset_AppliesDimensions()
    {
        var vm = new PreviewViewModel
        {
            IsLandscape = true,
        };

        vm.SelectedPreset = "FHD 1920x1080";

        Assert.Equal(1920, vm.ViewportWidth);
        Assert.Equal(1080, vm.ViewportHeight);
    }

    [Fact]
    public void SelectWidescreenPreset_AppliesDimensions()
    {
        var vm = new PreviewViewModel
        {
            IsLandscape = true,
        };

        vm.SelectedPreset = "Widescreen 38 inch";

        Assert.Equal(3840, vm.ViewportWidth);
        Assert.Equal(1600, vm.ViewportHeight);
    }

    [Fact]
    public void SelectResponsivePreset_SetsUnconstrained()
    {
        var vm = new PreviewViewModel
        {
            SelectedPreset = "iPhone 14 Pro Max",
        };

        vm.SelectedPreset = "Responsive";

        Assert.Equal(double.PositiveInfinity, vm.ViewportWidth);
        Assert.Equal(double.PositiveInfinity, vm.ViewportHeight);
    }

    [Fact]
    public void CustomDimensions_OverridePreset()
    {
        var vm = new PreviewViewModel();

        vm.ViewportWidth = 500;
        vm.ViewportHeight = 600;

        Assert.Equal(500, vm.ViewportWidth);
        Assert.Equal(600, vm.ViewportHeight);
        Assert.Equal("Custom", vm.SelectedPreset);
    }

    [Fact]
    public void SelectedPreset_RaisesPropertyChanged()
    {
        var vm = new PreviewViewModel();
        var changed = new List<string>();
        vm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is not null)
            {
                changed.Add(e.PropertyName);
            }
        };

        vm.SelectedPreset = "iPhone 14 Pro Max";

        Assert.Contains(nameof(PreviewViewModel.SelectedPreset), changed);
        Assert.Contains(nameof(PreviewViewModel.ViewportWidth), changed);
        Assert.Contains(nameof(PreviewViewModel.ViewportHeight), changed);
    }

    [Fact]
    public void ViewportWidth_RaisesPropertyChanged()
    {
        var vm = new PreviewViewModel();
        var changed = new List<string>();
        vm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is not null)
            {
                changed.Add(e.PropertyName);
            }
        };

        vm.ViewportWidth = 400;

        Assert.Contains(nameof(PreviewViewModel.ViewportWidth), changed);
    }

    [Fact]
    public void Presets_ContainsExpectedValues()
    {
        var vm = new PreviewViewModel();
        var presets = vm.ViewportPresets;

        Assert.Contains("Responsive", presets);
        Assert.Contains("iPhone 14 Pro Max", presets);
        Assert.Contains("Pixel 8 Pro", presets);
        Assert.Contains("iPad Mini", presets);
        Assert.Contains("HD 1366x768", presets);
        Assert.Contains("FHD 1920x1080", presets);
        Assert.Contains("Desktop 2560x1440", presets);
        Assert.Contains("Widescreen 38 inch", presets);
    }

    [Fact]
    public void SetSamePreset_DoesNotRaisePropertyChanged()
    {
        var vm = new PreviewViewModel();
        var changeCount = 0;
        vm.PropertyChanged += (_, e) =>
        {
            if (string.Equals(e.PropertyName, nameof(PreviewViewModel.SelectedPreset), StringComparison.Ordinal))
            {
                changeCount++;
            }
        };

        // Default is Responsive, setting Responsive again should not fire
        vm.SelectedPreset = "Responsive";

        Assert.Equal(0, changeCount);
    }

    [Fact]
    public void DisplayViewportWidth_ReturnsActualSize_WhenResponsive()
    {
        var vm = new PreviewViewModel
        {
            ActualViewportWidth = 1200,
        };

        Assert.Equal(1200, vm.DisplayViewportWidth);
    }

    [Fact]
    public void DisplayViewportHeight_ReturnsActualSize_WhenResponsive()
    {
        var vm = new PreviewViewModel
        {
            ActualViewportHeight = 800,
        };

        Assert.Equal(800, vm.DisplayViewportHeight);
    }

    [Fact]
    public void DisplayViewportWidth_ReturnsSetValue_WhenConstrained()
    {
        var vm = new PreviewViewModel
        {
            SelectedPreset = "iPhone 14 Pro Max",
        };

        Assert.Equal(430, vm.DisplayViewportWidth);
    }

    [Fact]
    public void DisplayViewportHeight_ReturnsSetValue_WhenConstrained()
    {
        var vm = new PreviewViewModel
        {
            SelectedPreset = "iPhone 14 Pro Max",
        };

        Assert.Equal(932, vm.DisplayViewportHeight);
    }

    [Fact]
    public void ActualViewportWidth_RaisesDisplayPropertyChanged()
    {
        var vm = new PreviewViewModel();
        var changed = new List<string>();
        vm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is not null)
            {
                changed.Add(e.PropertyName);
            }
        };

        vm.ActualViewportWidth = 900;

        Assert.Contains(nameof(PreviewViewModel.ActualViewportWidth), changed);
        Assert.Contains(nameof(PreviewViewModel.DisplayViewportWidth), changed);
    }

    [Fact]
    public void ActualViewportHeight_RaisesDisplayPropertyChanged()
    {
        var vm = new PreviewViewModel();
        var changed = new List<string>();
        vm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is not null)
            {
                changed.Add(e.PropertyName);
            }
        };

        vm.ActualViewportHeight = 700;

        Assert.Contains(nameof(PreviewViewModel.ActualViewportHeight), changed);
        Assert.Contains(nameof(PreviewViewModel.DisplayViewportHeight), changed);
    }

    [Fact]
    public void ViewportWidth_RaisesDisplayPropertyChanged()
    {
        var vm = new PreviewViewModel();
        var changed = new List<string>();
        vm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is not null)
            {
                changed.Add(e.PropertyName);
            }
        };

        vm.ViewportWidth = 500;

        Assert.Contains(nameof(PreviewViewModel.DisplayViewportWidth), changed);
    }
}
