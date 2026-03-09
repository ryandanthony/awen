// -----------------------------------------------------------------------
// <copyright file="BoolKnobEditor.axaml.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;

namespace Awen.Editors;

/// <summary>
/// Editor for <see cref="Awen.Sdk.Knobs.BoolKnob"/> values using a ToggleSwitch.
/// </summary>
public sealed partial class BoolKnobEditor : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BoolKnobEditor"/> class.
    /// </summary>
    public BoolKnobEditor()
    {
        InitializeComponent();
    }
}
