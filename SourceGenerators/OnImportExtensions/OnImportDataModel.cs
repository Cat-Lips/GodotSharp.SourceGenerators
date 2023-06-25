using System.ComponentModel;
using System.Diagnostics;
using Godot;
using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.OnImportExtensions
{
    internal class OnImportDataModel : MemberDataModel
    {
        public string MethodName { get; }
        public string PassedArgs { get; }

        public OnImportAttribute Config { get; }
        public ImportOption[] Options { get; }

        public bool HasStringValue { get; private set; }

        public OnImportDataModel(IMethodSymbol method, OnImportAttribute config)
            : base(method)
        {
            var memberNames = new HashSet<string>(method.ContainingType.MemberNames);

            MethodName = method.Name;
            PassedArgs = string.Join(", ", method.Parameters.Select(x => $"{x.Name}"));

            Config = config;
            Options = method.Parameters
                .Where(x => x.HasExplicitDefaultValue)
                .Select(CreateImportOption)
                .ToArray();

            ImportOption CreateImportOption(IParameterSymbol x)
            {
                GetDefaultValue(out var defaultValue);
                HasStringValue |= defaultValue is string or null;
                GetHintData(out var propertyHint, out var hintString);
                return new(x.Name, x.Name.ToTitleCase(), defaultValue, $"{x.Type}", propertyHint, hintString);

                void GetDefaultValue(out object value)
                {
                    if (!TryGetDefaultValueFromAttribute(out value))
                        value = x.ExplicitDefaultValue;
                    value = DecorateDefaultValue(value);
                    Debug.Assert(value is not null);

                    bool TryGetDefaultValueFromAttribute(out object value)
                    {
                        var attribute = x.GetAttributes().SingleOrDefault(x => x.AttributeClass.Name == nameof(DefaultValueAttribute));
                        value = attribute?.ConstructorArguments[0].Value;
                        return attribute is not null;
                    }

                    object DecorateDefaultValue(object value)
                        => value is string str && !memberNames.Contains(str) ? $@"""{str}""" : value;
                }

                void GetHintData(out int? propertyHint, out string hintString)
                {
                    var attribute = x.GetAttributes().SingleOrDefault(x => x.AttributeClass.Name == nameof(HintAttribute));
                    propertyHint = (int?)(attribute?.ConstructorArguments[0].Value);
                    hintString = (string)(attribute?.ConstructorArguments[1].Value);
                }
            }
        }

        protected override string Str()
        {
            return string.Join("\n", Parts());

            IEnumerable<string> Parts()
            {
                yield return $" - Calling Declaration: {MethodName}({PassedArgs})";
                if (Options.Length is 1) yield return $" - Options: {Options.Single()}";
                else if (Options.Length is > 1) yield return $" - Options:\n   - {string.Join<ImportOption>("\n   - ", Options)}";
            }
        }

        public record ImportOption(string Name, string DisplayName, object DefaultValue, string Type, int? PropertyHint = default, string HintString = default);
    }
}
