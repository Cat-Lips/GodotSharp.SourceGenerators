using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.AutoloadExtensions;

[Generator]
internal class AutoloadSourceGenerator : SourceGeneratorForDeclaredTypeWithAttribute<Godot.AutoloadAttribute>
{
    private static Template AutoloadTemplate => field ??= Template.Parse(Resources.AutoloadTemplate);
    private static readonly string RenameAttribute = typeof(Godot.AutoloadRenameAttribute).Name;

    protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, INamedTypeSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
    {
        var csPath = GD.CS(node);
        var gdRoot = GD.ROOT(options, csPath);
        var lookup = GetAutoloadRenames();

        var model = new AutoloadDataModel(compilation, symbol, csPath, gdRoot, lookup);
        Log.Debug($"--- MODEL ---\n{model}\n");

        var output = AutoloadTemplate.Render(model, Shared.Utils);
        Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

        return (output, null);

        IDictionary<string, string> GetAutoloadRenames()
        {
            return symbol.GetAttributes()
                .Where(x => x.AttributeClass.Name == RenameAttribute)
                .ToDictionary(
                    x => (string)x.ConstructorArguments[1].Value,   // 1: GodotName
                    x => (string)x.ConstructorArguments[0].Value);  // 0: DisplayName
        }
    }
}
