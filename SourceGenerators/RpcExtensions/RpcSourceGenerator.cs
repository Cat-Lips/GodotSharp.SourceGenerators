using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.RpcExtensions;

[Generator]
internal class RpcSourceGenerator : SourceGeneratorForDeclaredMethodWithAttribute
{
    private static Template RpcTemplate => field ??= Template.Parse(Resources.RpcTemplate);

    protected override string AttributeType => "Rpc";

    protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, IMethodSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
    {
        var model = new RpcDataModel(symbol);
        Log.Debug($"--- MODEL ---\n{model}\n");

        var output = RpcTemplate.Render(model, Shared.Utils);
        Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

        return (output, null);
    }
}
