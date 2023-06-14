using Microsoft.CodeAnalysis;
using Scriban;

namespace GodotSharp.SourceGenerators.OnInstantiateExtensions
{
    [Generator]
    internal class OnInstantiateSourceGenerator : SourceGeneratorForDeclaredMethodWithAttribute<Godot.OnInstantiateAttribute>
    {
        private static Template _onInstantiateTemplate;
        private static Template OnInstantiateTemplate => _onInstantiateTemplate ??= Template.Parse(Resources.OnInstantiateTemplate);

        protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, IMethodSymbol symbol, AttributeData attribute)
        {
            var model = new OnInstantiateDataModel(symbol/*, Context.GetGodotProjectDir()*/);
            Log.Debug($"--- MODEL ---\n{model}\n");

            var output = OnInstantiateTemplate.Render(model, member => member.Name);
            Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

            return (output, null);
        }
    }
}
