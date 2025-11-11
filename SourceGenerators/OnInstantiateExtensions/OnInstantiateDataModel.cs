using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.OnInstantiateExtensions;

internal class OnInstantiateDataModel(IMethodSymbol method, bool main, string ctor, string tscn) : MemberDataModel(method)
{
    public bool LoadScene { get; } = main;
    public string ResourcePath { get; } = tscn;
    public string ConstructorScope { get; } = main ? ctor : null;
    public string MethodName { get; } = method.Name;
    public string Params { get; } = string.Join(", ", method.Parameters.Select(p => p.ToParameterString()));
    public string Args { get; } = string.Join(", ", method.Parameters.Select(p => p.ToArgumentString()));

    protected override string Str()
    {
        return string.Join("\n", Parts());

        IEnumerable<string> Parts()
        {
            yield return $" - LoadScene: {LoadScene}";
            yield return $" - ResourcePath: {ResourcePath}";
            yield return $" - ConstructorScope: {ConstructorScope}";
            yield return $" - Method Signature: {MethodName}({Params})";
            yield return $" - Calling Declaration: {MethodName}({Args})";
        }
    }
}
