using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.TranslationExtensions;

[Generator]
internal class TranslationSourceGenerator : SourceGeneratorForDeclaredTypeWithAttribute<Godot.TRAttribute>
{
    private static Template TranslationTemplate => field ??= Template.Parse(Resources.TranslationTemplate);

    protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, INamedTypeSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
    {
        var data = ReconstructAttribute();

        var (source, error) = GD.GetRealPath(data.Source, node, options, "csv");
        if (error is not null) return (null, error);

        var model = new TranslationDataModel(symbol, source, data.Xtras);
        Log.Debug($"--- MODEL ---\n{model}\n");

        var output = TranslationTemplate.Render(model, Shared.Utils);
        Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

        return (output, null);

        Godot.TRAttribute ReconstructAttribute() => new(
            (string)attribute.ConstructorArguments[0].Value,
            (bool)attribute.ConstructorArguments[1].Value);
    }
}
