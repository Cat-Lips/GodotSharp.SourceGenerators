using Microsoft.CodeAnalysis;

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

        public GodotNotifyDataModel(IPropertySymbol property)
            : base(property)
        {
            Type = property.Type.ToString();
            Name = property.Name;
            Field = $"{char.ToLower(Name[0])}{Name[1..]}";
            ClassIsResource = IsResource(property.ContainingType);
            if (property.Type is IArrayTypeSymbol arrayType)
                ValueIsResourceArray = IsResource(arrayType.ElementType);
            else
                ValueIsResource = IsResource(property.Type);

            static bool IsResource(ITypeSymbol type)
                => type.InheritsFrom("Resource");
        }

        public override string ToString()
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
                }
            }
        }
    }
}
