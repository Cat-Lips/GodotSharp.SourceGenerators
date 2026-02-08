using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.ShaderGlobalsExtensions;

[Generator]
internal class ShaderGlobalsSourceGenerator : SourceGeneratorForDeclaredTypeWithAttribute<Godot.ShaderGlobalsAttribute>
{
    private static Template ShaderGlobalsTemplate => field ??= Template.Parse(Resources.ShaderGlobalsTemplate);

    protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, INamedTypeSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
    {
        var model = new ShaderGlobalsDataModel(symbol, GD.ROOT(node, options));
        Log.Debug($"--- MODEL ---\n{model}\n");

        var output = ShaderGlobalsTemplate.Render(model, Shared.Utils);
        Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

        return (output, null);
    }
}
