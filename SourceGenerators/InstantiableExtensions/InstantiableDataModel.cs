using System.Globalization;
using Godot;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace GodotSharp.SourceGenerators.InstantiableExtensions;

internal class InstantiableDataModel(INamedTypeSymbol symbol, InstantiableAttribute data, string tscn) : ClassDataModel(symbol)
{
    public record InitArgs(string Params, string Args);

    public string Tscn { get; } = tscn;
    public string Initialise { get; } = data.Initialise;
    public string Instantiate { get; } = data.Instantiate;
    public string ConstructorScope { get; } = data.ConstructorScope;
    public InitArgs[] InitList { get; } = GetArgs(symbol, data.Initialise);

    private static InitArgs[] GetArgs(INamedTypeSymbol symbol, string initFunc)
    {
        return [.. symbol
            .GetMembers(initFunc)
            .OfType<IMethodSymbol>()
            .Select(x => new InitArgs(
                Params: string.Join(", ", x.Parameters.Select(FormatParameter)),
                Args: string.Join(", ", x.Parameters.Select(FormatArgument))))];

        static string FormatParameter(IParameterSymbol p)
            => $"{Modifier(p, checkParams: true)}{Type(p)} {p.Name}{Default(p)}";

        static string FormatArgument(IParameterSymbol p)
            => $"{Modifier(p, checkParams: false)}{p.Name}";

        static string Modifier(IParameterSymbol p, bool checkParams) => p.RefKind switch
        {
            RefKind.In => "in ",
            RefKind.Ref => "ref ",
            RefKind.Out => "out ",
            _ => checkParams && p.IsParams ? "params " : "",
        };

        static string Type(IParameterSymbol p)
        {
            return $"{Type()}{Annotation()}";

            string Type() => p.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            string Annotation() => !p.Type.IsNullable() && p.NullableAnnotation is NullableAnnotation.Annotated ? "?" : "";
        }

        static string Default(IParameterSymbol p)
        {
            return p.HasExplicitDefaultValue ? $" = {FormatValue(p.ExplicitDefaultValue, p.Type)}" : "";

            string FormatValue(object value, ITypeSymbol type)
            {
                return value is null ? FormatDefault() : FormatType(type);

                string FormatDefault()
                    => p.NullableAnnotation is NullableAnnotation.NotAnnotated ? "default!" : "default";

                string FormatType(ITypeSymbol type) => type.SpecialType switch
                {
                    SpecialType.System_Char => SymbolDisplay.FormatLiteral((char)value, quote: true),
                    SpecialType.System_String => SymbolDisplay.FormatLiteral((string)value, quote: true),
                    SpecialType.System_Boolean => $"{((bool)value ? "true" : "false")}",

                    SpecialType.System_Single => $"{((float)value).ToString("R", CultureInfo.InvariantCulture)}f",
                    SpecialType.System_Double => $"{((double)value).ToString("R", CultureInfo.InvariantCulture)}",
                    SpecialType.System_Decimal => $"{((decimal)value).ToString(CultureInfo.InvariantCulture)}m",

                    // Required if value > int
                    SpecialType.System_UInt32 => $"{value}u",
                    SpecialType.System_Int64 => $"{value}L",
                    SpecialType.System_UInt64 => $"{value}UL",

                    _ when type.IsEnum() => FormatEnum(type),
                    _ when type.IsNullable() => FormatNullable(type),

                    _ => $"{value}",
                };

                string FormatEnum(ITypeSymbol type)
                {
                    var member = ((INamedTypeSymbol)type).GetMembers().OfType<IFieldSymbol>()
                        .FirstOrDefault(x => x.ConstantValue?.Equals(value) is true);
                    return member is null
                        ? $"({Type()}){value}"
                        : $"{Type()}.{member.Name}";

                    string Type()
                        => type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                }

                string FormatNullable(ITypeSymbol type)
                    => FormatType(((INamedTypeSymbol)type).TypeArguments.First());
            }
        }
    }

    protected override string Str()
    {
        return string.Join("\n", Parts());

        IEnumerable<string> Parts()
        {
            yield return $" - Tscn: {Tscn}";
            yield return $" - Initialise: {Initialise}";
            yield return $" - Instantiate: {Instantiate}";
            yield return $" - ConstructorScope: {ConstructorScope}";
            yield return $" - InitList:";
            foreach (var initFunc in InitList)
                yield return $"   - {initFunc}";
        }
    }
}
