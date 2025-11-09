using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.SceneTreeExtensions;

internal class SceneTreeDataModel : ClassDataModel
{
    public string Root { get; }
    public string TscnResource { get; }
    public Tree<SceneTreeNode> SceneTree { get; }
    public IEnumerable<UniqueNode> UniqueNodes { get; }
    internal record UniqueNode(SceneTreeNode Node, string Scope);

    public SceneTreeDataModel(Compilation compilation, INamedTypeSymbol symbol, string root, string tscnFile, bool traverseInstancedScenes, string godotProjectDir) : base(symbol)
    {
        Root = root;
        TscnResource = GD.GetResourcePath(tscnFile, godotProjectDir);
        var (SceneTree, UniqueNodes) = SceneTreeScraper.GetNodes(compilation, tscnFile, traverseInstancedScenes);
        this.SceneTree = SceneTree;
        this.UniqueNodes = [.. UniqueNodes.Select(x => new UniqueNode(x, Scope(x.Name)))];

        string Scope(string name)
        {
            return Scope()?.AddSuffix(" partial") ?? "public";

            string Scope() => symbol
                .GetMembers(name)
                .OfType<IPropertySymbol>()
                .SingleOrDefault()?.Scope();
        }
    }

    protected override string Str()
    {
        return string.Join("\n", Parts());

        IEnumerable<string> Parts()
        {
            yield return $"Root: {Root}";
            yield return $"Tscn: {TscnResource}";
            yield return $"Tree:-\n{SceneTree.ToString().TrimEnd()}";

            if (UniqueNodes.Any())
                yield return $"\nUniqueNodes:\n - {string.Join("\n - ", UniqueNodes)}";
        }
    }
}
