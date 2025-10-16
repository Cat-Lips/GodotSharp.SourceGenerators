using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.ShaderExtensions;

[Generator]
internal class ShaderSourceGenerator : SourceGeneratorForDeclaredTypeWithAttribute<Godot.ShaderAttribute>
{
    private static Template ShaderTemplate => field ??= Template.Parse(Resources.ShaderTemplate);

    protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, INamedTypeSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
    {
        var data = ReconstructAttribute();

        var (source, error) = GD.GetRealPath(data.Source, node, options, "gdshader");
        if (error is not null) return (null, error);

        var model = new ShaderDataModel(compilation, symbol, source, data.GenerateTests);
        Log.Debug($"--- MODEL ---\n{model}\n");

        var output = ShaderTemplate.Render(model, Shared.Utils);
        Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

        return (output, null);

        Godot.ShaderAttribute ReconstructAttribute() => new(
            (string)attribute.ConstructorArguments[0].Value,
            (bool)attribute.ConstructorArguments[1].Value);
    }
}
