namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class TRAttribute(string source = "Assets/tr/tr", bool xtras = true) : Attribute
{
    public bool Xtras { get; } = xtras;
    public string Source { get; } = source;
}
