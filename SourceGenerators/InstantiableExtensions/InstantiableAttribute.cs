namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class InstantiableAttribute(string init = "Init", string name = "New", string ctor = "protected") : Attribute
{
    public string Initialise { get; } = init;
    public string Instantiate { get; } = name;
    public string ConstructorScope { get; } = ctor;
}
