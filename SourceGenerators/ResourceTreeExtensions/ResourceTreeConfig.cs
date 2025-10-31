namespace GodotSharp.SourceGenerators.ResourceTreeExtensions;

[Flags]
public enum Res
{
    Uid = 1,
    Load = 2,
    Scenes = 4,
    Scripts = 8,
    //ResPaths = 16, // Always included (currently)
    All = Uid | Load | Scenes | Scripts /*| ResPaths*/,
    Default = None /*| ResPaths*/,
    None = 0,
}

public interface IResourceTreeConfig
{
    bool Uid { get; }
    bool Load { get; }
    bool Scenes { get; }
    bool Scripts { get; }
    //bool ResPaths { get; }
    HashSet<string> Xtras { get; }
    HashSet<string> Xclude { get; }

    string ToString();
}
