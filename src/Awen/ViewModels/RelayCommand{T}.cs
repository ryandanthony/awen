// -----------------------------------------------------------------------
// <copyright file="RelayCommand{T}.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Windows.Input;

namespace Awen.ViewModels;

/// <summary>
/// A lightweight <see cref="ICommand"/> implementation that delegates to an <see cref="Action{T}"/> with a typed parameter.
/// </summary>
/// <typeparam name="T">The command parameter type.</typeparam>
internal sealed class RelayCommand<T> : ICommand
{
    private readonly Action<T> _execute;

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
    /// </summary>
    /// <param name="execute">The action to execute with the command parameter.</param>
    internal RelayCommand(Action<T> execute)
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
    void ICommand.Execute(object? parameter)
    {
        if (parameter is T typed)
        {
            _execute(typed);
        }
    }
}
