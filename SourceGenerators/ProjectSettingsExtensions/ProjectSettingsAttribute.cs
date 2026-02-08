using GodotSharp.SourceGenerators.ProjectSettingsExtensions;

namespace Godot;

// Always use named (not positional) arguments; parameter names and order may change.

[AttributeUsage(AttributeTargets.Class)]
public sealed class ProjectSettingsAttribute(
    Generate Gravity = Generate.All) : Attribute
{
    public Generate Gravity { get; } = Gravity;
}
