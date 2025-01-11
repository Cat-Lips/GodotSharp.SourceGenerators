using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators;

internal abstract class ClassDataModel : BaseDataModel
{
    protected ClassDataModel(INamedTypeSymbol symbol)
        : base(symbol, symbol)
    {
    }
}
