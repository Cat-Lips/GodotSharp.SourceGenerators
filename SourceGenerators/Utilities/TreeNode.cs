namespace GodotSharp.SourceGenerators;

public class TreeNode<T>(T value, TreeNode<T> parent)
{
    public T Value { get; } = value;
    public TreeNode<T> Parent { get; } = parent;
    public List<TreeNode<T>> Children { get; } = [];

    public bool IsRoot => Parent is null;
    public bool HasChildren => Children.Count > 0;

    public TreeNode<T> Add(T value)
    {
        var node = new TreeNode<T>(value, this);
        Children.Add(node);
        return node;
    }
}
