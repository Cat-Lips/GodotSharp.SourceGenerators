using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GodotSharp.SourceGenerators
{
    public abstract class SourceGeneratorForDeclaredPropertiesWithAttribute<TAttribute> : SourceGeneratorForDeclaredMembersWithAttribute<TAttribute, PropertyDeclarationSyntax>
        where TAttribute : Attribute
    {
        protected abstract (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, IPropertySymbol symbol, AttributeData attribute);
        protected sealed override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, ISymbol symbol, AttributeData attribute)
            => GenerateCode(compilation, (IPropertySymbol)symbol, attribute);
    }
}
