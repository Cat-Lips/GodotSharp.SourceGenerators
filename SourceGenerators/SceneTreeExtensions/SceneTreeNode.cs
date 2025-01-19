namespace GodotSharp.SourceGenerators.SceneTreeExtensions;

internal class SceneTreeNode
{
    public string Name { get; init; }
    public string Type { get; set; }
    public string Path { get; init; }
    public bool Visible { get; set; }

    public SceneTreeNode(string name, string type, string path, bool visible = true)
    {
        Name = name;
        Type = type;
        Path = path;
        Visible = visible;
    }

    public override string ToString()
        => $"{(Visible ? null : "<HIDDEN> ")}Name: {Name}, Type: {Type}, Path: {Path}";
}
