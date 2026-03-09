// -----------------------------------------------------------------------
// <copyright file="TextKnobEditor.axaml.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;

namespace Awen.Editors;

/// <summary>
/// Editor for <see cref="Awen.Sdk.Knobs.TextKnob"/> values using a TextBox.
/// </summary>
public sealed partial class TextKnobEditor : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TextKnobEditor"/> class.
    /// </summary>
    public TextKnobEditor()
    {
        InitializeComponent();
    }
}
