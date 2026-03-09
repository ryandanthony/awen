// -----------------------------------------------------------------------
// <copyright file="Properties.axaml.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using ExampleUI.Controls;

namespace ExampleUI.Stories.Testing.KnobShowcase.AllKnobs;

/// <summary>
/// Properties panel for the KnobShowcase story.
/// </summary>
public sealed partial class Properties : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Properties"/> class.
    /// </summary>
    public Properties()
    {
        InitializeComponent();
        AlignmentComboBox.ItemsSource = Enum.GetValues<ShowcaseAlignment>();
    }
}
