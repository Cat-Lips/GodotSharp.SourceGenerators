# GodotSharp.SourceGenerators

C# Source Generators for use with the Godot Game Engine
- NB: On GitHub, items marked as [NEW] are only available in pre-release
* `SceneTree` class attribute:
  * Provides strongly typed access to the scene hierarchy (via `_` operator)
  * Generates direct access to uniquely named nodes via class properties
  * [NEW] ISceneTree interface for use with generics (GD4 only)
    * Provides static access to TscnFilePath
  * [NEW] IInstantiable interface for use with generics (GD4 only)
    * Provides static Instantiate method
* [NEW] `ResourceTree` class attribute (GD4 only):
  * Provides strongly typed access to the resource hierarchy
* [NEW] `Singleton` class attribute (GD4 only):
  * Provides single instance access to data or scene objects
* [NEW] `Shader` class attribute (GD4 only):
  * Provides strongly typed access to shader uniforms
* [NEW] `AutoEnum` class attribute (GD4 only):
  * Generates enum for static data classes (for editor/network use)
* [NEW] `AutoEnum` enum attribute (GD4 only):
  * Generates efficient Str/Parse for enums
* [NEW] `AudioBus` class attribute (GD4 only):
  * Provides strongly typed access to audio bus names and ids
* [NEW] `AnimNames` class attribute (GD4 only):
  * Provides strongly typed access to animation names defined in .tres and .tscn files
* [NEW] `GlobalGroups` class attribute (GD4 only):
  * Provides strongly typed access to global groups defined in godot.project
* [NEW] `Instantiable` class attribute (GD4 only):
  * Generates configurable static method(s) to instantiate scene
* [NEW] Generators for `Rpc` methods (GD4 only):
  * Provides strongly typed access to Rpc and RpcId methods
* [NEW] `TR` class attribute (GD4 only):
  * Provides strongly typed access to translation locales and keys (as defined in csv)
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
* `Autoload` class attribute:
  * Provide strongly typed access to autoload nodes defined in godot.project
* `CodeComments` class attribute:
  * Provides a nested static class to access property comments from code (useful for in-game tooltips, etc)
* `OnInstantiate` method attribute:
  * Generates a static Instantiate method with matching args that calls attributed method as part of the instantiation process
  * (Also generates a protected constructor to ensure proper initialisation - can be deactivated via attribute)
  * (This will be removed in favour of `Instantiable` in next major release)
* `OnImport` method attribute (GD4 only):
  * Generates default plugin overrides and options to make plugin class cleaner (inherit from OnImportEditorPlugin)
  * (This will be removed in next major release)
* Includes base classes/helpers to create project specific source generators

- Version 1.x supports Godot 3 only
- Version 2.x supports Godot 3 & 4
- Version 3.x will support Godot 4 only
- Post comments/questions/suggestions in the discussion area or raise an issue :)

