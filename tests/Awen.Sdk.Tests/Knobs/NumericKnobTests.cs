// -----------------------------------------------------------------------
// <copyright file="NumericKnobTests.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Awen.Sdk.Knobs;

namespace Awen.Sdk.Tests.Knobs;

public class NumericKnobTests
{
    [Fact]
    public void Constructor_SetsInitialValue()
    {
        var knob = new NumericKnob("Size", 42.0, _ => { });

        Assert.Equal(42.0, knob.Value);
    }

    [Fact]
    public void Constructor_SetsLabel()
    {
        var knob = new NumericKnob("Font Size", 16.0, _ => { });

        Assert.Equal("Font Size", knob.Label);
    }

    [Fact]
    public void Constructor_SetsMinMaxStep()
    {
        var knob = new NumericKnob("Size", 10.0, _ => { }, minimum: 0.0, maximum: 100.0, step: 0.5);

        Assert.Equal(0.0, knob.Minimum);
        Assert.Equal(100.0, knob.Maximum);
        Assert.Equal(0.5, knob.Step);
    }

    [Fact]
    public void Constructor_DefaultMinMaxStep()
    {
        var knob = new NumericKnob("Size", 10.0, _ => { });

        Assert.Equal(double.MinValue, knob.Minimum);
        Assert.Equal(double.MaxValue, knob.Maximum);
        Assert.Equal(1.0, knob.Step);
    }

    [Fact]
    public void SetValue_UpdatesValue()
    {
        var knob = new NumericKnob("Size", 10.0, _ => { });

        knob.SetValue(25.0);

        Assert.Equal(25.0, knob.Value);
    }

    [Fact]
    public void SetValue_InvokesOnChangeCallback()
    {
        double? received = null;
        var knob = new NumericKnob("Size", 10.0, v => received = v);

        knob.SetValue(20.0);

        Assert.Equal(20.0, received);
    }

    [Fact]
    public void SetValue_ClampsToMinimum()
    {
        var knob = new NumericKnob("Size", 10.0, _ => { }, minimum: 5.0, maximum: 100.0);

        knob.SetValue(2.0);

        Assert.Equal(5.0, knob.Value);
    }

    [Fact]
    public void SetValue_ClampsToMaximum()
    {
        var knob = new NumericKnob("Size", 10.0, _ => { }, minimum: 0.0, maximum: 50.0);

        knob.SetValue(999.0);

        Assert.Equal(50.0, knob.Value);
    }

    [Fact]
    public void SetValue_AcceptsIntValue()
    {
        var knob = new NumericKnob("Size", 10.0, _ => { });

        knob.SetValue(42);

        Assert.Equal(42.0, knob.Value);
    }

    [Fact]
    public void SetValue_AcceptsFloatValue()
    {
        var knob = new NumericKnob("Size", 10.0, _ => { });

        knob.SetValue(3.14f);

        Assert.Equal(3.14, (double)knob.Value!, 2);
    }

    [Fact]
    public void SetValue_WithNonNumericValue_KeepsCurrent()
    {
        var knob = new NumericKnob("Size", 10.0, _ => { });

        knob.SetValue("not a number");

        Assert.Equal(10.0, knob.Value);
    }

    [Fact]
    public void Constructor_ThrowsOnNullLabel()
    {
        Assert.Throws<ArgumentNullException>(() => new NumericKnob(null!, 10.0, _ => { }));
    }

    [Fact]
    public void Constructor_ThrowsOnNullOnChange()
    {
        Assert.Throws<ArgumentNullException>(() => new NumericKnob("Label", 10.0, null!));
    }
}
