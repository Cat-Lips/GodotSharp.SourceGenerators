using Microsoft.CodeAnalysis;
using InputAction = (string GodotAction, string MemberName);
using NestedInputAction = (string ClassName, (string GodotAction, string MemberName) InputAction);

namespace GodotSharp.SourceGenerators.InputMapExtensions
{
    internal class InputMapDataModel : ClassDataModel
    {
        public IList<InputAction> Actions { get; }
        public ILookup<string, InputAction> NestedActions { get; }

        public InputMapDataModel(INamedTypeSymbol symbol, string csPath, string gdRoot) : base(symbol)
        {
            var actions = InputMapScraper
                .GetInputActions(csPath, gdRoot)
                .ToLookup(x => x.Contains('.'));

            Actions = actions[false].Select(InputAction).ToArray();
            NestedActions = actions[true].Select(NestedInputAction).ToLookup(x => x.ClassName, x => x.InputAction);

            static InputAction InputAction(string source)
                => (source, SafeName(source));

            static NestedInputAction NestedInputAction(string source)
            {
                var parts = source.Split(['.'], 2);
                var className = SafeName(parts.First());
                var memberName = SafeName(parts.Last().Replace(".", ""));
                return (className, (source, memberName));
            }

            static string SafeName(string source)
                => source.ToTitleCase().Replace(" ", "");
        }

        protected override string Str()
        {
            return string.Join("\n", Actions().Concat(NestedActions()));

            IEnumerable<string> Actions()
            {
                foreach (var (GodotAction, MemberName) in this.Actions)
                    yield return $"MemberName: {MemberName}, GodotAction: {GodotAction}";
            }

            IEnumerable<string> NestedActions()
            {
                foreach (var lookup in this.NestedActions)
                {
                    foreach (var (GodotAction, MemberName) in lookup)
                        yield return $"ClassName: {lookup.Key}, MemberName: {MemberName}, GodotAction: {GodotAction}";
                }
            }
        }
    }
}
