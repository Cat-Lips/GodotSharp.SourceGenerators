using System.Runtime.CompilerServices;

namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class SceneTreeAttribute : Attribute
{
    public SceneTreeAttribute(
        string tscnRelativeToClassPath = null,
        bool traverseInstancedScenes = false,
        string root = "_",
        [CallerFilePath] string classPath = null)
    {
        SceneFile = tscnRelativeToClassPath is null
            ? Path.ChangeExtension(classPath, "tscn")
            : Path.GetFullPath(Path.Combine(Path.GetDirectoryName(classPath), tscnRelativeToClassPath));

        TraverseInstancedScenes = traverseInstancedScenes;
        Root = root;
    }

    public string Root { get; }
    public string SceneFile { get; }
    public bool TraverseInstancedScenes { get; }
}
