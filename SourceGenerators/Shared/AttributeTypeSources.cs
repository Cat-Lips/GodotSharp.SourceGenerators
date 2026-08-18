using System.Reflection;

namespace GodotSharp.SourceGenerators;

internal static class AttributeTypeSources
{
    public static readonly string AnimNamesAttribute = """
namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class AnimNamesAttribute(string source = null, [System.Runtime.CompilerServices.CallerFilePath] string classPath = null) : Attribute
{
    public string Source { get; } = source;
    public string ClassPath { get; } = classPath;
}
""".Trim();

    public static readonly string AudioBusAttribute = """
namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class AudioBusAttribute(string source = "default_bus_layout") : Attribute
{
    public string Source { get; } = source;
}
""".Trim();

    public static readonly string AutoEnumAttribute = """
namespace Godot;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum)]
public sealed class AutoEnumAttribute : Attribute;
""".Trim();

    public static readonly string AutoloadAttribute = """
namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class AutoloadAttribute() : Attribute;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class AutoloadRenameAttribute(string DisplayName, string GodotName) : Attribute
{
    public string DisplayName { get; } = DisplayName;
    public string GodotName { get; } = GodotName;
}
""".Trim();

    public static readonly string CodeCommentsAttribute = """
namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class CodeCommentsAttribute : Attribute
{
    public CodeCommentsAttribute(string strip = "// ")
        => Strip = strip;

    public string Strip { get; }
}
""".Trim();

    public static readonly string GlobalGroupsAttribute = """
namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class GlobalGroupsAttribute([System.Runtime.CompilerServices.CallerFilePath] string classPath = null) : Attribute
{
    public string ClassPath { get; } = classPath;
}
""".Trim();

    public static readonly string GodotOverrideAttribute = """
namespace Godot;

[AttributeUsage(AttributeTargets.Method)]
public sealed class GodotOverrideAttribute : Attribute
{
    public GodotOverrideAttribute(bool replace = false)
        => Replace = replace;

    public bool Replace { get; }
}
""".Trim();

    public static readonly string InputMapAttribute = """
namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class InputMapAttribute(string dataType = "StringName", [System.Runtime.CompilerServices.CallerFilePath] string classPath = null) : Attribute
{
    public string DataType { get; } = dataType;
    public string ClassPath { get; } = classPath;
}
""".Trim();

    public static readonly string InstantiableAttribute = """
using GodotSharp.SourceGenerators;

namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class InstantiableAttribute(string init = "Init", string name = "New", Scope ctor = Scope.Protected) : Attribute
{
    public string Initialise { get; } = init;
    public string Instantiate { get; } = name;
    public string ConstructorScope { get; } = ctor.ToCodeString();
}
""".Trim();

    public static readonly string LayerNamesAttribute = """
namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class LayerNamesAttribute([System.Runtime.CompilerServices.CallerFilePath] string classPath = null) : Attribute
{
    public string ClassPath { get; } = classPath;
}
""".Trim();

    public static readonly string NotifyAttribute = """
namespace Godot;

[AttributeUsage(AttributeTargets.Property)]
public sealed class NotifyAttribute : Attribute
{
}
""".Trim();

    public static readonly string OnImportAttribute = """
namespace Godot;

[AttributeUsage(AttributeTargets.Method)]
public sealed class OnImportAttribute : Attribute
{
    public OnImportAttribute(string recognizedExtensions, string importAs = null, string resourceType = "PackedScene", string saveExtension = "scn", float priority = 1, int importOrder = 0, string presets = "Default")
    {
        DisplayName = importAs;
        ResourceType = resourceType;
        SaveExtension = saveExtension;
        RecognizedExtensions = recognizedExtensions.Split(',', '|');

        Priority = priority;
        ImportOrder = importOrder;
        Presets = presets.Split(',', '|');
    }

    public string DisplayName { get; }
    public string ResourceType { get; }
    public string SaveExtension { get; }
    public string[] RecognizedExtensions { get; }

    public float Priority { get; }
    public int ImportOrder { get; }
    public string[] Presets { get; }
}
""".Trim();

