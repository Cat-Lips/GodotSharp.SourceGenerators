# GodotSharp.SourceGenerators

C# Source Generators for use with the Godot Game Engine (supports Godot 4 and .NET 9!)
* `SceneTree` class attribute:
  * Generates class property for uniquely named nodes
  * Provides strongly typed access to the scene hierarchy (via `_` operator)
  * TscnFilePath for static access to tscn file
* [NEW] `Singleton` class attribute (GD4 only):
  * Provides single instance access to data or scene objects
* [NEW] `AudioBus` class attribute (GD4 only):
  * Provides strongly typed access to audio bus names and ids
* [NEW] `AnimNames` class attribute (GD4 only):
  * Provides strongly typed access to animation names defined in .tres and .tscn files
* [NEW] `GlobalGroups` class attribute (GD4 only):
  * Provides strongly typed access to global groups defined in godot.project
* `GodotOverride` method attribute:
  * Allows use of On*, instead of virtual _* overrides
  * (Requires partial method declaration for use with Godot 4)
* `Notify` property attribute:
  * Generates boiler plate code, triggering only when values differ
  * (Automagically triggers nested changes for Resource and Resource[])
* `InputMap` class attribute:
  * Provides strongly typed access to input actions defined in godot.project
  * Attribute option to replace StringName with your own custom object/handler
* `LayerNames` class attribute:
  * Provide strongly typed access to layer names defined in godot.project
* `Autoload`/`AutoloadRename` class attribute:
  * Provide strongly typed access to autoload nodes defined in godot.project
* `CodeComments` class attribute:
  * Provides a nested static class to access property comments from code (useful for in-game tooltips, etc)
* `OnInstantiate` method attribute:
  * Generates a static Instantiate method with matching args that calls attributed method as part of the instantiation process
  * (Also generates a protected constructor to ensure proper initialisation - can be deactivated via attribute)
* `OnImport` method attribute (GD4 only):
  * Generates default plugin overrides and options to make plugin class cleaner (inherit from OnImportEditorPlugin)
* Includes base classes/helpers to create project specific source generators

- Version 1.x supports Godot 3 only
- Version 2.x supports Godot 3 & 4
- Version 3.x will support Godot 4 only
  - `Notify` could be improved
  - `OnImport` will be removed
  - `SceneTree` could be simplified
  - Post comments/questions/suggestions in the discussion area
    - eg, should `_` operator be replaced to avoid conflict with C# discard operator?

