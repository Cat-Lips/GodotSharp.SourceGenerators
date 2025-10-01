using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.ShaderNamesExtensions;

[Generator]
internal class ShaderNamesSourceGenerator : SourceGeneratorForDeclaredTypeWithAttribute<Godot.ShaderNamesAttribute>
{
    private static Template ShaderNamesTemplate => field ??= Template.Parse(Resources.ShaderNamesTemplate);

    protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, INamedTypeSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
    {
        var data = ReconstructAttribute();

        var (source, error) = GD.GetRealPath(data.Source, node, options, "gdshader");
        if (error is not null) return (null, error);

        var model = new ShaderNamesDataModel(compilation, symbol, source);
        Log.Debug($"--- MODEL ---\n{model}\n");

        var output = ShaderNamesTemplate.Render(model, Shared.Utils);
        Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

        return (output, null);

        Godot.ShaderNamesAttribute ReconstructAttribute()
            => new((string)attribute.ConstructorArguments[0].Value);
    }
}
