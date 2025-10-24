namespace GodotSharp.SourceGenerators.ResourceTreeExtensions;

public interface IResourceTreeConfig
{
    bool Scenes { get; }
    bool Scripts { get; }
    bool Uid { get; }
    string[] Xtras { get; }
}
