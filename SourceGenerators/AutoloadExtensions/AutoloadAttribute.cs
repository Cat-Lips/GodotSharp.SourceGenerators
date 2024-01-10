using System.Runtime.CompilerServices;

namespace Godot
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AutoloadAttribute : Attribute
    {
        public AutoloadAttribute([CallerFilePath] string classPath = null)
            => ClassPath = classPath;

        public string ClassPath { get; }
    }
}
