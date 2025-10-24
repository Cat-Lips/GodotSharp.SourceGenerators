using GodotSharp.SourceGenerators.ResourceTreeExtensions;

namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ResourceTreeAttribute(string source = null, bool scenes = false, bool scripts = false, bool uid = false, params string[] xtras) : Attribute, IResourceTreeConfig
{
    public bool Uid { get; } = uid;
    public bool Scenes { get; } = scenes;
    public bool Scripts { get; } = scripts;
    public string Source { get; } = source;
    public string[] Xtras { get; } = xtras;
}
