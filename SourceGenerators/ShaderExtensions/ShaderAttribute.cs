namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ShaderAttribute(string source = null, bool generate_tests = false) : Attribute
{
    public string Source { get; } = source;
    public bool GenerateTests { get; } = generate_tests;
}
