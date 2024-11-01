using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Scriban;

namespace GodotSharp.SourceGenerators.CodeCommentsExtensions
{
    [Generator]
    internal class CodeCommentsSourceGenerator : SourceGeneratorForDeclaredTypeWithAttribute<Godot.CodeCommentsAttribute>
    {
        private static Template _codeCommentsTemplate;
        private static Template CodeCommentsTemplate => _codeCommentsTemplate ??= Template.Parse(Resources.CodeCommentsTemplate);

        protected override CodeGenerationResult GenerateCode(
            Compilation compilation,
            SyntaxNode node,
            INamedTypeSymbol symbol,
            AttributeData attribute,
            AnalyzerConfigOptions options)
        {
            var model = new CodeCommentsDataModel(symbol, node, ReconstructAttribute().Strip);
            Log.Debug($"--- MODEL ---\n{model}\n");

            var output = CodeCommentsTemplate.Render(model, member => member.Name);
            Log.Debug($"--- OUTPUT ---\n{output}<END>\n");

            return new CodeGenerationResult.Success(output);

            Godot.CodeCommentsAttribute ReconstructAttribute()
                => new((string)attribute.ConstructorArguments[0].Value);
        }
    }
}
