using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.OnInstantiateExtensions;

[Generator]
internal class OnInstantiateSourceGenerator : SourceGeneratorForDeclaredMethodWithAttribute<Godot.OnInstantiateAttribute>
{
    private static Template OnInstantiateTemplate => field ??= Template.Parse(Resources.OnInstantiateTemplate);

    protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, IMethodSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
    {
        var model = new OnInstantiateDataModel(compilation, symbol, node, ReconstructAttribute().ConstructorScope, options.TryGetGodotProjectDir());
        Log.Debug($"--- MODEL ---\n{model}\n");

        var output = OnInstantiateTemplate.Render(model, member => member.Name);
        Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

        return (output, null);

        Godot.OnInstantiateAttribute ReconstructAttribute()
            => new((string)attribute.ConstructorArguments[0].Value);
    }
}
