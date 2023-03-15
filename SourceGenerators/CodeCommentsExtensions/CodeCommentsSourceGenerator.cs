using Microsoft.CodeAnalysis;
using Scriban;

namespace GodotSharp.SourceGenerators.CodeCommentsExtensions
{
    [Generator]
    internal class CodeCommentsSourceGenerator : SourceGeneratorForDeclaredTypeWithAttribute<Godot.CodeCommentsAttribute>
    {
        private static Template _codeCommentsTemplate;
        private static Template CodeCommentsTemplate => _codeCommentsTemplate ??= Template.Parse(Resources.CodeCommentsTemplate);

        protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, INamedTypeSymbol symbol, AttributeData attribute)
        {
            var model = new CodeCommentsDataModel(symbol, node, ReconstructAttribute().Strip);
            Log.Debug(); Log.Debug($"--- MODEL ---\n{model}");

            var output = CodeCommentsTemplate.Render(model, member => member.Name);
            Log.Debug(); Log.Debug($"--- OUTPUT ---\n{output}<END>");

            return (output, null);

            Godot.CodeCommentsAttribute ReconstructAttribute()
                => new((string)attribute.ConstructorArguments[0].Value);
        }
    }
}
