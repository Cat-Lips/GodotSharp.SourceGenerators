using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.InputMapExtensions
{
    internal class InputMapDataModel : ClassDataModel
    {
        public List<(string GdAction, string CsMember)> Actions { get; }

        public InputMapDataModel(INamedTypeSymbol symbol, string csPath) : base(symbol)
            => Actions = InputMapScraper.GetInputActions(csPath);

        public override string ToString()
            => string.Join("\n", Actions.Select(x => $"GD Action: {x.GdAction} => CS Member: {x.CsMember}"));
    }
}
