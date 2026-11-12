// -----------------------------------------------------------------------
// <copyright file="AvaloniaVersionCheckTests.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using Awen.Discovery;

namespace Awen.Tests.Discovery;

/// <summary>
/// Scanner and metadata tests against a real story assembly (ExampleUI.Stories, built for this flavor).
/// </summary>
public sealed class AvaloniaVersionCheckTests : IDisposable
{
    private const string StoriesFile = "ExampleUI.Stories.dll";
    private readonly DirectoryInfo _testDir;

    /// <summary>
    /// Initializes a new instance of the <see cref="AvaloniaVersionCheckTests"/> class.
    /// Copies the example story assembly and its dependency into an isolated scan directory.
    /// </summary>
    public AvaloniaVersionCheckTests()
    {
        _testDir = Directory.CreateDirectory(
            Path.Combine(AppContext.BaseDirectory, "version-check-" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)[..8]));

        foreach (var file in new[] { StoriesFile, "ExampleUI.dll", "Awen.Sdk.dll" })
        {
            File.Copy(Path.Combine(AppContext.BaseDirectory, file), Path.Combine(_testDir.FullName, file));
        }
    }

    private static Version OtherMajor => AvaloniaCompatibility.HostAvaloniaVersion.Major == 11
        ? new Version(12, 1, 6, 0)
        : new Version(11, 3, 23, 0);

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_testDir.Exists)
        {
            _testDir.Delete(recursive: true);
        }
    }

    [Fact]
    public void Metadata_StoryAssembly_IsDetectedWithItsAvaloniaVersion()
    {
        var info = StoryAssemblyMetadata.Read(Path.Combine(_testDir.FullName, StoriesFile));

        Assert.NotNull(info);
        Assert.True(info.IsStoryAssembly);
        Assert.NotNull(info.AvaloniaVersion);
        Assert.Equal(AvaloniaCompatibility.HostAvaloniaVersion.Major, info.AvaloniaVersion.Major);
    }

    [Fact]
    public void Metadata_AvaloniaLibraryWithoutAttribute_IsNotAStoryAssembly()
    {
        var info = StoryAssemblyMetadata.Read(Path.Combine(_testDir.FullName, "ExampleUI.dll"));

        Assert.NotNull(info);
        Assert.False(info.IsStoryAssembly);
        Assert.NotNull(info.AvaloniaVersion);
    }

    [Fact]
    public void Metadata_CorruptFile_Throws_BadImageFormat()
    {
        var path = Path.Combine(_testDir.FullName, "corrupt.dll");
        File.WriteAllBytes(path, [0x4D, 0x5A, 0x00]);

        Assert.Throws<BadImageFormatException>(() => StoryAssemblyMetadata.Read(path));
    }

    [Fact]
    public void Scan_MatchingAvalonia_LoadsStories()
    {
        var scanner = new AssemblyScanner(null, AvaloniaCompatibility.HostAvaloniaVersion);

        var (assemblies, errors) = scanner.Scan(_testDir);

        Assert.Empty(errors);
        var library = Assert.Single(assemblies);
        Assert.Equal("Example UI", library.LibraryName);
        Assert.NotEmpty(library.Stories);
        Assert.Equal("ExampleUI.Stories.Startup", library.StartupType?.FullName);
    }

    [Fact]
    public void Scan_DifferentAvaloniaMajor_ReportsClearMismatchError()
    {
        var scanner = new AssemblyScanner(null, OtherMajor);

        var (assemblies, errors) = scanner.Scan(_testDir);

        Assert.Empty(assemblies);
        var error = Assert.Single(errors);
        Assert.EndsWith(StoriesFile, error.FilePath, StringComparison.Ordinal);
        Assert.IsType<AvaloniaVersionMismatchException>(error.Exception);
        Assert.Contains("was built against Avalonia", error.ErrorMessage, StringComparison.Ordinal);
        Assert.Contains("Use Awen", error.ErrorMessage, StringComparison.Ordinal);
    }

    [Fact]
    public void Scan_NonStoryDlls_AreNotReportedAsErrors()
    {
        File.Delete(Path.Combine(_testDir.FullName, StoriesFile));
        var scanner = new AssemblyScanner(null, OtherMajor);

        var (assemblies, errors) = scanner.Scan(_testDir);

        Assert.Empty(assemblies);
        Assert.Empty(errors);
    }
}
