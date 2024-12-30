using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.AutoloadExtensions
{
    [Generator]
    internal class AutoloadSourceGenerator : SourceGeneratorForDeclaredTypeWithAttribute<Godot.AutoloadAttribute>
    {
        private static Template AutoloadTemplate => field ??= Template.Parse(Resources.AutoloadTemplate);

        protected override IEnumerable<(string Name, string Source)> StaticSources
        {
            get
            {
                yield return (nameof(Resources.AutoloadExtensions), Resources.AutoloadExtensions);
            }
        }

        protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode _, INamedTypeSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
        {
            var model = new AutoloadDataModel(symbol, ReconstructAttribute().ClassPath, options.TryGetGodotProjectDir());
            Log.Debug($"--- MODEL ---\n{model}\n");

            var output = AutoloadTemplate.Render(model, member => member.Name);
            Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

            return (output, null);

            Godot.AutoloadAttribute ReconstructAttribute()
                => new((string)attribute.ConstructorArguments[0].Value);
        }
    }
}
