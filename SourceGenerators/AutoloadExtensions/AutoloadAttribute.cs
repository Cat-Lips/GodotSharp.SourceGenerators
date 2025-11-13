namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class AutoloadAttribute() : Attribute;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class AutoloadRenameAttribute(string DisplayName, string GodotName) : Attribute
{
    public string DisplayName { get; } = DisplayName;
    public string GodotName { get; } = GodotName;
}
