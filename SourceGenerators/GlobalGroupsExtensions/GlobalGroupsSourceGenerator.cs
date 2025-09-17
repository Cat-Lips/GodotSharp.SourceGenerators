using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.GlobalGroupsExtensions;

[Generator]
internal class GlobalGroupsSourceGenerator : SourceGeneratorForDeclaredTypeWithAttribute<Godot.GlobalGroupsAttribute>
{
    private static Template GlobalGroupsTemplate => field ??= Template.Parse(Resources.GlobalGroupsTemplate);

    protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, INamedTypeSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
    {
        var model = new GlobalGroupsDataModel(symbol, ReconstructAttribute().ClassPath, options.TryGetGodotProjectDir());
        Log.Debug($"--- MODEL ---\n{model}\n");

        var output = GlobalGroupsTemplate.Render(model, member => member.Name);
        Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

        return (output, null);

        Godot.GlobalGroupsAttribute ReconstructAttribute()
            => new((string)attribute.ConstructorArguments[0].Value);
    }
}
