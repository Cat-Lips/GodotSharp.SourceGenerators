namespace GodotSharp.SourceGenerators.ResourceTreeExtensions;

[Flags]
public enum Res
{
    Uid = 1,
    Load = 2,
    Scenes = 4,
    Scripts = 8,
    ResPaths = 16,
    DirPaths = 32,

    All = AllIn | AllOut,
    AllIn = Uid | Scenes | Scripts,
    AllOut = Load | ResPaths | DirPaths,

    Default = Load,

    None = 0,
}

public interface IResourceTreeConfig
{
    bool Uid { get; }
    bool Load { get; }
    bool Scenes { get; }
    bool Scripts { get; }
    bool ShowResPaths { get; }
    bool ShowDirPaths { get; }
    HashSet<string> Xtras { get; }
    HashSet<string> Xclude { get; }

    string ToString();
}
