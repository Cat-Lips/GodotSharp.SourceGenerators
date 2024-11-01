using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.SceneTreeExtensions
{
    [Generator]
    internal class SceneTreeNodeSourceGenerator : SourceGeneratorForDeclaredTypeWithAttribute<Godot.SceneTreeAttribute>
    {
        private static Template SceneTreeNodeTemplate { get; } = Template.Parse(Resources.SceneTreeNodeTemplate);

        protected override CodeGenerationResult GenerateCode(
            Compilation compilation,
            SyntaxNode node,
            INamedTypeSymbol symbol,
            AttributeData attribute,
            AnalyzerConfigOptions options)
        {
            var sceneTree = ReconstructAttribute();

            if (!File.Exists(sceneTree.SceneFile))
                return new CodeGenerationResult.Error(Diagnostics.SceneFileNotFound(sceneTree.SceneFile));

            var sceneTreeDependency = new CodeDependency.SceneTree(
                sceneTree.SceneFile,
                sceneTree.TraverseInstancedScenes);
            var model = new SceneTreeNodeDataModel(symbol, sceneTreeDependency);
            Log.Debug($"--- MODEL ---\n{model}\n");

            var output = SceneTreeNodeTemplate.Render(model, member => member.Name);
            Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

            return new CodeGenerationResult.Success(output, Dependencies: [sceneTreeDependency]);

            Godot.SceneTreeAttribute ReconstructAttribute()
            {
                return new(
                    (string)attribute.ConstructorArguments[0].Value,
                    (bool)attribute.ConstructorArguments[1].Value,
                    (string)attribute.ConstructorArguments[2].Value);
            }
        }
    }
}
