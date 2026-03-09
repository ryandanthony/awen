// -----------------------------------------------------------------------
// <copyright file="RelayCommand.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Windows.Input;

namespace Awen.ViewModels;

/// <summary>
/// A lightweight <see cref="ICommand"/> implementation that delegates to an <see cref="Action"/>.
/// </summary>
internal sealed class RelayCommand : ICommand
{
    private readonly Action _execute;

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand"/> class.
    /// </summary>
    /// <param name="execute">The action to execute.</param>
    internal RelayCommand(Action execute)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
    }

    /// <inheritdoc/>
#pragma warning disable CS0067, S108, RCS0025
    event EventHandler? ICommand.CanExecuteChanged
    {
        add { /* ICommand contract — event not used */ }
        remove { /* ICommand contract — event not used */ }
    }
#pragma warning restore CS0067, S108, RCS0025

    /// <inheritdoc/>
    bool ICommand.CanExecute(object? parameter) => true;

    /// <inheritdoc/>
    void ICommand.Execute(object? parameter) => _execute();
}
