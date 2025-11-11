using GodotSharp.SourceGenerators;

namespace Godot;

[AttributeUsage(AttributeTargets.Method)]
public sealed class OnInstantiateAttribute : Attribute
{
    public bool PrimaryAttribute { get; }
    public string ConstructorScope { get; }

    public OnInstantiateAttribute(Scope ctor = Scope.Protected)
    {
        PrimaryAttribute = true;
        ConstructorScope = ctor.ToCodeString();
    }

    public OnInstantiateAttribute(bool _)
    {
        PrimaryAttribute = false;
        ConstructorScope = null;
    }
}
