using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.SceneTreeExtensions;

internal class SceneTreeDataModel : ClassDataModel
{
    public string Root { get; }
    public string TscnResource { get; }
    public Tree<SceneTreeNode> SceneTree { get; }
    public List<SceneTreeNode> UniqueNodes { get; }

    public SceneTreeDataModel(Compilation compilation, INamedTypeSymbol symbol, string root, string tscnFile, bool traverseInstancedScenes, string? godotProjectDir) : base(symbol)
    {
        Root = root;
        TscnResource = GD.GetResourcePath(tscnFile, godotProjectDir);
        (SceneTree, UniqueNodes) = SceneTreeScraper.GetNodes(compilation, tscnFile, traverseInstancedScenes);
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
