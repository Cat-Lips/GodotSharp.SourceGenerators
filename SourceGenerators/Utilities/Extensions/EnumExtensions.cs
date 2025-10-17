using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators;

public static class EnumExtensions
{
    public static INamedTypeSymbol GetEnumType(this Compilation compilation, string name)
    {
        var enums = GetEnums().Take(2).ToArray();
        return enums.Length is 1 ? enums[0] : null;

        IEnumerable<INamedTypeSymbol> GetEnums() => compilation
            .GetSymbolsWithName(name, SymbolFilter.Type)
            .Cast<INamedTypeSymbol>()
            .Where(x => x.IsEnum());
    }

    public static string GetEnumValue(this ITypeSymbol type, object value)
    {
        var member = type?.GetMembers().OfType<IFieldSymbol>()
            .FirstOrDefault(m => Equals(m.ConstantValue, value));

        return member is null
            ? $"({type.FullName()}){value}"
            : $"{type.FullName()}.{member.Name}";
    }
}
