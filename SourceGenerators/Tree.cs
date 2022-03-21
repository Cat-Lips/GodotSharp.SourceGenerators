using System.Text;

namespace GodotSharp.SourceGenerators
{
    public class Tree<T>
    {
        public List<TreeNode<T>> Nodes { get; } = new();

        public void Traverse(Action<TreeNode<T>> action)
        {
            Nodes.ForEach(Traverse);

            void Traverse(TreeNode<T> node)
            {
                action(node);
                node.Children.ForEach(Traverse);
            }
        }

        public void Traverse(Action<TreeNode<T>, int> action)
        {
            Nodes.ForEach(x => Traverse(x, 0));

            void Traverse(TreeNode<T> node, int depth)
            {
                action(node, depth); ++depth;
                node.Children.ForEach(x => Traverse(x, depth));
            }
        }

        public override string ToString()
        {
            StringBuilder str = new();
            Traverse(PrintNode);
            return str.ToString();

            void PrintNode(TreeNode<T> node, int level)
            {
                var indent = new string(' ', level * 2);
                str.AppendLine($"{indent}{node.Value}");
            }
        }
    }
}
