namespace Godot;

[AttributeUsage(AttributeTargets.Method)]
public sealed class OnInstantiateAttribute : Attribute
{
    public OnInstantiateAttribute(string? ctor = "protected")
        => ConstructorScope = ctor is null or "" or "none" ? null : ctor;

    public string? ConstructorScope { get; }
}
