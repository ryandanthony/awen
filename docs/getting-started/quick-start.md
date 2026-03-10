---
sidebar_position: 2
title: Quick Start
---

# Quick Start

This guide walks you through creating your first Awen story from scratch.

## 1. Create Your Projects

You need two projects: a **control library** (your Avalonia UI controls) and a **stories assembly** (the stories that showcase them).

```bash
# Control library
dotnet new classlib -n MyControls
cd MyControls
dotnet add package Avalonia
dotnet add package Avalonia.Themes.Fluent
cd ..

# Stories assembly
dotnet new classlib -n MyControls.Stories
cd MyControls.Stories
dotnet add reference ../MyControls/MyControls.csproj
dotnet add package Awen.Sdk
dotnet add package Avalonia
cd ..
```

## 2. Register Your Story Assembly

Create an `AssemblyInfo.cs` in your stories project:

```csharp
using Awen.Sdk;

[assembly: AwenStoryAssembly("My Controls")]
```

The `"My Controls"` string becomes the top-level sidebar node in Awen.

## 3. Write a Story

Stories follow a **5-file convention** inside a folder structure that mirrors the sidebar hierarchy:

```
MyControls.Stories/
  Buttons/
    MyButton/
      Default/
        Story.cs
        Control.axaml
        Control.axaml.cs
        Properties.axaml
        Properties.axaml.cs
```

### Story.cs — The ViewModel

The story class implements `IStory<TControl, TStoryProperties>` and acts as the shared `DataContext` for both the preview and properties panel:

```csharp
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Awen.Sdk;

namespace MyControls.Stories.Buttons.MyButton.Default;

public sealed class Story : IStory<UserControl, UserControl>, INotifyPropertyChanged
{
    private string _label = "Click Me";

    public event PropertyChangedEventHandler? PropertyChanged;

    string IStory<UserControl, UserControl>.Name => "Default";
    string IStory<UserControl, UserControl>.Group => "Atoms/Buttons";
    int IStory<UserControl, UserControl>.Order => 0;
    string IStory<UserControl, UserControl>.Description =>
        "A basic button with a configurable label.";

    public string Label
    {
        get => _label;
        set { _label = value; OnPropertyChanged(); }
    }

    UserControl IStory<UserControl, UserControl>.CreateControl() => new Control();
    UserControl IStory<UserControl, UserControl>.CreateProperties() => new Properties();

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
```

### Control.axaml — The Preview

This wraps your actual control and binds its properties to the Story:

```xml
<UserControl xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:controls="using:MyControls"
             xmlns:local="using:MyControls.Stories.Buttons.MyButton.Default"
             x:Class="MyControls.Stories.Buttons.MyButton.Default.Control"
             x:DataType="local:Story">
    <controls:MyButton Content="{Binding Label}"
                       HorizontalAlignment="Center"
                       VerticalAlignment="Center" />
</UserControl>
```

### Properties.axaml — The Editor Panel

Provides interactive editors for the story's properties:

```xml
<UserControl xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:local="using:MyControls.Stories.Buttons.MyButton.Default"
             x:Class="MyControls.Stories.Buttons.MyButton.Default.Properties"
             x:DataType="local:Story">
    <StackPanel Spacing="12" Margin="8">
        <StackPanel Spacing="4">
            <TextBlock Text="Label" FontWeight="SemiBold" />
            <TextBox Text="{Binding Label}" />
        </StackPanel>
    </StackPanel>
</UserControl>
```

### Code-Behind Files

Both `Control.axaml.cs` and `Properties.axaml.cs` are minimal:

```csharp
using Avalonia.Controls;

namespace MyControls.Stories.Buttons.MyButton.Default;

public sealed partial class Control : UserControl
{
    public Control() { InitializeComponent(); }
}
```

## 4. Build and Run

```bash
# Build your stories
dotnet build MyControls.Stories

# Launch Awen pointing at the stories output
Awen --dir MyControls.Stories/bin/Debug/net10.0/
```

Your control should appear in the sidebar under **My Controls > Atoms > Buttons > Default**.
