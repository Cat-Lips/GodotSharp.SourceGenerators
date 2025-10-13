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

    public static bool IsOrInherits(this ITypeSymbol type, string qualifiedBaseType, Compilation compilation)
        => type.IsOrInherits(compilation.GetTypeByMetadataName(qualifiedBaseType));

    public static bool IsOrInherits(this ITypeSymbol type, ITypeSymbol baseType)
    {
        for (var current = type; current != null; current = current.BaseType)
        {
            if (SymbolEqualityComparer.Default.Equals(current, baseType))
                return true;
        }

        return false;
    }

    public static bool IsOrInherits(this ITypeSymbol source, string unqualifiedType)
    {
        for (var current = source; current != null; current = current.BaseType)
        {
            if (current.Name == unqualifiedType)
                return true;
        }

        return false;
    }

    public static string GetDeclaredAccessibility(this ISymbol symbol)
        => SyntaxFacts.GetText(symbol.DeclaredAccessibility);

    public static bool IsEnum(this ITypeSymbol type)
        => type.TypeKind is TypeKind.Enum;

    public static bool IsNullable(this ITypeSymbol type)
        => type.OriginalDefinition.SpecialType is SpecialType.System_Nullable_T;
}
