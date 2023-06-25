using Microsoft.CodeAnalysis;
using Scriban;

namespace GodotSharp.SourceGenerators.OnImportExtensions
{
    [Generator]
    internal class OnImportSourceGenerator : SourceGeneratorForDeclaredMethodWithAttribute<Godot.OnImportAttribute>
    {
        private static Template _onImportTemplate;
        private static Template OnImportTemplate => _onImportTemplate ??= Template.Parse(Resources.OnImportTemplate);

        protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, IMethodSymbol symbol, AttributeData attribute)
        {
            var model = new OnImportDataModel(symbol, ReconstructAttribute());
            Log.Debug($"--- MODEL ---\n{model}\n");

            var output = OnImportTemplate.Render(model, member => member.Name);
            Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

            return (output, null);

            Godot.OnImportAttribute ReconstructAttribute() => new
            (
                (string)attribute.ConstructorArguments[0].Value,
                (string)attribute.ConstructorArguments[1].Value,
                (string)attribute.ConstructorArguments[2].Value,
                (string)attribute.ConstructorArguments[3].Value,
                (string)attribute.ConstructorArguments[4].Value,
                (float)attribute.ConstructorArguments[5].Value,
                (int)attribute.ConstructorArguments[6].Value,
                (string)attribute.ConstructorArguments[7].Value
            );
        }
    }
}
