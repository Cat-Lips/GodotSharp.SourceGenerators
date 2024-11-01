using Microsoft.CodeAnalysis;
using Scriban;

namespace GodotSharp.SourceGenerators.SceneTreeExtensions;

public class SceneTreeSourceGenerator
{
    private static Template SceneTreeTemplate { get; } = Template.Parse(Resources.SceneTreeTemplate);

    public IEnumerable<CodeDependency> GenerateSceneTree(
        SourceProductionContext context,
        Compilation compilation,
        CodeDependency.SceneTree sceneTree)
    {
        var model = new SceneTreeDataModel(compilation, sceneTree);
        Log.Debug($"--- MODEL ---\n{model}\n");

        var output = SceneTreeTemplate.Render(model, member => member.Name);
        Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

        context.AddSource(sceneTree.ClassName + ".g.cs", output);

        return [];
    }
}
