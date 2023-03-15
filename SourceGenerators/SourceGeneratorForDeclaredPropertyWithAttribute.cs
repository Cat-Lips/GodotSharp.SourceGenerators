using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GodotSharp.SourceGenerators
{
    public abstract class SourceGeneratorForDeclaredPropertyWithAttribute<TAttribute> : SourceGeneratorForDeclaredMemberWithAttribute<TAttribute, PropertyDeclarationSyntax>
        where TAttribute : Attribute
    {
        protected abstract (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, IPropertySymbol symbol, AttributeData attribute);
        protected sealed override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, ISymbol symbol, AttributeData attribute)
            => GenerateCode(compilation, node, (IPropertySymbol)symbol, attribute);
    }
}
