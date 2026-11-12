// -----------------------------------------------------------------------
// <copyright file="AvaloniaCompatibilityTests.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Awen.Discovery;

namespace Awen.Tests.Discovery;

/// <summary>
/// Tests for <see cref="AvaloniaCompatibility"/>.
/// </summary>
public sealed class AvaloniaCompatibilityTests
{
    [Theory]
    [InlineData("11.0.0.0", "11.3.23.0")]
    [InlineData("11.3.23.0", "11.0.0.0")]
    [InlineData("12.1.6.0", "12.0.0.0")]
    public void GetMismatchMessage_SameMajor_ReturnsNull(string story, string host)
    {
        Assert.Null(AvaloniaCompatibility.GetMismatchMessage("Stories.dll", Version.Parse(story), Version.Parse(host)));
    }

    [Fact]
    public void GetMismatchMessage_Avalonia12StoriesInAvalonia11Host_PointsToAwen2()
    {
        var message = AvaloniaCompatibility.GetMismatchMessage(
            "MyLib.Stories.dll", new Version(12, 1, 6, 0), new Version(11, 3, 23, 0));

        Assert.NotNull(message);
        Assert.Contains("MyLib.Stories.dll", message, StringComparison.Ordinal);
        Assert.Contains("Avalonia 12.1.6", message, StringComparison.Ordinal);
        Assert.Contains("Avalonia 11.3.23", message, StringComparison.Ordinal);
        Assert.Contains("Awen 2.x", message, StringComparison.Ordinal);
        Assert.Contains("--version \"2.*\"", message, StringComparison.Ordinal);
    }

    [Fact]
    public void GetMismatchMessage_Avalonia11StoriesInAvalonia12Host_PointsToAwen1()
    {
        var message = AvaloniaCompatibility.GetMismatchMessage(
            "MyLib.Stories.dll", new Version(11, 3, 23, 0), new Version(12, 1, 6, 0));

        Assert.NotNull(message);
        Assert.Contains("Awen 1.x", message, StringComparison.Ordinal);
        Assert.Contains("--version \"1.*\"", message, StringComparison.Ordinal);
    }

    [Fact]
    public void GetMismatchMessage_UnsupportedAvalonia_SaysNotSupported()
    {
        var message = AvaloniaCompatibility.GetMismatchMessage(
            "Old.Stories.dll", new Version(0, 10, 22, 0), new Version(11, 3, 23, 0));

        Assert.NotNull(message);
        Assert.Contains("Avalonia 0 is not supported", message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(11, 1)]
    [InlineData(12, 2)]
    public void AwenMajorFor_SupportedAvalonia_MapsToAwenMajor(int avaloniaMajor, int awenMajor)
    {
        Assert.Equal(awenMajor, AvaloniaCompatibility.AwenMajorFor(avaloniaMajor));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    [InlineData(13)]
    public void AwenMajorFor_UnsupportedAvalonia_ReturnsNull(int avaloniaMajor)
    {
        Assert.Null(AvaloniaCompatibility.AwenMajorFor(avaloniaMajor));
    }

    [Fact]
    public void HostAvaloniaVersion_MatchesBuildFlavor()
    {
#if AVALONIA11
        const int expectedMajor = 11;
#else
        const int expectedMajor = 12;
#endif
        Assert.Equal(expectedMajor, AvaloniaCompatibility.HostAvaloniaVersion.Major);
    }
}
