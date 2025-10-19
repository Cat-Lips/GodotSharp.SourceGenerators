using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.OnInstantiateExtensions;

internal class OnInstantiateDataModel(IMethodSymbol method, string ctor, string tscn) : MemberDataModel(method)
{
    public string ResourcePath { get; } = tscn;
    public string ConstructorScope { get; } = ctor;
    public string MethodName { get; } = method.Name;
    public string Params { get; } = string.Join(", ", method.Parameters.Select(p => p.ToParameterString()));
    public string Args { get; } = string.Join(", ", method.Parameters.Select(p => p.ToArgumentString()));

    protected override string Str()
    {
        return string.Join("\n", Parts());

        IEnumerable<string> Parts()
        {
            yield return $" - Resource Path: {ResourcePath}";
            yield return $" - Constructor Scope: {ConstructorScope}";
            yield return $" - Method Signature: {MethodName}({Params})";
            yield return $" - Calling Declaration: {MethodName}({Args})";
        }
    }
}
