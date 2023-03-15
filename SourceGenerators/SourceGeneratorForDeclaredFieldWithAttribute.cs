using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GodotSharp.SourceGenerators
{
    public abstract class SourceGeneratorForDeclaredFieldWithAttribute<TAttribute> : SourceGeneratorForDeclaredMemberWithAttribute<TAttribute, FieldDeclarationSyntax>
        where TAttribute : Attribute
    {
        protected abstract (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, IFieldSymbol symbol, AttributeData attribute);
        protected sealed override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, ISymbol symbol, AttributeData attribute)
            => GenerateCode(compilation, node, (IFieldSymbol)symbol, attribute);

        protected override SyntaxNode Node(FieldDeclarationSyntax node)
            => node.Declaration.Variables.Single();
    }
}
