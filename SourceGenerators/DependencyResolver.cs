using GodotSharp.SourceGenerators.SceneTreeExtensions;
using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators;

public class DependencyResolver
{
    public IEnumerable<CodeDependency> ResolveDependency(
        SourceProductionContext context,
        Compilation compilation,
        CodeDependency dependency)
    {
        switch (dependency)
        {
            case CodeDependency.SceneTree sceneTree:
                return new SceneTreeSourceGenerator().GenerateSceneTree(context, compilation, sceneTree);
            default:
                throw new InvalidOperationException($"Unresolved dependency of type {dependency.GetType().Name}.");
        }
    }
}
