using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.InstantiableExtensions;

[Generator]
internal class InstantiableSourceGenerator : SourceGeneratorForDeclaredTypeWithAttribute<Godot.InstantiableAttribute>
{
    private static Template InstantiableTemplate => field ??= Template.Parse(Resources.InstantiableTemplate);

    protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, INamedTypeSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
    {
        var model = new InstantiableDataModel(symbol, ReconstructAttribute(), GD.TSCN(node, options));
        Log.Debug($"--- MODEL ---\n{model}\n");

        var output = InstantiableTemplate.Render(model, Shared.Utils);
        Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

        return (output, null);

        Godot.InstantiableAttribute ReconstructAttribute() => new(
            (string)attribute.ConstructorArguments[0].Value,
            (string)attribute.ConstructorArguments[1].Value,
            (Scope)attribute.ConstructorArguments[2].Value);
    }
}
