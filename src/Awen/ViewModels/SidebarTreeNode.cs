// -----------------------------------------------------------------------
// <copyright file="SidebarTreeNode.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Awen.Discovery;

namespace Awen.ViewModels;

/// <summary>
/// A node in the hierarchical sidebar tree.
/// </summary>
public sealed class SidebarTreeNode : INotifyPropertyChanged
{
    private bool _isExpanded;
    private bool _isSelected;
    private bool _isVisible = true;

    /// <summary>
    /// Gets or sets the display text.
    /// </summary>
    public required string Label { get; set; }

    /// <summary>
    /// Gets or sets the node classification.
    /// </summary>
    public required SidebarNodeType NodeType { get; set; }

    /// <summary>
    /// Gets the collection of child nodes.
    /// </summary>
    public ObservableCollection<SidebarTreeNode> Children { get; } = [];

    /// <summary>
    /// Gets a value indicating whether this node has children (is a group).
    /// </summary>
    public bool IsGroup => Children.Count > 0;

    /// <summary>
    /// Gets or sets the story descriptor. Only present when <see cref="NodeType"/> is <see cref="SidebarNodeType.Story"/>.
    /// </summary>
    public StoryDescriptor? Story { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this node is expanded in the UI.
    /// </summary>
    public bool IsExpanded
    {
        get => _isExpanded;
        set
        {
            if (_isExpanded != value)
            {
                _isExpanded = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this node is currently selected.
    /// </summary>
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this node is visible (passes filter).
    /// </summary>
    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            if (_isVisible != value)
            {
                _isVisible = value;
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
}
