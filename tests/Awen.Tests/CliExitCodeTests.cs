// -----------------------------------------------------------------------
// <copyright file="CliExitCodeTests.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Awen.Tests;

/// <summary>
/// Tests for CLI argument validation and exit codes per contracts/cli.md.
/// Exit code 1: Invalid arguments. Exit code 2: Directory does not exist.
/// Exit codes 0 and 3 are runtime behaviors tested via integration tests.
/// </summary>
public sealed class CliExitCodeTests
{
    [Fact]
    public void MissingDir_Returns_NonZero_ExitCode()
    {
        // Arrange — no --dir provided
        var command = Program.BuildRootCommand([]);
        var parseResult = command.Parse([]);

        // Assert — required option missing produces errors
        Assert.NotEmpty(parseResult.Errors);
    }

    [Fact]
    public void NonExistentDir_Parse_HasErrors()
    {
        // Arrange — directory does not exist
        var command = Program.BuildRootCommand(["--dir", "/tmp/nonexistent-dir-9999"]);
        var parseResult = command.Parse(["--dir", "/tmp/nonexistent-dir-9999"]);

        // Assert — AcceptExistingOnly produces validation error
        Assert.NotEmpty(parseResult.Errors);
    }

    [Fact]
    public void InvalidTheme_Parse_HasErrors()
    {
        // Arrange — invalid theme value
        var dir = Directory.GetCurrentDirectory();
        var command = Program.BuildRootCommand(["--dir", dir, "--theme", "neon"]);
        var parseResult = command.Parse(["--dir", dir, "--theme", "neon"]);

        // Assert — AcceptOnlyFromAmong rejects "neon"
        Assert.NotEmpty(parseResult.Errors);
    }

    [Fact]
    public void ValidArgs_Parse_HasNoErrors()
    {
        // Arrange — valid arguments
        var dir = Directory.GetCurrentDirectory();
        var command = Program.BuildRootCommand(["--dir", dir]);
        var parseResult = command.Parse(["--dir", dir]);

        // Assert — no validation errors
        Assert.Empty(parseResult.Errors);
    }

    [Fact]
    public void ValidArgs_WithThemeDark_Parse_HasNoErrors()
    {
        // Arrange
        var dir = Directory.GetCurrentDirectory();
        var command = Program.BuildRootCommand(["--dir", dir, "--theme", "dark"]);
        var parseResult = command.Parse(["--dir", dir, "--theme", "dark"]);

        // Assert
        Assert.Empty(parseResult.Errors);
    }

    [Fact]
    public void ValidArgs_WithFilter_Parse_HasNoErrors()
    {
        // Arrange
        var dir = Directory.GetCurrentDirectory();
        var command = Program.BuildRootCommand(["--dir", dir, "--filter", "button"]);
        var parseResult = command.Parse(["--dir", dir, "--filter", "button"]);

        // Assert
        Assert.Empty(parseResult.Errors);
    }

    [Fact]
    public void NoWatch_Overrides_Watch()
    {
        // Arrange —  --no-watch should override default --watch=true
        var dir = Directory.GetCurrentDirectory();
        var command = Program.BuildRootCommand(["--dir", dir, "--no-watch"]);
        var parseResult = command.Parse(["--dir", dir, "--no-watch"]);

        // Assert — parses successfully
        Assert.Empty(parseResult.Errors);
        Assert.True(parseResult.GetValue<bool>("--no-watch"));
    }

    [Fact]
    public void Restore_Option_Is_Hidden()
    {
        // Arrange
        var command = Program.BuildRootCommand([]);
        var restoreOption = command.Options
            .FirstOrDefault(o => o.Name.Equals("--restore", StringComparison.Ordinal));

        // Assert
        Assert.NotNull(restoreOption);
        Assert.True(restoreOption.Hidden);
    }
}
