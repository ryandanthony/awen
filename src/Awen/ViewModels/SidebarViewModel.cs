// -----------------------------------------------------------------------
// <copyright file="SidebarViewModel.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Awen.Discovery;

namespace Awen.ViewModels;

/// <summary>
/// ViewModel for the sidebar tree and story selection.
/// </summary>
public sealed class SidebarViewModel : INotifyPropertyChanged
{
    private readonly StoryRegistry _registry;
    private readonly ICommand? _selectStoryCommand;
    private string _filterText = string.Empty;
    private StoryDescriptor? _selectedStory;
    private IReadOnlyList<StoryDescriptor> _filteredStories;

    /// <summary>
    /// Initializes a new instance of the <see cref="SidebarViewModel"/> class.
    /// </summary>
    /// <param name="registry">The story registry to build the tree from.</param>
    /// <param name="initialFilter">Optional initial filter pattern from CLI --filter.</param>
    public SidebarViewModel(StoryRegistry registry, string? initialFilter = null)
    {
        ArgumentNullException.ThrowIfNull(registry);

        _registry = registry;

        var treeNodes = registry.BuildSidebarTree();
        foreach (var node in treeNodes)
        {
            TreeNodes.Add(node);
        }

        _filterText = initialFilter ?? string.Empty;
        _filteredStories = registry.FilterStories(_filterText);
        _selectStoryCommand = new RelayCommand<StoryDescriptor?>(s => SelectedStory = s);
    }

    /// <summary>
    /// Gets the command to select a story from the sidebar.
    /// </summary>
    public ICommand SelectStoryCommand => _selectStoryCommand ?? throw new InvalidOperationException();

    /// <summary>
    /// Gets the hierarchical tree nodes for display.
    /// </summary>
    public ObservableCollection<SidebarTreeNode> TreeNodes { get; } = [];

    /// <summary>
    /// Gets the currently filtered stories.
    /// </summary>
    public IReadOnlyList<StoryDescriptor> FilteredStories => _filteredStories;

    /// <summary>
    /// Gets a value indicating whether the catalog is empty (no stories discovered).
    /// </summary>
    public bool IsEmpty => TreeNodes.Count == 0;

    /// <summary>
    /// Gets or sets the filter text for narrowing visible stories.
    /// </summary>
    public string FilterText
    {
        get => _filterText;
        set
        {
            if (!_filterText.Equals(value, StringComparison.Ordinal))
            {
                _filterText = value;
                _filteredStories = _registry.FilterStories(value);
                UpdateVisibility(TreeNodes, value);
                OnPropertyChanged();
                OnPropertyChanged(nameof(FilteredStories));
            }
        }
    }

    /// <summary>
    /// Gets or sets the currently selected story.
    /// </summary>
    public StoryDescriptor? SelectedStory
    {
        get => _selectedStory;
        set
        {
            if (_selectedStory != value)
            {
                _selectedStory = value;
                UpdateSelectionState(TreeNodes, value);
                OnPropertyChanged();
            }
        }
    }

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private static void UpdateSelectionState(IEnumerable<SidebarTreeNode> nodes, StoryDescriptor? selectedStory)
    {
        foreach (var node in nodes)
        {
            node.IsSelected = node.Story is not null && node.Story == selectedStory;
            UpdateSelectionState(node.Children, selectedStory);
        }
    }

    private static void UpdateVisibility(IEnumerable<SidebarTreeNode> nodes, string filter)
    {
        foreach (var node in nodes)
        {
            UpdateVisibility(node.Children, filter);

            node.IsVisible = node.Story is not null
                ? string.IsNullOrWhiteSpace(filter)
                    || node.Label.Contains(filter, StringComparison.OrdinalIgnoreCase)
                    || (node.Story.Group?.Contains(filter, StringComparison.OrdinalIgnoreCase) ?? false)
                : node.Children.Any(c => c.IsVisible);
        }
    }
}
