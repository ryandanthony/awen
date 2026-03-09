// -----------------------------------------------------------------------
// <copyright file="StoryRegistryTests.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Awen.Discovery;
using Awen.Sdk;
using Awen.Tests.TestFixtures;
using Awen.ViewModels;

namespace Awen.Tests.Discovery;

/// <summary>
/// Tests for <see cref="StoryRegistry"/>.
/// </summary>
public sealed class StoryRegistryTests
{
    private static StoryRegistry CreateRegistryWithTestStories()
    {
        var stories = new List<StoryDescriptor>
        {
            CreateDescriptor("Test Lib", new PrimaryButtonStory()),
            CreateDescriptor("Test Lib", new DisabledButtonStory()),
            CreateDescriptor("Test Lib", new TextFieldStory()),
            CreateDescriptor("Test Lib", new SearchBarStory()),
        };

        var assemblyInfo = new StoryAssemblyInfo
        {
            LibraryName = "Test Lib",
            Assembly = typeof(PrimaryButtonStory).Assembly,
            FilePath = "/test/assembly.dll",
            Stories = stories,
        };

        return new StoryRegistry([assemblyInfo], []);
    }

    private static StoryDescriptor CreateDescriptor(string libraryName, IStory<Control, Control> story)
    {
        var group = story.Group ?? string.Empty;
        return new StoryDescriptor
        {
            LibraryName = libraryName,
            Name = story.Name,
            Group = group,
            GroupSegments = group.Split('/', StringSplitOptions.RemoveEmptyEntries),
            Order = story.Order,
            Description = story.Description,
            StoryInstance = story,
            Identity = $"{libraryName}/{group}/{story.Name}",
        };
    }

    [Fact]
    public void AllStories_Contains_All_Stories_Across_Assemblies()
    {
        var registry = CreateRegistryWithTestStories();

        Assert.Equal(4, registry.AllStories.Count);
    }

    [Fact]
    public void FindByIdentity_Returns_Correct_Story()
    {
        var registry = CreateRegistryWithTestStories();

        var result = registry.FindByIdentity("Test Lib/Atoms/Buttons/Primary");

        Assert.NotNull(result);
        Assert.Equal("Primary", result.Name);
    }

    [Fact]
    public void FindByIdentity_Returns_Null_For_Unknown()
    {
        var registry = CreateRegistryWithTestStories();

        var result = registry.FindByIdentity("NonExistent/Group/Story");

        Assert.Null(result);
    }

    [Fact]
    public void BuildSidebarTree_Produces_Library_Root_Node()
    {
        var registry = CreateRegistryWithTestStories();

        var tree = registry.BuildSidebarTree();

        Assert.Single(tree);
        Assert.Equal("Test Lib", tree[0].Label);
        Assert.Equal(SidebarNodeType.Library, tree[0].NodeType);
    }

    [Fact]
    public void BuildSidebarTree_Creates_Group_Hierarchy()
    {
        var registry = CreateRegistryWithTestStories();

        var tree = registry.BuildSidebarTree();
        var libraryNode = tree[0];

        // Library should have 2 top-level groups: "Atoms" and "Molecules"
        Assert.Equal(2, libraryNode.Children.Count);

        var atomsNode = libraryNode.Children
            .First(c => c.Label.Equals("Atoms", StringComparison.Ordinal));
        Assert.Equal(SidebarNodeType.Group, atomsNode.NodeType);

        var moleculesNode = libraryNode.Children
            .First(c => c.Label.Equals("Molecules", StringComparison.Ordinal));
        Assert.Equal(SidebarNodeType.Group, moleculesNode.NodeType);
    }

    [Fact]
    public void BuildSidebarTree_Creates_Nested_Groups()
    {
        var registry = CreateRegistryWithTestStories();

        var tree = registry.BuildSidebarTree();
        var atomsNode = tree[0].Children
            .First(c => c.Label.Equals("Atoms", StringComparison.Ordinal));

        // Atoms should have "Buttons" and "Inputs" children
        Assert.Equal(2, atomsNode.Children.Count);
        Assert.Contains(atomsNode.Children, c => c.Label.Equals("Buttons", StringComparison.Ordinal));
        Assert.Contains(atomsNode.Children, c => c.Label.Equals("Inputs", StringComparison.Ordinal));
    }

    [Fact]
    public void BuildSidebarTree_Story_Leaves_Have_Story_Type()
    {
        var registry = CreateRegistryWithTestStories();

        var tree = registry.BuildSidebarTree();
        var buttonsNode = tree[0].Children
            .First(c => c.Label.Equals("Atoms", StringComparison.Ordinal)).Children
            .First(c => c.Label.Equals("Buttons", StringComparison.Ordinal));

        // Should have "Primary" (Order:1) and "Disabled" (Order:2)
        Assert.Equal(2, buttonsNode.Children.Count);
        Assert.All(buttonsNode.Children, c => Assert.Equal(SidebarNodeType.Story, c.NodeType));
        Assert.All(buttonsNode.Children, c => Assert.NotNull(c.Story));
    }

    [Fact]
    public void BuildSidebarTree_Stories_Sorted_By_Order_Then_Name()
    {
        var registry = CreateRegistryWithTestStories();

        var tree = registry.BuildSidebarTree();
        var buttonsNode = tree[0].Children
            .First(c => c.Label.Equals("Atoms", StringComparison.Ordinal)).Children
            .First(c => c.Label.Equals("Buttons", StringComparison.Ordinal));

        Assert.Equal("Primary", buttonsNode.Children[0].Label);   // Order: 1
        Assert.Equal("Disabled", buttonsNode.Children[1].Label);  // Order: 2
    }

    [Fact]
    public void FilterStories_Matches_Name_CaseInsensitive()
    {
        var registry = CreateRegistryWithTestStories();

        var results = registry.FilterStories("primary");

        Assert.Single(results);
        Assert.Equal("Primary", results[0].Name);
    }

    [Fact]
    public void FilterStories_Matches_Group_CaseInsensitive()
    {
        var registry = CreateRegistryWithTestStories();

        var results = registry.FilterStories("buttons");

        Assert.Equal(2, results.Count); // Primary + Disabled both in Atoms/Buttons
    }

    [Fact]
    public void FilterStories_Empty_Pattern_Returns_All()
    {
        var registry = CreateRegistryWithTestStories();

        var results = registry.FilterStories(string.Empty);

        Assert.Equal(4, results.Count);
    }

    [Fact]
    public void FilterStories_NoMatch_Returns_Empty()
    {
        var registry = CreateRegistryWithTestStories();

        var results = registry.FilterStories("nonexistent");

        Assert.Empty(results);
    }

    [Fact]
    public void Empty_Registry_BuildSidebarTree_Returns_Empty()
    {
        var registry = new StoryRegistry([], []);

        var tree = registry.BuildSidebarTree();

        Assert.Empty(tree);
    }

    [Fact]
    public void LoadErrors_Are_Accessible()
    {
        var error = new LoadError
        {
            FilePath = "/test/bad.dll",
            ErrorMessage = "Bad format",
            Exception = new BadImageFormatException("Bad format"),
            Timestamp = DateTimeOffset.UtcNow,
        };

        var registry = new StoryRegistry([], [error]);

        Assert.Single(registry.LoadErrors);
        Assert.Equal("/test/bad.dll", registry.LoadErrors[0].FilePath);
    }
}
