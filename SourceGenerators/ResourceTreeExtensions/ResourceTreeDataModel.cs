using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.ResourceTreeExtensions;

internal class ResourceTreeDataModel : ClassDataModel
{
    public Tree<ResourceTreeNode> SceneTree { get; }

    public ResourceTreeDataModel(Compilation compilation, INamedTypeSymbol symbol, string godotProjectDir) : base(symbol)
    {
        SceneTree = ResourceTreeScraper.GetNodes(compilation, symbol.Name, godotProjectDir);
    }

    protected override string Str()
    {
        return $"Tree:-\n{SceneTree.ToString().TrimEnd()}";
    }
}
