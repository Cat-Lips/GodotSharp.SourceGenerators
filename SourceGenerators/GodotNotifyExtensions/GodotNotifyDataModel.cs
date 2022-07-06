using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.GodotNotifyExtensions
{
    internal class GodotNotifyDataModel
    {
        public string NSOpen { get; }
        public string NSClose { get; }
        public string NSIndent { get; }
        public string ClassName { get; }

        public string Type { get; }
        public string Name { get; }
        public string Field { get; }
        public string SetScope { get; }

        public GodotNotifyDataModel(IFieldSymbol field, string setter)
        {
            ClassName = field.ContainingType.ClassDef();
            (NSOpen, NSClose, NSIndent) = field.GetNamespaceDeclaration();

            Type = $"{field.Type}";
            Name = GetPropertyName();
            Field = field.Name;
            SetScope = setter is "public" ? null : $"{setter} ";

            string GetPropertyName()
            {
                var name = field.Name.Trim('_');
                name = $"{char.ToUpper(name[0])}{name[1..]}";
                return name;
            }
        }
    }
}
