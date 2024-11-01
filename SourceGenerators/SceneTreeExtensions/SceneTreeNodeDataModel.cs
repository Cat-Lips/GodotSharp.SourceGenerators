using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.SceneTreeExtensions;

internal class SceneTreeNodeDataModel(INamedTypeSymbol symbol, CodeDependency.SceneTree tree)
    : ClassDataModel(symbol)
{
    public string SceneTreeClassName { get; } = tree.ClassName;
    public string TscnFile { get; } = $"res://{tree.TscnFileName[(GD.GetProjectRoot(tree.TscnFileName).Length+1)..]}";

    protected override string Str() => $"SceneTreeClassName: {SceneTreeClassName}";
}
