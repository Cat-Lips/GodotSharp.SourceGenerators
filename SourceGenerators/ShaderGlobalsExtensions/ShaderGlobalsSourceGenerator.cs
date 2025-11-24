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
        var data = ReconstructAttribute();
        var model = new ShaderGlobalsDataModel(symbol, data.ClassPath, options.TryGetGodotProjectDir());
        Log.Debug($"--- MODEL ---\n{model}\n");

        var output = ShaderGlobalsTemplate.Render(model, member => member.Name);
        Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

        return (output, null);

        Godot.ShaderGlobalsAttribute ReconstructAttribute() => new(
            (string)attribute.ConstructorArguments[0].Value);
    }
}
