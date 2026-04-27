// -----------------------------------------------------------------------
// <copyright file="ViewportConfigStore.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Awen.Configuration;

/// <summary>
/// Reads and writes viewport configuration from user file with embedded defaults fallback.
/// </summary>
public sealed class ViewportConfigStore
{
    private const string DefaultEmbeddedResourceName = "Awen.Configuration.viewport-defaults.json";
    private readonly string _userConfigPath;

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    /// <summary>
    /// Gets the default viewport config path under the user profile.
    /// </summary>
    public static string DefaultUserConfigPath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        ".awen",
        "viewport.json");

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewportConfigStore"/> class.
    /// </summary>
    /// <param name="userConfigPath">Optional override for user config path.</param>
    public ViewportConfigStore(string? userConfigPath = null)
    {
        _userConfigPath = string.IsNullOrWhiteSpace(userConfigPath)
            ? DefaultUserConfigPath
            : userConfigPath;
    }

    /// <summary>
    /// Gets the effective user config path used by this store.
    /// </summary>
    public string UserConfigPath => _userConfigPath;

    /// <summary>
    /// Loads viewport config from user file or embedded defaults.
    /// </summary>
    /// <returns>The loaded config.</returns>
    public ViewportConfig Load()
    {
        if (TryReadUserConfig(out var userConfig))
        {
            return userConfig;
        }

        return ReadEmbeddedDefaults();
    }

    /// <summary>
    /// Ensures user config exists by writing embedded defaults when absent.
    /// </summary>
    public void EnsureUserConfigExists()
    {
        if (File.Exists(_userConfigPath))
        {
            return;
        }

        Write(ReadEmbeddedDefaults());
    }

    /// <summary>
    /// Writes config to the default user file location.
    /// </summary>
    /// <param name="config">Config to write.</param>
    public void Write(ViewportConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);

        var directory = Path.GetDirectoryName(_userConfigPath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var json = JsonSerializer.Serialize(config, SerializerOptions);
        File.WriteAllText(_userConfigPath, json);
    }

    private bool TryReadUserConfig(out ViewportConfig config)
    {
        config = new ViewportConfig();

        if (!File.Exists(_userConfigPath))
        {
            return false;
        }

        try
        {
            var json = File.ReadAllText(_userConfigPath);
            var deserialized = JsonSerializer.Deserialize<ViewportConfig>(json, SerializerOptions);
            if (deserialized?.Devices is null)
            {
                return false;
            }

            config = deserialized;
            return true;
        }
#pragma warning disable CA1031 // Catch all so bad user JSON falls back to embedded defaults.
        catch (Exception)
#pragma warning restore CA1031
        {
            return false;
        }
    }

    private static ViewportConfig ReadEmbeddedDefaults()
    {
        var assembly = typeof(ViewportConfigStore).Assembly;

        using var stream = assembly.GetManifestResourceStream(DefaultEmbeddedResourceName)
            ?? throw new InvalidOperationException($"Embedded viewport defaults not found: {DefaultEmbeddedResourceName}");
        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();

        var config = JsonSerializer.Deserialize<ViewportConfig>(json, SerializerOptions);
        if (config?.Devices is null)
        {
            throw new InvalidOperationException("Embedded viewport defaults are invalid.");
        }

        return config;
    }
}
