using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace GodotSharp.SourceGenerators;

public abstract class SourceGeneratorForDeclaredMethodWithAttribute : SourceGeneratorForDeclaredMemberWithAttribute<MethodDeclarationSyntax>
{
    protected abstract (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, IMethodSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options);
    protected sealed override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, ISymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
        => GenerateCode(compilation, node, (IMethodSymbol)symbol, attribute, options);
}

public abstract class SourceGeneratorForDeclaredMethodWithAttribute<TAttribute> : SourceGeneratorForDeclaredMemberWithAttribute<TAttribute, MethodDeclarationSyntax>
    where TAttribute : Attribute
{
    protected abstract (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, IMethodSymbol symbol, AttributeData attribute, AnalyzerConfigOptions options);
    protected sealed override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, ISymbol symbol, AttributeData attribute, AnalyzerConfigOptions options)
        => GenerateCode(compilation, node, (IMethodSymbol)symbol, attribute, options);
}
