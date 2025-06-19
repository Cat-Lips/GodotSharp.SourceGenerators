using System.Runtime.CompilerServices;

namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class InputMapAttribute(string dataType = "StringName", [CallerFilePath] string classPath = null) : Attribute
{
    public string DataType { get; } = dataType;
    public string ClassPath { get; } = classPath;
}
