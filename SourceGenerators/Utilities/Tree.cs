using System.Text;

namespace GodotSharp.SourceGenerators
{
    public class Tree<T>(T value) : TreeNode<T>(value, null)
    {
        public T Root => Value;

        public void Traverse(Action<TreeNode<T>> action)
        {
            Traverse(this);

            void Traverse(TreeNode<T> node)
            {
                action(node);
                node.Children.ForEach(Traverse);
            }
        }

        public void Traverse(Action<TreeNode<T>, int> action)
        {
            Traverse(this, 0);

            void Traverse(TreeNode<T> node, int depth)
            {
                action(node, depth++);
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
                _ = str.AppendLine($"{indent}{node.Value}");
            }
        }
    }
}
