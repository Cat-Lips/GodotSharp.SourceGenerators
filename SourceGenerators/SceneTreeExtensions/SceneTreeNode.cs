namespace GodotSharp.SourceGenerators.SceneTreeExtensions
{
    internal class SceneTreeNode
    {
        public string Name { get; init; }
        public string Type { get; set; }
        public string Path { get; init; }

        public SceneTreeNode(string name, string type, string path)
        {
            Name = name;
            Type = type;
            Path = path;
        }

        public override string ToString()
            => $"Name: {Name}, Type: {Type}, Path: {Path}";
    }
}
