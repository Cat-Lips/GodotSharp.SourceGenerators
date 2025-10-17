using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators;

public static class TypeExtensions
{
    public static bool IsOrInherits(this ITypeSymbol type, ITypeSymbol baseType)
    {
        for (var current = type; current != null; current = current.BaseType)
        {
            if (SymbolEqualityComparer.Default.Equals(current, baseType))
                return true;
        }

        return false;
    }

    public static bool IsOrInherits(this ITypeSymbol type, string baseType)
    {
        for (var current = type; current != null; current = current.BaseType)
        {
            if (current.Name == baseType)
                return true;
        }

        return false;
    }

    public static bool IsEnum(this ITypeSymbol type)
        => type.TypeKind is TypeKind.Enum;

    public static bool IsNullable(this ITypeSymbol type)
        => type.OriginalDefinition.SpecialType is SpecialType.System_Nullable_T;
}
