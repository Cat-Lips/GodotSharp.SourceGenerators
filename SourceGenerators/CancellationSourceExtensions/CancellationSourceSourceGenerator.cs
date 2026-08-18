using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.CancellationSourceExtensions;

[Generator]
internal class CancellationSourceSourceGenerator : SourceGeneratorForDeclaredTypeWithAttribute<Godot.CancellationSourceAttribute>
{
    private static Template CancellationSourceTemplate => field ??= Template.Parse(Resources.CancellationSourceTemplate);

    protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, INamedTypeSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
    {
        var model = new CancellationSourceDataModel(symbol);
        Log.Debug($"--- MODEL ---\n{model}\n");

        var output = CancellationSourceTemplate.Render(model, Shared.Utils);
        Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

        return (output, null);
    }
}
