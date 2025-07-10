using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace GodotSharp.SourceGenerators;

public static class SymbolExtensions
{
    public static string FullName(this ISymbol symbol)
    {
        var ns = symbol.NamespaceOrNull();
        return ns is null ? $"global::{symbol.Name}" : $"{ns}.{symbol.Name}";
    }

    public static string NamespaceOrNull(this ISymbol symbol)
        => symbol.ContainingNamespace.IsGlobalNamespace ? null : string.Join(".", symbol.ContainingNamespace.ConstituentNamespaces);

    public static string GetNamespaceDeclaration(this ISymbol symbol)
    {
        var ns = symbol.NamespaceOrNull();
        return ns is null ? null : $"namespace {ns};\n";
    }

    public static INamedTypeSymbol OuterType(this ISymbol symbol)
        => symbol.ContainingType?.OuterType() ?? symbol as INamedTypeSymbol;

    public static string ClassDef(this INamedTypeSymbol symbol)
        => symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);

    public static string ClassPath(this INamedTypeSymbol symbol)
        => symbol.DeclaringSyntaxReferences.FirstOrDefault()?.SyntaxTree?.FilePath;

    public static string GeneratePartialClass(this INamedTypeSymbol symbol, IEnumerable<string> content, IEnumerable<string> usings = null)
    {
        return $@"
{usings?.Join("\n")}

{symbol.GetNamespaceDeclaration()}
partial class {symbol.ClassDef()}
{{
    {content?.Join("\n")}
}}".TrimStart();
    }

    public static bool InheritsFrom(this ITypeSymbol symbol, string type)
    {
        var baseType = symbol.BaseType;

        while (baseType != null)
        {
            if (baseType.Name == type)
                return true;

            baseType = baseType.BaseType;
        }

        return false;
    }

    public static string GetDeclaredAccessibility(this ISymbol symbol)
        => SyntaxFacts.GetText(symbol.DeclaredAccessibility);
}