## Table of Contents
- [GodotSharp.SourceGenerators](#godotsharpsourcegenerators)
  - [Table of Contents](#table-of-contents)
  - [Installation](#installation)
  - [Attributes](#attributes)
    - [`SceneTree`](#scenetree)
    - [`Singleton`](#singleton)
    - [`AudioBus`](#audiobus)
    - [`AnimNames`](#animnames)
    - [`GlobalGroups`](#globalgroups)
    - [`GodotOverride`](#godotoverride)
    - [`Notify`](#notify)
    - [`InputMap`](#inputmap)
    - [`LayerNames`](#layernames)
    - [`Autoload`/`AutoloadRename`](#autoload/autoloadrename)
    - [`CodeComments`](#codecomments)
    - [`OnInstantiate`](#oninstantiate)
    - [`OnImport`](#onimport)

## Installation
Install via [NuGet](https://www.nuget.org/packages/GodotSharp.SourceGenerators)

## Attributes

### `SceneTree`
  * Class attribute
  * Generates class properties for uniquely named nodes
  * Generates a static property to retrieve tscn resource (TscnFilePath)
  * Provides strongly typed access to the scene hierarchy (via `_` operator)
  * Nodes are cached on first retrieval to avoid interop overhead
  * Advanced options available as attribute arguments
    * tscnRelativeToClassPath: (default null) Specify path to tscn relative to current class
    * traverseInstancedScenes: (default false) Include instanced scenes in the generated hierarchy
    * root: (default _) Provide alternative to `_` operator (eg, to allow use of C# discard variable)
```cs
// Attach a C# script on the root node of the scene with the same name.
// [SceneTree] will generate the members as the scene hierarchy.
[SceneTree]
//[SceneTree(root: "ME")]                       // Use this for alternative to `_`
//[SceneTree("my_scene.tscn")]                  // Use this if tscn has different name
//[SceneTree("../Scenes/MyScene.tscn")]         // Use relative path if tscn located elsewhere
//[SceneTree(traverseInstancedScenes: true)]    // Use this to include instanced scenes in current hierarchy
public partial class MyScene : Node2D 
{
    public override void _Ready() 
    {
        // You can access the node via '_' object.
        GD.Print(_.Node1.Node11.Node12.Node121);
        GD.Print(_.Node4.Node41.Node412);

        // You can also directly access nodes marked as having a unique name in the editor
        GD.Print(MyNodeWithUniqueName);
        GD.Print(_.Path.To.MyNodeWithUniqueName); // Long equivalent

        // Only leaf nodes are Godot types (call .Get() on branch nodes)
        // Lets say you have _.Node1.Node2, observe the following code
        GD.Print(_.Node1.Name); // invalid
        GD.Print(_.Node1.Get().Name); // valid
        Node node1 = _.Node1; // implicit conversion also possible!
        GD.Print(node1.Name); // valid
        GD.Print(_.Node1.Node2.Name); // valid
    }
}

...

// (elsewhere)
public void NextScene()
    => GetTree().ChangeSceneToFile(MyScene.TscnFilePath);
```

### `Singleton`
  * Class attribute
  * Provides single instance access to data or scene objects
  * Staticly created on first use with private constructor
  * If present, Invokes an `Init` method on instance creation
  * Advanced options available as attribute arguments
    * init: (default 'Init') Override name of default init function
```cs
[Singleton] // no tscn
public partial class MyData;

[Singleton] // no tscn, with Init function
public partial class MyNode : Node
{
    private void Init() { }
}

[Singleton(nameof(InitScene))] // with tscn (same folder, same name) and init override
public partial class MyScene : Node
{
    private void InitScene() { }
}
```
Generates:
```cs
partial class MyData
{
    public static MyData Instance { get; } = new();
    private MyData() { }
}

partial class MyNode
{
    public static MyNode Instance { get; } = Init(new());
    [EditorBrowsable(EditorBrowsableState.Never)] private static MyNode Init(MyNode x) { x.Init(); return x; }
    private MyNode() { }
}

partial class MyScene
{
    public static MyScene Instance { get; } = InitScene((MyScene)GD.Load<PackedScene>("res://PathTo/MyScene.tscn").Instantiate());
    [EditorBrowsable(EditorBrowsableState.Never)] private static MyScene InitScene(MyScene x) { x.InitScene(); return x; }
    private MyScene() { }
}
```

### `AudioBus`
  * Class attribute
  * Provides strongly typed access to audio bus names and ids
  * Scrapes data from res://default_bus_layout.tres (or other provided path)
  * Advanced options available as attribute arguments
    * source: (default 'default_bus_layout') relative or absolute resource path
```cs
[AudioBus]
//[AudioBus("Resources/custom_bus_layout")] // Relative to current C# file or absolute path from project root (res:// prefix or .tres extension optional)
public static partial class AudioBus;
```
Generates:
```cs
partial class AudioBus
{
    public const int MasterId = 0;
    public const int MusicId = 1;
    public const int FxId = 2;

    public static readonly StringName Master = "Master";
    public static readonly StringName Music = "Music";
    public static readonly StringName Fx = "FX";
}
```

### `AnimNames`
  * Class attribute
  * Provides strongly typed access to animation names defined in .tres and .tscn files
  * Supports AnimationLibrary (AnimationPlayer) and SpriteFrames (AnimatedSprite) animation names
  * Supports animations saved to tres or embedded in tscn
  * Advanced options available as attribute arguments:
    * path: (default null) Provide path to tscn/tres if not same folder/same name
```cs
// MyAnims.tres (AnimLib or SpriteFrames)
//  - Anim1
//  - Anim2
// MyAnims.cs (ie, same folder, same name)

[AnimNames]
//[AnimNames("path")] // (optional path to tscn/tres)
public static partial class MyAnims;
```
Generates:
```cs
partial class MyAnims
{
    public static readonly StringName Anim1 = "Anim1";
    public static readonly StringName Anim2 = "Anim2";
}
```
```cs
// MyScene.tscn (with embedded AnimLib or SpriteFrames)
//  - Anim1
//  - Anim2
// MyScene.cs (ie, same folder, same name)

[SceneTree, AnimNames] // Anims can be defined here
public partial class MyScene : Node
{
    [AnimNames] private static partial class MyAnims; // Or nested here
}
```
Generates:
```cs
partial class MyScene
{
    public static readonly StringName Anim1 = "Anim1";
    public static readonly StringName Anim2 = "Anim2";

    partial class MyAnims
    {
        public static readonly StringName Anim1 = "Anim1";
        public static readonly StringName Anim2 = "Anim2";
    }
}
```

### `GlobalGroups`
  * Class attribute
  * Provides strongly typed access to global groups defined in godot.project
```project.godot
# (project.godot)

[global_group]

Group1="Test Group"
Group2="Test Group"
```
```cs
[GlobalGroups]
public static partial class GRP;
```
Generates:
```cs
partial class GRP
{
    public static readonly StringName Group1 = "Group1";
    public static readonly StringName Group2 = "Group2";
}
```
Alternatively, 
```cs
[SceneTree]
public partial class MyScene : Node
{
    [GlobalGroups] private static partial class GRP;
}
```
Generates:
```cs
partial class MyScene
{
    partial class GRP
    {
        public static readonly StringName Group1 = "Group1";
        public static readonly StringName Group2 = "Group2";
    }
}
```

### `GodotOverride`
  * Method attribute
  * Allows use of On*, instead of virtual _* overrides
  * (Requires partial method declaration for use with Godot 4)
  * Advanced options available as attribute arguments
    * replace: (default false) Skip base call generation (ie, override will replace base)
```cs
public partial class MyNode : Node2D
{
    [GodotOverride]
    protected virtual void OnReady()
        => GD.Print("Ready");

    [GodotOverride(replace: true)]
    private void OnProcess(double delta)
        => GD.Print("Processing");

    // Requires partial method declaration for use with Godot 4
    public override partial void _Ready(); 
    public override partial void _Process(double delta); 
}
```
Generates:
```cs
    public override void _Ready()
    {
        base._Ready();
        OnReady();
    }

    public override _Process(double delta)
        => OnProcess(delta);
```
### `Notify`
  * Property attribute
  * Generates public events ValueChanged & ValueChanging
    * (Automagically triggers nested changes for Resource and Resource[])
  * Events are triggered only if value is different
  * Initial value can be set without triggering event
```cs
public partial class NotifyTest : Node
{
    // Recommended usage: Partial properties were introduced in C# 13
    [Notify] public partial int Value { get; set; }

    // Original usage
    [Notify] public float Value1 { get => _value1.Get(); set => _value1.Set(value); }

    // Original usage with private changed event handler
    [Notify] public float Value2 { get => _value2.Get(); set => _value2.Set(value, OnValue2Changed); }
    private void OnValue2Changed() { GD.Print("Value2 has changed"); }

    // Incorrect usage: Must use partial or implement get/set as above
    [Notify] public int Value3 { get; set; }

    public NotifyTest()
    {
        // Optional: Set default values in constructor
        InitValue(7); // Set initial value without triggering events
        Value = 7; // Or set directly to trigger events
    }

    public override void _Ready()
    {
        ValueChanging += () => GD.Print($"Value is about to change from {Value}");
        ValueChanged += () => GD.Print($"Value has been changed to {Value}");

        // You can also subscribe to private events if needed
        // These will always be called before public facing events
        // This might be useful if you need to reset public listeners
        //_value.Changing += OnValueChanging;
        //_value.Changed += OnValueChanged;

        Value = 1; // Raises changing/changed events
        Value = 2; // Raises changing/changed events
        Value = 2; // No events are raised since value is the same
    }
}
```
### `InputMap`
  * Class attribute
  * Provides strongly typed access to input actions defined in godot.project (set via editor)
  * If you want access to built-in actions, see [BuiltinInputActions.cs](https://gist.github.com/qwe321qwe321qwe321/bbf4b135c49372746e45246b364378c4)
  * Advanced options available as attribute arguments
    * dataType: (default StringName)
```cs
[InputMap]
public static partial class MyInput;

[InputMap(nameof(GameInput))]
public static partial class MyGameInput;

// Example custom input action class
public class GameInput(StringName action)
{
    public StringName Action => action;

    public bool IsPressed => Input.IsActionPressed(action);
    public bool IsJustPressed => Input.IsActionJustPressed(action);
    public bool IsJustReleased => Input.IsActionJustReleased(action);
    public float Strength => Input.GetActionStrength(action);

    public void Press() => Input.ActionPress(action);
    public void Release() => Input.ActionRelease(action);
}
```
Equivalent (for defined input actions) to:
```cs
// (static optional)
// (string rather than StringName for Godot 3)
// (does not provide access to built-in actions)
partial static class MyInput
{
    public static readonly StringName MoveLeft = new("move_left");
    public static readonly StringName MoveRight = new("move_right");
    public static readonly StringName MoveUp = new("move_up");
    public static readonly StringName MoveDown = new("move_down");
}

partial static class MyGameInput
{
    public static readonly GameInput MoveLeft = new("move_left");
    public static readonly GameInput MoveRight = new("move_right");
    public static readonly GameInput MoveUp = new("move_up");
    public static readonly GameInput MoveDown = new("move_down");
}
```
### `LayerNames`
  * Class attribute
  * Provides strongly typed access to layer names defined in godot.project (set via editor)
  * WARNING: In Godot 3 all layer helper functions start from 0 instead of 1:
    - `Camera3D.GetCullMaskBit(x - 1)`
    - `VisualInstance.GetLayerMaskBit(x - 1)`
    - `CollisionObject.GetCollisionMaskBit(x - 1)`
    - `CollisionObject.GetCollisionLayerBit(x - 1)`
    - `CollisionObject2D.GetCollisionMaskBit(x - 1)`
    - `CollisionObject2D.GetCollisionLayerBit(x - 1)`
  * In Godot 4 this **only** applies to visibility/cull layer functions (which are also uint):
    - `Camera3D.GetCanvasCullMaskBit((uint)x - 1)`
    - `VisualInstance.GetVisibilityLayerBit((uint)x - 1)`
```cs
[LayerNames]
public static partial class MyLayers;
```
Equivalent (for defined layers) to:
```cs
// (static optional)
public static partial class MyLayers
{
    public static class Render2D
    {
        public const int MyLayer1 = 1;
        public const int MyLayer2 = 2;
        public const int MyLayer7 = 7;
        public const int _11reyaLyM = 11; // prefixed with underscore if required

        public static class Mask
        {
            public const uint MyLayer1 = 1u << 0;
            public const uint MyLayer2 = 1u << 1;
            public const uint MyLayer7 = 1u << 6;
            public const uint _11reyaLyM = 1u << 10;
        }
    }

    // Repeat for Render3D, Physics2D, Physics3D, Navigation2D, Navigation3D, Avoidance
}
```
### `Autoload`/`AutoloadRename`
  * `Autoload` is a generated class (ie, not attribute) in Godot namespace
    * Provides strongly typed access to autoload nodes defined in editor project settings
    * Supports tscn nodes & gd/cs scripts with C# compatible types inferred wherever possible
  * `AutoloadRename` is an additional attribute that can be used to provide C# friendly names
#### Examples:
For the following autoloads (defined in project.godot):
```project.godot
[autoload]

gd_utils="*res://addons/handy_utils/gd_utils.gd"
cs_utils="*res://addons/silly_sausage/MyUtils.cs"
DebugMenu="*res://addons/debug_menu/debug_menu.tscn"
```
With the following renames (optionally defined in your project):
```cs
namespace Godot;

[AutoloadRename("UtilsGD", "gd_utils")]
[AutoloadRename("UtilsCS", "cs_utils")]
static partial class Autoload;
```
The following class is generated:
```cs
namespace Godot;

static partial class Autoload
{
    private static Node root = (Engine.GetMainLoop() as SceneTree)?.Root;

    /// <summary>Autoload: gd_utils</summary>
    public static Node UtilsGD => field ??= root?.GetNodeOrNull<Node>("gd_utils");

    /// <summary>Autoload: cs_utils</summary>
    public static MyUtils UtilsCS => field ??= root?.GetNodeOrNull<MyUtils>("cs_utils");

    /// <summary>Autoload: DebugMenu</summary>
    public static CanvasLayer DebugMenu => field ??= root?.GetNodeOrNull<CanvasLayer>("DebugMenu");
}
```
### `CodeComments`
  * Class attribute
  * Provides a nested static class to access property comments from code
  * Advanced options available as attribute arguments
    * strip: (default "// ") The characters to remove from the start of each line
```cs
[CodeComments]
public partial class CodeCommentsTest : Node
{
    // This a comment for Value1
    // [CodeComments] only works with Property
    [Export] public float Value1 { get; set; }

    // Value 2 is a field so no comment
    [Export] public float value2;

    public override void _Ready() 
    {
        GD.Print(GetComment(nameof(Value1))); // output: "This a comment for Value1\n[CodeComments] only works with Property"
        GD.Print(GetComment(nameof(value2))); // output: "" (No output for fields, but could be added if needed)
    }
}
```
### `OnInstantiate`
  * Method attribute
  * Generates a static Instantiate method with matching args that calls attributed method as part of the instantiation process
  * (Also generates a protected constructor to ensure proper initialisation - can be deactivated via attribute)
  * Advanced options available as attribute arguments
    * ctor: (default "protected") Scope of generated constructor (null, "" or "none" to skip)
```cs
// Initialise can be public or protected if required; args also optional
// Currently assumes tscn is in same folder with same name
// Obviously only valid for root nodes
public partial class MyScene : Node
{
    [OnInstantiate]
    private void Initialise(string myArg1, int myArg2)
        => GD.PrintS("Init", myArg1, myArg2);
}
```
Generates (simplified):
```cs
    private static PackedScene __scene__;
    private static PackedScene __Scene__ => __scene__ ??= GD.Load<PackedScene>("res://Path/To/MyScene.tscn");

    public static MyScene Instantiate(string myArg1, int myArg2)
    {
        var scene = __Scene__.Instantiate<MyScene>();
        scene.Initialise(myArg1, myArg2);
        return scene;
    }

    private Test3Arg() {}
```
Usage:
```cs
    // ... in some class
    private void AddSceneToWorld()
    {
        var myScene = MyScene.Instantiate("str", 3);
        MyWorld.AddChild(myScene);
    }
```
### `OnImport`
  * Method attribute (GD4 only)
  * Generates default plugin overrides and options to make plugin class cleaner (inherit from OnImportEditorPlugin)
  * Includes base classes/helpers to create project specific source generators
  * (Not that useful unless writing lots of plugins - will be removed in v3)
