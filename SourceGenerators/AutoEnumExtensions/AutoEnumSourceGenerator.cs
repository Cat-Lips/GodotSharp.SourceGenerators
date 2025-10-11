using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.AutoEnumExtensions;

[Generator]
internal class AutoEnumSourceGenerator : SourceGeneratorForDeclaredTypeWithAttribute<Godot.AutoEnumAttribute>
{
    private static Template AutoEnumTemplate => field ??= Template.Parse(Resources.AutoEnumTemplate);

    protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(
        Compilation compilation,
        SyntaxNode node,
        INamedTypeSymbol symbol,
        AttributeData attribute,
        AnalyzerConfigOptions options)
    {
        var model = new AutoEnumDataModel(symbol);
        Log.Debug($"--- MODEL ---\n{model}\n");

        var output = AutoEnumTemplate.Render(model, member => member.Name);
        Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

        return (output, null);
    }
}
