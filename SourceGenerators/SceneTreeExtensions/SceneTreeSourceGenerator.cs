using System.IO;
using Godot;
using GodotSharp.SourceGenerators.Utilities;
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
            var tscnFile = new SceneTreeAttribute(
                (string)attribute.ConstructorArguments[0].Value,
                (string)attribute.ConstructorArguments[1].Value).SceneFile;

            if (!File.Exists(tscnFile))
            {
                Log.Debug($"SceneFileNotFound: {tscnFile}");
                return (null, Diagnostics.SceneFileNotFound(tscnFile));
            }

            Log.Debug($"Parsing: {tscnFile}");
            var model = new SceneTreeDataModel(compilation, symbol, tscnFile);
            var output = SceneTreeTemplate.Render(model, member => member.Name);
            Log.Debug($"<SceneTree-{symbol}>\n{output}<End-SceneTree>");
            return (output, null);
        }
    }
}
