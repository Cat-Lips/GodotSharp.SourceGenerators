using System.Runtime.CompilerServices;

namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class GlobalGroupsAttribute : Attribute
{
    public GlobalGroupsAttribute([CallerFilePath] string classPath = null)
        => ClassPath = classPath;

    public string ClassPath { get; }
}
