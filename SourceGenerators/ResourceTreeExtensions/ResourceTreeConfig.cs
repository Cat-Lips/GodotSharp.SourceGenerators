namespace GodotSharp.SourceGenerators.ResourceTreeExtensions;

[Flags]
public enum ResX // (Res Xtras)
{
    None,
    Uid = 1,
    Scenes = 2,
    Scripts = 3,
    All = Uid | Scenes | Scripts
}

[Flags]
public enum ResI // (Res Include)
{
    LoadRes = 1,
    ResPaths = 2,
    DirPaths = 4,
    All = LoadRes | ResPaths | DirPaths
}

public interface IResourceTreeConfig
{
    bool Uid { get; }
    bool Scenes { get; }
    bool Scripts { get; }
    bool UseGdLoad { get; }
    bool UseResPaths { get; }
    bool ShowDirPaths { get; }
    HashSet<string> Xtras { get; }
    HashSet<string> Xclude { get; }

    string ToString();
}
