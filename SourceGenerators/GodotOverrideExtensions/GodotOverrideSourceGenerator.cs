using Godot;
using GodotSharp.SourceGenerators.Utilities;
using Microsoft.CodeAnalysis;
using Scriban;

namespace GodotSharp.SourceGenerators.GodotOverrideExtensions
{
    [Generator]
    internal class GodotOverrideSourceGenerator : SourceGeneratorForDeclaredMethodsWithAttribute<GodotOverrideAttribute>
    {
        private static Template _godotOverrideTemplate;
        private static Template GodotOverrideTemplate => _godotOverrideTemplate ??= Template.Parse(Resources.GodotOverrideTemplate);

        protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, IMethodSymbol symbol, AttributeData attribute)
        {
            var model = new GodotOverrideDataModel(symbol);
            var output = GodotOverrideTemplate.Render(model, member => member.Name);
            Log.Debug($"<GodotOverride-{symbol}>\n{output}<End-GodotOverride>");
            return (output, null);
        }
    }
}
