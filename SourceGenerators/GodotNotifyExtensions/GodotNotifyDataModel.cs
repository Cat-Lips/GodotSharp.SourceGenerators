using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GodotSharp.SourceGenerators.GodotNotifyExtensions
{
    internal class GodotNotifyDataModel : MemberDataModel
    {
        public string Type { get; }
        public string Name { get; }
        public string Field { get; }
        public bool ClassIsResource { get; }
        public bool ValueIsResource { get; }
        public bool ValueIsResourceArray { get; }
        public bool IsPartial { get; }
        public bool ImplicitGetMethod { get; }
        public bool ImplicitSetMethod { get; }
        public string GetMethodAccess { get; }
        public string SetMethodAccess { get; }
        public string Modifiers { get; }

        public GodotNotifyDataModel(IPropertySymbol property, SyntaxNode node)
            : base(property)
        {
            Type = property.Type.ToString();
            Name = property.Name;
            Field = $"{char.ToLower(Name[0])}{Name[1..]}";
            ClassIsResource = IsResource(property.ContainingType);
            Modifiers = "";
            if (HasPartialModifier(node, out var modifiers))
            {
                IsPartial = true;
                Modifiers = modifiers;
                ImplicitGetMethod = property.GetMethod?.IsImplicitlyDeclared ?? false;
                ImplicitSetMethod = property.SetMethod?.IsImplicitlyDeclared ?? false;
                GetMethodAccess = AccessString(property.GetMethod);
                SetMethodAccess = AccessString(property.SetMethod);
            }
            if (property.Type is IArrayTypeSymbol arrayType)
                ValueIsResourceArray = IsResource(arrayType.ElementType);
            else
                ValueIsResource = IsResource(property.Type);

            static bool IsResource(ITypeSymbol type)
                => type.InheritsFrom("Resource");

            static bool HasPartialModifier(SyntaxNode node, out string modifiers)
            {
                if (node is not PropertyDeclarationSyntax dec)
                {
                    modifiers = "";
                    return false;
                }
                var ret = dec.Modifiers.Any(token => token.ValueText == "partial");
                modifiers = dec.Modifiers.ToString();
                return ret;
            }

            static string AccessString(IMethodSymbol symbol)
            {
                var value = SyntaxFacts.GetText(symbol?.DeclaredAccessibility ?? Accessibility.NotApplicable) ?? "";
                return value.Length > 0 ? value + " " : value;
            }
        }

        protected override string Str()
        {
            return $"MemberType: {Type}, MemberName: {Name}, FieldName: {Field}{Notes()}";

            string Notes()
            {
                return string.Join("", Notes());

                IEnumerable<string> Notes()
                {
                    if (ClassIsResource) yield return ", Parent Class is Resource";
                    if (ValueIsResource) yield return ", Value Type is Resource";
                    if (ValueIsResourceArray) yield return ", Value Type is Resource Array";
                    if (IsPartial) yield return ", IsPartial";
                    if (ImplicitGetMethod) yield return ", GetMethod is Implicit";
                    if (ImplicitSetMethod) yield return ", SetMethod is Implicit";
                    if (Modifiers.Length > 0) yield return $", Modifiers are \"{Modifiers}\"";
                }
            }
        }
    }
}
