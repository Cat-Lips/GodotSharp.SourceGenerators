using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.Utilities.Extensions
{
    public static class SymbolExtensions
    {
        public static string NamespaceOrNull(this ISymbol symbol)
            => symbol.ContainingNamespace.IsGlobalNamespace ? null : string.Join(".", symbol.ContainingNamespace.ConstituentNamespaces);

        public static (string NamespaceDeclaration, string NamespaceClosure, string NamespaceIndent) GetNamespaceDeclaration(this ISymbol symbol, string indent = "    ")
        {
            var ns = symbol.NamespaceOrNull();
            if (ns is null) return (null, null, null);
            return ($"namespace {ns}\n{{\n", "}\n", indent);
        }
    }
}
