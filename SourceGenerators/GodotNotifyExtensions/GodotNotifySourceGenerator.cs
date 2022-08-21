using Microsoft.CodeAnalysis;
using Scriban;

namespace GodotSharp.SourceGenerators.GodotNotifyExtensions
{
    [Generator]
    internal class GodotNotifySourceGenerator : SourceGeneratorForDeclaredFieldsWithAttribute<Godot.NotifyAttribute>
    {
        private static Template _godotNotifyTemplate;
        private static Template GodotNotifyTemplate => _godotNotifyTemplate ??= Template.Parse(Resources.GodotNotifyTemplate);

        protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation _, IFieldSymbol symbol, AttributeData attribute)
        {
            var attrib = ReconstructAttribute();

            var model = new GodotNotifyDataModel(symbol, attrib.Setter, attrib.Export);
            var output = GodotNotifyTemplate.Render(model, member => member.Name);
            return (output, null);

            Godot.NotifyAttribute ReconstructAttribute()
                => new((string)attribute.ConstructorArguments[0].Value, (bool)attribute.ConstructorArguments[1].Value);
        }
    }
}
