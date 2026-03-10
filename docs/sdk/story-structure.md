---
sidebar_position: 3
title: Story Structure
---

# Story Structure

Each story in Awen follows a **5-file convention** inside a folder that mirrors the sidebar hierarchy.

## Folder Layout

```
MyLib.Stories/
  AssemblyInfo.cs                      # [AwenStoryAssembly("My Library")]
  {Category}/
    {ControlName}/
      {Variant}/
        Story.cs                       # IStory implementation (ViewModel)
        Control.axaml / Control.axaml.cs   # Preview wrapper (View)
        Properties.axaml / Properties.axaml.cs # Editor panel (View)
```

### Example

```
ExampleUI.Stories/
  AssemblyInfo.cs
  Buttons/
    PrimaryButton/
      Default/
        Story.cs
        Control.axaml
        Control.axaml.cs
        Properties.axaml
        Properties.axaml.cs
      Disabled/
        Story.cs
        Control.axaml
        Control.axaml.cs
        Properties.axaml
        Properties.axaml.cs
  Feedback/
    AlertBanner/
      Default/
        Story.cs
        Control.axaml
        Control.axaml.cs
        Properties.axaml
        Properties.axaml.cs
```

## The Three Parts

### 1. Story.cs — The ViewModel

The story class is the heart of every story. It:

- Implements `IStory<UserControl, UserControl>` to define metadata and factory methods
- Optionally implements `INotifyPropertyChanged` for interactive stories
- Holds all mutable state as public properties
- Acts as the shared `DataContext` for both the preview and properties panel

```csharp
public sealed class Story : IStory<UserControl, UserControl>, INotifyPropertyChanged
{
    private string _label = "Click Me";
    private bool _isEnabled = true;

    public event PropertyChangedEventHandler? PropertyChanged;

    // IStory metadata — use explicit interface implementation
    string IStory<UserControl, UserControl>.Name => "Default";
    string IStory<UserControl, UserControl>.Group => "Atoms/Buttons";
    int IStory<UserControl, UserControl>.Order => 0;
    string IStory<UserControl, UserControl>.Description => "A styled primary button.";

    // Public properties — bound by both Control and Properties
    public string Label
    {
        get => _label;
        set { _label = value; OnPropertyChanged(); }
    }

    public bool IsEnabled
    {
        get => _isEnabled;
        set { _isEnabled = value; OnPropertyChanged(); }
    }

    // Factory methods
    UserControl IStory<UserControl, UserControl>.CreateControl() => new Control();
    UserControl IStory<UserControl, UserControl>.CreateProperties() => new Properties();

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
```

### 2. Control.axaml — The Preview

A thin wrapper that instantiates your actual control and binds its properties to the Story:

```xml
<UserControl xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:controls="using:MyLib.Controls"
             xmlns:local="using:MyLib.Stories.Buttons.MyButton.Default"
             x:Class="MyLib.Stories.Buttons.MyButton.Default.Control"
             x:DataType="local:Story">
    <controls:MyButton Label="{Binding Label}"
                       IsEnabled="{Binding IsEnabled}"
                       HorizontalAlignment="Center"
                       VerticalAlignment="Center" />
</UserControl>
```

Key points:
- Set `x:DataType="local:Story"` for compiled bindings
- Bind control properties to the Story's public properties
- Use `HorizontalAlignment` / `VerticalAlignment` to center the control in the preview canvas

The code-behind is minimal:

```csharp
public sealed partial class Control : UserControl
{
    public Control() { InitializeComponent(); }
}
```

### 3. Properties.axaml — The Editor Panel

Provides interactive editors using standard Avalonia controls:

```xml
<UserControl xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:local="using:MyLib.Stories.Buttons.MyButton.Default"
             x:Class="MyLib.Stories.Buttons.MyButton.Default.Properties"
             x:DataType="local:Story">
    <StackPanel Spacing="12" Margin="8">
        <StackPanel Spacing="4">
            <TextBlock Text="Label" FontWeight="SemiBold" />
            <TextBox Text="{Binding Label}" />
        </StackPanel>
        <StackPanel Spacing="4">
            <TextBlock Text="Enabled" FontWeight="SemiBold" />
            <ToggleSwitch IsChecked="{Binding IsEnabled}" />
        </StackPanel>
    </StackPanel>
</UserControl>
```

### Common Editor Controls

| Property Type | Avalonia Control | Binding |
|--------------|-----------------|---------|
| `string` | `TextBox` | `Text="{Binding Prop}"` |
| `bool` | `ToggleSwitch` | `IsChecked="{Binding Prop}"` |
| `double` | `NumericUpDown` | `Value="{Binding Prop}"` |
| `enum` | `ComboBox` | `SelectedItem="{Binding Prop}"` |
| Read-only text | `TextBox` | `Text="{Binding Prop, Mode=OneWay}" IsReadOnly="True"` |

For enum properties, populate ComboBox options in code-behind:

```csharp
public sealed partial class Properties : UserControl
{
    public Properties()
    {
        InitializeComponent();
        SeverityComboBox.ItemsSource = Enum.GetValues<AlertSeverity>();
    }
}
```

## Sidebar Mapping

The `Group` property determines where stories appear in the sidebar:

| Group | Sidebar Path |
|-------|-------------|
| `"Atoms/Buttons"` | Library Name > Atoms > Buttons > Story Name |
| `"Molecules/Feedback"` | Library Name > Molecules > Feedback > Story Name |
| `"Themes"` | Library Name > Themes > Story Name |

Multiple stories can share the same group — they'll appear as siblings sorted by `Order`.
