using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.SingletonExtensions;

[Generator]
internal class SingletonSourceGenerator : SourceGeneratorForDeclaredTypeWithAttribute<Godot.SingletonAttribute>
{
    private static Template SingletonTemplate => field ??= Template.Parse(Resources.SingletonTemplate);

    protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, INamedTypeSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
    {
        var model = new SingletonDataModel(symbol, ReconstructAttribute().InitFunc, GD.TSCN(node, options));
        Log.Debug($"--- MODEL ---\n{model}\n");

        var output = SingletonTemplate.Render(model, Shared.Utils);
        Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

        return (output, null);

        Godot.SingletonAttribute ReconstructAttribute()
            => new((string)attribute.ConstructorArguments[0].Value);
    }
}
