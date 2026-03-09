// -----------------------------------------------------------------------
// <copyright file="PropertyPanel.axaml.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;

namespace Awen.Views;

/// <summary>
/// Code-behind for the property panel that hosts story property editors.
/// </summary>
public sealed partial class PropertyPanel : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyPanel"/> class.
    /// </summary>
    public PropertyPanel()
    {
        InitializeComponent();
    }
}
