using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.SceneTreeExtensions;

[Generator]
internal class SceneTreeSourceGenerator : SourceGeneratorForDeclaredTypeWithAttribute<Godot.SceneTreeAttribute>
{
    private static Template SceneTreeTemplate => field ??= Template.Parse(Resources.SceneTreeTemplate);

    protected override IEnumerable<(string Name, string Source)> StaticSources
    {
        get
        {
            yield return (nameof(Resources.ISceneTree), Resources.ISceneTree);
            yield return (nameof(Resources.IInstantiable), Resources.IInstantiable);
        }
    }

    protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, INamedTypeSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
    {
        var sceneTree = ReconstructAttribute();

        if (!File.Exists(sceneTree.SceneFile))
            return (null, Diagnostics.FileNotFound(sceneTree.SceneFile));

        var model = new SceneTreeDataModel(compilation, symbol, sceneTree.Root, sceneTree.SceneFile, sceneTree.TraverseInstancedScenes, options.TryGetGodotProjectDir());
        Log.Debug($"--- MODEL ---\n{model}\n");

        var output = SceneTreeTemplate.Render(model, member => member.Name);
        Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

        return (output, null);

        Godot.SceneTreeAttribute ReconstructAttribute()
        {
            return new(
                (string)attribute.ConstructorArguments[0].Value,
                (bool)attribute.ConstructorArguments[1].Value,
                (string)attribute.ConstructorArguments[2].Value,
                (string)attribute.ConstructorArguments[3].Value);
        }
    }
}
