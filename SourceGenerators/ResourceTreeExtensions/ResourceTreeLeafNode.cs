namespace GodotSharp.SourceGenerators.ResourceTreeExtensions;

internal class ResourceTreeLeafNode : ResourceTreeNode
{
    public string Type { get; set; }
    public string Path { get; init; }

    public ResourceTreeLeafNode(string name, string type, string path) : base(name)
    {
        Type = type;
        Path = path;
    }

    public override string ToString()
        => $"Name: {Name}, Type: {Type}, Path: {Path}";
}
