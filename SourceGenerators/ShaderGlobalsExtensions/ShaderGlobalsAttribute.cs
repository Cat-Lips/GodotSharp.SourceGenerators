using System.Runtime.CompilerServices;

namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ShaderGlobalsAttribute([CallerFilePath] string classPath = null) : Attribute
{
    public string ClassPath { get; } = classPath;
}
