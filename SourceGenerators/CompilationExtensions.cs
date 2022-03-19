using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators
{
    public static class CompilationExtensions
    {
        public static string GetNamespace(this Compilation compilation, string type)
        {
            return compilation
                .GetSymbolsWithName(type, SymbolFilter.Type)
                .First().NamespaceOrNull();
        }

        public static string GetFullName(this Compilation compilation, string type)
        {
            var ns = compilation.GetNamespace(type);
            return ns is null ? type : $"{ns}.{type}";
        }
    }
}