    public static readonly string OnInstantiateAttribute = """
using GodotSharp.SourceGenerators;

namespace Godot;

[AttributeUsage(AttributeTargets.Method)]
public sealed class OnInstantiateAttribute : Attribute
{
    public bool PrimaryAttribute { get; }
    public string ConstructorScope { get; }

    public OnInstantiateAttribute(Scope ctor = Scope.Protected)
    {
        PrimaryAttribute = true;
        ConstructorScope = ctor.ToCodeString();
    }

    public OnInstantiateAttribute(bool _)
    {
        PrimaryAttribute = false;
        ConstructorScope = null;
    }
}
""".Trim();

    public static readonly string ProjectSettingsAttribute = """
using GodotSharp.SourceGenerators.ProjectSettingsExtensions;

namespace Godot;

// Always use named (not positional) arguments; parameter names and order may change.

[AttributeUsage(AttributeTargets.Class)]
public sealed class ProjectSettingsAttribute(
    Generate Gravity = Generate.All) : Attribute
{
    public Generate Gravity { get; } = Gravity;
}
""".Trim();

    public static readonly string ResourceTreeAttribute = """
using GodotSharp.SourceGenerators;

namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ResourceTreeAttribute(string source = null, ResG resg = ResG.LoadRes, ResI resi = ResI.None, string[] xtras = null, string[] xclude = null) : Attribute, IResourceTreeConfig
{
    public string Source { get; } = source;
    public bool Uid { get; } = (resi & ResI.Uid) != 0;
    public bool Scenes { get; } = (resi & ResI.Scenes) != 0;
    public bool Scripts { get; } = (resi & ResI.Scripts) != 0;
    public bool UseGdLoad { get; } = (resg & ResG.LoadRes) != 0;
    public bool UseResPaths { get; } = (resg & ResG.ResPaths) != 0;
    public bool ShowDirPaths { get; } = (resg & ResG.DirPaths) != 0;
    public HashSet<string> Xtras { get; } = [.. xtras ?? []];
    public HashSet<string> Xclude { get; } = [.. xclude ?? []];

    public override string ToString() => $"ResourceTreeAttribute [Source: {Source}, {((IResourceTreeConfig)this).ToString()}]";
    string IResourceTreeConfig.ToString() => $"ResG: {resg}, ResI: {resi}, Xtras: {string.Join("|", Xtras)}, Xclude: {string.Join("|", Xclude)}";
}
""".Trim();

    public static readonly string SceneTreeAttribute = """
using GodotSharp.SourceGenerators;

namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class SceneTreeAttribute(
    string tscnRelativeToClassPath = null,
    bool traverseInstancedScenes = false,
    string root = "_",
    Scope uqScope = Scope.Public,
    [System.Runtime.CompilerServices.CallerFilePath] string classPath = null) : Attribute
{
    public string Root { get; } = root;
    public string SceneFile { get; } = tscnRelativeToClassPath is null
            ? Path.ChangeExtension(classPath, "tscn")
            : Path.GetFullPath(Path.Combine(Path.GetDirectoryName(classPath), tscnRelativeToClassPath));
    public bool TraverseInstancedScenes { get; } = traverseInstancedScenes;
    public string DefaultUniqueNodeScope { get; } = uqScope.ToCodeString() ?? "public";
}
""".Trim();

    public static readonly string ShaderAttribute = """
namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ShaderAttribute(string source = null, bool generate_tests = false) : Attribute
{
    public string Source { get; } = source;
    public bool GenerateTests { get; } = generate_tests;
}
""".Trim();

    public static readonly string ShaderGlobalsAttribute = """
namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ShaderGlobalsAttribute() : Attribute;
""".Trim();

    public static readonly string SingletonAttribute = """
namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class SingletonAttribute(string init = "Init") : Attribute
{
    public string InitFunc { get; } = init;
}
""".Trim();

    public static readonly string TRAttribute = """
namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class TRAttribute(string source = "Assets/tr/tr", bool xtras = true) : Attribute
{
    public bool Xtras { get; } = xtras;
    public string Source { get; } = source;
}
""".Trim();

