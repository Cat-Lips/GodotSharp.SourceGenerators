using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace GodotSharp.SourceGenerators;

public abstract class SourceGeneratorForDeclaredTypeWithAttribute<TAttribute> : SourceGeneratorForDeclaredMemberWithAttribute<TAttribute, TypeDeclarationSyntax>
    where TAttribute : Attribute
{
    protected abstract (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, INamedTypeSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options);
    protected sealed override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, ISymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
        => GenerateCode(compilation, node, (INamedTypeSymbol)symbol, attribute, options);
}
