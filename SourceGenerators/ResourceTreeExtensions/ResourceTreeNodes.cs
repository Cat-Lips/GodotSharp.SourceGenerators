namespace GodotSharp.SourceGenerators.ResourceTreeExtensions;

internal abstract class ResourceTreeNode(string name)
{
    public string Name { get; internal set; } = name;
}

internal sealed class ResourceTreeDir(string name) : ResourceTreeNode(name)
{
    public bool IsDir { get; } = true;

    public override string ToString()
        => $"Name: {Name}";
}

internal sealed class ResourceTreeFile(string name, string type, string resource) : ResourceTreeNode(name)
{
    public bool IsFile { get; } = true;

    public string Type { get; } = type;
    public string Resource { get; } = resource;

    public override string ToString()
        => $"Name: {Name}, Type: {Type}, Resource: {Resource}";
}
