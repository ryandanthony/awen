// -----------------------------------------------------------------------
// <copyright file="Properties.axaml.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using ExampleUI.Controls;

namespace ExampleUI.Stories.Indicators.Badge.Default;

/// <summary>
/// Properties panel for the default Badge story.
/// </summary>
public sealed partial class Properties : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Properties"/> class.
    /// </summary>
    public Properties()
    {
        InitializeComponent();
        VariantComboBox.ItemsSource = Enum.GetValues<BadgeVariant>();
    }
}
