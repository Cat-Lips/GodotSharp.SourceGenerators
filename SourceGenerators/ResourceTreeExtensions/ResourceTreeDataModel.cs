using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.ResourceTreeExtensions;

internal class ResourceTreeDataModel(Compilation compilation, INamedTypeSymbol symbol, string gdRoot, string source, IResourceTreeConfig cfg) : ClassDataModel(symbol)
{
    public Tree<ResourceTreeNode> MyResourceTree { get; } = ResourceTreeScraper.GetResourceTree(compilation, gdRoot, source, cfg);

    protected override string Str()
        => MyResourceTree.ToString().TrimEnd();
}
