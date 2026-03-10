---
sidebar_position: 1
---

# What is Awen?

Awen is a **Storybook-like component explorer for Avalonia UI**. It lets you develop, preview, and interact with your Avalonia controls in isolation.

## SDK

A lightweight SDK for defining component stories. Implement the `IStory` interface, register your assemblies with `[AwenStoryAssembly]`, and let Awen discover and render your UI components automatically.

- [IStory Interface](/docs/sdk/istory-interface) — the core contract for stories
- [Story Structure](/docs/sdk/story-structure) — the 5-file convention for organizing stories
- [Assembly Registration](/docs/sdk/assembly-registration) — marking assemblies for discovery and registering themes

## User Interface

A visual explorer for browsing and interacting with your component stories. Navigate by category, see live previews, and iterate on your Avalonia UI components in real time.

- [UI Overview](/docs/ui/overview) — the three-panel layout and how panels connect
- [Sidebar](/docs/ui/sidebar) — hierarchical navigation with search filtering
- [Preview Canvas](/docs/ui/preview-canvas) — live rendering with viewport presets and theme switching
- [Properties Panel](/docs/ui/properties-panel) — interactive editors for story state
- [Log Panel](/docs/ui/log-panel) — diagnostic output and error reporting
- [Hot Reload](/docs/ui/hot-reload) — automatic reload when assemblies are rebuilt

## How It Works

1. You create a **stories assembly** — a .NET class library that references your controls and the Awen SDK
2. Each story implements `IStory<TControl, TStoryProperties>` with a preview wrapper and an editor panel
3. You run `Awen --dir <path>` pointing at your compiled stories
4. Awen discovers stories via reflection and displays them in an interactive UI

## Next Steps

- [Installation](/docs/getting-started/installation) — download or build Awen
- [Quick Start](/docs/getting-started/quick-start) — create your first story
- [CLI Usage](/docs/cli/usage) — all command-line options
- [Examples](/docs/examples/overview) — a complete example project
