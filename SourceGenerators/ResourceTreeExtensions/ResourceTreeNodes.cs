namespace GodotSharp.SourceGenerators.ResourceTreeExtensions;

internal abstract record ResourceTreeNode(string Name, string Path);

internal record ResourceTreeRoot(string Path) : ResourceTreeNode(null, Path)
{
    public bool IsRoot { get; } = true;
    public override string ToString() => $"ResTreeRoot [Path: {Path}]";
}

internal record ResourceTreeDir(string Name, string Path) : ResourceTreeNode(Name, Path)
{
    public bool IsDir { get; } = true;
    public override string ToString() => $"ResTreeDir [Name: {Name}, Path: {Path}]";
}

internal record ResourceTreeFile(string Name, string Path, string Type) : ResourceTreeNode(Name, Path)
{
    public bool IsFile { get; } = true;
    public override string ToString() => $"ResTreeFile [Name: {Name}, Path: {Path}, Type: {Type ?? "<null>"}]";
}
