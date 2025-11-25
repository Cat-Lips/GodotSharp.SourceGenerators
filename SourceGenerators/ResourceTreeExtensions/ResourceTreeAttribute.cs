using GodotSharp.SourceGenerators;

namespace Godot;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ResourceTreeAttribute(string source = null, ResG resg = ResG.LoadRes, ResI resi = ResI.None, string[] xtras = null, string[] xclude = null) : Attribute, IResourceTreeConfig
{
    public string Source { get; } = source;
    public bool Uid { get; } = (resi & ResI.Uid) != 0;
    public bool Scenes { get; } = (resi & ResI.Scenes) != 0;
    public bool Scripts { get; } = (resi & ResI.Scripts) != 0;
    public bool UseGdLoad { get; } = (resg & ResG.LoadRes) != 0;
    public bool UseResPaths { get; } = (resg & ResG.ResPaths) != 0;
    public bool ShowDirPaths { get; } = (resg & ResG.DirPaths) != 0;
    public HashSet<string> Xtras { get; } = [.. xtras ?? []];
    public HashSet<string> Xclude { get; } = [.. xclude ?? []];

    public override string ToString() => $"ResourceTreeAttribute [Source: {Source}, {((IResourceTreeConfig)this).ToString()}]";
    string IResourceTreeConfig.ToString() => $"ResG: {resg}, ResI: {resi}, Xtras: {string.Join("|", Xtras)}, Xclude: {string.Join("|", Xclude)}";
}
