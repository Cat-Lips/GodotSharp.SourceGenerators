using System.Runtime.CompilerServices;

namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class InputMapAttribute : Attribute
{
    public InputMapAttribute([CallerFilePath] string classPath = null!)
        => ClassPath = classPath;

    public string ClassPath { get; }
}
