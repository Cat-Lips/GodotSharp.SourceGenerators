using GodotSharp.SourceGenerators.ResourceTreeExtensions;

namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ResourceTreeAttribute(string source = null, Res res = Res.Default, string[] xtras = null, string[] xclude = null) : Attribute, IResourceTreeConfig
{
    public string Source { get; } = source;
    public bool Uid { get; } = (res & Res.Uid) != 0;
    public bool Load { get; } = (res & Res.Load) != 0;
    public bool Scenes { get; } = (res & Res.Scenes) != 0;
    public bool Scripts { get; } = (res & Res.Scripts) != 0;
    public bool ShowResPaths { get; } = (res & Res.ResPaths) != 0;
    public bool ShowDirPaths { get; } = (res & Res.DirPaths) != 0;
    public HashSet<string> Xtras { get; } = [.. xtras ?? []];
    public HashSet<string> Xclude { get; } = [.. xclude ?? []];

    public override string ToString() => $"ResourceTreeAttribute [Source: {Source}, {((IResourceTreeConfig)this).ToString()}]";
    string IResourceTreeConfig.ToString() => $"{res}, Xtras: {string.Join("|", Xtras)}, Xclude: {string.Join("|", Xclude)}";
}
