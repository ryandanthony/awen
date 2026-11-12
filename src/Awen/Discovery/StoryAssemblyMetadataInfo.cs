// -----------------------------------------------------------------------
// <copyright file="StoryAssemblyMetadataInfo.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Awen.Discovery;

/// <summary>
/// Facts about an assembly file read from its metadata, without loading it.
/// </summary>
/// <param name="IsStoryAssembly">Whether the assembly carries <c>[AwenStoryAssembly]</c>.</param>
/// <param name="AvaloniaVersion">The Avalonia version the assembly was compiled against, or <see langword="null"/> if it does not reference Avalonia.</param>
internal sealed record StoryAssemblyMetadataInfo(bool IsStoryAssembly, Version? AvaloniaVersion);
