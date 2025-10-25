using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.AutoEnumExtensions;

[Generator]
internal class AutoEnumClassSourceGenerator : SourceGeneratorForDeclaredTypeWithAttribute<Godot.AutoEnumAttribute>
{
    private static Template AutoEnumClassTemplate => field ??= Template.Parse(Resources.AutoEnumClassTemplate);

    protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, INamedTypeSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
    {
        var model = new AutoEnumClassDataModel(symbol);
        Log.Debug($"--- MODEL ---\n{model}\n");

        var output = AutoEnumClassTemplate.Render(model, Shared.Utils);
        Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

        return (output, null);
    }
}
