// -----------------------------------------------------------------------
// <copyright file="Properties.axaml.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using ExampleUI.Controls;

namespace ExampleUI.Stories.Feedback.AlertBanner.Default;

/// <summary>
/// Properties panel for the default AlertBanner story.
/// </summary>
public sealed partial class Properties : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Properties"/> class.
    /// </summary>
    public Properties()
    {
        InitializeComponent();
        SeverityComboBox.ItemsSource = Enum.GetValues<AlertSeverity>();
    }
}
