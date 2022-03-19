using System.Text;

namespace GodotSharp.SourceGenerators
{
    public class Tree<T>
    {
        public List<TreeNode<T>> Nodes { get; } = new();

        public override string ToString()
        {
            StringBuilder str = new();
            Nodes.ForEach(x => PrintNode(x, 0));
            return str.ToString();

            void PrintNode(TreeNode<T> node, int level)
            {
                var indent = new string(' ', level * 2);
                str.AppendLine($"{indent}{node.Value}");
                ++level;

                node.Children.ForEach(x => PrintNode(x, level));
            }
        }
    }
}
