using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.GodotOverrideExtensions;

internal class GodotOverrideDataModel : MemberDataModel
{
    public bool Partial { get; }
    public bool Replace { get; }
    public string ReturnType { get; }
    public string MethodName { get; }
    public string MethodArgs { get; }
    public string PassedArgs { get; }

    public GodotOverrideDataModel(IMethodSymbol method, bool replace)
        : base(method)
    {
        ReturnType = $"{method.ReturnType}";
        MethodName = Regex.Replace(method.Name, "^(On|_)", "", RegexOptions.Compiled);

        Partial = method.IsPartialDefinition;
        Replace = replace || ReturnType is not "void";

        MethodArgs = string.Join(", ", method.Parameters.Select(x => $"{x.Type} {x.Name}"));
        PassedArgs = string.Join(", ", method.Parameters.Select(x => $"{x.Name}"));
    }

    protected override string Str()
    {
        return string.Join("\n", Parts());

        IEnumerable<string> Parts()
        {
            yield return $" - Method Signature: {ReturnType} {MethodName}({MethodArgs})";
            yield return $" - Calling Declaration: {MethodName}({PassedArgs})";
        }
    }
}
