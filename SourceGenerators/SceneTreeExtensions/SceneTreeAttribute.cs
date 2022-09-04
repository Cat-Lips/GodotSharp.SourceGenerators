using System.Runtime.CompilerServices;

namespace Godot
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SceneTreeAttribute : Attribute
    {
        public SceneTreeAttribute(
            string tscnRelativeToClassPath = null,
            bool traverseInstancedScenes = false,
            [CallerFilePath] string classPath = null)
        {
            SceneFile = tscnRelativeToClassPath is null
                ? Path.ChangeExtension(classPath, "tscn")
                : Path.Combine(Path.GetDirectoryName(classPath), tscnRelativeToClassPath);

            TraverseInstancedScenes = traverseInstancedScenes;
        }

        public string SceneFile { get; }
        public bool TraverseInstancedScenes { get; }
    }
}
