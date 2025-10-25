using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.AutoEnumExtensions;

internal class AutoEnumDataModel(INamedTypeSymbol symbol) : ClassDataModel(symbol)
{
    public string[] Members { get; } = EnumMembers(symbol);

    private static string[] EnumMembers(INamedTypeSymbol symbol)
        => symbol.GetMembers().OfType<IFieldSymbol>()
            .Where(f => f.HasConstantValue)
            .Select(f => f.Name).ToArray();

    protected override string Str()
        => $"{ClassName} => {Members.Join("|")}";
}
