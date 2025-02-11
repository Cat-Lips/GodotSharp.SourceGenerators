using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Godot;
using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.OnImportExtensions;

internal class OnImportDataModel : MemberDataModel
{
    private const string Has = "Has";           // eg, HasOptionX (with arg subset of _GetImportOptions)
    private const string Show = "Show";         // eg, ShowOptionX (with arg subset of _GetOptionVisibility)
    private const string Default = "Default";   // eg, DefaultOptionX (with arg subset of _GetImportOptions)
    private static readonly HashSet<string> ImportOptionsArgs = new(new[] { "string path", "int preset" });
    private static readonly HashSet<string> OptionVisibilityArgs = new(new[] { "string path", "Godot.Collections.Dictionary options" });

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
        PassedArgs = string.Join(", ", method.Parameters.Select(x => x.Name));

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
            GetParams(out var hasParams, out var showParams, out var defaultParams);
            return new(x.Name, x.Name.ToTitleCase(), defaultValue, $"{x.Type}", UnderlyingEnumType(), propertyHint, hintString, hasParams, showParams, defaultParams);

            void GetDefaultValue(out object? value)
            {
                if (!TryGetDefaultValueFromAttribute(out value))
                    value = x.ExplicitDefaultValue;
                value = DecorateDefaultValue(value);

                bool TryGetDefaultValueFromAttribute([NotNullWhen(true)] out object? value)
                {
                    var attribute = x.GetAttributes().SingleOrDefault(x => x.AttributeClass?.Name == nameof(DefaultValueAttribute));
                    value = attribute?.ConstructorArguments[0].Value;
                    return attribute is not null;
                }

                object? DecorateDefaultValue(object? value)
                    => value is string str && !memberNames.Contains(str) ? $@"""{str}""" : value;
            }

            void GetParams(out string? hasParams, out string? showParams, out string? defaultParams)
            {
                var baseName = $"{char.ToUpper(x.Name[0])}{x.Name[1..]}";
                hasParams = GetParams(Has + baseName, ImportOptionsArgs);
                showParams = GetParams(Show + baseName, OptionVisibilityArgs);
                defaultParams = GetParams(Default + baseName, ImportOptionsArgs);

                string? GetParams(string methodName, HashSet<string> callerArgs)
                {
                    if (!memberNames.Contains(methodName)) return null;

                    var matchingMembers = method.ContainingType.GetMembers(methodName);
                    return matchingMembers.Length != 1 ? null
                        : matchingMembers.Single() is not IMethodSymbol matchingMethod ? null
                        : !callerArgs.IsSupersetOf(matchingMethod.Parameters.Select(x => $"{x.Type} {x.Name}")) ? null
                        : string.Join(", ", matchingMethod.Parameters.Select(x => x.Name));
                }
            }

            void GetHintData(out long? propertyHint, out string hintString)
            {
                var attribute = x.GetAttributes().SingleOrDefault(a => a.AttributeClass?.Name == nameof(Resources.HintAttribute));
                propertyHint = (long?)attribute?.ConstructorArguments[0].Value;
                hintString = (string)attribute?.ConstructorArguments[1].Value!;
            }

            string? UnderlyingEnumType()
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

    public record ImportOption(
        string Name, string DisplayName,
        object? DefaultValue, string ValueType, string? UnderlyingEnumType,
        long? PropertyHint, string HintString,
        string? HasParams, string? ShowParams, string? DefaultParams);
}
