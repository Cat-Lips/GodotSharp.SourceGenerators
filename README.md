# GodotSharp.SourceGenerators

C# Source Generators for use with the Godot Game Engine (available via Nuget):
* `SceneTree` class attribute: Provides strongly typed access to the scene hierarchy (via `_` operator)
  * Also generates class property for any uniquely named nodes (ie, Godot 3.5 - GetNode("%MyUniqueNode"))
* `GodotOverride` method attribute: Allows use of On*, instead of virtual _* overrides
* `Notify` field attribute: Generates property and changed event, triggered only when values differ
* Base classes/helpers to create project specific source generators

(See tests for example usage patterns)
