using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.CancellationSourceExtensions;

internal class CancellationSourceDataModel(INamedTypeSymbol symbol) : ClassDataModel(symbol)
{
    protected override string Str()
    {
        return $"ClassName: {ClassName}";
    }
}
