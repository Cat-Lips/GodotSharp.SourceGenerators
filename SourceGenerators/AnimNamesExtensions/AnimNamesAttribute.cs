using System.Runtime.CompilerServices;

namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class AnimNamesAttribute(string resPath = null, [CallerFilePath] string classPath = null) : Attribute
{
    public string ResPath { get; } = resPath ?? Path.ChangeExtension(classPath, "tres");
    public string ClassPath { get; } = classPath;
}
