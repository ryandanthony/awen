// -----------------------------------------------------------------------
// <copyright file="EnumKnobEditor.axaml.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Awen.Sdk.Knobs;
using Awen.ViewModels;

namespace Awen.Editors;

/// <summary>
/// Editor for <see cref="EnumKnob"/> values using a ComboBox.
/// </summary>
public sealed partial class EnumKnobEditor : UserControl
{
    private bool _isUpdating;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnumKnobEditor" /> class.
    /// </summary>
    public EnumKnobEditor()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is not KnobDescriptor descriptor || descriptor.Knob is not EnumKnob enumKnob)
        {
            return;
        }

        _isUpdating = true;
        try
        {
            // Clear and repopulate items
            PART_ComboBox.ItemsSource = enumKnob.Options;

            // Find and set the selected index
            var currentValue = enumKnob.Value;
            var selectedIndex = -1;
            for (var i = 0; i < enumKnob.Options.Count; i++)
            {
                if (enumKnob.Options[i].Equals(currentValue))
                {
                    selectedIndex = i;
                }
            }

            PART_ComboBox.SelectedIndex = selectedIndex;

            // Wire up selection changed
            PART_ComboBox.SelectionChanged -= OnSelectionChanged;
            PART_ComboBox.SelectionChanged += OnSelectionChanged;
        }
        finally
        {
            _isUpdating = false;
        }
    }

    private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_isUpdating)
        {
            return;
        }

        if (DataContext is not KnobDescriptor descriptor || descriptor.Knob is not EnumKnob enumKnob)
        {
            return;
        }

        var selectedIndex = PART_ComboBox.SelectedIndex;
        if (selectedIndex >= 0 && selectedIndex < enumKnob.Options.Count)
        {
            descriptor.CurrentValue = enumKnob.Options[selectedIndex];
        }
    }
}
