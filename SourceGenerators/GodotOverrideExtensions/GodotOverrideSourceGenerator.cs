using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.GodotOverrideExtensions
{
    [Generator]
    internal class GodotOverrideSourceGenerator : SourceGeneratorForDeclaredMethodWithAttribute<Godot.GodotOverrideAttribute>
    {
        private static Template _godotOverrideTemplate;
        private static Template GodotOverrideTemplate => _godotOverrideTemplate ??= Template.Parse(Resources.GodotOverrideTemplate);

        protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, IMethodSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
        {
            var model = new GodotOverrideDataModel(symbol, ReconstructAttribute().Replace);
            Log.Debug($"--- MODEL ---\n{model}\n");

            var output = GodotOverrideTemplate.Render(model, member => member.Name);
            Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

            return (output, null);

            Godot.GodotOverrideAttribute ReconstructAttribute()
                => new((bool)attribute.ConstructorArguments[0].Value);
        }
    }
}
