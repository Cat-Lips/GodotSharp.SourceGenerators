using GodotSharp.SourceGenerators;

namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class InstantiableAttribute(string init = "Init", string name = "New", Scope ctor = Scope.Protected) : Attribute
{
    public string Initialise { get; } = init;
    public string Instantiate { get; } = name;
    public string ConstructorScope { get; } = ctor.ToCodeString();
}
