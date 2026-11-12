// -----------------------------------------------------------------------
// <copyright file="StoryStartupRunnerTests.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Awen.Discovery;
using Awen.Tests.TestFixtures;

namespace Awen.Tests.Discovery;

/// <summary>
/// Tests for <see cref="StoryStartupRunner"/>.
/// </summary>
public sealed class StoryStartupRunnerTests
{
    private readonly Application _app = new();

    private static StoryAssemblyInfo Library(string name, Type? startupType) => new()
    {
        LibraryName = name,
        Assembly = typeof(RecordingStartup).Assembly,
        FilePath = $"/test/{name}.dll",
        Stories = [],
        StartupType = startupType,
    };

    [Fact]
    public void Run_NoStartupType_RunsNothing()
    {
        var results = StoryStartupRunner.Run([Library("Plain", startupType: null)], _app);

        Assert.Empty(results);
    }

    [Fact]
    public void Run_ValidHook_CallsOnStartupOnceWithContext()
    {
        var results = StoryStartupRunner.Run([Library("My Lib", typeof(RecordingStartup))], _app);

        var result = Assert.Single(results);
        Assert.True(result.Success);
        Assert.Equal("My Lib", result.LibraryName);
        Assert.Equal(["My Lib"], RecordingStartup.CallsOn(_app));
    }

    [Fact]
    public void Run_HookThrows_ReportsErrorAndKeepsGoing()
    {
        var results = StoryStartupRunner.Run(
            [Library("Broken", typeof(ThrowingStartup)), Library("Good", typeof(RecordingStartup))],
            _app);

        Assert.Equal(2, results.Count);
        Assert.False(results[0].Success);
        Assert.Contains("Font file missing.", results[0].Error, StringComparison.Ordinal);
        Assert.IsType<InvalidOperationException>(results[0].Exception);
        Assert.True(results[1].Success);
        Assert.Equal(["Good"], RecordingStartup.CallsOn(_app));
    }

    [Fact]
    public void Run_ConstructorThrows_ReportsInnerException()
    {
        var result = Assert.Single(StoryStartupRunner.Run([Library("Lib", typeof(ThrowingConstructorStartup))], _app));

        Assert.False(result.Success);
        Assert.Contains("Constructor failed.", result.Error, StringComparison.Ordinal);
        Assert.IsType<InvalidOperationException>(result.Exception);
    }

    [Fact]
    public void Run_TypeWithoutIAwenStartup_ReportsClearError()
    {
        var result = Assert.Single(StoryStartupRunner.Run([Library("Lib", typeof(StoryStartupRunnerTests))], _app));

        Assert.False(result.Success);
        Assert.Contains("does not implement IAwenStartup", result.Error, StringComparison.Ordinal);
    }

    [Fact]
    public void Run_NoParameterlessConstructor_ReportsClearError()
    {
        var result = Assert.Single(StoryStartupRunner.Run([Library("Lib", typeof(NoDefaultConstructorStartup))], _app));

        Assert.False(result.Success);
        Assert.Contains("public parameterless constructor", result.Error, StringComparison.Ordinal);
    }
}
