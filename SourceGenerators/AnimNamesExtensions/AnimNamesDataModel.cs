using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.AnimNamesExtensions;

internal class AnimNamesDataModel(INamedTypeSymbol symbol, string source, string csPath) : ClassDataModel(symbol)
{
    public IDictionary<string, string> AnimNames { get; } = AnimNamesScraper
        .GetAnimNames(source ?? csPath.Get("tscn", "tres"), csPath).ToDictionary(x => x, x => x.ToSafeName());

    protected override string Str()
    {
        return string.Join("\n", AnimNames());

        IEnumerable<string> AnimNames()
            => this.AnimNames.Select(kvp => $"{kvp.Key} => {kvp.Value}");
    }
}
