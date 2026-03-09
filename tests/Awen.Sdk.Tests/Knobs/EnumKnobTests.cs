// -----------------------------------------------------------------------
// <copyright file="EnumKnobTests.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Awen.Sdk.Knobs;

namespace Awen.Sdk.Tests.Knobs;

public class EnumKnobTests
{
    private enum TestColor
    {
        Red,
        Green,
        Blue,
    }

    [Fact]
    public void Constructor_SetsInitialValue()
    {
        var knob = new EnumKnob("Color", TestColor.Red, _ => { });

        Assert.Equal(TestColor.Red, knob.Value);
    }

    [Fact]
    public void Constructor_SetsLabel()
    {
        var knob = new EnumKnob("Color", TestColor.Red, _ => { });

        Assert.Equal("Color", knob.Label);
    }

    [Fact]
    public void Constructor_PopulatesOptions()
    {
        var knob = new EnumKnob("Color", TestColor.Red, _ => { });

        Assert.Equal(3, knob.Options.Count);
        Assert.Contains(TestColor.Red, knob.Options);
        Assert.Contains(TestColor.Green, knob.Options);
        Assert.Contains(TestColor.Blue, knob.Options);
    }

    [Fact]
    public void SetValue_UpdatesValue()
    {
        var knob = new EnumKnob("Color", TestColor.Red, _ => { });

        knob.SetValue(TestColor.Blue);

        Assert.Equal(TestColor.Blue, knob.Value);
    }

    [Fact]
    public void SetValue_InvokesOnChangeCallback()
    {
        Enum? received = null;
        var knob = new EnumKnob("Color", TestColor.Red, v => received = v);

        knob.SetValue(TestColor.Green);

        Assert.Equal(TestColor.Green, received);
    }

    [Fact]
    public void SetValue_WithNonEnumValue_DoesNotChange()
    {
        var knob = new EnumKnob("Color", TestColor.Red, _ => { });

        knob.SetValue("not an enum");

        Assert.Equal(TestColor.Red, knob.Value);
    }

    [Fact]
    public void Constructor_ThrowsOnNullLabel()
    {
        Assert.Throws<ArgumentNullException>(() => new EnumKnob(null!, TestColor.Red, _ => { }));
    }

    [Fact]
    public void Constructor_ThrowsOnNullInitialValue()
    {
        Assert.Throws<ArgumentNullException>(() => new EnumKnob("Label", null!, _ => { }));
    }

    [Fact]
    public void Constructor_ThrowsOnNullOnChange()
    {
        Assert.Throws<ArgumentNullException>(() => new EnumKnob("Label", TestColor.Red, null!));
    }
}
