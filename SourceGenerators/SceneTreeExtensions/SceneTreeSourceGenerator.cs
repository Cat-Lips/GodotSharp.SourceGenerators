using Godot;
using Microsoft.CodeAnalysis;
using Scriban;

namespace GodotSharp.SourceGenerators.SceneTreeExtensions
{
    [Generator]
    internal class SceneTreeSourceGenerator : SourceGeneratorForDeclaredTypesWithAttribute<SceneTreeAttribute>
    {
        private static Template _sceneTreeTemplate;
        private static Template SceneTreeTemplate => _sceneTreeTemplate ??= Template.Parse(Resources.SceneTreeTemplate);

        protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, INamedTypeSymbol symbol, AttributeData attribute)
        {
            var sceneTree = ReconstructAttribute();

            if (!File.Exists(sceneTree.SceneFile))
                return (null, Diagnostics.SceneFileNotFound(sceneTree.SceneFile));

            var model = new SceneTreeDataModel(compilation, symbol, sceneTree.SceneFile, sceneTree.TraverseInstancedScenes);
            Log.Debug($"--- NODES ---\n{model.SceneTree}");

            var output = SceneTreeTemplate.Render(model, member => member.Name);
            Log.Debug($"--- OUTPUT ---\n{output}<END>");

            return (output, null);

            SceneTreeAttribute ReconstructAttribute()
            {
                return new SceneTreeAttribute(
                    (string)attribute.ConstructorArguments[0].Value,
                    (bool)attribute.ConstructorArguments[1].Value,
                    (string)attribute.ConstructorArguments[2].Value);
            }
        }
    }
}
