namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class AudioBusAttribute(string source = "default_bus_layout") : Attribute
{
    public string Source { get; } = source;
}
