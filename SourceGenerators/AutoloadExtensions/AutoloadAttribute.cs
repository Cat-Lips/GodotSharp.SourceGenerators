using System.Runtime.CompilerServices;

namespace Godot
{
    [AttributeUsage(AttributeTargets.Struct)]
    public sealed class AutoloadAttribute([CallerFilePath] string classPath = null) : Attribute
    {
        public string ClassPath { get; } = classPath;
    }
}
