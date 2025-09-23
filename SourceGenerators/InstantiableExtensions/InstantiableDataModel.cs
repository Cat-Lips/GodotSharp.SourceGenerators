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
            var type = p.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            return p.NullableAnnotation is NullableAnnotation.Annotated ? $"{type}?" : type;
        }

        static string Default(IParameterSymbol p)
        {
            return p.HasExplicitDefaultValue ? $" = {Format(p.ExplicitDefaultValue, p.Type)}" : "";

            string Format(object dflt, ITypeSymbol type)
            {
                return dflt is null ? FormatDefault() : FormatType(type);

                string FormatDefault()
                    => p.NullableAnnotation is NullableAnnotation.NotAnnotated ? "default!" : "default";

                string FormatType(ITypeSymbol type) => type.SpecialType switch
                {
                    SpecialType.System_Char => SymbolDisplay.FormatLiteral((char)dflt, quote: true),
                    SpecialType.System_String => SymbolDisplay.FormatLiteral((string)dflt, quote: true),
                    SpecialType.System_Boolean => $"{((bool)dflt ? "true" : "false")}",

                    SpecialType.System_Single => $"{((float)dflt).ToString("R", CultureInfo.InvariantCulture)}f",
                    SpecialType.System_Double => $"{((double)dflt).ToString("R", CultureInfo.InvariantCulture)}",
                    SpecialType.System_Decimal => $"{((decimal)dflt).ToString(CultureInfo.InvariantCulture)}m",

                    // Required if value > int
                    SpecialType.System_UInt32 => $"{dflt}u",
                    SpecialType.System_Int64 => $"{dflt}L",
                    SpecialType.System_UInt64 => $"{dflt}UL",

                    _ when IsEnum(type) => FormatEnum(type),
                    _ when IsNullable(type) => FormatNullable(type),

                    _ => $"{dflt}",
                };

                string FormatEnum(ITypeSymbol type)
                {
                    var member = ((INamedTypeSymbol)type).GetMembers().OfType<IFieldSymbol>()
                        .FirstOrDefault(x => x.ConstantValue?.Equals(dflt) is true);
                    return member is null
                        ? $"({Type()}){dflt}"
                        : $"{Type()}.{member.Name}";

                    string Type()
                        => type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                }

                string FormatNullable(ITypeSymbol type)
                    => FormatType(((INamedTypeSymbol)type).TypeArguments.First());

                static bool IsEnum(ITypeSymbol type)
                    => type.TypeKind is TypeKind.Enum;

                static bool IsNullable(ITypeSymbol type)
                    => type.OriginalDefinition.SpecialType is SpecialType.System_Nullable_T;
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
