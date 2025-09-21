namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class SingletonAttribute(string init = "Init") : Attribute
{
    public string InitFunc { get; } = init;
}
