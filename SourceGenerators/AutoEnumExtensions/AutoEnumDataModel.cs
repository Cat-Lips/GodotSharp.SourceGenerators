using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.AutoEnumExtensions;

internal class AutoEnumDataModel : ClassDataModel
{
    public IList<string> EnumMembers { get; }

    public AutoEnumDataModel(INamedTypeSymbol symbol)
        : base(symbol)
    {
        EnumMembers = symbol
            .GetMembers()
            .OfType<IFieldSymbol>()
            .Where(f =>
                f.DeclaredAccessibility == Accessibility.Public &&
                f.IsStatic &&
                f.IsReadOnly &&
                SymbolEqualityComparer.Default.Equals(f.Type, symbol))
            .Select(f => f.Name)
            .ToList();
    }

    protected override string Str()
    {
        return $"Enum Members: {string.Join(", ", EnumMembers)}";
    }
}
