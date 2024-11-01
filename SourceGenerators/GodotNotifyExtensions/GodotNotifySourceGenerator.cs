using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.GodotNotifyExtensions
{
    [Generator]
    internal class GodotNotifySourceGenerator : SourceGeneratorForDeclaredPropertyWithAttribute<Godot.NotifyAttribute>
    {
        private static Template _godotNotifyTemplate;
        private static Template GodotNotifyTemplate => _godotNotifyTemplate ??= Template.Parse(Resources.GodotNotifyTemplate);

        protected override CodeGenerationResult GenerateCode(
            Compilation compilation,
            SyntaxNode node,
            IPropertySymbol symbol,
            AttributeData attribute,
            AnalyzerConfigOptions options)
        {
            var model = new GodotNotifyDataModel(symbol);
            Log.Debug($"--- MODEL ---\n{model}\n");

            var output = GodotNotifyTemplate.Render(model, member => member.Name);
            Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

            return new CodeGenerationResult.Success(output);
        }
    }
}