## Table of Contents
- [GodotSharp.SourceGenerators](#godotsharpsourcegenerators)
  - [Table of Contents](#table-of-contents)
  - [Installation](#installation)
  - [Attributes](#attributes)
    - [`SceneTree`](#scenetree)
    - [`ResourceTree`](#resourcetree)
    - [`Singleton`](#singleton)
    - [`Shader`](#shader)
    - [`AutoEnum`](#autoenum)
    - [`AudioBus`](#audiobus)
    - [`AnimNames`](#animnames)
    - [`GlobalGroups`](#globalgroups)
    - [`Instantiable`](#instantiable)
    - [`Rpc`](#rpc)
    - [`TR`](#tr)
    - [`GodotOverride`](#godotoverride)
    - [`Notify`](#notify)
    - [`InputMap`](#inputmap)
    - [`LayerNames`](#layernames)
    - [`Autoload`](#autoload)
    - [`CodeComments`](#codecomments)
    - [`OnInstantiate`](#oninstantiate)
    - [`OnImport`](#onimport)

## Installation
Install via [NuGet](https://www.nuget.org/packages/GodotSharp.SourceGenerators)

## Attributes

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
public partial class MyScene : Node
{
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

### `ResourceTree`
  * Class attribute
  * Provides strongly typed access to the resource hierarchy
  * By default, scans files & folders from location of decorated class
  * Advanced options available as attribute arguments:
    * source: (default null) relative or absolute path ("", ".", "/" for root)
    * res: flags to configure generated output (see examples below)
    * xtras: (default none) scan for other file types (eg, txt, cfg, etc)
    * xclude: (default none) directories to exclude (addons is always excluded)
#### Examples:
```cs
[ResourceTree] // Scans for resources from class location by default
//[ResourceTree("" or "." or "/")] // Scans for resources from res://
//[ResourceTree("Assets")] // Scans for resources from res://Assets or <classpath>/Assets
//[ResourceTree(res: Res.Uid)] // Include uid files
//[ResourceTree(res: Res.Scenes)] // Include tscn/scn files
//[ResourceTree(res: Res.Scripts)] // Include cs/gd files
//[ResourceTree(res: Res.Load)] // Add methods to load resources by type
//[ResourceTree(res: Res.All)] // All or combine as required (eg, Res.Scenes | Res.Load)
//[ResourceTree(xtras: ["cfg", "txt"])] // Include cfg/txt files
//[ResourceTree(xclude: ["Tests"])] // Ignore Tests folder
public static partial class MyRes;
```
Generates:
```cs
[ResourceTree]
static partial class MyRes
{
    private static StringName _ResPath;
    public static StringName ResPath => _ResPath ??= "res://";

    // Each folder generates a nested type
    public static partial class Assets
    {
        private static StringName _ResPath;
        public static StringName ResPath => _ResPath ??= "res://Assets";

        private static StringName _IconSvg;
        public static StringName IconSvg => _IconSvg ??= "res://Assets/icon.svg";

        // If configured with xtras
        private static StringName _HelpTxt;
        public static StringName HelpTxt => _HelpTxt ??= "res://Assets/Help.txt";

        public static class Tr
        {
            private static StringName _ResPath;
            public static StringName ResPath => _ResPath ??= "res://Assets/tr";

            // If an importer generates files, these are provided rather than the input file as the input file will not be available when project is exported

            private static StringName _TrEnTranslation;
            public static StringName TrEnTranslation => _TrEnTranslation ??= "res://Assets/tr/tr.en.translation";

            private static StringName _TrFrTranslation;
            public static StringName TrFrTranslation => _TrFrTranslation ??= "res://Assets/tr/tr.fr.translation";
        }
    }

    // If nothing is found, folder is ignored
    public static partial class Scenes
    {
        private static StringName _ResPath;
        public static StringName ResPath => _ResPath ??= "res://Scenes";

        // If configured with Res.Scenes
        private static StringName _MySceneTscn;
        public static StringName MySceneTscn => _MySceneTscn ??= "res://Scenes/MyScene.tscn";

        // If configured with Res.Scripts
        private static StringName _MySceneGd;
        public static StringName MySceneGd => _MySceneGd ??= "res://Scenes/MyScene.gd";

        private static StringName _MySceneCs;
        public static StringName MySceneCs => _MySceneCs ??= "res://Scenes/MyScene.cs";

        // If configured with Res.Uid
        private static StringName _MySceneGdUid;
        public static StringName MySceneGdUid => _MySceneGdUid ??= "uid://sho6tst545eo";

        private static StringName _MySceneCsUid;
        public static StringName MySceneCsUid => _MySceneCsUid ??= "uid://tyjsxc2njtw2";
    }
}
```
If configured with Res.Load:
```cs
[ResourceTree]
static partial class MyRes
{
    private static StringName _ResPath;
    public static StringName ResPath => _ResPath ??= "res://";

    // Each folder generates a nested type
    public static partial class Assets
    {
        private static StringName _ResPath;
        public static StringName ResPath => _ResPath ??= "res://Assets";

        // Each file generates a nested type
        public static partial class IconSvg
        {
            private static StringName _ResPath;
            public static StringName ResPath => _ResPath ??= "res://Assets/icon.svg";
            public static CompressedTexture2D Load() => GD.Load<CompressedTexture2D>(ResPath);
        }

        // xtras have no type
        private static StringName _HelpTxt;
        public static StringName HelpTxt => _HelpTxt ??= "res://Assets/Help.txt";

        public static class Tr
        {
            private static StringName _ResPath;
            public static StringName ResPath => _ResPath ??= "res://Assets/tr";

            public static class TrEnTranslation
            {
                private static StringName _ResPath;
                public static StringName ResPath => _ResPath ??= "res://Assets/tr/tr.en.translation";
                public static Translation Load() => GD.Load<Translation>(ResPath);
            }

            public static class TrFrTranslation
            {
                private static StringName _ResPath;
                public static StringName ResPath => _ResPath ??= "res://Assets/tr/tr.fr.translation";
                public static Translation Load() => GD.Load<Translation>(ResPath);
            }
        }
    }

    // If nothing is found, folder is ignored
    public static partial class Scenes
    {
        private static StringName _ResPath;
        public static StringName ResPath => _ResPath ??= "res://Scenes";

        // If configured with Res.Scenes
        public static class MySceneTscn
        {
            private static StringName _ResPath;
            public static StringName ResPath => _ResPath ??= "res://Scenes/MyScene.tscn";
            public static PackedScene Load() => GD.Load<PackedScene>(ResPath);
        }

        // If configured with Res.Scripts
        public static class MySceneGd
        {
            private static StringName _ResPath;
            public static StringName ResPath => _ResPath ??= "res://Scenes/MyScene.gd";
            public static GDScript Load() => GD.Load<GDScript>(ResPath);
        }

        public static class MySceneCs
        {
            private static StringName _ResPath;
            public static StringName ResPath => _ResPath ??= "res://Scenes/MyScene.cs";
            public static CSharpScript Load() => GD.Load<CSharpScript>(ResPath);
        }

        // Res.Uid has no type
        private static StringName _MySceneGdUid;
        public static StringName MySceneGdUid => _MySceneGdUid ??= "uid://sho6tst545eo";

        private static StringName _MySceneCsUid;
        public static StringName MySceneCsUid => _MySceneCsUid ??= "uid://tyjsxc2njtw2";
    }
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

### `Instantiable`
  * Class attribute
  * Generates configurable static method(s) to instantiate scene
  * Generates configurable constructor to ensure safe construction
  * Advanced options available as attribute arguments:
    * init: (default 'Init') Override name of init function
    * name: (default 'New') Override name of instantiation function
    * ctor: (default 'protected') Override scope of generated constructor (null to skip)
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
partial class TR
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
partial class GRP
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
#### Examples:
```cs
[LayerNames]
public static partial class MyLayers;
```
Generates:
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

### `Autoload`
  * `Autoload` is a generated class (ie, not attribute) in Godot namespace
    * Provides strongly typed access to autoload nodes defined in editor project settings
    * Supports tscn nodes & gd/cs scripts with C# compatible types inferred wherever possible
  * `AutoloadRename` is an additional attribute that can be used to provide C# friendly names
eg, for the following autoloads (defined in project.godot):
#### Examples:
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
Generates:
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

### `OnInstantiate`
  * Method attribute
  * Generates a static Instantiate method with matching args that calls attributed method as part of the instantiation process
  * (Also generates a protected constructor to ensure proper initialisation - can be deactivated via attribute)
  * Advanced options available as attribute arguments:
    * ctor: (default "protected") Scope of generated constructor (null, "" or "none" to skip)
#### Examples:
```cs
// Initialise can be public or protected if required; args also optional
// Currently assumes tscn is in same folder with same name
public partial class MyScene : Node
{
    [OnInstantiate]
    private void Initialise(string myArg1, int myArg2)
        => GD.PrintS("Init", myArg1, myArg2);
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
```
Usage:
```cs
    AddChild(MyScene.Instantiate("str", 3));
```

### `OnImport`
  * Method attribute (GD4 only)
  * Generates default plugin overrides and options to make plugin class cleaner (inherit from OnImportEditorPlugin)
  * DEPRECATED - (Not that useful unless writing lots of plugins - will be removed next major update)
