using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.AnimNamesExtensions;

internal class AnimNamesDataModel(INamedTypeSymbol symbol, string resPath, string csPath) : ClassDataModel(symbol)
{
    public IDictionary<string, string> AnimNames { get; } = AnimNamesScraper
        .GetAnimNames(resPath, csPath).ToDictionary(x => x, x => x.ToSafeName());

    protected override string Str()
    {
        return string.Join("\n", AnimNames());

        IEnumerable<string> AnimNames()
            => this.AnimNames.Select(kvp => $"{kvp.Key} => {kvp.Value}");
    }
}
