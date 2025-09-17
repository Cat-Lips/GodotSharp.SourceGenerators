using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.GlobalGroupsExtensions;

internal class GlobalGroupsDataModel(INamedTypeSymbol symbol, string csPath, string gdRoot) : ClassDataModel(symbol)
{
    public IDictionary<string, string> GroupNames { get; } = GlobalGroupsScraper
        .GetGlobalGroups(csPath, gdRoot).ToDictionary(x => x, x => x.ToSafeName());

    protected override string Str()
    {
        return string.Join("\n", GroupNames());

        IEnumerable<string> GroupNames()
            => this.GroupNames.Select(kvp => $"GD Name '{kvp.Key}' => CS Name '{kvp.Value}'");
    }
}
