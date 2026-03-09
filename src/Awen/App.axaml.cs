// -----------------------------------------------------------------------
// <copyright file="App.axaml.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Awen.ViewModels;
using Awen.Views;

namespace Awen;

/// <summary>
/// Avalonia application entry point.
/// </summary>
public sealed class App : Application
{
    /// <summary>
    /// Gets or sets the parsed CLI options. Must be set before <see cref="OnFrameworkInitializationCompleted"/> is called.
    /// </summary>
    internal AwenOptions? Options { get; set; }

    /// <summary>
    /// Gets or sets the original CLI arguments for process restart.
    /// </summary>
    internal string[]? OriginalArgs { get; set; }

    /// <inheritdoc/>
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    /// <inheritdoc/>
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop && Options is not null)
        {
            var viewModel = new MainWindowViewModel(Options, OriginalArgs);
            desktop.MainWindow = new MainWindow
            {
                DataContext = viewModel,
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
