using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators;

internal abstract class MemberDataModel(ISymbol symbol) : BaseDataModel(symbol, symbol.ContainingType);
