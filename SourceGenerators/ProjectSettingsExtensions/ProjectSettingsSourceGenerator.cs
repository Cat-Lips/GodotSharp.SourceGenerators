using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.ProjectSettingsExtensions;

[Generator]
internal class ProjectSettingsSourceGenerator : SourceGeneratorForDeclaredTypeWithAttribute<Godot.ProjectSettingsAttribute>
{
    private static Template ProjectSettingsTemplate => field ??= Template.Parse(Resources.ProjectSettingsTemplate);

    protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, INamedTypeSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
    {
        var model = new ProjectSettingsDataModel(symbol, ReconstructAttribute());
        Log.Debug($"--- MODEL ---\n{model}\n");

        var output = ProjectSettingsTemplate.Render(model, Shared.Utils);
        Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

        return (output, null);

        Godot.ProjectSettingsAttribute ReconstructAttribute() => new(
            (Generate)attribute.ConstructorArguments[0].Value);
    }
}
