// -----------------------------------------------------------------------
// <copyright file="PreviewViewModelThemeTests.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Styling;
using Awen.ViewModels;

namespace Awen.Tests.ViewModels;

/// <summary>
/// Tests for theme switching in <see cref="PreviewViewModel"/>.
/// </summary>
public sealed class PreviewViewModelThemeTests
{
    [Fact]
    public void Default_Theme_Is_Light()
    {
        var vm = new PreviewViewModel();

        Assert.False(vm.IsDarkTheme);
        Assert.Equal(ThemeVariant.Light, vm.ThemeVariant);
    }

    [Fact]
    public void Setting_IsDarkTheme_True_Changes_ThemeVariant_To_Dark()
    {
        var vm = new PreviewViewModel();

        vm.IsDarkTheme = true;

        Assert.True(vm.IsDarkTheme);
        Assert.Equal(ThemeVariant.Dark, vm.ThemeVariant);
    }

    [Fact]
    public void Setting_IsDarkTheme_False_Changes_ThemeVariant_To_Light()
    {
        var vm = new PreviewViewModel { IsDarkTheme = true };

        vm.IsDarkTheme = false;

        Assert.False(vm.IsDarkTheme);
        Assert.Equal(ThemeVariant.Light, vm.ThemeVariant);
    }

    [Fact]
    public void Toggle_Theme_Raises_PropertyChanged_For_IsDarkTheme_And_ThemeVariant()
    {
        var vm = new PreviewViewModel();
        var raised = new List<string>();
        vm.PropertyChanged += (_, e) => raised.Add(e.PropertyName!);

        vm.IsDarkTheme = true;

        Assert.Contains(nameof(PreviewViewModel.IsDarkTheme), raised);
        Assert.Contains(nameof(PreviewViewModel.ThemeVariant), raised);
    }

    [Fact]
    public void Same_Value_Does_Not_Raise_PropertyChanged()
    {
        var vm = new PreviewViewModel();
        var raised = new List<string>();
        vm.PropertyChanged += (_, e) => raised.Add(e.PropertyName!);

        vm.IsDarkTheme = false; // Already false by default

        Assert.DoesNotContain(nameof(PreviewViewModel.IsDarkTheme), raised);
    }
}
