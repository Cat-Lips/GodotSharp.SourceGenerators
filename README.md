# GodotSharp.SourceGenerators

C# Source Generators for use with the Godot Game Engine (supports Godot 4!)
* `SceneTree` class attribute:
  * Generates class property for uniquely named nodes
  * Provides strongly typed access to the scene hierarchy (via `_` operator)
* `GodotOverride` method attribute:
  * Allows use of On*, instead of virtual _* overrides
  * (Requires partial method declaration for use with Godot 4.0)
* `Notify` property attribute:
  * Generates boiler plate code, triggering only when values differ
  * (Automagically triggers nested changes for Resource and Resource[])
  * NEW: Inline changed action on setter
* `InputMap` class attribute:
  * Provides strongly typed access to project input actions
* NEW: `CodeComments` class attribute:
  * Provides a nested static class to access property comments from code (useful for in-game tooltips, etc)
* NEW: `OnInstantiate` method attribute:
  * Generates a static Instantiate method with matching args that calls attributed method as part of the instantiation process
  * (Also generates a protected constructor to ensure proper initialisation - can be deactivated via attribute)
* Includes base classes/helpers to create project specific source generators

- Version 2.x supports Godot 4.x

(See [GodotSharp.BuildingBlocks][1] or local tests for example usage patterns)

[1]: https://github.com/Cat-Lips/GodotSharp.BuildingBlocks
