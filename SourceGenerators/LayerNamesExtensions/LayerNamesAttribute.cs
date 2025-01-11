using System.Runtime.CompilerServices;

namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class LayerNamesAttribute : Attribute
{
    public LayerNamesAttribute([CallerFilePath] string classPath = null)
        => ClassPath = classPath;

    public string ClassPath { get; }
}
