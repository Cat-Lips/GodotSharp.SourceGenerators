using System.Runtime.CompilerServices;

namespace Godot
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SceneTreeAttribute : Attribute
    {
        //public SceneTreeAttribute([CallerFilePath] string classPath = null)
        //    : this(null, false, classPath) { }

        //public SceneTreeAttribute(string tscnRelativeToClassPath, [CallerFilePath] string classPath = null)
        //    : this(tscnRelativeToClassPath, false, classPath) { }

        //public SceneTreeAttribute(bool traverseInstancedScenes, [CallerFilePath] string classPath = null)
        //    : this(null, traverseInstancedScenes, classPath) { }

        //public SceneTreeAttribute(string tscnRelativeToClassPath, bool traverseInstancedScenes, [CallerFilePath] string classPath = null)
        public SceneTreeAttribute(string tscnRelativeToClassPath = null, bool traverseInstancedScenes = false, [CallerFilePath] string classPath = null)
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
