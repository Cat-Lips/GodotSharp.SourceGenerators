using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.RpcExtensions;

internal class RpcDataModel(IMethodSymbol symbol) : MemberDataModel(symbol)
{
    public string Scope { get; } = symbol.Scope();
    public string MethodName { get; } = symbol.Name;
    public bool HasParams { get; } = symbol.Parameters.Length > 0;
    public string Params { get; } = string.Join(", ", symbol.Parameters.Select(p => p.ToParameterString()));
    public string Args { get; } = string.Join(", ", symbol.Parameters.Select(p => p.ToArgumentString(castEnum: true)));

    protected override string Str()
    {
        return string.Join("\n", Parts());

        IEnumerable<string> Parts()
        {
            yield return $"{nameof(HasParams)}: {HasParams}";
            yield return $"{nameof(MethodName)}: {MethodName}";
            yield return $"{nameof(Params)}: {Params}";
            yield return $"{nameof(Args)}: {Args}";
        }
    }
}
