namespace GodotSharp.SourceGenerators.ResourceTreeExtensions;

internal abstract record ResourceTreeNode(string Name, string Path, bool Show);

internal record ResourceTreeRoot(string Path, bool Show) : ResourceTreeNode(null, Path, Show)
{
    public bool IsRoot { get; } = true;
    public override string ToString() => $"ResTreeRoot [Path: {Path}, Show: {Show}]";
}

internal record ResourceTreeDir(string Name, string Path, bool Show) : ResourceTreeNode(Name, Path, Show)
{
    public bool IsDir { get; } = true;
    public override string ToString() => $"ResTreeDir [Name: {Name}, Path: {Path}, Show: {Show}]";
}

internal record ResourceTreeFile(string Name, string Path, string Type, bool Show) : ResourceTreeNode(Name, Path, Show)
{
    public bool IsFile { get; } = true;
    public override string ToString() => $"ResTreeFile [Name: {Name}, Path: {Path}, Type: {Type ?? "<null>"}, Show: {Show}]";
}
