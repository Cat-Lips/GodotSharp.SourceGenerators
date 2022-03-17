using System.Diagnostics;
using System.Text;

namespace GodotSharp.SourceGenerators.SceneTreeExtensions
{
    internal class SceneTreeNode
    {
        private readonly Dictionary<string, SceneTreeNode> children = new();

        public SceneTreeNode(string name = ".", string type = null)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; init; }
        public string Type { get; set; }
        public string Path { get; set; }
        public ICollection<SceneTreeNode> Children => children.Values;

        public SceneTreeNode Add(Span<string> nodePath, string nodeType)
        {
            Trace.Assert(nodePath.Length is > 0);
            return nodePath.Length is 1
                ? AddChild(nodePath[0], nodeType)
                : GetChild(nodePath[0]).Add(nodePath[1..], nodeType);

            SceneTreeNode AddChild(string name, string type)
            {
                SceneTreeNode child = new(name, type);
                children.Add(name, child);
                return child;
            }

            SceneTreeNode GetChild(string name)
                => children[name];
        }

        public override string ToString()
        {
            var str = new StringBuilder();
            BuildStr(this);
            return str.ToString();

            void BuildStr(SceneTreeNode node, string indent = "")
            {
                str.AppendLine($"{indent}{node.Name} ({node.Type})");
                foreach (var child in node.Children)
                {
                    BuildStr(child, indent + '-');
                }
            }
        }
    }
}
