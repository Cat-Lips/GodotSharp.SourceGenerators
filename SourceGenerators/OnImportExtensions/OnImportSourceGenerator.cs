using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.OnImportExtensions;

[Generator]
internal class OnImportSourceGenerator : SourceGeneratorForDeclaredMethodWithAttribute<Godot.OnImportAttribute>
{
    private static Template OnImportTemplate => field ??= Template.Parse(Resources.OnImportTemplate);

    protected override IEnumerable<(string Name, string Source)> StaticSources
    {
        get
        {
            yield return (nameof(Resources.HintAttribute), Resources.HintAttribute);
            yield return (nameof(Resources.OnImportEditorPlugin), Resources.OnImportEditorPlugin);
        }
    }

    protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, IMethodSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
    {
        var model = new OnImportDataModel(symbol, ReconstructAttribute());
        Log.Debug($"--- MODEL ---\n{model}\n");

        var output = OnImportTemplate.Render(model, member => member.Name);
        Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

        return (output, null);

        Godot.OnImportAttribute ReconstructAttribute() => new
        (
            (string)attribute.ConstructorArguments[0].Value,
            (string)attribute.ConstructorArguments[1].Value,
            (string)attribute.ConstructorArguments[2].Value,
            (string)attribute.ConstructorArguments[3].Value,
            (float)attribute.ConstructorArguments[4].Value,
            (int)attribute.ConstructorArguments[5].Value,
            (string)attribute.ConstructorArguments[6].Value
        );
    }
}
