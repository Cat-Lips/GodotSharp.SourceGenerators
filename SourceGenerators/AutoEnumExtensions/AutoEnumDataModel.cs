using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.AutoEnumExtensions;

internal class AutoEnumDataModel : ClassDataModel
{
    public string IdentityProperty { get; }
    public string[] EnumMembers { get; }

    public AutoEnumDataModel(INamedTypeSymbol symbol, string identityProperty)
        : base(symbol)
    {
        IdentityProperty = identityProperty;

        EnumMembers = symbol
            .GetMembers()
            .OfType<IFieldSymbol>()
            .Where(f =>
                f.IsStatic &&
                f.IsReadOnly &&
                SymbolEqualityComparer.Default.Equals(f.Type, symbol))
            .Select(f => f.Name)
            .ToArray();
    }

    protected override string Str()
    {
        return string.Join("\n", EnumMembers.Select(x => $" - Member: {x}"))
             + $"\nIdentityProperty: {IdentityProperty}";
    }
}
