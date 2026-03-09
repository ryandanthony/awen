// -----------------------------------------------------------------------
// <copyright file="HotReloadStateTests.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Text.Json;
using Awen.Discovery;

namespace Awen.Tests.Discovery;

/// <summary>
/// Tests for <see cref="HotReloadState"/> JSON serialization.
/// </summary>
public sealed class HotReloadStateTests
{
    [Fact]
    public void RoundTrip_AllFields_PreservesValues()
    {
        var state = new HotReloadState
        {
            SelectedStoryIdentity = "MyLib/Buttons/Primary",
            ThemeVariant = "dark",
            ViewportWidth = 1024.0,
            ViewportHeight = 768.0,
            SidebarFilter = "button",
        };

        var json = JsonSerializer.Serialize(state, HotReloadState.SerializerOptions);
        var deserialized = JsonSerializer.Deserialize<HotReloadState>(json, HotReloadState.SerializerOptions);

        Assert.NotNull(deserialized);
        Assert.Equal("MyLib/Buttons/Primary", deserialized.SelectedStoryIdentity);
        Assert.Equal("dark", deserialized.ThemeVariant);
        Assert.Equal(1024.0, deserialized.ViewportWidth);
        Assert.Equal(768.0, deserialized.ViewportHeight);
        Assert.Equal("button", deserialized.SidebarFilter);
    }

    [Fact]
    public void RoundTrip_EmptyState_UsesDefaults()
    {
        var state = new HotReloadState();

        var json = JsonSerializer.Serialize(state, HotReloadState.SerializerOptions);
        var deserialized = JsonSerializer.Deserialize<HotReloadState>(json, HotReloadState.SerializerOptions);

        Assert.NotNull(deserialized);
        Assert.Null(deserialized.SelectedStoryIdentity);
        Assert.Null(deserialized.ThemeVariant);
        Assert.Null(deserialized.ViewportWidth);
        Assert.Null(deserialized.ViewportHeight);
        Assert.Null(deserialized.SidebarFilter);
    }

    [Fact]
    public void Deserialize_MissingFields_UsesDefaults()
    {
        var json = """{ "themeVariant": "light" }""";

        var deserialized = JsonSerializer.Deserialize<HotReloadState>(json, HotReloadState.SerializerOptions);

        Assert.NotNull(deserialized);
        Assert.Null(deserialized.SelectedStoryIdentity);
        Assert.Equal("light", deserialized.ThemeVariant);
        Assert.Null(deserialized.ViewportWidth);
        Assert.Null(deserialized.ViewportHeight);
        Assert.Null(deserialized.SidebarFilter);
    }

    [Fact]
    public void Deserialize_EmptyJson_ReturnsDefaults()
    {
        var json = "{}";

        var deserialized = JsonSerializer.Deserialize<HotReloadState>(json, HotReloadState.SerializerOptions);

        Assert.NotNull(deserialized);
        Assert.Null(deserialized.SelectedStoryIdentity);
        Assert.Null(deserialized.ThemeVariant);
    }

    [Fact]
    public void WriteToFile_And_ReadFromFile_RoundTrips()
    {
        var state = new HotReloadState
        {
            SelectedStoryIdentity = "Lib/Group/Story",
            ThemeVariant = "dark",
        };

        var filePath = Path.Combine(AppContext.BaseDirectory, "test-state-" + Guid.NewGuid().ToString("N", System.Globalization.CultureInfo.InvariantCulture)[..8] + ".json");

        try
        {
            HotReloadState.WriteToFile(state, filePath);
            var loaded = HotReloadState.ReadFromFile(filePath);

            Assert.NotNull(loaded);
            Assert.Equal("Lib/Group/Story", loaded.SelectedStoryIdentity);
            Assert.Equal("dark", loaded.ThemeVariant);
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    [Fact]
    public void ReadFromFile_NonExistentFile_ReturnsNull()
    {
        var result = HotReloadState.ReadFromFile("/nonexistent/path/state.json");

        Assert.Null(result);
    }
}
