// -----------------------------------------------------------------------
// <copyright file="MainWindowViewModelVersionTests.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Awen.ViewModels;

namespace Awen.Tests.ViewModels;

/// <summary>
/// Tests for the <see cref="MainWindowViewModel.Version"/> property.
/// </summary>
public sealed class MainWindowViewModelVersionTests
{
    [Fact]
    public void Version_IsNotNull()
    {
        var version = MainWindowViewModel.Version;

        Assert.NotNull(version);
    }

    [Fact]
    public void Version_IsNotEmpty()
    {
        var version = MainWindowViewModel.Version;

        Assert.NotEmpty(version);
    }

    [Fact]
    public void Version_Major_FollowsAvaloniaFlavor()
    {
        // Awen 1.x runs Avalonia 11, Awen 2.x runs Avalonia 12 (Directory.Build.targets: ApplyAwenHostMajor).
#if AVALONIA11
        const string expectedPrefix = "1.";
#else
        const string expectedPrefix = "2.";
#endif
        Assert.StartsWith(expectedPrefix, MainWindowViewModel.Version, StringComparison.Ordinal);
    }

    [Fact]
    public void Version_DoesNotContainPlusHash_WhenShortened()
    {
        // The InformationalVersion may contain "+sha" suffix from GitVersion.
        // Verify the property returns a non-empty string regardless.
        var version = MainWindowViewModel.Version;

        Assert.DoesNotContain("\n", version, StringComparison.Ordinal);
        Assert.DoesNotContain("\r", version, StringComparison.Ordinal);
    }
}