    public static readonly string ICodeComments = """
namespace Godot;

public interface ICodeComments
{
    string GetComment(string property);
}
""".Trim();

    public static readonly string ScopeSource = """
namespace GodotSharp.SourceGenerators;

public enum Scope
{
    None,
    Public,
    Private,
    Internal,
    Protected,
    ProtectedOrInternal,
    ProtectedAndInternal,
}

public static class ScopeExtensions
{
    public static string ToCodeString(this Scope x) => x switch
    {
        Scope.None => null,
        Scope.Public => "public",
        Scope.Private => "private",
        Scope.Internal => "internal",
        Scope.Protected => "protected",
        Scope.ProtectedOrInternal => "protected internal",
        Scope.ProtectedAndInternal => "private protected",
        _ => throw new System.NotImplementedException(),
    };
}
""".Trim();

    public static readonly string ResourceTreeConfigSource = """
using System.Collections.Generic;

namespace GodotSharp.SourceGenerators;

[System.Flags]
public enum ResG
{
    LoadRes = 1,
    ResPaths = 2,
    DirPaths = 4,
    All = LoadRes | ResPaths | DirPaths
}

[System.Flags]
public enum ResI
{
    None,
    Uid = 1,
    Scenes = 2,
    Scripts = 4,
    All = Uid | Scenes | Scripts
}

public interface IResourceTreeConfig
{
    bool Uid { get; }
    bool Scenes { get; }
    bool Scripts { get; }
    bool UseGdLoad { get; }
    bool UseResPaths { get; }
    bool ShowDirPaths { get; }
    HashSet<string> Xtras { get; }
    HashSet<string> Xclude { get; }

    string ToString();
}
""".Trim();

    public static readonly string ProjectSettingsConfigSource = """
namespace GodotSharp.SourceGenerators.ProjectSettingsExtensions;

[System.Flags]
public enum Generate
{
    None,
    Get2D = 1,
    Set2D = 2,
    Get3D = 4,
    Set3D = 8,
    GetSet2D = Get2D | Set2D,
    GetSet3D = Get3D | Set3D,
    All = Get2D | Get3D | Set2D | Set3D
}
""".Trim();

    public static readonly (string Name, string Source)[] All =
    [
        (nameof(AnimNamesAttribute), AnimNamesAttribute),
        (nameof(AudioBusAttribute), AudioBusAttribute),
        (nameof(AutoEnumAttribute), AutoEnumAttribute),
        (nameof(AutoloadAttribute), AutoloadAttribute),
        (nameof(CodeCommentsAttribute), CodeCommentsAttribute),
        (nameof(GlobalGroupsAttribute), GlobalGroupsAttribute),
        (nameof(GodotOverrideAttribute), GodotOverrideAttribute),
        (nameof(InputMapAttribute), InputMapAttribute),
        (nameof(InstantiableAttribute), InstantiableAttribute),
        (nameof(LayerNamesAttribute), LayerNamesAttribute),
        (nameof(NotifyAttribute), NotifyAttribute),
        (nameof(OnImportAttribute), OnImportAttribute),
        (nameof(OnInstantiateAttribute), OnInstantiateAttribute),
        (nameof(ProjectSettingsAttribute), ProjectSettingsAttribute),
        (nameof(ResourceTreeAttribute), ResourceTreeAttribute),
        (nameof(SceneTreeAttribute), SceneTreeAttribute),
        (nameof(ShaderAttribute), ShaderAttribute),
        (nameof(ShaderGlobalsAttribute), ShaderGlobalsAttribute),
        (nameof(SingletonAttribute), SingletonAttribute),
        (nameof(TRAttribute), TRAttribute),
        (nameof(ICodeComments), ICodeComments),
        (nameof(ScopeSource), ScopeSource),
        (nameof(ResourceTreeConfigSource), ResourceTreeConfigSource),
        (nameof(ProjectSettingsConfigSource), ProjectSettingsConfigSource),
    ];
}
