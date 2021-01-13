using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Godot
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SceneTreeAttribute : Attribute
    {
        public SceneTreeAttribute(string tscnRelativeToClassPath = null, [CallerFilePath] string classPath = null)
        {
            SceneFile = tscnRelativeToClassPath is null
                ? Path.ChangeExtension(classPath, "tscn")
                : Path.Combine(Path.GetDirectoryName(classPath), tscnRelativeToClassPath);
        }

        public string SceneFile { get; }
    }
}
