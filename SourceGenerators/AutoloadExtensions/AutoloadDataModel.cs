using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.AutoloadExtensions;

internal record AutoloadData(string Type, string GodotName, string DisplayName);

internal class AutoloadDataModel(Compilation compilation, INamedTypeSymbol symbol, string csPath, string gdRoot, IDictionary<string, string> lookup) : ClassDataModel(symbol)
{
    public IList<AutoloadData> Autoloads { get; } = [..
        AutoloadScraper.GetAutoloads(compilation, csPath, gdRoot)
            .Select(autoload => new AutoloadData(autoload.Type, autoload.Name, lookup.Get(autoload.Name) ?? autoload.Name))];

    protected override string Str()
        => string.Join("\n", Autoloads);
}
