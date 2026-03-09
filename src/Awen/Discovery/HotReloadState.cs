// -----------------------------------------------------------------------
// <copyright file="HotReloadState.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Awen.Discovery;

/// <summary>
/// Serializable session state for process-restart hot reload.
/// Written to a temp JSON file before restart, read by the new process.
/// </summary>
public sealed record HotReloadState
{
    /// <summary>
    /// Gets the JSON serializer options shared by all state operations.
    /// </summary>
    public static JsonSerializerOptions SerializerOptions { get; } = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    /// <summary>
    /// Gets the identity string of the selected story (<c>LibraryName/Group/Name</c>).
    /// </summary>
    [JsonPropertyName("selectedStoryIdentity")]
    public string? SelectedStoryIdentity { get; init; }

    /// <summary>
    /// Gets the theme variant name (<c>"light"</c> or <c>"dark"</c>).
    /// </summary>
    [JsonPropertyName("themeVariant")]
    public string? ThemeVariant { get; init; }

    /// <summary>
    /// Gets the custom viewport width.
    /// </summary>
    [JsonPropertyName("viewportWidth")]
    public double? ViewportWidth { get; init; }

    /// <summary>
    /// Gets the custom viewport height.
    /// </summary>
    [JsonPropertyName("viewportHeight")]
    public double? ViewportHeight { get; init; }

    /// <summary>
    /// Gets the current sidebar filter text.
    /// </summary>
    [JsonPropertyName("sidebarFilter")]
    public string? SidebarFilter { get; init; }

    /// <summary>
    /// Serializes the state to a JSON file.
    /// </summary>
    /// <param name="state">The state to serialize.</param>
    /// <param name="filePath">The target file path.</param>
    public static void WriteToFile(HotReloadState state, string filePath)
    {
        ArgumentNullException.ThrowIfNull(state);
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        var json = JsonSerializer.Serialize(state, SerializerOptions);
        File.WriteAllText(filePath, json);
    }

    /// <summary>
    /// Reads and deserializes state from a JSON file.
    /// Returns <c>null</c> if the file does not exist or cannot be parsed.
    /// </summary>
    /// <param name="filePath">The source file path.</param>
    /// <returns>The deserialized state, or <c>null</c>.</returns>
    public static HotReloadState? ReadFromFile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            return null;
        }

        try
        {
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<HotReloadState>(json, SerializerOptions);
        }
#pragma warning disable CA1031 // Catch general exception for resilient deserialization
        catch (Exception)
#pragma warning restore CA1031
        {
            return null;
        }
    }
}
