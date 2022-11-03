using Microsoft.CodeAnalysis;
using Scriban;

namespace GodotSharp.SourceGenerators.GodotOverrideExtensions
{
    [Generator]
    internal class GodotOverrideSourceGenerator : SourceGeneratorForDeclaredMethodWithAttribute<Godot.GodotOverrideAttribute>
    {
        private static Template _godotOverrideTemplate;
        private static Template GodotOverrideTemplate => _godotOverrideTemplate ??= Template.Parse(Resources.GodotOverrideTemplate);

        protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation _, IMethodSymbol symbol, AttributeData attribute)
        {
            var attrib = ReconstructAttribute();

            var model = new GodotOverrideDataModel(symbol, attrib.Replace);
            var output = GodotOverrideTemplate.Render(model, member => member.Name);
            return (output, null);

            Godot.GodotOverrideAttribute ReconstructAttribute()
                => new((bool)attribute.ConstructorArguments[0].Value);
        }
    }
}
