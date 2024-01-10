namespace GodotSharp.SourceGenerators.AutoloadExtensions
{
    internal class AutoloadNode
    {
        public string Name { get; init; }
        public string Type { get; set; }

        public AutoloadNode(string name, string type)
        {
            Name = name;
            Type = type;
        }

        public override string ToString()
            => $"Name: {Name}, Type: {Type}";
    }
}
