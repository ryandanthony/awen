// -----------------------------------------------------------------------
// <copyright file="NumericKnobEditor.axaml.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Awen.Sdk.Knobs;
using Awen.ViewModels;

namespace Awen.Editors;

/// <summary>
/// Editor for <see cref="NumericKnob"/> values using a NumericUpDown.
/// </summary>
public sealed partial class NumericKnobEditor : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NumericKnobEditor"/> class.
    /// </summary>
    public NumericKnobEditor()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is KnobDescriptor { Knob: NumericKnob numericKnob })
        {
            var upDown = this.FindControl<NumericUpDown>("PART_NumericUpDown");
            if (upDown is not null)
            {
                upDown.Minimum = (decimal)numericKnob.Minimum;
                upDown.Maximum = (decimal)numericKnob.Maximum;
                upDown.Increment = (decimal)numericKnob.Step;
            }
        }
    }
}
