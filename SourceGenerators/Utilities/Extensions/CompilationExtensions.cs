using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators;

public static class CompilationExtensions
{
    public static string GetFullName(this Compilation compilation, string type, string hint)
    {
        return ResolveDuplicates(compilation.GetSymbolsWithName(type, SymbolFilter.Type))?.FullName();

        ISymbol ResolveDuplicates(IEnumerable<ISymbol> symbols)
        {
            if (symbols.Skip(1).Any())
            {
                symbols = symbols // Ignore generics
                    .Where(x => x.MetadataName == type);

                if (symbols.Skip(1).Any())
                {
                    // Differentiate by path
                    symbols = symbols.Where(x => x.Locations.Select(x => x.GetLineSpan().Path.Replace("\\", "/")).Any(x => x.EndsWith(hint)));

                    if (symbols.Skip(1).Any())
                        Log.Warn($"Choosing first from multiple candidates [Type: {type}, Namespaces: {string.Join("|", symbols.Select(x => x.NamespaceOrNull() ?? "<global>"))}]");
                }
            }

            return symbols.FirstOrDefault();
        }
    }

    public static string GetValidType(this Compilation compilation, string type)
    {
        return SearchCurrentCompilation()
            ?? SearchReferencedAssemblies();

        string SearchCurrentCompilation()
            => compilation.ContainsSymbolsWithName(type, SymbolFilter.Type) ? type : null;

        string SearchReferencedAssemblies()
        {
            var refs = compilation.GetUsedAssemblyReferences()
                .Select(compilation.GetAssemblyOrModuleSymbol)
                .Cast<IAssemblySymbol>();

            foreach (var asm in refs)
            {
                if (asm.Name.StartsWith("GodotSharp"))
                {
                    // Some names in Godot have mismatched case (mostly 2d/2D & 3d,3D)
                    var csType = asm.TypeNames.FirstOrDefault(x => x.Equals(type, StringComparison.OrdinalIgnoreCase));
                    if (csType is not null) return csType;
                }

                if (asm.TypeNames.Contains(type))
                    return type;
            }

            return null;
        }
    }
}
