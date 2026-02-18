# GodotSharp.SourceGenerators

C# Source Generators for use with the Godot Game Engine

## Notes
 - On GitHub, items marked as [NEW] are only available in pre-release (ie, well tested, but subject to subtle API changes - a good opportunity to test against your own use cases!)
 - Version 2.7 introduces an ever so slight [BREAKING CHANGE] in that scope strings have been replaced with a more definitive enum set.
 - Version 2.7 introduces an ever so slight [BREAKING CHANGE] in that the Autoload class must now be explicitly decorated instead of implicitly generated.
 - Version 2.7 introduces an ever so slight [BREAKING CHANGE] in that identifiers comprised of unicode characters no longer need to be prefixed with `_` and other invalid characters are now removed instead of being replaced with `_`.

## Features
* `SceneTree` class attribute:
  * Provides strongly typed access to the scene hierarchy (via `_` operator)
  * Generates direct access to uniquely named nodes via class properties
  * [NEW] ISceneTree interface for use with generics (GD4 only)
    * Provides static access to TscnFilePath
  * [NEW] IInstantiable interface for use with generics (GD4 only)
    * Provides static Instantiate method
  * [NEW] Custom default scope for uniquely named node properties
    * Specific properties can be overridden with a partial property (GD4 only)
* [NEW] `AnimNames` class attribute (GD4 only):
  * Provides strongly typed access to animation names defined in .tres and .tscn files
* [NEW] `AudioBus` class attribute (GD4 only):
  * Provides strongly typed access to audio bus names and ids
* [NEW] `AutoEnum` enum/class attribute (GD4 only):
  * (enum) Generates efficient Str/Parse for enums
  * (class) Generates enum for static data classes (for editor/network use)
* `Autoload` class attribute:
  * Provide strongly typed access to autoload nodes defined in godot.project
* `CodeComments` class attribute:
  * Provides a nested static class to access property comments from code (useful for in-game tooltips, etc)
* [NEW] `GlobalGroups` class attribute (GD4 only):
  * Provides strongly typed access to global groups defined in godot.project
* `GodotOverride` method attribute:
  * Allows use of On*, instead of virtual _* overrides
  * (Requires partial method declaration for use with Godot 4)
* `InputMap` class attribute:
  * Provides strongly typed access to input actions defined in godot.project
  * Attribute option to replace StringName with your own custom object/handler
* [NEW] `Instantiable` class attribute (GD4 only):
  * Generates configurable static method(s) to instantiate scene
* `LayerNames` class attribute:
  * Provide strongly typed access to layer names defined in godot.project
* `Notify` property attribute:
  * Generates boiler plate code, triggering only when values differ
  * (Automagically triggers nested changes for Resource and Resource[])
* `OnImport` method attribute (GD4 only):
  * Generates default plugin overrides and options to make plugin class cleaner (inherit from OnImportEditorPlugin)
  * (This will be removed in next major release)
* `OnInstantiate` method attribute:
  * Generates a static Instantiate method with matching args that calls attributed method as part of the instantiation process
  * (Also generates a protected constructor to ensure proper initialisation - can be deactivated via attribute)
  * (This will be removed in favour of `Instantiable` in next major release)
* [NEW] `ResourceTree` class attribute (GD4 only):
  * Provides strongly typed access to the resource hierarchy
* [NEW] Generators for `Rpc` methods (GD4 only):
  * Provides strongly typed access to Rpc and RpcId methods
* [NEW] `Shader` class attribute (GD4 only):
  * Provides strongly typed access to shader uniforms
* [NEW] `ShaderGlobals` class attribute (GD4 only):
  * Provides strongly typed access to global shader uniforms defined in godot.project
* [NEW] `Singleton` class attribute (GD4 only):
  * Provides single instance access to data or scene objects
* [NEW] `TR` class attribute (GD4 only):
  * Provides strongly typed access to translation locales and keys (as defined in csv)
* Includes base classes/helpers to create project specific source generators

- Version 1.x supports Godot 3 only
- Version 2.x supports Godot 3 & 4
- Version 3.x will support Godot 4 only

