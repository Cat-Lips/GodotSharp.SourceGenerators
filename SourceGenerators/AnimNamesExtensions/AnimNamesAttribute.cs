using System.Runtime.CompilerServices;

namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class AnimNamesAttribute(string source = null, [CallerFilePath] string classPath = null) : Attribute
{
    public string Source { get; } = source;
    public string ClassPath { get; } = classPath;
}
