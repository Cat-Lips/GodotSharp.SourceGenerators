﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.LayerNamesExtensions
{
    [Generator]
    internal class LayerNameSourceGenerator : SourceGeneratorForDeclaredTypeWithAttribute<Godot.LayerNamesAttribute>
    {
        private static Template _layerNameTemplate;
        private static Template LayerTemplate => _layerNameTemplate ??= Template.Parse(Resources.LayerNamesTemplate);

        protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, INamedTypeSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
        {
            var model = new LayerNamesDataModel(symbol, ReconstructAttribute().ClassPath, options.TryGetGodotProjectDir());
            Log.Debug($"--- MODEL ---\n{model}\n");

            var output = LayerTemplate.Render(model, member => member.Name);
            Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

            return (output, null);

            Godot.LayerNamesAttribute ReconstructAttribute()
                => new((string)attribute.ConstructorArguments[0].Value);
        }
    }
}
