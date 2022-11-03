# GodotSharp.SourceGenerators

C# Source Generators for use with the Godot Game Engine (now supports Godot 4!)
* `SceneTree` class attribute:
  * Generates class property for uniquely named nodes
  * Provides strongly typed access to the scene hierarchy (via `_` operator)
* `GodotOverride` method attribute:
  * Allows use of On*, instead of virtual _* overrides
  * (Requires partial method declaration for use with Godot 4.0)
* `Notify` property attribute:
  * Generates boiler plate code, triggering only when values differ
  * (Automagically triggers nested changes for Resource and Resource[])
* NEW: `InputMap` class attribute:
  * Provides strongly typed access to project input actions
* Includes base classes/helpers to create project specific source generators

- Version 2.x supports Godot 4.x; Version 1.x supports Godot 3.x

(See [GodotSharp.BuildingBlocks][1] or local tests for example usage patterns)

[1]: https://github.com/Cat-Lips/GodotSharp.BuildingBlocks
