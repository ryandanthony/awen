// -----------------------------------------------------------------------
// <copyright file="PreviewViewportConfigTests.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Awen.Configuration;
using Awen.ViewModels;

namespace Awen.Tests.ViewModels;

/// <summary>
/// Tests for viewport configuration loading and editing.
/// </summary>
public sealed class PreviewViewportConfigTests
{
    [Fact]
    public void MissingUserConfig_FallsBackToEmbeddedDefaults()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"awen-viewport-{Guid.NewGuid():N}.json");
        var store = new ViewportConfigStore(tempFile);
        var vm = new PreviewViewModel(store);

        Assert.Contains("Responsive", vm.ViewportPresets);
        Assert.Contains("iPhone 14 Pro Max", vm.ViewportPresets);
        Assert.Contains("Pixel 8 Pro", vm.ViewportPresets);
    }

    [Fact]
    public void FirstEdit_CreatesUserConfigFile()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"awen-viewport-{Guid.NewGuid():N}.json");

        try
        {
            var store = new ViewportConfigStore(tempFile);
            var vm = new PreviewViewModel(store);

            _ = vm.LoadViewportDevicesForEdit();

            Assert.True(File.Exists(tempFile));
        }
        finally
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }

    [Fact]
    public void SaveViewportDevices_ReloadsEnabledDevicesOnly()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"awen-viewport-{Guid.NewGuid():N}.json");

        try
        {
            var store = new ViewportConfigStore(tempFile);
            var vm = new PreviewViewModel(store);

            vm.SaveViewportDevices(
            [
                new ViewportDevice
                {
                    Name = "Enabled Device",
                    Width = 500,
                    Height = 900,
                    Enabled = true,
                },
                new ViewportDevice
                {
                    Name = "Disabled Device",
                    Width = 600,
                    Height = 1000,
                    Enabled = false,
                },
            ]);

            Assert.Contains("Responsive", vm.ViewportPresets);
            Assert.Contains("Enabled Device", vm.ViewportPresets);
            Assert.DoesNotContain("Disabled Device", vm.ViewportPresets);
        }
        finally
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }
}
