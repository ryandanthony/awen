// -----------------------------------------------------------------------
// <copyright file="StoryAssemblyMetadata.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace Awen.Discovery;

/// <summary>
/// Reads story-relevant facts from an assembly file without loading it into the process.
/// Lets the scanner skip non-story DLLs and reject stories built for a different Avalonia major
/// version before the runtime fails with an opaque missing-assembly or missing-method error.
/// </summary>
internal static class StoryAssemblyMetadata
{
    private const string SdkNamespace = "Awen.Sdk";
    private const string StoryAttributeName = "AwenStoryAssemblyAttribute";

    /// <summary>
    /// Core Avalonia assemblies versioned in lockstep with the Avalonia release.
    /// Third-party "Avalonia.*" packages are versioned independently, so they are not used.
    /// </summary>
    private static readonly string[] CoreAvaloniaAssemblies = ["Avalonia.Base", "Avalonia.Controls"];

    /// <summary>
    /// Reads metadata for the assembly at <paramref name="path"/>.
    /// </summary>
    /// <param name="path">Path to a DLL.</param>
    /// <returns>The metadata, or <see langword="null"/> when the file is not a managed assembly (e.g. a native library).</returns>
    /// <exception cref="BadImageFormatException">The file is not a valid PE image.</exception>
    internal static StoryAssemblyMetadataInfo? Read(string path)
    {
        using var stream = File.OpenRead(path);
        using var peReader = new PEReader(stream);

        if (!peReader.HasMetadata)
        {
            return null;
        }

        var reader = peReader.GetMetadataReader();
        if (!reader.IsAssembly)
        {
            return null;
        }

        return new StoryAssemblyMetadataInfo(HasStoryAttribute(reader), GetReferencedAvaloniaVersion(reader));
    }

    private static bool HasStoryAttribute(MetadataReader reader)
    {
        foreach (var handle in reader.GetAssemblyDefinition().GetCustomAttributes())
        {
            var constructor = reader.GetCustomAttribute(handle).Constructor;
            if (constructor.Kind != HandleKind.MemberReference)
            {
                continue; // Attribute defined in this assembly, so not the SDK attribute.
            }

            var parent = reader.GetMemberReference((MemberReferenceHandle)constructor).Parent;
            if (parent.Kind != HandleKind.TypeReference)
            {
                continue;
            }

            var type = reader.GetTypeReference((TypeReferenceHandle)parent);
            if (string.Equals(reader.GetString(type.Name), StoryAttributeName, StringComparison.Ordinal)
                && string.Equals(reader.GetString(type.Namespace), SdkNamespace, StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }

    private static Version? GetReferencedAvaloniaVersion(MetadataReader reader)
    {
        Version? highest = null;
        foreach (var handle in reader.AssemblyReferences)
        {
            var reference = reader.GetAssemblyReference(handle);
            var name = reader.GetString(reference.Name);
            var isCoreAvalonia = Array.Exists(
                CoreAvaloniaAssemblies,
                core => string.Equals(name, core, StringComparison.Ordinal));

            if (isCoreAvalonia && (highest is null || reference.Version > highest))
            {
                highest = reference.Version;
            }
        }

        return highest;
    }
}
