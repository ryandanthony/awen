// -----------------------------------------------------------------------
// <copyright file="StoryAssemblyLoadContextTests.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Loader;
using Awen.Discovery;

namespace Awen.Tests.Discovery;

/// <summary>
/// Tests for <see cref="StoryAssemblyLoadContext"/> dependency resolution.
/// </summary>
public sealed class StoryAssemblyLoadContextTests : IDisposable
{
    private readonly DirectoryInfo _storyDir;
    private readonly string _suffix = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)[..8];

    /// <summary>
    /// Initializes a new instance of the <see cref="StoryAssemblyLoadContextTests"/> class.
    /// </summary>
    public StoryAssemblyLoadContextTests()
    {
        _storyDir = Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, "alc-" + _suffix));
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        // Loaded assemblies keep their files mapped on Windows, so cleanup is best effort.
        try
        {
            _storyDir.Delete(recursive: true);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            _ = ex; // Leftover temp files under the test output directory are harmless.
        }
    }

    [Theory]
    [InlineData("Avalonia.Fonts.Inter", true)]
    [InlineData("Avalonia.Base", true)]
    [InlineData("Awen.Sdk", true)]
    [InlineData("System.Text.Json", true)]
    [InlineData("Microsoft.Extensions.Logging", true)]
    [InlineData("netstandard", true)]
    [InlineData("Mise.App.DesignSystem", false)]
    [InlineData("Material.Icons.Avalonia", false)]
    public void IsSharedAssembly_ClassifiesByName(string name, bool expected)
    {
        Assert.Equal(expected, StoryAssemblyLoadContext.IsSharedAssembly(new AssemblyName(name)));
    }

    /// <summary>
    /// A story can ship a shared-named assembly the host does not (e.g. Avalonia.Fonts.Inter).
    /// Avalonia resolves avares:// URIs with Assembly.Load from the default context, so the
    /// default context must find it in the story directory.
    /// </summary>
    [Fact]
    public void DefaultContext_ResolvesStoryOnlySharedAssembly_FromStoryDirectory()
    {
        var sharedName = "Avalonia.AwenTestOnly" + _suffix;
        var root = EmitAssembly("StoryRoot" + _suffix);
        EmitAssembly(sharedName);

        _ = new StoryAssemblyLoadContext(root);

        var assembly = Assembly.Load(new AssemblyName(sharedName));

        Assert.Same(AssemblyLoadContext.Default, AssemblyLoadContext.GetLoadContext(assembly));
        Assert.Equal(_storyDir.FullName, Path.GetDirectoryName(assembly.Location));
    }

    [Fact]
    public void StoryContext_LoadsStoryOnlySharedAssembly_IntoDefaultContext()
    {
        var sharedName = "Avalonia.AwenTestOnly" + _suffix;
        var root = EmitAssembly("StoryRoot" + _suffix);
        EmitAssembly(sharedName);

        var context = new StoryAssemblyLoadContext(root);
        var assembly = context.LoadFromAssemblyName(new AssemblyName(sharedName));

        Assert.Same(AssemblyLoadContext.Default, AssemblyLoadContext.GetLoadContext(assembly));
    }

    [Fact]
    public void DefaultContext_DoesNotResolveNonSharedStoryAssembly()
    {
        var privateName = "StoryPrivate" + _suffix;
        var root = EmitAssembly("StoryRoot" + _suffix);
        EmitAssembly(privateName);

        var context = new StoryAssemblyLoadContext(root);

        Assert.Throws<FileNotFoundException>(() => Assembly.Load(new AssemblyName(privateName)));
        var fromStory = context.LoadFromAssemblyName(new AssemblyName(privateName));
        Assert.Same(context, AssemblyLoadContext.GetLoadContext(fromStory));
    }

    private string EmitAssembly(string name)
    {
        var builder = new PersistedAssemblyBuilder(new AssemblyName(name), typeof(object).Assembly);
        var module = builder.DefineDynamicModule(name);
        module.DefineType(name + ".Marker", TypeAttributes.Public | TypeAttributes.Class).CreateType();

        var path = Path.Combine(_storyDir.FullName, name + ".dll");
        builder.Save(path);
        return path;
    }
}
