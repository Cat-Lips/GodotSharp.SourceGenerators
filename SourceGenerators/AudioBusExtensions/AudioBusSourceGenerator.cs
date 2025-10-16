using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.AudioBusExtensions;

[Generator]
internal class AudioBusSourceGenerator : SourceGeneratorForDeclaredTypeWithAttribute<Godot.AudioBusAttribute>
{
    private static Template AudioBusTemplate => field ??= Template.Parse(Resources.AudioBusTemplate);

    protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, INamedTypeSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
    {
        var data = ReconstructAttribute();

        var (source, error) = GD.GetRealPath(data.Source, node, options, "tres");
        if (error is not null) return (null, error);

        var model = new AudioBusDataModel(symbol, source);
        Log.Debug($"--- MODEL ---\n{model}\n");

        var output = AudioBusTemplate.Render(model, Shared.Utils);
        Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

        return (output, null);

        Godot.AudioBusAttribute ReconstructAttribute() => new(
            (string)attribute.ConstructorArguments[0].Value);
    }
}
