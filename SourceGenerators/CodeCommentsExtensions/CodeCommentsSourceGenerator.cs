using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.CodeCommentsExtensions;

[Generator]
internal class CodeCommentsSourceGenerator : SourceGeneratorForDeclaredTypeWithAttribute<Godot.CodeCommentsAttribute>
{
    [field: MaybeNull]
    private static Template CodeCommentsTemplate => field ??= Template.Parse(Resources.CodeCommentsTemplate);

    protected override (string? GeneratedCode, DiagnosticDetail? Error) GenerateCode(Compilation compilation, SyntaxNode node, INamedTypeSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
    {
        var model = new CodeCommentsDataModel(symbol, node, ReconstructAttribute().Strip);
        Log.Debug($"--- MODEL ---\n{model}\n");

        var output = CodeCommentsTemplate.Render(model, member => member.Name);
        Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

        return (output, null);

        Godot.CodeCommentsAttribute ReconstructAttribute()
            => new((string)attribute.ConstructorArguments[0].Value!);
    }
}
