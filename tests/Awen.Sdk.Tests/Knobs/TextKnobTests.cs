// -----------------------------------------------------------------------
// <copyright file="TextKnobTests.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Awen.Sdk.Knobs;

namespace Awen.Sdk.Tests.Knobs;

public class TextKnobTests
{
    [Fact]
    public void Constructor_SetsInitialValue()
    {
        var knob = new TextKnob("Label", "hello", _ => { });

        Assert.Equal("hello", knob.Value);
    }

    [Fact]
    public void Constructor_SetsLabel()
    {
        var knob = new TextKnob("My Label", "test", _ => { });

        Assert.Equal("My Label", knob.Label);
    }

    [Fact]
    public void SetValue_UpdatesValue()
    {
        var knob = new TextKnob("Label", "initial", _ => { });

        knob.SetValue("updated");

        Assert.Equal("updated", knob.Value);
    }

    [Fact]
    public void SetValue_InvokesOnChangeCallback()
    {
        string? received = null;
        var knob = new TextKnob("Label", "initial", v => received = v);

        knob.SetValue("new value");

        Assert.Equal("new value", received);
    }

    [Fact]
    public void SetValue_WithNull_SetsEmptyString()
    {
        var knob = new TextKnob("Label", "initial", _ => { });

        knob.SetValue(null);

        Assert.Equal(string.Empty, knob.Value);
    }

    [Fact]
    public void Constructor_ThrowsOnNullLabel()
    {
        Assert.Throws<ArgumentNullException>(() => new TextKnob(null!, "value", _ => { }));
    }

    [Fact]
    public void Constructor_ThrowsOnNullInitialValue()
    {
        Assert.Throws<ArgumentNullException>(() => new TextKnob("Label", null!, _ => { }));
    }

    [Fact]
    public void Constructor_ThrowsOnNullOnChange()
    {
        Assert.Throws<ArgumentNullException>(() => new TextKnob("Label", "value", null!));
    }
}
