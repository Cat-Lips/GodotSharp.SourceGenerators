using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GodotSharp.SourceGenerators
{
    public abstract class SourceGeneratorForDeclaredMethodWithAttribute<TAttribute> : SourceGeneratorForDeclaredMemberWithAttribute<TAttribute, MethodDeclarationSyntax>
        where TAttribute : Attribute
    {
        protected abstract (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, IMethodSymbol symbol, AttributeData attribute);
        protected sealed override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, ISymbol symbol, AttributeData attribute)
            => GenerateCode(compilation, node, (IMethodSymbol)symbol, attribute);
    }
}
