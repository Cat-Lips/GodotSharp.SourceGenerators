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
        var cfg = ReconstructAttribute();

        var (source, error) = GD.GetRealDir(cfg.Source, node, options);
        if (error is not null) return (null, error);

        var gdRoot = GD.ROOT(node, options);
        var model = new ResourceTreeDataModel(compilation, symbol, gdRoot, source, cfg);
        Log.Debug($"--- MODEL ---\n{model}\n");

        var output = ResourceTreeTemplate.Render(model, Shared.Utils);
        Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

        return (output, null);

        Godot.ResourceTreeAttribute ReconstructAttribute() => new(
            (string)attribute.ConstructorArguments[0].Value,
            (ResG)attribute.ConstructorArguments[1].Value,
            (ResX)attribute.ConstructorArguments[2].Value,
            attribute.ConstructorArguments[3].Values.Args<string>(),
            attribute.ConstructorArguments[4].Values.Args<string>());
    }
}
