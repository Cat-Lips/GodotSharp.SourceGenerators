﻿using System.ComponentModel;
using Godot;
using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.OnImportExtensions
{
    internal class OnImportDataModel : MemberDataModel
    {
        public string MethodName { get; }
        public string PassedArgs { get; }

        public string ImporterName { get; }
        public string DisplayName { get; }

        public OnImportAttribute Config { get; }
        public ImportOption[] Options { get; }

        public OnImportDataModel(IMethodSymbol method, OnImportAttribute config)
            : base(method)
        {
            var memberNames = new HashSet<string>(method.ContainingType.MemberNames);

            MethodName = method.Name;
            PassedArgs = string.Join(", ", method.Parameters.Select(x => $"{x.Name}"));

            ImporterName = $"{method.ContainingNamespace}.{ClassName}";
            DisplayName = config.DisplayName ?? ClassName.ToTitleCase();

            Config = config;
            Options = method.Parameters
                .Where(x => x.HasExplicitDefaultValue)
                .Select(CreateImportOption)
                .ToArray();

            ImportOption CreateImportOption(IParameterSymbol x)
            {
                GetDefaultValue(out var defaultValue);
                GetHintData(out var propertyHint, out var hintString);
                return new(x.Name, x.Name.ToTitleCase(), defaultValue, $"{x.Type}", UnderlyingEnumType(), propertyHint, hintString);

                void GetDefaultValue(out object value)
                {
                    if (!TryGetDefaultValueFromAttribute(out value))
                        value = x.ExplicitDefaultValue;
                    value = DecorateDefaultValue(value);

                    bool TryGetDefaultValueFromAttribute(out object value)
                    {
                        var attribute = x.GetAttributes().SingleOrDefault(x => x.AttributeClass.Name == nameof(DefaultValueAttribute));
                        value = attribute?.ConstructorArguments[0].Value;
                        return attribute is not null;
                    }

                    object DecorateDefaultValue(object value)
                        => value is string str && !memberNames.Contains(str) ? $@"""{str}""" : value;
                }

                void GetHintData(out long? propertyHint, out string hintString)
                {
                    var attribute = x.GetAttributes().SingleOrDefault(x => x.AttributeClass.Name == nameof(Resources.HintAttribute));
                    propertyHint = (long?)(attribute?.ConstructorArguments[0].Value);
                    hintString = (string)(attribute?.ConstructorArguments[1].Value);
                }

                string UnderlyingEnumType()
                {
                    var t = (x.Type as INamedTypeSymbol)?.EnumUnderlyingType;
                    return t is null ? null : $"{t}";
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

        public record ImportOption(string Name, string DisplayName, object DefaultValue, string ValueType, string UnderlyingEnumType, long? PropertyHint = default, string HintString = default);
    }
}
