using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.InputMapExtensions;

[Generator]
internal class InputMapSourceGenerator : SourceGeneratorForDeclaredTypeWithAttribute<Godot.InputMapAttribute>
{
    private static Template InputMapTemplate => field ??= Template.Parse(Resources.InputMapTemplate);

    protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, INamedTypeSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
    {
        var model = new InputMapDataModel(symbol, ReconstructAttribute().ClassPath, options.TryGetGodotProjectDir());
        Log.Debug($"--- MODEL ---\n{model}\n");

        var output = InputMapTemplate.Render(model, member => member.Name);
        Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

        return (output, null);

        Godot.InputMapAttribute ReconstructAttribute()
            => new((string)attribute.ConstructorArguments[0].Value);
    }
}
