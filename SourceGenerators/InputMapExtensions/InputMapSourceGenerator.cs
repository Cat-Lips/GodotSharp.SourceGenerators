using Microsoft.CodeAnalysis;
using Scriban;

namespace GodotSharp.SourceGenerators.InputMapExtensions
{
    [Generator]
    internal class InputMapSourceGenerator : SourceGeneratorForDeclaredTypeWithAttribute<Godot.InputMapAttribute>
    {
        private static Template _inputMapTemplate;
        private static Template InputMapTemplate => _inputMapTemplate ??= Template.Parse(Resources.InputMapTemplate);

        protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, INamedTypeSymbol symbol, AttributeData attribute)
        {
            var model = new InputMapDataModel(symbol, ReconstructAttribute().ClassPath);
            Log.Debug($"--- MODEL ---\n{model}");

            var output = InputMapTemplate.Render(model, member => member.Name);
            Log.Debug($"--- OUTPUT ---\n{output}<END>");

            return (output, null);

            Godot.InputMapAttribute ReconstructAttribute()
                => new((string)attribute.ConstructorArguments[0].Value);
        }
    }
}
