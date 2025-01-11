namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class CodeCommentsAttribute : Attribute
{
    public CodeCommentsAttribute(string strip = "// ")
        => Strip = strip;

    public string Strip { get; }
}
