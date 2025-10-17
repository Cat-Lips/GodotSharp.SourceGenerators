namespace GodotSharp.SourceGenerators.ResourceTreeExtensions;

internal class ResourceTreeNode
{
    public string Name { get; init; }

    public ResourceTreeNode(string name)
    {
        Name = name;
    }

    public override string ToString()
        => $"Name: {Name}";
}
