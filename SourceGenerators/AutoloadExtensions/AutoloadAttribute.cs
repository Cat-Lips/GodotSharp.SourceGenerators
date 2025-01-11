using System.Runtime.CompilerServices;

namespace Godot;

using NameMap = (string DisplayName, string GodotName);

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class AutoloadAttribute : Attribute
{
    public AutoloadAttribute([CallerFilePath] string classPath = null)
        : this([], classPath)
    {
    }

    public AutoloadAttribute(NameMap map, [CallerFilePath] string classPath = null)
        : this([map], classPath)
    {
    }

    public AutoloadAttribute(NameMap[] map, [CallerFilePath] string classPath = null)
    {
        NameMap = map ?? [];
        ClassPath = classPath;
    }

    public string ClassPath { get; }
    public NameMap[] NameMap { get; }
}
