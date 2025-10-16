using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GodotSharp.SourceGenerators.GodotNotifyExtensions;

internal class GodotNotifyDataModel : MemberDataModel
{
    public string Type { get; }
    public string Name { get; }
    public string Field { get; }

    public string Modifiers { get; }
    public string GetAccess { get; }
    public string SetAccess { get; }

    public bool ClassIsResource { get; }
    public bool ValueIsResource { get; }
    public bool ValueIsResourceArray { get; }

    public GodotNotifyDataModel(IPropertySymbol symbol, SyntaxNode node)
        : base(symbol)
    {
        Type = symbol.Type.ToString();
        Name = symbol.Name;
        Field = $"{char.ToLower(Name[0])}{Name[1..]}";
        Modifiers = $"{((PropertyDeclarationSyntax)node).Modifiers}";
        var @default = symbol.GetDeclaredAccessibility();
        GetAccess = GetAccessibility(symbol.GetMethod, @default);
        SetAccess = GetAccessibility(symbol.SetMethod, @default);

        ClassIsResource = IsResource(symbol.ContainingType);
        if (symbol.Type is IArrayTypeSymbol arrayType)
            ValueIsResourceArray = IsResource(arrayType.ElementType);
        else ValueIsResource = IsResource(symbol.Type);

        static bool IsResource(ITypeSymbol type)
            => type.IsOrInherits("Resource");

        static string GetAccessibility(IMethodSymbol accessor, string @default)
        {
            var accessibility = accessor?.GetDeclaredAccessibility();
            return accessibility is null ? null
                : accessibility == @default ? ""
                : $"{accessibility} ";
        }
    }

    protected override string Str()
    {
        return string.Join(", ", Parts());

        IEnumerable<string> Parts()
        {
            yield return $"MemberType: {Type}";
            yield return $"MemberName: {Name}";
            yield return $"FieldName: {Field}";
            yield return $"Modifiers: {Modifiers} ({string.Join("/", GetSet())})";
            if (ClassIsResource) yield return $"ClassIsResource: {ClassIsResource}";
            if (ValueIsResource) yield return $"ValueIsResource: {ValueIsResource}";
            if (ValueIsResourceArray) yield return $"ValueIsResourceArray: {ValueIsResourceArray}";

            IEnumerable<string> GetSet()
            {
                if (GetAccess is not null) yield return $"{GetAccess}get";
                if (SetAccess is not null) yield return $"{SetAccess}set";
            }
        }
    }
}
