---
sidebar_position: 1
title: Overview
---

# Examples

The Awen repository includes a complete example to demonstrate how to build stories for an Avalonia component library.

## Project Structure

```
examples/
  ExampleUI/                    # The component library
    Controls/
      PrimaryButton.axaml/.cs   # A styled primary button
      AlertBanner.axaml/.cs     # An alert banner with severity levels
      Badge.axaml/.cs           # A status badge
      Card.axaml/.cs            # A content card

  ExampleUI.Stories/            # The stories assembly
    AssemblyInfo.cs             # [AwenStoryAssembly("Example UI")]
    Buttons/
      PrimaryButton/
        Default/                # Interactive story with label, enable, font size
        Disabled/               # Static story showing disabled state
    Feedback/
      AlertBanner/
        Default/                # Story with message, severity, dismissable
    Indicators/
      Badge/
        Default/                # Story with text and variant
    Layout/
      Card/
        Default/                # Story with title, subtitle, shadow, width
    Themes/
      ThemeDemo/
        Default/                # Theme preview story
```

## Running the Examples

From the repository root:

```bash
./run-examples.sh
```

Or manually:

```bash
dotnet build examples/ExampleUI.Stories/ExampleUI.Stories.csproj
dotnet run --project src/Awen -- --dir examples/ExampleUI.Stories/bin/Debug/net10.0/
```

## ExampleUI Controls

### PrimaryButton

A styled action button with configurable label, font size, color, and a click command.

**Story properties:** Label (string), Enabled (bool), Font Size (double), Click Log (read-only)

### AlertBanner

A notification banner with severity-based styling (Info, Success, Warning, Error) and optional dismiss button.

**Story properties:** Message (string), Severity (enum), Dismissable (bool)

### Badge

A small status indicator with text and visual variant.

**Story properties:** Text (string), Variant (enum)

### Card

A content container with title, subtitle, optional shadow, and configurable width.

**Story properties:** Title (string), Subtitle (string), Show Shadow (bool), Card Width (double)
