namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ShaderNamesAttribute(string source = null) : Attribute
{
    public string Source { get; } = source;
}
