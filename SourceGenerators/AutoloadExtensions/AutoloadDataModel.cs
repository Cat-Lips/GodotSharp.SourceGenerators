using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.AutoloadExtensions;

using NameMap = (string DisplayName, string GodotName);

internal class AutoloadDataModel : ClassDataModel
{
    public record AutoloadData(string Type, string GodotName, string DisplayName);

    public IList<AutoloadData> Autoloads { get; }

    public AutoloadDataModel(Compilation compilation, INamedTypeSymbol symbol, string csPath, string gdRoot, IEnumerable<NameMap> names)
        : base(symbol)
    {
        var lookup = names.ToDictionary(x => x.GodotName, x => x.DisplayName);
        Autoloads = AutoloadScraper.GetAutoloads(compilation, csPath, gdRoot)
            .Select(autoload => new AutoloadData(autoload.Type, autoload.Name, lookup.Get(autoload.Name) ?? autoload.Name))
            .ToArray();
    }

    protected override string Str()
        => string.Join("\n", Autoloads);
}
