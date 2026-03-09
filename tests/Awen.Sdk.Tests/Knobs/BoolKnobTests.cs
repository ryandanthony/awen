// -----------------------------------------------------------------------
// <copyright file="BoolKnobTests.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Awen.Sdk.Knobs;

namespace Awen.Sdk.Tests.Knobs;

public class BoolKnobTests
{
    [Fact]
    public void Constructor_SetsInitialValue()
    {
        var knob = new BoolKnob("Label", true, _ => { });

        Assert.True((bool)knob.Value!);
    }

    [Fact]
    public void Constructor_SetsLabel()
    {
        var knob = new BoolKnob("Enabled", false, _ => { });

        Assert.Equal("Enabled", knob.Label);
    }

    [Fact]
    public void SetValue_UpdatesValueToTrue()
    {
        var knob = new BoolKnob("Label", false, _ => { });

        knob.SetValue(true);

        Assert.True((bool)knob.Value!);
    }

    [Fact]
    public void SetValue_UpdatesValueToFalse()
    {
        var knob = new BoolKnob("Label", true, _ => { });

        knob.SetValue(false);

        Assert.False((bool)knob.Value!);
    }

    [Fact]
    public void SetValue_InvokesOnChangeCallback()
    {
        bool? received = null;
        var knob = new BoolKnob("Label", false, v => received = v);

        knob.SetValue(true);

        Assert.True(received);
    }

    [Fact]
    public void SetValue_WithNull_SetsFalse()
    {
        var knob = new BoolKnob("Label", true, _ => { });

        knob.SetValue(null);

        Assert.False((bool)knob.Value!);
    }

    [Fact]
    public void Constructor_ThrowsOnNullLabel()
    {
        Assert.Throws<ArgumentNullException>(() => new BoolKnob(null!, false, _ => { }));
    }

    [Fact]
    public void Constructor_ThrowsOnNullOnChange()
    {
        Assert.Throws<ArgumentNullException>(() => new BoolKnob("Label", false, null!));
    }
}
