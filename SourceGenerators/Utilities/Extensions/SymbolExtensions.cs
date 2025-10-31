using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace GodotSharp.SourceGenerators;

public static class SymbolExtensions
{
    public static string Namespace(this ISymbol symbol)
        => symbol.ContainingNamespace.FullName();

    public static bool HasNamespace(this ISymbol symbol)
        => !symbol.ContainingNamespace.IsGlobalNamespace;

    public static string NamespaceOrNull(this ISymbol symbol)
        => symbol.HasNamespace() ? symbol.Namespace() : null;

    public static string FullName(this ISymbol symbol)
        => symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).TrimPrefix("global::");

    public static string GlobalName(this ISymbol symbol)
        => symbol.HasNamespace() ? symbol.FullName() : symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

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

    public static string Scope(this ISymbol symbol) => symbol.GetDeclaredAccessibility();
    public static string GetDeclaredAccessibility(this ISymbol symbol)
        => SyntaxFacts.GetText(symbol.DeclaredAccessibility);

    public static bool Is(this ISymbol symbol, ISymbol other)
        => SymbolEqualityComparer.Default.Equals(symbol, other);

    public static T[] Args<T>(this ImmutableArray<TypedConstant> values)
        => values.IsDefault ? null : [.. values.Select(x => (T)x.Value)];
}