## Table of Contents
- [GodotSharp.SourceGenerators](#godotsharpsourcegenerators)
  - [Notes](#notes)
  - [Features](#features)
  - [Table of Contents](#table-of-contents)
  - [Installation](#installation)
  - [Documentation](#documentation)
    - [`SceneTree`](#scenetree)
    - [`AnimNames`](#animnames)
    - [`AudioBus`](#audiobus)
    - [`AutoEnum`](#autoenum)
    - [`Autoload`](#autoload)
    - [`CodeComments`](#codecomments)
    - [`GlobalGroups`](#globalgroups)
    - [`GodotOverride`](#godotoverride)
    - [`InputMap`](#inputmap)
    - [`Instantiable`](#instantiable)
    - [`LayerNames`](#layernames)
    - [`Notify`](#notify)
    - [`OnImport`](#onimport)
    - [`OnInstantiate`](#oninstantiate)
    - [`ResourceTree`](#resourcetree)
    - [`Rpc`](#rpc)
    - [`Shader`](#shader)
    - [`ShaderGlobals`](#shaderglobals)
    - [`Singleton`](#singleton)
    - [`TR`](#tr)

## Installation
Install via [NuGet](https://www.nuget.org/packages/GodotSharp.SourceGenerators)

## Documentation

### `SceneTree`
  * Class attribute
  * Provides strongly typed access to the scene hierarchy (via `_` operator)
  * Generates direct access class properties for uniquely named nodes
  * (GD4 only) Generates an interface for static tscn retrieval (ISceneTree.TscnFilePath)
  * (GD4 only) Generates an interface for static instantiation (IInstantiable.Instantiate)
  * Note that nodes are cached on first access to avoid interop overhead
  * Advanced options available as attribute arguments:
    * tscnRelativeToClassPath: (default null) Specify path to tscn relative to current class
    * traverseInstancedScenes: (default false) Include instanced scenes in the generated hierarchy
    * root: (default _) Provide alternative to `_` operator (eg, to allow use of C# discard variable)
#### Examples:
```cs
// Attach a C# script on the root node of the scene with the same name
// [SceneTree] will generate the members as the scene hierarchy and TscnFilePath property
[SceneTree]
//[SceneTree(root: "ME")]                       // Use this for alternative to `_`
//[SceneTree("my_scene.tscn")]                  // Use this if tscn has different name
//[SceneTree("../Scenes/MyScene.tscn")]         // Use relative path if tscn located elsewhere
//[SceneTree(traverseInstancedScenes: true)]    // Use this to include instanced scenes in current hierarchy
//[SceneTree(uqScope: Scope.Protected)]         // Use this to specify default scope of uniquely named nodes (default: 'Public') [NEW]
public partial class MyScene : Node
{
    // Default scope of uniquely named nodes can be overridden using partial properties [NEW - GD4 only]
    private partial MyNodeType MyNodeWithUniqueName { get; }

    public override void _Ready() 
    {
        // You can access the node via '_' object
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

// TscnFilePath usage:
public void NextScene()
    => GetTree().ChangeSceneToFile(MyScene.TscnFilePath);
```
#### ISceneTree (GD4 only)
 * Generated for any class decorated with [SceneTree]
```cs
namespace Godot;

public partial interface ISceneTree
{
    static abstract string TscnFilePath { get; }
}
```
Usage:
```cs
public void NextScene<T>() where T : ISceneTree
    => GetTree().ChangeSceneToFile(T.TscnFilePath);
```
#### IInstantiable (GD4 only)
 * Provides a default Instantiate method that uses TscnFilePath
 * Both non-generic and generic versions are available
 * A default Instantiator class is also available
```cs
public partial interface IInstantiable
{
    static T Instantiate<T>() where T : Node, ISceneTree
        => GD.Load<PackedScene>(T.TscnFilePath).Instantiate<T>();
}

public partial interface IInstantiable<T> where T : Node, IInstantiable<T>, ISceneTree
{
    static T Instantiate() => GD.Load<PackedScene>(T.TscnFilePath).Instantiate<T>();
}

public static partial class Instantiator
{
    public static T Instantiate<T>() where T : Node, ISceneTree
        => GD.Load<PackedScene>(T.TscnFilePath).Instantiate<T>();
}
```
Usage:
```cs
[SceneTree]
public partial class Scene1 : Node;

[SceneTree]
public partial class Scene2 : Node, IInstantiable;

[SceneTree]
public partial class Scene3 : Node, IInstantiable<Scene3>;

*****

// Instantiator works for all ISceneTree types
var scene1 = Instantiator.Instantiate<Scene1>();
var scene2 = Instantiator.Instantiate<Scene2>();
var scene3 = Instantiator.Instantiate<Scene3>();

// The non-generic interface can also instantiate any ISceneTree type (but why would you want to)
var scene1 = IInstantiable.Instantiate<Scene1>();
var scene2 = IInstantiable.Instantiate<Scene2>();
var scene3 = IInstantiable.Instantiate<Scene3>();

// The generic interface can only instantiate it's own ISceneTree type (but why would you want to)
var scene3 = IInstantiable<Scene3>.Instantiate();

// Use generics to instantiate specific types
static T Instantiate<T>() where T : Node, ISceneTree, IInstantiable
    => IInstantiable.Instantiate<T>(); // or Instantiator.Instantiate<T>();
var scene2 = Instantiate<Scene2>();

OR

static T Instantiate<T>() where T : Node, ISceneTree, IInstantiable<T>
    => IInstantiable<T>.Instantiate(); // or Instantiator.Instantiate<T>();
var scene3 = Instantiate<Scene3>();

```

### `AnimNames`
  * Class attribute
  * Provides strongly typed access to animation names defined in .tres and .tscn files
  * Supports AnimationLibrary (AnimationPlayer) and SpriteFrames (AnimatedSprite) animation names
  * Supports animations saved to tres or embedded in tscn
  * Supports flat list of names for static classes
  * Advanced options available as attribute arguments:
    * path: (default null) Provide path to tscn/tres if not same folder/same name
#### Examples:
```cs
[SceneTree, AnimNames]
public partial class MyScene : Node;

[AnimNames]
//[AnimNames("path")] // (optional path to tscn/tres)
public static partial class MyAnims;
```
Generates:
```cs
partial class MyScene
{
    public static class AnimName
    {
        public static readonly StringName Anim1 = "Anim1";
        public static readonly StringName Anim2 = "Anim2";
    }
}

public static class MyAnims
{
    public static readonly StringName Anim1 = "Anim1";
    public static readonly StringName Anim2 = "Anim2";
}
```

### `AudioBus`
  * Class attribute
  * Provides strongly typed access to audio bus names and ids
  * Scrapes data from res://default_bus_layout.tres (or other provided path)
  * Advanced options available as attribute arguments:
    * source: (default 'default_bus_layout') relative or absolute resource path
#### Examples:
```cs
[AudioBus]
//[AudioBus("Resources/custom_bus_layout")] // Relative to current C# file or absolute path from project root (res:// prefix and .tres extension optional)
public static partial class AudioBus;
```
Generates:
```cs
static partial class AudioBus
{
    public const int MasterId = 0;
    public const int MusicId = 1;
    public const int FxId = 2;

    public static readonly StringName Master = "Master";
    public static readonly StringName Music = "Music";
    public static readonly StringName Fx = "FX";
}
```

### `AutoEnum`
  * Class/Enum attribute
  * When decorating enum, generates Str/Parse extensions
  * When decorating class, generates enum & conversions for static data
    * Can be used to select enum in editor and translate to data in script or serialise across network
#### Examples:
```cs
// Decorated Enum

[AutoEnum]
public enum MapType
{
    City,
    Corridor,
    Apartment,
}

// Decorated Class

[AutoEnum]
public partial class MapData
{
    private MapData() { }
    public static readonly MapData Outside = new(/* Init data */);
    public static readonly MapData Corridor = new(/* Init data */);
    public static readonly MapData Apartment = new(/* Init data */);

    // Add data fields here
}
```
Generates:
```cs
// For Decorated Enum

static partial class MapTypeExtensions
{
    public static string Str(this MapType e) => e switch
    {
        MapType.Outside => "Outside",
        MapType.Corridor => "Corridor",
        MapType.Apartment => "Apartment",
        _ => throw new ArgumentOutOfRangeException(...)
    };
}

public static class MapTypeStr
{
    public static MapType Parse(string str) => str switch
    {
        "Outside" => MapType.Outside,
        "Corridor" => MapType.Corridor,
        "Apartment" => MapType.Apartment,
        _ => throw new ArgumentOutOfRangeException(...)
    };
}

// For Decorated Class

partial class MapData
{
    public enum Enum
    {
        Outside,
        Corridor,
        Apartment,
    }

    public Enum ToEnum() => this switch
    {
        var x when x == Outside => Enum.Outside,
        var x when x == Corridor => Enum.Corridor,
        var x when x == Apartment => Enum.Apartment,
        _ => throw new ArgumentOutOfRangeException(...)
    };

    public static MapData FromEnum(Enum e) => e switch
    {
        Enum.Outside => Outside,
        Enum.Corridor => Corridor,
        Enum.Apartment => Apartment,
        _ => throw new ArgumentOutOfRangeException(...)
    };

    public string ToStr() => this switch
    {
        var x when x == Outside => "Outside",
        var x when x == Corridor => "Corridor",
        var x when x == Apartment => "Apartment",
        _ => throw new ArgumentOutOfRangeException(...)
    };

    public static MapData FromStr(string str) => str switch
    {
        "Outside" => Outside,
        "Corridor" => Corridor,
        "Apartment" => Apartment,
        _ => throw new ArgumentOutOfRangeException(...)
    };
}
```
Usage:
```cs
// For Decorated Enum

var s = MapType.Outside.Str(); // s = "Outside"
var e = MapTypeStr.Parse(s);   // e = MapType.Outside

// For Decorated Class

var e = MapData.Outside.ToEnum(); // e = MapData.Enum.Outside
var d = MapData.FromEnum(e);      // d = MapData.Outside

var s = MapData.Outside.ToStr(); // s = "Outside"
var d = MapData.FromStr(s);      // d = MapData.Outside
```

### `Autoload`
  * `Autoload` class attribute
    * Provides strongly typed access to autoload nodes defined in editor project settings
    * Supports tscn nodes & gd/cs scripts with C# compatible types inferred wherever possible
  * `AutoloadRename` is an additional attribute that can be used to provide C# friendly names if required
#### Examples:
project.godot:
```project.godot
[autoload]

gd_utils="*res://addons/handy_utils/gd_utils.gd"
cs_utils="*res://addons/silly_sausage/MyUtils.cs"
DebugMenu="*res://addons/debug_menu/debug_menu.tscn"
```
C#:
```cs
[Autoload]
[AutoloadRename("UtilsGD", "gd_utils")]
[AutoloadRename("UtilsCS", "cs_utils")]
public static partial class Autoload;
```
Generates:
```cs
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
  * Advanced options available as attribute arguments:
    * strip: (default "// ") The characters to remove from the start of each line
#### Examples:
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

### `GlobalGroups`
  * Class attribute
  * Provides strongly typed access to global groups defined in godot.project
#### Examples:
```project.godot
# (project.godot)

[global_group]

Group1="Test Group"
Group2="Test Group"
```
with
```cs
[GlobalGroups]
public static partial class GRP;
```
Generates:
```cs
static partial class GRP
{
    public static readonly StringName Group1 = "Group1";
    public static readonly StringName Group2 = "Group2";
}
```

### `GodotOverride`
  * Method attribute
  * Allows use of On*, instead of virtual _* overrides
   * (Requires partial method declaration for use with Godot 4)
  * Advanced options available as attribute arguments:
    * replace: (default false) Skip base call generation (ie, override will replace base)
#### Examples:
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

### `InputMap`
  * Class attribute
  * Provides strongly typed access to input actions defined in godot.project (set via editor)
  * If you want access to built-in actions, see [BuiltinInputActions.cs](https://gist.github.com/qwe321qwe321qwe321/bbf4b135c49372746e45246b364378c4)
  * Advanced options available as attribute arguments:
    * dataType: (default StringName)
#### Examples:
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
Generates:
```cs
// (Godot 3 => string, Godot 4 => StringName)
// (does not provide access to built-in actions)
partial class MyInput
{
    public static readonly StringName MoveLeft = new("move_left");
    public static readonly StringName MoveRight = new("move_right");
    public static readonly StringName MoveUp = new("move_up");
    public static readonly StringName MoveDown = new("move_down");
}

partial class MyGameInput
{
    public static readonly GameInput MoveLeft = new("move_left");
    public static readonly GameInput MoveRight = new("move_right");
    public static readonly GameInput MoveUp = new("move_up");
    public static readonly GameInput MoveDown = new("move_down");
}
```

### `Instantiable`
  * Class attribute
  * Generates configurable static method(s) to instantiate scene
  * Generates configurable constructor to ensure safe construction
  * Advanced options available as attribute arguments:
    * init: (default 'Init') Name of init function
    * name: (default 'New') Name of instantiate function
    * ctor: (default 'Protected') Scope of generated constructor ('None' to skip)
#### Examples:
```cs
[Instantiate]
public partial class Scene1 : Node
{
    // No Init()
}

[Instantiate]
public partial class Scene2 : Node
{
    private void Init()
    private void Init(int arg)
}

[Instantiate(nameof(Initialise), "Instantiate", "private")]
public partial class Scene3 : Node
{
    private void Initialise(int arg1, string arg2, object arg3 = null)
}
```
Generates:
```cs
partial class Scene1
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    private static PackedScene _Scene1 => field ??= GD.Load<PackedScene>("res://Path/To/Scene1.tscn");

    public static Scene1 New() => (Scene1)_Scene1.Instantiate();

    protected Scene1() {}
}

partial class Scene2
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    private static PackedScene _Scene2 => field ??= GD.Load<PackedScene>("res://Path/To/Scene2.tscn");

    public static Scene2 New()
    {
        var scene = (Scene2)_Scene2.Instantiate();
        scene.Init();
        return scene;
    }

    public static Scene2 New(int arg)
    {
        var scene = (Scene2)_Scene2.Instantiate();
        scene.Init(arg);
        return scene;
    }

    protected Scene2() {}
}

partial class Scene3
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    private static PackedScene _Scene3 => field ??= GD.Load<PackedScene>("res://Path/To/Scene3.tscn");

    public static Scene3 Instantiate(int arg1, string arg2, object arg3 = null)
    {
        var scene = (Scene3)_Scene3.Instantiate();
        scene.Initialise(arg1, arg2, arg3);
        return scene;
    }

    private Scene2() {}
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
#### Examples:
```cs
[LayerNames]
public static partial class MyLayers;
```
Generates:
```cs
public partial class MyLayers
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

### `Notify`
  * Property attribute
  * Generates public events ValueChanging/ValueChanged
    * (Automagically triggers nested changes for Resource and Resource[])
  * Events are triggered only if value is different
  * Initial value can be set without triggering event
  * [NEW] Events can be paused
#### Examples:
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

        // [NEW]
        PauseValueEvents = true;
        Value = 3; // No events raised
        PauseValueEvents = false;
    }
}

// Memory allocation note: structs not implementing IEqualityComparer<T> 
// will cause a heap memory allocation every time the property is set as the
// struct is boxed to `object?`.
// Most built-in structs and primitives implement IEqualityComparer<T>, so this is usually
// only an issue for user-defined structs.
// This is also not an issues for classes, as they are already heap allocated.
public struct BadStruct(float val) 
{
    public float Value { get; set; } = val;
}

public struct GoodStruct(float val) : IEquatable<GoodStruct> 
{
    public float Value { get; set; } = val;

    public bool Equals(GoodStruct other)
    {
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is GoodStruct other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(GoodStruct left, GoodStruct right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(GoodStruct left, GoodStruct right)
    {
        return !left.Equals(right);
    }
}

public partial class NotifyAllocationTest : Node 
{
    [Notify] public partial BadStruct AllocatingSet { get; set; }
    [Notify] public partial GoodStruct NonAllocatingSet { get; set; }
    public override void _Ready() 
    {
        base._Ready();
        AllocatingSet = new BadStruct(10); // This will cause a memory allocation.
        NonAllocatingSet = new GoodStruct(10); // This will not.
    }
}
```

### `OnImport`
  * Method attribute (GD4 only)
  * Generates default plugin overrides and options to make plugin class cleaner (inherit from OnImportEditorPlugin)
  * DEPRECATED - (Not that useful unless writing lots of plugins - will be removed next major update)

### `OnInstantiate`
  * Method attribute
  * Generates a static Instantiate method with matching args that calls attributed method as part of the instantiation process
  * (Also generates a protected constructor to ensure proper initialisation - can be deactivated via attribute)
  * Advanced options available as attribute arguments:
    * ctor: (default 'Protected') Scope of generated constructor ('None' to skip)
    * _: Pass a boolean instead of scope if instantiate overrides are required
      * (Yes, this is a hack - Use [Instantiable] class attribute instead)
#### Examples:
```cs
// Initialise can be public or protected if required; args also optional
// Currently assumes tscn is in same folder with same name
public partial class MyScene : Node
{
    [OnInstantiate]
    //[OnInstantiate(Scope.Private)]
    private void Initialise(string myArg1, int myArg2)
        => GD.PrintS("Init", myArg1, myArg2);

    [OnInstantiate(true)] // Use flag for all overrides
    private void Initialise(string myArg1, int myArg2, float myArg3)
        => GD.PrintS("Init", myArg1, myArg2, myArg3);
}
```
Generates:
```cs
partial class MyScene
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    private static PackedScene _MyScene => field ??= GD.Load<PackedScene>("res://Path/To/MyScene.tscn");

    public static MyScene Instantiate(string myArg1, int myArg2)
    {
        var scene = (MyScene)_MyScene.Instantiate();
        scene.Initialise(myArg1, myArg2);
        return scene;
    }

    protected MyScene() {}
}

partial class MyScene
{
    public static MyScene Instantiate(string myArg1, int myArg2, float myArg3)
    {
        var scene = (MyScene)_MyScene.Instantiate();
        scene.Initialise(myArg1, myArg2, myArg3);
        return scene;
    }
}
```
Usage:
```cs
    AddChild(MyScene.Instantiate("str", 3));
    AddChild(MyScene.Instantiate("str", 3, .7f));
```

### `ResourceTree`
  * Class attribute
  * Provides strongly typed access to the resource hierarchy
  * By default, scans files & folders from location of decorated class
  * Advanced options available as attribute arguments:
    * source: relative or absolute path (use `/` as shortcut for `res://`)
    * resg: flags to configure generated output (see examples below)
    * resi: flags to configure extra input (see examples below)
    * xtras: scan for other file types (eg, txt, cfg, etc)
    * xclude: directories to exclude (addons is always excluded)
#### Examples:
```cs
//[ResourceTree]                        // Scan from <classpath>
//[ResourceTree(".")]                   // Scan from <classpath>
//[ResourceTree("/")]                   // Scan from 'res://'
//[ResourceTree("res://")]              // Scan from 'res://'
//[ResourceTree("Assets")]              // Scan from <classpath>/Assets or res://Assets if former not found
//[ResourceTree("/Assets")]             // Scan from res://Assets
//[ResourceTree("./Assets")]            // Scan from <classpath>/Assets
//[ResourceTree("res://Assets")]        // Scan from res://Assets

//[ResourceTree(resg: ResG.LoadRes)]    // Generate strongly typed properties that call GD.Load (default)
//[ResourceTree(resg: ResG.ResPaths)]   // Generate resource paths for files (in addition to or instead of GD.Load)
//[ResourceTree(resg: ResG.DirPaths)]   // Generate resource paths for directories

//[ResourceTree(resg: ResG.All)]                        // Everything
//[ResourceTree(resg: ResG.LoadRes | ResG.ResPaths)]    // Generate nested type with Load method and ResPath property
//[ResourceTree(resg: ResG.ResPaths | ResG.DirPaths)]   // Just paths

//[ResourceTree(resi: ResI.Uid)]        // Include uid files (as uid string)
//[ResourceTree(resi: ResI.Scenes)]     // Include tscn/scn files (as PackedScene)
//[ResourceTree(resi: ResI.Scripts)]    // Include cs/gd files (as CSharpScript/GdScript)

//[ResourceTree(resi: ResI.All)]                        // Include all of the above
//[ResourceTree(resi: ResI.None)]                       // Include none of the above (default)
//[ResourceTree(resi: ResI.Scenes | ResI.Scripts)]      // Just scenes & scripts (or any combination)

//[ResourceTree(xtras: ["cfg", "txt"])] // Include file types not recognised as a Godot resource (these could match those added to export configs)
//[ResourceTree(xclude: ["Tests"])]     // Ignore specified folders
```
#### Generated Output:
**Example 1:**
```cs
[ResourceTree("/", Res.All, ["txt"])]
public static partial class MyRes;
```
Generates:
```
partial class MyRes
{
    public static string ResPath => "res://";                       // -- (Res.DirPaths)

    public static partial class Assets                              // -- (Each folder generates a nested type)
    {
        public static string ResPath => "res://Assets";             // -- (Res.DirPaths)

        public static partial class IconSvg                         // -- (Res.ResPaths | Res.Load - generates nested type)
        {
            public static string ResPath => "res://Assets/icon.svg";
            public static CompressedTexture2D Load() => GD.Load<CompressedTexture2D>(ResPath);
        }

        public static string HelpTxt => "res://Assets/Help.txt";    // -- (xtras - always generated as resource path)

        public static class Tr                                      // -- (Only folders with discoverable resources are generated)
        {
            public static string ResPath => "res://Assets/tr";      // -- (Res.DirPaths)

            public static class TrEnTranslation                     // -- (uses importer generated files instead of raw input file)
            {
                public static string ResPath => "res://Assets/tr/tr.en.translation";
                public static OptimizedTranslation Load() => GD.Load<OptimizedTranslation>(ResPath);
            }

            public static class TrFrTranslation
            {
                public static string ResPath => "res://Assets/tr/tr.fr.translation";
                public static OptimizedTranslation Load() => GD.Load<OptimizedTranslation>(ResPath);
            }
        }
    }

    public static partial class Scenes
    {
        public static string ResPath => "res://Scenes";

        public static class MySceneTscn
        {
            public static string ResPath => "res://Scenes/MyScene.tscn";
            public static PackedScene Load() => GD.Load<PackedScene>(ResPath);
        }

        public static class MySceneGd
        {
            public static string ResPath => "res://Scenes/MyScene.gd";
            public static GDScript Load() => GD.Load<GDScript>(ResPath);
        }

        public static class MySceneCs
        {
            public static string ResPath => "res://Scenes/MyScene.cs";
            public static CSharpScript Load() => GD.Load<CSharpScript>(ResPath);
        }

        public static string MySceneCsUid => "uid://tyjsxc2njtw2";  // -- (Res.Uid)
        public static string MySceneGdUid => "uid://sho6tst545eo";
    }
}
```
**Example 2:**
```cs
[ResourceTree("/")]
public static partial class MyRes;
```
Generates:
```
partial class MyRes
{
    public static partial class Assets
    {
        public static CompressedTexture2D IconSvg => GD.Load<CompressedTexture2D>("res://Assets/icon.svg");

        public static class Tr
        {
            public static OptimizedTranslation TrEnTranslation => GD.Load<OptimizedTranslation>("res://Assets/tr/tr.en.translation");
            public static OptimizedTranslation TrFrTranslation => GD.Load<OptimizedTranslation>("res://Assets/tr/tr.fr.translation");
        }
    }
}
```
**Example 3:**
```cs
[ResourceTree("/", Res.ResPath)]
public static partial class MyRes;
```
Generates:
```
partial class MyRes
{
    public static partial class Assets
    {
        public static string IconSvg => "res://Assets/icon.svg";

        public static class Tr
        {
            public static string TrEnTranslation => "res://Assets/tr/tr.en.translation";
            public static string TrFrTranslation => "res://Assets/tr/tr.fr.translation";
        }
    }
}
```

### `Rpc`
  * Generates strongly typed Rpc/RpcId methods
#### Examples:
```cs
    [Rpc]
    public void FireWeapon()

    [Rpc]
    private void UpdateName(string name)

    [Rpc]
    protected void BuyItem(MyItemEnum item, float price, int count = 1)
```
Generates:
```cs
    public void FireWeaponRpc() => Rpc(MethodName.FireWeapon);
    public void FireWeaponRpcId(long id) => RpcId(id, MethodName.FireWeapon);

    private void UpdateNameRpc(string name) => Rpc(MethodName.UpdateName, name);
    private void UpdateNameRpcId(long id, string name) => RpcId(id, MethodName.UpdateName, name);

    protected void BuyItemRpc(MyItemEnum item, float price, int count = 1) => Rpc(MethodName.BuyItem, (int)item, price, count);
    protected void BuyItemRpcId(long id, MyItemEnum item, float price, int count = 1) => RpcId(id, MethodName.BuyItem, (int)item, price, count);
```

### `Shader`
  * Class attribute
  * Provides strongly typed access to shader uniforms
    * Decorate class to generate wrapper with properties
    * Decorate static class to generate static Get/Set methods
    * Decorate ShaderMaterial if required for .tres script
  * Advanced options available as attribute arguments:
    * source (default null): relative or absolute path to shader file
#### Example shader:
```MyShader.gdshader
uniform int my_int = 7;
uniform float my_float = 7.7;

// Express enum type with comment
//uniform int my_enum : hint_enum(...); // MyEnumType

// Express color type with hint
//uniform vec3 my_color : source_color;

// Parameterised defaults can be scalar or explicit
//uniform vec4 with_scalar_default = vec(7.7);
//uniform vec4 with_explicit_default = vec(7.7, 7.7, 7.7);

// TODO: arrays
// TODO: alternate types (Rect2, Plane, Quaternion for bvec4 & Projection for mat4)
```
#### With decorated class
```cs
[Shader]
//[Shader("Shaders/my_shader")] // Relative or absolute (res:// & .gdshader optional)
public partial class MyShader;
```
Generates:
```cs
partial class MyShader
{
    public const string ShaderPath = "res://Path/To/MyShader.gdshader";
    public static Shader LoadShader() => GD.Load<Shader>(ShaderPath);

    public ShaderMaterial Material { get; private init; }

    public static implicit operator ShaderMaterial(MyShader self) => self.Material;
    public static implicit operator MyShader(ShaderMaterial material) => new(material);

    public MyShader()
    {
        Material = new ShaderMaterial { Shader = LoadShader() };

        MyInt = Default.MyInt;
        MyFloat = Default.MyFloat;
    }

    public MyShader(ShaderMaterial material)
    {
        if (material is null) throw new ArgumentNullException(nameof(material));
        if (material.Shader is null) throw new InvalidOperationException($"MyShader.InitMaterial() - Null Shader Error [Expected: {ShaderPath}]");
        if (material.Shader.ResourcePath != ShaderPath) throw new InvalidOperationException($"MyShader.InitMaterial() - Shader Mismatch Error [Expected: {ShaderPath}, Found: {material.Shader.ResourcePath}]");

        Material = material;
    }

    public static class Default
    {
        public static readonly int MyInt = 7;
        public static readonly float MyFloat = 7.7f;
    }

    public int MyInt
    {
        get => (int)Material.GetShaderParameter(Params.MyInt);
        set => Material.SetShaderParameter(Params.MyInt, value);
    }

    public float MyFloat
    {
        get => (float)Material.GetShaderParameter(Params.MyFloat);
        set => Material.SetShaderParameter(Params.MyFloat, value);
    }

    public static class Params
    {
        public static readonly StringName MyInt = "my_int";
        public static readonly StringName MyFloat = "my_float";
    }
}
```
#### With decorated static class
```cs
[Shader]
//[Shader("Shaders/my_shader")] // Relative or absolute (res:// & .gdshader optional)
public static partial class MyShader;
```
Generates:
```cs
static partial class MyShader
{
    public const string ShaderPath = "res://Path/To/MyShader.gdshader";
    public static Shader LoadShader() => GD.Load<Shader>(ShaderPath);
    public static ShaderMaterial NewMaterial()
    {
        var material = new ShaderMaterial { Shader = LoadShader() };
        InitMaterial(material);
        return material;
    }

    public static void InitMaterial(ShaderMaterial material)
    {
        if (material is null) throw new ArgumentNullException(nameof(material));
        if (material.Shader is null) throw new InvalidOperationException($"MyShader.InitMaterial() - Null Shader Error [Expected: {ShaderPath}]");
        if (material.Shader.ResourcePath != ShaderPath) throw new InvalidOperationException($"MyShader.InitMaterial() - Shader Mismatch Error [Expected: {ShaderPath}, Found: {material.Shader.ResourcePath}]");

        SetMyInt(material, Default.MyInt);
        SetMyFloat(material, Default.MyFloat);
    }

    public static class Default
    {
        public static readonly int MyInt = 7;
        public static readonly float MyFloat = 7.7f;
    }

    public static int GetMyInt(ShaderMaterial material) => (int)material.GetShaderParameter(Params.MyInt);
    public static float GetMyFloat(ShaderMaterial material) => (float)material.GetShaderParameter(Params.MyFloat);

    public static void SetMyInt(ShaderMaterial material, int value) => material.SetShaderParameter(Params.MyInt, value);
    public static void SetMyFloat(ShaderMaterial material, float value) => material.SetShaderParameter(Params.MyFloat, value);

    public static class Params
    {
        public static readonly StringName MyInt = "my_int";
        public static readonly StringName MyFloat = "my_float";
    }
}
```
#### With decorated ShaderMaterial
```cs
[Shader]
//[Shader("Shaders/my_shader")] // Relative or absolute (res:// & .gdshader optional)
public partial class MyShader : ShaderMaterial;
```
Generates:
```cs
partial class MyShader
{
    public const string ShaderPath = "res://Path/To/MyShader.gdshader";
    public static Shader LoadShader() => GD.Load<Shader>(ShaderPath);

    public MyShader()
    {
        Shader = LoadShader();
        MyInt = Default.MyInt;
        MyFloat = Default.MyFloat;
    }

    public static class Default
    {
        public static readonly int MyInt = 1;
        public static readonly float MyFloat = 0.1f;
    }

    public int MyInt
    {
        get => (int)GetShaderParameter(Params.MyInt);
        set => SetShaderParameter(Params.MyInt, value);
    }

    public float MyFloat
    {
        get => (float)GetShaderParameter(Params.MyFloat);
        set => SetShaderParameter(Params.MyFloat, value);
    }

    public static class Params
    {
        public static readonly StringName MyInt = "my_int";
        public static readonly StringName MyFloat = "my_float";
    }
}
```

### `ShaderGlobals`
  * Class attribute
  * Provides strongly typed access to global shader uniforms defined in godot.project
#### Examples:
```project.godot
# (project.godot)

[shader_globals]

my_col={
"type": "color",
"value": Color(0.1, 0.2, 0.3, 0.4)
}
my_pos={
"type": "vec3",
"value": Vector3(0, 0.5, 0)
}
```
with
```cs
[ShaderGlobals]
public static partial class MyShaderGlobals;
```
Generates:
```cs
static partial class MyShaderGlobals
{
    public static class Default
    {
        public static Color MyCol = new Color(0.1f, 0.2f, 0.3f, 0.4f);
        public static Vector3 MyPos = new Vector3(0, 0.5f, 0);
    }

    private static class Name
    {
        public static readonly StringName MyCol = "my_col";
        public static readonly StringName MyPos = "my_pos";
    }

    private static class Value
    {
        public static Color MyCol = Default.MyCol;
        public static Vector3 MyPos = Default.MyPos;
    }

    public static Color MyCol { get => Value.MyCol; set => RenderingServer.GlobalShaderParameterSet(Name.MyCol, Value.MyCol = value); }
    public static Vector3 MyPos { get => Value.MyPos; set => RenderingServer.GlobalShaderParameterSet(Name.MyPos, Value.MyPos = value); }
}
```

### `Singleton`
  * Class attribute
  * Provides single instance access to data or scene objects
  * Staticly created on first use with private constructor
  * If present, invokes an `Init` method on instance creation
  * Advanced options available as attribute arguments:
    * init: (default 'Init') Override name of init function
#### Examples:
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

### `TR`
  * Class attribute
  * Provides strongly typed access to translation locales and keys (as defined in csv)
  * Advanced options available as attribute arguments:
    * source: (default 'res://Assets/tr/tr.csv') Override path to csv (relative or absolute)
    * xtras: (default true) Generate Tr* extension methods for easier formatting
#### Example CSV:
```csv
keys,en,es,ja,_notes
GREET,"Hello, friend!","Hola, amigo!",こんにちは,
ASK,How are you?,Cómo está?,元気ですか,
BYE,Goodbye,Adiós,さようなら,
QUOTE,"""Hello"" said the man.","""Hola"" dijo el hombre.",「こんにちは」男は言いました,

FULL_NAME,My full name is {0} {1},Mi nombre completo es {0} {1},私のフルネームは{0} {1}です。,Example with 2 args
DATE_OF_BIRTH,My date of birth is {0:yyyy-MM-dd},Mi fecha de nacimiento es {0:yyyy-MM-dd},私の生年月日は{0:yyyy-MM-dd}です。,Example with 1 arg
```
with
```cs
[TR]
//[TR(xtras: false)] // (optional flag to skip Tr* extension methods)
//[TR("Assets/tr.csv")] // (optional path to csv, relative to current C# file or absolute path from project root (res:// prefix & .csv extension are optional))
public static partial class TR;
```
Generates:
```cs
static partial class TR
{
    public static partial class Loc
    {
        public const string En = "en";
        public const string Es = "es";
        public const string Ja = "ja";

        public static readonly string[] All = [En, Es, Ja];
    }

    public static partial class Key
    {
        public static readonly StringName Greet = "GREET";
        public static readonly StringName Ask = "ASK";
        public static readonly StringName Bye = "BYE";
        public static readonly StringName Quote = "QUOTE";
        public static readonly StringName FullName = "FULL_NAME";
        public static readonly StringName DateOfBirth = "DATE_OF_BIRTH";

        public static readonly string[] All = [Greet, Ask, Bye, Quote, FullName, DateOfBirth];
    }
}

static partial class TRExtensions
{
    public static string TrGreet(this GodotObject self) => self.Tr(TR.Key.Greet);
    public static string TrAsk(this GodotObject self) => self.Tr(TR.Key.Ask);
    public static string TrBye(this GodotObject self) => self.Tr(TR.Key.Bye);
    public static string TrQuote(this GodotObject self) => self.Tr(TR.Key.Quote);
    public static string TrFullName(this GodotObject self, object arg0, object arg1) => string.Format(self.Tr(TR.Key.FullName), arg0, arg1);
    public static string TrDateOfBirth(this GodotObject self, object arg0) => string.Format(self.Tr(TR.Key.DateOfBirth), arg0);
}
```
Usage:
```cs
TranslationServer.SetLocale(TR.Loc.Es);

GD.Print(this.TrGreet()); // Hola, amigo!
GD.Print(this.Tr(TR.Key.Greet)); // Hola, amigo!

GD.Print(this.TrFullName("Cat", "Lips")); // Mi nombre completo es Cat Lips
```
