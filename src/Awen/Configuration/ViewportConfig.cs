// -----------------------------------------------------------------------
// <copyright file="ViewportConfig.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Awen.Configuration;

/// <summary>
/// Root viewport configuration model.
/// </summary>
public sealed record ViewportConfig
{
    /// <summary>
    /// Gets the configured devices.
    /// </summary>
    [JsonPropertyName("devices")]
    public IReadOnlyList<ViewportDevice> Devices { get; init; } = Array.Empty<ViewportDevice>();
}
