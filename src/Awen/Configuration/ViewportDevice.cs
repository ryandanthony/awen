// -----------------------------------------------------------------------
// <copyright file="ViewportDevice.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Awen.Configuration;

/// <summary>
/// Device viewport preset definition.
/// </summary>
public sealed record ViewportDevice
{
    /// <summary>
    /// Gets the device name shown in the viewport dropdown.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Gets the viewport width in logical pixels.
    /// </summary>
    [JsonPropertyName("width")]
    public required double Width { get; init; }

    /// <summary>
    /// Gets the viewport height in logical pixels.
    /// </summary>
    [JsonPropertyName("height")]
    public required double Height { get; init; }

    /// <summary>
    /// Gets a value indicating whether this device is enabled in the UI dropdown.
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool Enabled { get; init; } = true;
}
