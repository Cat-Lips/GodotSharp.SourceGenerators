# GodotSharp.SourceGenerators

C# Source Generators for use with the Godot Game Engine (supports Godot 4 and .NET 8!)
* `SceneTree` class attribute:
  * Generates class property for uniquely named nodes
  * Provides strongly typed access to the scene hierarchy (via `_` operator)
* `GodotOverride` method attribute:
  * Allows use of On*, instead of virtual _* overrides
  * (Requires partial method declaration for use with Godot 4.0)
* `Notify` property attribute:
  * Generates boiler plate code, triggering only when values differ
  * (Automagically triggers nested changes for Resource and Resource[])
* `InputMap` class attribute:
  * Provides strongly typed access to project input actions
* `CodeComments` class attribute:
  * Provides a nested static class to access property comments from code (useful for in-game tooltips, etc)
* `OnInstantiate` method attribute:
  * Generates a static Instantiate method with matching args that calls attributed method as part of the instantiation process
  * (Also generates a protected constructor to ensure proper initialisation - can be deactivated via attribute)
* `OnImport` method attribute (GD4 only):
  * Generates default plugin overrides and options to make plugin class cleaner (inherit from OnImportEditorPlugin)
* Includes base classes/helpers to create project specific source generators

- Version 2.x supports Godot 4
- Version 1.x supports Godot 3

(See [GodotSharp.BuildingBlocks][1] or local tests for example usage patterns)

[1]: https://github.com/Cat-Lips/GodotSharp.BuildingBlocks

## Table of Content
- [GodotSharp.SourceGenerators](#godotsharpsourcegenerators)
  - [Table of Content](#table-of-content)
  - [Installation](#installation)
  - [Attributes](#attributes)
    - [`SceneTree`](#scenetree)
    - [`GodotOverride`](#godotoverride)
    - [`Notify`](#notify)
    - [`InputMap`](#inputmap)
    - [`CodeComments`](#codecomments)
    - [`OnInstantiate`](#oninstantiate)
    - [`OnImport`](#onimport)
  
## Installation
Install from [NuGet](https://www.nuget.org/packages/GodotSharp.SourceGenerators)

## Attributes

### `SceneTree`
  * class attribute
  * Generates class property for uniquely named nodes
  * Provides strongly typed access to the scene hierarchy (via `_` operator)
```cs
// Attach a C# script on the root node of the scene with the same name.
// [SceneTree] will generate the members as the scene hierarchy.
[SceneTree]
public partial class SceneA : Node2D 
{
    public override void _Ready() 
    {
        // You can access the node via '_' object.
        GD.Print(_.Node1.Node11.Node12.Node121);
        GD.Print(_.Node4.Node41.Node412);
    }
}
```
### `GodotOverride`
  * method attribute
  * Allows use of On*, instead of virtual _* overrides
  * (Requires partial method declaration for use with Godot 4.0)
```cs
public partial class MyNode : Node2D 
{
    // Requires partial method declaration for use with Godot 4.0
    public override partial void _Ready(); 
    
    [GodotOverride]
    protected virtual void OnReady() 
    {
        GD.Print("Ready");   
    }
}
```
  Equivalent with
  ```cs
  public override void _Ready() 
  {
      GD.Print("Ready");
  }
  ```
### `Notify`
  * property attribute
  * Generates boiler plate code, triggering only when values differ
  * (Automagically triggers nested changes for Resource and Resource[])
```cs
public partial class NotifyTest : Node {
    // [Notify] attribute is used to generate a private field _value1, a public event Action Value1Changing, and Value1Changed.
    [Notify]
    public float Value1 
    {
        get => _value1.Get();
        set => _value1.Set(value);
    }

    public override void _Ready() 
    {
        Value1Changing += () => GD.Print("Value1Changing raised before changing the value.");
        Value1Changed += () => GD.Print("Value1Changed raised after changing the value.");

        Value1 = 1; // Raise Value1Changing and Value1Changed
        Value1 = 2; // Raise Value1Changing and Value1Changed
        Value1 = 2;// Not raise any events because the value is the same.
    }
}
```
### `InputMap`
  * class attribute
  * Provides strongly typed access to project input actions
  Declare a class with [InputMap] attribute.
```cs
/// <summary>
/// This class is used to define the input actions in the project.
/// </summary>
[InputMap]
public static partial class InputMapConst { }
```
The following code will be auto-generated by Source Genreator. (no built-in actions)
```cs
partial class InputMapConsts
{
    public static readonly StringName MoveLeft = "move_left";
    public static readonly StringName MoveRight = "move_right";
    public static readonly StringName MoveUp = "move_up";
    public static readonly StringName MoveDown = "move_down";
}

```
### `CodeComments`
  * class attribute
  * Provides a nested static class to access property comments from code (useful for in-game tooltips, etc)
```cs
[CodeComments]
public partial class CodeCommentsTest : Node 
{
    // This a comment for Value1.
    // [CodeComments] only works with Property.
    [Export] public float Value1 { get; set; }

    // Value 2 comment.
    [Export] public float value2;

    public override void _Ready() 
    {
        GD.Print(GetComment(nameof(Value1))); // output: "This a comment for Value1\n[CodeComments] only works with Property."
        GD.Print(GetComment(nameof(value2))); // output: ""
    }
}
```
### `OnInstantiate`
  * method attribute
  * Generates a static Instantiate method with matching args that calls attributed method as part of the instantiation process
  * (Also generates a protected constructor to ensure proper initialisation - can be deactivated via attribute)
### `OnImport`
  * method attribute (GD4 only)
  * Generates default plugin overrides and options to make plugin class cleaner (inherit from OnImportEditorPlugin)
  * Includes base classes/helpers to create project specific source generators
