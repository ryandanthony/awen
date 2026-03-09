// -----------------------------------------------------------------------
// <copyright file="AwenStoryAssemblyAttributeTests.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Awen.Sdk;

namespace Awen.Sdk.Tests;

public class AwenStoryAssemblyAttributeTests
{
    [Fact]
    public void Constructor_SetsLibraryName()
    {
        var attr = new AwenStoryAssemblyAttribute("My Library");

        Assert.Equal("My Library", attr.LibraryName);
    }

    [Fact]
    public void ThemeResourcePaths_DefaultToNull()
    {
        var attr = new AwenStoryAssemblyAttribute("Test");

        Assert.Null(attr.LightThemeResourcePath);
        Assert.Null(attr.DarkThemeResourcePath);
    }

    [Fact]
    public void ThemeResourcePaths_CanBeSet()
    {
        var attr = new AwenStoryAssemblyAttribute("Test")
        {
            LightThemeResourcePath = "Light.axaml",
            DarkThemeResourcePath = "Dark.axaml",
        };

        Assert.Equal("Light.axaml", attr.LightThemeResourcePath);
        Assert.Equal("Dark.axaml", attr.DarkThemeResourcePath);
    }

    [Fact]
    public void Attribute_TargetsAssembly()
    {
        var usage = typeof(AwenStoryAssemblyAttribute)
            .GetCustomAttributes(typeof(AttributeUsageAttribute), false)
            .Cast<AttributeUsageAttribute>()
            .Single();

        Assert.Equal(AttributeTargets.Assembly, usage.ValidOn);
    }

    [Fact]
    public void Attribute_AllowMultipleIsFalse()
    {
        var usage = typeof(AwenStoryAssemblyAttribute)
            .GetCustomAttributes(typeof(AttributeUsageAttribute), false)
            .Cast<AttributeUsageAttribute>()
            .Single();

        Assert.False(usage.AllowMultiple);
    }
}
