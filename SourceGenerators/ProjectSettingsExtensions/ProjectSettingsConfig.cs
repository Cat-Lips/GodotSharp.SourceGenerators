namespace GodotSharp.SourceGenerators.ProjectSettingsExtensions;

[Flags]
public enum Generate
{
    None,
    Get2D = 1,
    Set2D = 2,
    Get3D = 4,
    Set3D = 8,
    GetSet2D = Get2D | Set2D,
    GetSet3D = Get3D | Set3D,
    All = Get2D | Get3D | Set2D | Set3D
}
