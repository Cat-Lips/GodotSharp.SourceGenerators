using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators;

internal abstract class MemberDataModel : BaseDataModel
{
    protected MemberDataModel(ISymbol symbol)
        : base(symbol, symbol.ContainingType)
    {
    }
}
