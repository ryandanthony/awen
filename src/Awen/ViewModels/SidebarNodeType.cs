// -----------------------------------------------------------------------
// <copyright file="SidebarNodeType.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Awen.ViewModels;

/// <summary>
/// Classification of a sidebar tree node.
/// </summary>
public enum SidebarNodeType
{
    /// <summary>Top-level library root node.</summary>
    Library,

    /// <summary>A group folder node.</summary>
    Group,

    /// <summary>A leaf story node.</summary>
    Story,
}
