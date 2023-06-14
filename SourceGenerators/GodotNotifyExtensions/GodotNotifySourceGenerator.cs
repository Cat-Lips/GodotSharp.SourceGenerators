using Microsoft.CodeAnalysis;
using Scriban;

namespace GodotSharp.SourceGenerators.GodotNotifyExtensions
{
    [Generator]
    internal class GodotNotifySourceGenerator : SourceGeneratorForDeclaredPropertyWithAttribute<Godot.NotifyAttribute>
    {
        private static Template _godotNotifyTemplate;
        private static Template GodotNotifyTemplate => _godotNotifyTemplate ??= Template.Parse(Resources.GodotNotifyTemplate);

        protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, IPropertySymbol symbol, AttributeData attribute)
        {
            var model = new GodotNotifyDataModel(symbol);
            Log.Debug($"--- MODEL ---\n{model}\n");

            var output = GodotNotifyTemplate.Render(model, member => member.Name);
            Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

            return (output, null);
        }
    }
}
