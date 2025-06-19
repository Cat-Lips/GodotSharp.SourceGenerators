using Microsoft.CodeAnalysis;
using InputAction = (string MemberName, string GodotAction);
using NestedInputAction = (string ClassName, (string MemberName, string GodotAction) InputAction);

namespace GodotSharp.SourceGenerators.InputMapExtensions;

internal class InputMapDataModel : ClassDataModel
{
    public string Type { get; }
    public IList<InputAction> Actions { get; }
    public ILookup<string, InputAction> NestedActions { get; }

    public InputMapDataModel(INamedTypeSymbol symbol, string type, string csPath, string gdRoot) : base(symbol)
    {
        var actions = InputMapScraper
            .GetInputActions(csPath, gdRoot)
            .ToLookup(IsNestedAction);

        Type = type;
        Actions = actions[false].Select(InputAction).ToArray();
        NestedActions = actions[true].Select(NestedInputAction).ToLookup(x => x.ClassName, x => x.InputAction);

        static bool IsNestedAction(string source)
            => source.Contains('.');

        static InputAction InputAction(string source)
            => (source.ToSafeName(), source);

        static NestedInputAction NestedInputAction(string source)
        {
            var parts = source.Split(['.'], 2);
            var className = parts.First().ToSafeName();
            var memberName = parts.Last().Replace(".", "").ToSafeName();
            return (className, (memberName, source));
        }
    }

    protected override string Str()
    {
        return string.Join("\n", Type().Concat(Actions().Concat(NestedActions())));

        IEnumerable<string> Type()
        {
            yield return $"Type: {this.Type}";
        }

        IEnumerable<string> Actions()
        {
            foreach (var (name, action) in this.Actions)
                yield return $"MemberName: {name}, GodotAction: {action}";
        }

        IEnumerable<string> NestedActions()
        {
            foreach (var lookup in this.NestedActions)
            {
                foreach (var (name, action) in lookup)
                    yield return $"ClassName: {lookup.Key}, MemberName: {name}, GodotAction: {action}";
            }
        }
    }
}
