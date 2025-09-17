using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.AnimNamesExtensions;

[Generator]
internal class AnimNamesSourceGenerator : SourceGeneratorForDeclaredTypeWithAttribute<Godot.AnimNamesAttribute>
{
    private static Template AnimNamesTemplate => field ??= Template.Parse(Resources.AnimNamesTemplate);

    protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, INamedTypeSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
    {
        var data = ReconstructAttribute();
        var model = new AnimNamesDataModel(symbol, data.Source, data.ClassPath);
        Log.Debug($"--- MODEL ---\n{model}\n");

        var output = AnimNamesTemplate.Render(model, member => member.Name);
        Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

        return (output, null);

        Godot.AnimNamesAttribute ReconstructAttribute() => new(
            (string)attribute.ConstructorArguments[0].Value,
            (string)attribute.ConstructorArguments[1].Value);
    }
}
