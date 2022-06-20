using Microsoft.CodeAnalysis;
using Scriban;

namespace GodotSharp.SourceGenerators.GodotNotifyExtensions
{
    [Generator]
    internal class GodotNotifySourceGenerator : SourceGeneratorForDeclaredFieldsWithAttribute<Godot.NotifyAttribute>
    {
        private static Template _godotNotifyTemplate;
        private static Template GodotNotifyTemplate => _godotNotifyTemplate ??= Template.Parse(Resources.GodotNotifyTemplate);

        protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation _, IFieldSymbol symbol, AttributeData __)
        {
            var model = new GodotNotifyDataModel(symbol);
            var output = GodotNotifyTemplate.Render(model, member => member.Name);
            return (output, null);
        }
    }
}
