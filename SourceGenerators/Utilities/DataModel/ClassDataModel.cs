using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators;

internal abstract class ClassDataModel(INamedTypeSymbol symbol) : BaseDataModel(symbol, symbol)
{
    public bool IsStatic { get; } = symbol.IsStatic;
}
