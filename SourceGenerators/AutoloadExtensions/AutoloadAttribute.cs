using System.Runtime.CompilerServices;

namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public class AutoloadAttribute([CallerFilePath] string ClassPath = null!) : Attribute
{
    public string ClassPath { get; } = ClassPath;
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class AutoloadRenameAttribute(string DisplayName, string GodotName) : Attribute
{
    public string DisplayName { get; } = DisplayName;
    public string GodotName { get; } = GodotName;
}
