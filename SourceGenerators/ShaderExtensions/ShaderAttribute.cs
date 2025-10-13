namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ShaderAttribute(string source = null) : Attribute
{
    public string Source { get; } = source;
}
