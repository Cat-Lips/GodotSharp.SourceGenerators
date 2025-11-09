using System.Runtime.CompilerServices;
using GodotSharp.SourceGenerators;

namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class SceneTreeAttribute(
    string tscnRelativeToClassPath = null,
    bool traverseInstancedScenes = false,
    string root = "_",
    Scope uqScope = Scope.Public,
    [CallerFilePath] string classPath = null) : Attribute
{
    public string Root { get; } = root;
    public string SceneFile { get; } = tscnRelativeToClassPath is null
            ? Path.ChangeExtension(classPath, "tscn")
            : Path.GetFullPath(Path.Combine(Path.GetDirectoryName(classPath), tscnRelativeToClassPath));
    public bool TraverseInstancedScenes { get; } = traverseInstancedScenes;
    public string DefaultUniqueNodeScope { get; } = uqScope.ToCodeString() ?? "public";
}
