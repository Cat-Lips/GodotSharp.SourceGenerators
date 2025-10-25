using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.AutoEnumExtensions;

internal class AutoEnumClassDataModel(INamedTypeSymbol symbol) : ClassDataModel(symbol)
{
    public string[] Members { get; } = MatchingMembers(symbol);

    private static string[] MatchingMembers(INamedTypeSymbol symbol)
        => symbol.GetMembers().OfType<IFieldSymbol>()
            .Where(f => f.IsStatic && f.IsReadOnly && f.Type.Is(symbol))
            .Select(f => f.Name).ToArray();

    protected override string Str()
        => $"{ClassName} => {Members.Join("|")}";
}
