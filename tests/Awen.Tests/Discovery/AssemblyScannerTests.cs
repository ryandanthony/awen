// -----------------------------------------------------------------------
// <copyright file="AssemblyScannerTests.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using Awen.Discovery;

namespace Awen.Tests.Discovery;

/// <summary>
/// Tests for <see cref="AssemblyScanner"/>.
/// </summary>
public sealed class AssemblyScannerTests : IDisposable
{
    private readonly DirectoryInfo _testDir;
    private readonly FakeTimeProvider _timeProvider;
    private readonly AssemblyScanner _scanner;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyScannerTests"/> class.
    /// </summary>
    public AssemblyScannerTests()
    {
        _testDir = Directory.CreateDirectory(
            Path.Combine(AppContext.BaseDirectory, "scanner-test-" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)[..8]));
        _timeProvider = new FakeTimeProvider();
        _scanner = new AssemblyScanner(_timeProvider);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_testDir.Exists)
        {
            _testDir.Delete(recursive: true);
        }
    }

    [Fact]
    public void Scan_NullDirectory_Throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _scanner.Scan(null!));
    }

    [Fact]
    public void Scan_EmptyDirectory_Returns_EmptyLists()
    {
        var (assemblies, errors) = _scanner.Scan(_testDir);

        Assert.Empty(assemblies);
        Assert.Empty(errors);
    }

    [Fact]
    public void Scan_DirectoryWithNoDlls_Returns_EmptyLists()
    {
        File.WriteAllText(Path.Combine(_testDir.FullName, "readme.txt"), "not a dll");
        File.WriteAllText(Path.Combine(_testDir.FullName, "data.json"), "{}");

        var (assemblies, errors) = _scanner.Scan(_testDir);

        Assert.Empty(assemblies);
        Assert.Empty(errors);
    }

    [Fact]
    public void Scan_CorruptDll_Returns_LoadError()
    {
        var corruptPath = Path.Combine(_testDir.FullName, "corrupt.dll");
        File.WriteAllBytes(corruptPath, [0x00, 0x01, 0x02, 0x03]);

        var (assemblies, errors) = _scanner.Scan(_testDir);

        Assert.Empty(assemblies);
        Assert.Single(errors);
        Assert.Equal(corruptPath, errors[0].FilePath);
        Assert.NotNull(errors[0].Exception);
        Assert.False(string.IsNullOrEmpty(errors[0].ErrorMessage));
    }

    [Fact]
    public void Scan_CorruptDll_Uses_TimeProvider_For_Timestamp()
    {
        var expectedTime = new DateTimeOffset(2025, 6, 15, 12, 0, 0, TimeSpan.Zero);
        _timeProvider.SetUtcNow(expectedTime);

        File.WriteAllBytes(Path.Combine(_testDir.FullName, "bad.dll"), [0xFF, 0xFE]);

        var (_, errors) = _scanner.Scan(_testDir);

        Assert.Single(errors);
        Assert.Equal(expectedTime, errors[0].Timestamp);
    }

    [Fact]
    public void Scan_DllWithoutStoryAttribute_Returns_EmptyAssemblies()
    {
        // Copy a known .NET DLL that lacks [AwenStoryAssembly] into the test directory
        var sourceDir = new DirectoryInfo(AppContext.BaseDirectory);
        var xunitDll = sourceDir.GetFiles("xunit.assert.dll").FirstOrDefault();

        if (xunitDll is null)
        {
            // Skip gracefully if xunit DLL not found in output
            return;
        }

        File.Copy(xunitDll.FullName, Path.Combine(_testDir.FullName, xunitDll.Name));

        var (assemblies, _) = _scanner.Scan(_testDir);

        Assert.Empty(assemblies);
    }

    [Fact]
    public void Scan_MultipleCorruptFiles_Returns_AllErrors()
    {
        File.WriteAllBytes(Path.Combine(_testDir.FullName, "bad1.dll"), [0x00]);
        File.WriteAllBytes(Path.Combine(_testDir.FullName, "bad2.dll"), [0x00]);
        File.WriteAllBytes(Path.Combine(_testDir.FullName, "bad3.dll"), [0x00]);

        var (assemblies, errors) = _scanner.Scan(_testDir);

        Assert.Empty(assemblies);
        Assert.Equal(3, errors.Count);
    }

    [Fact]
    public void Constructor_DefaultTimeProvider_DoesNotThrow()
    {
        var scanner = new AssemblyScanner();
        var (assemblies, loadErrors) = scanner.Scan(_testDir);

        Assert.Empty(assemblies);
        Assert.Empty(loadErrors);
    }

    /// <summary>
    /// A simple fake time provider for testing.
    /// </summary>
    private sealed class FakeTimeProvider : TimeProvider
    {
        private DateTimeOffset _utcNow = DateTimeOffset.UtcNow;

        public override DateTimeOffset GetUtcNow() => _utcNow;

        public void SetUtcNow(DateTimeOffset value) => _utcNow = value;
    }
}
