// -----------------------------------------------------------------------
// <copyright file="HeadlessCollection.cs" company="Ryan Anthony">
// Copyright (c) Ryan Anthony. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Awen.Tests;

/// <summary>
/// Headless tests own Avalonia's UI-thread dispatcher while they run. Other tests create controls on xunit
/// threads, which fail Avalonia's thread check if they run at the same time or after the dispatcher is bound
/// to the headless thread. xunit runs non-parallel collections after all parallel ones, so these run last, alone.
/// Use <c>[Collection("Headless Avalonia")]</c> on headless test classes.
/// </summary>
[CollectionDefinition("Headless Avalonia", DisableParallelization = true)]
public sealed class HeadlessCollection : ICollectionFixture<HeadlessSessionFixture>;
