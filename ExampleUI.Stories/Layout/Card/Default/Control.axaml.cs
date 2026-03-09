// -----------------------------------------------------------------------
// <copyright file="Control.axaml.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;

namespace ExampleUI.Stories.Layout.Card.Default;

/// <summary>
/// Preview control for the default Card story.
/// </summary>
public sealed partial class Control : UserControl
{
    private const double ContentOpacity = 0.8;

    /// <summary>
    /// Initializes a new instance of the <see cref="Control"/> class.
    /// </summary>
    public Control()
    {
        InitializeComponent();

        PART_Card.ContentArea.Content = new TextBlock
        {
            Text = "A classic Italian pasta dish made with eggs, cheese, pancetta, and pepper.",
            TextWrapping = Avalonia.Media.TextWrapping.Wrap,
            Opacity = ContentOpacity,
        };
    }
}
