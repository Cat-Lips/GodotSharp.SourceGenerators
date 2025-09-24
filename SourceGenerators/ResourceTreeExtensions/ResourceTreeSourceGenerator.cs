using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.ResourceTreeExtensions;

[Generator]
internal class ResourceTreeSourceGenerator : SourceGeneratorForDeclaredTypeWithAttribute<Godot.ResourceTreeAttribute>
{
    private static Template ResourceTreeTemplate => field ??= Template.Parse(Resources.ResourceTreeTemplate);

    protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, INamedTypeSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
    {
        var model = new ResourceTreeDataModel(compilation, symbol, options.TryGetGodotProjectDir());
        Log.Debug($"--- MODEL ---\n{model}\n");

        var output = ResourceTreeTemplate.Render(model, member => member.Name);
        Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

        return (output, null);
    }
}
