using System.Globalization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace GodotSharp.SourceGenerators;

public static class ParamExtensions
{
    public static string ToParameterString(this IParameterSymbol p)
        => $"{p.Modifier(p.IsParams)}{p.Type()} {p.Name}{p.Default()}";

    public static string ToArgumentString(this IParameterSymbol p, bool castEnum = false)
        => $"{p.Modifier()}{p.Cast(castEnum)}{p.Name}";

    #region Parts

    private static string Modifier(this IParameterSymbol p, bool isParams = false)
    {
        return p.RefKind switch
        {
            RefKind.In => "in ",
            RefKind.Ref => "ref ",
            RefKind.Out => "out ",
            _ => isParams ? "params " : "",
        };
    }

    private static string Cast(this IParameterSymbol p, bool castEnum = false)
        => castEnum && p.Type.IsEnum() ? "(int)" : "";

    private static string Type(this IParameterSymbol p)
    {
        return $"{Type()}{Annotation()}";

        string Type() => p.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        string Annotation() => !p.Type.IsNullable() && p.NullableAnnotation is NullableAnnotation.Annotated ? "?" : "";
    }

    private static string Default(this IParameterSymbol p)
        => p.HasExplicitDefaultValue ? $" = {p.Format(p.ExplicitDefaultValue)}" : "";

    private static string Format(this IParameterSymbol p, object value)
    {
        return value is null ? FormatDefault() : p.Type.Format(value);

        string FormatDefault()
            => p.NullableAnnotation == NullableAnnotation.NotAnnotated ? "default!" : "default";
    }

    private static string Format(this ITypeSymbol type, object value) => type.SpecialType switch
    {
        SpecialType.System_Char => SymbolDisplay.FormatLiteral((char)value, true),
        SpecialType.System_String => SymbolDisplay.FormatLiteral((string)value, true),
        SpecialType.System_Boolean => (bool)value ? "true" : "false",
        SpecialType.System_Single => ((float)value).ToString("R", CultureInfo.InvariantCulture) + "f",
        SpecialType.System_Double => ((double)value).ToString("R", CultureInfo.InvariantCulture),
        SpecialType.System_Decimal => ((decimal)value).ToString(CultureInfo.InvariantCulture) + "m",
        SpecialType.System_UInt32 => $"{value}u",
        SpecialType.System_Int64 => $"{value}L",
        SpecialType.System_UInt64 => $"{value}UL",
        _ when type.IsEnum() => type.GetEnumValue(value),
        _ when type.IsNullable() => ((INamedTypeSymbol)type).TypeArguments.Single().Format(value),
        _ => value.ToString()
    };

    #endregion
}
