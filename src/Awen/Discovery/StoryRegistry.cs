// -----------------------------------------------------------------------
// <copyright file="StoryRegistry.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Awen.ViewModels;

namespace Awen.Discovery;

/// <summary>
/// Central catalog holding all loaded story assemblies and providing query operations.
/// </summary>
public sealed class StoryRegistry
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StoryRegistry"/> class.
    /// </summary>
    /// <param name="assemblies">Successfully loaded story assemblies.</param>
    /// <param name="loadErrors">Assemblies that failed to load.</param>
    public StoryRegistry(
        IReadOnlyList<StoryAssemblyInfo> assemblies,
        IReadOnlyList<LoadError> loadErrors)
    {
        Assemblies = assemblies;
        LoadErrors = loadErrors;
        AllStories = assemblies
            .SelectMany(a => a.Stories)
            .ToList();
    }

    /// <summary>
    /// Gets all successfully loaded story assemblies.
    /// </summary>
    public IReadOnlyList<StoryAssemblyInfo> Assemblies { get; }

    /// <summary>
    /// Gets a flattened list of all stories across all assemblies.
    /// </summary>
    public IReadOnlyList<StoryDescriptor> AllStories { get; }

    /// <summary>
    /// Gets the assemblies that failed to load.
    /// </summary>
    public IReadOnlyList<LoadError> LoadErrors { get; }

    /// <summary>
    /// Finds a story by its unique identity string (<c>LibraryName/Group/Name</c>).
    /// </summary>
    /// <param name="identity">The identity string to search for.</param>
    /// <returns>The matching story descriptor, or null if not found.</returns>
    public StoryDescriptor? FindByIdentity(string identity)
    {
        return AllStories.FirstOrDefault(
            s => s.Identity.Equals(identity, StringComparison.Ordinal));
    }

    /// <summary>
    /// Constructs a hierarchical sidebar tree from the flat story list.
    /// Tree structure: Library → Group segments → Story leaves.
    /// </summary>
    /// <returns>The root nodes of the sidebar tree.</returns>
    public IReadOnlyList<SidebarTreeNode> BuildSidebarTree()
    {
        var libraryNodes = new Dictionary<string, SidebarTreeNode>(StringComparer.Ordinal);

        foreach (var assembly in Assemblies)
        {
            if (!libraryNodes.TryGetValue(assembly.LibraryName, out var libraryNode))
            {
                libraryNode = new SidebarTreeNode
                {
                    Label = assembly.LibraryName,
                    NodeType = SidebarNodeType.Library,
                    IsExpanded = true,
                };
                libraryNodes[assembly.LibraryName] = libraryNode;
            }

            var sortedStories = assembly.Stories
                .OrderBy(s => s.Order)
                .ThenBy(s => s.Name, StringComparer.Ordinal);

            foreach (var story in sortedStories)
            {
                var currentNode = libraryNode;
                currentNode = EnsureGroupPath(currentNode, story.GroupSegments);

                currentNode.Children.Add(new SidebarTreeNode
                {
                    Label = story.Name,
                    NodeType = SidebarNodeType.Story,
                    Story = story,
                });
            }
        }

        return libraryNodes.Values.ToList();
    }

    private static SidebarTreeNode EnsureGroupPath(SidebarTreeNode parentNode, IReadOnlyList<string> segments)
    {
        var currentNode = parentNode;

        foreach (var segment in segments)
        {
            var existingGroup = currentNode.Children
                .FirstOrDefault(c => c.NodeType == SidebarNodeType.Group
                    && c.Label.Equals(segment, StringComparison.Ordinal));

            if (existingGroup is not null)
            {
                currentNode = existingGroup;
            }
            else
            {
                var groupNode = new SidebarTreeNode
                {
                    Label = segment,
                    NodeType = SidebarNodeType.Group,
                    IsExpanded = true,
                };
                currentNode.Children.Add(groupNode);
                currentNode = groupNode;
            }
        }

        return currentNode;
    }

    /// <summary>
    /// Filters stories by name and group path (case-insensitive substring match).
    /// </summary>
    /// <param name="pattern">The filter pattern.</param>
    /// <returns>Stories matching the pattern.</returns>
    public IReadOnlyList<StoryDescriptor> FilterStories(string pattern)
    {
        if (string.IsNullOrWhiteSpace(pattern))
        {
            return AllStories;
        }

        return AllStories
            .Where(s => s.Name.Contains(pattern, StringComparison.OrdinalIgnoreCase)
                || s.Group.Contains(pattern, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}
