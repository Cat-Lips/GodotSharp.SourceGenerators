using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.AutoloadExtensions;

using NameMap = (string DisplayName, string GodotName);

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
        var autoload = ReconstructAttribute();
        var model = new AutoloadDataModel(compilation, symbol, autoload.ClassPath, options.TryGetGodotProjectDir(), autoload.NameMap);
        Log.Debug($"--- MODEL ---\n{model}\n");

        var output = AutoloadTemplate.Render(model, member => member.Name);
        Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

        return (output, null);

        Godot.AutoloadAttribute ReconstructAttribute()
        {
            return attribute.ConstructorArguments.Length switch
            {
                1 => new(ClassPath(0)),
                2 => new(NameMapArray(0), ClassPath(1)),
                _ => throw new NotImplementedException(),
            };

            string ClassPath(int i)
                => (string)attribute.ConstructorArguments[i].Value;

            NameMap[] NameMapArray(int i)
            {
                var arg = attribute.ConstructorArguments[i];
                return arg.Kind is TypedConstantKind.Array
                    ? (NameMap[])arg.Value : [(NameMap)arg.Value];
            }
        }
    }
}
