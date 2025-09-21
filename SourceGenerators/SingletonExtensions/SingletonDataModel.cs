using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.SingletonExtensions;

internal class SingletonDataModel(INamedTypeSymbol symbol, string init, string tscn) : ClassDataModel(symbol)
{
    public string TSCN { get; } = tscn;
    public string Init { get; } = symbol.MemberNames.Contains(init) ? init : null;

    protected override string Str()
    {
        return string.Join("\n", Parts());

        IEnumerable<string> Parts()
        {
            yield return $"ClassName: {ClassName}";
            if (OuterType is not null) yield return $"OuterType: {OuterType}";
            if (TSCN is not null) yield return $"TSCN: {TSCN}";
            if (Init is not null) yield return $"Init: {Init}";
        }
    }
}
