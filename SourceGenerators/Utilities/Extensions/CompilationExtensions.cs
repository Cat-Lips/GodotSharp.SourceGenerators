using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators
{
    public static class CompilationExtensions
    {
        public static string GetNamespace(this Compilation compilation, string type, string hint)
        {
            var symbols = compilation.GetSymbolsWithName(type, SymbolFilter.Type);

            if (symbols.Skip(1).Any())
            {
                symbols = symbols // Ignore generics
                    .Where(x => x.MetadataName == type);

                if (symbols.Skip(1).Any())
                {
                    // Differentiate by path
                    hint = string.Join(@"\", hint.Split('/'));
                    symbols = symbols.Where(x => x.Locations.Select(x => x.GetLineSpan().Path).Any(x => x.EndsWith(hint)));

                    if (symbols.Skip(1).Any())
                        Log.Warn($"Multiple namespace candidates for type (choosing first) [Type: {type}, Namespaces: {string.Join("|", symbols.Select(x => x.NamespaceOrNull() ?? "<global>"))}]");
                }
            }

            return symbols.FirstOrDefault()?.NamespaceOrNull();
        }

        public static string GetFullName(this Compilation compilation, string type, string hint)
        {
            var ns = compilation.GetNamespace(type, hint);
            return ns is null ? $"global::{type}" : $"{ns}.{type}";
        }
    }
}
