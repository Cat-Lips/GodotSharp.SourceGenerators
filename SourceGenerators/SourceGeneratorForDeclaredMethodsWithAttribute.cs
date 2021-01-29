using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GodotSharp.SourceGenerators
{
    public abstract class SourceGeneratorForDeclaredMethodsWithAttribute<TAttribute> : SourceGeneratorForDeclaredMembersWithAttribute<TAttribute, MethodDeclarationSyntax>
        where TAttribute : Attribute
    {
        protected abstract (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, IMethodSymbol symbol, AttributeData attribute);
        protected sealed override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, ISymbol symbol, AttributeData attribute)
            => GenerateCode(compilation, (IMethodSymbol)symbol, attribute);
    }
}
