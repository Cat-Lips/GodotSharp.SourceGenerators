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
        var cfg = ReconstructAttribute();
        var model = new OnInstantiateDataModel(symbol, cfg.PrimaryAttribute, cfg.ConstructorScope, GD.TSCN(node, options));
        Log.Debug($"--- MODEL ---\n{model}\n");

        var output = OnInstantiateTemplate.Render(model, member => member.Name);
        Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

        return (output, null);

        Godot.OnInstantiateAttribute ReconstructAttribute()
        {
            var arg = attribute.ConstructorArguments[0].Value;
            return arg is bool b ? new(b) : new((Scope)arg);
        }
    }
}
