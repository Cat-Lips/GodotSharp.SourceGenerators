using GodotSharp.SourceGenerators;

namespace Godot;

[AttributeUsage(AttributeTargets.Method)]
public sealed class OnInstantiateAttribute(Scope ctor = Scope.Protected) : Attribute
{
    public string ConstructorScope { get; } = ctor.ToCodeString();
}
