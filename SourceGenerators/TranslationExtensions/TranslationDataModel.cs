using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.TranslationExtensions;

internal class TranslationDataModel : ClassDataModel
{
    public record SafeNamePair(string RawName, string SafeName);
    public record KeyPlural(SafeNamePair Key, SafeNamePair Plural);

    public readonly bool HasPlurals;
    public readonly SafeNamePair[] Locs;
    public readonly ILookup<SafeNamePair, KeyPlural> Keys;
    public readonly Godot.TRAttribute Config;

    public TranslationDataModel(INamedTypeSymbol symbol, string source, Godot.TRAttribute cfg) : base(symbol)
    {
        var data = CSVScraper.ParseCSV(source, cfg.Sep, out HasPlurals, out var _);
        Locs = [.. data.Locs.Select(x => new SafeNamePair(x, x.ToPascalCase()))];
        Keys = data.Keys.ToLookup(
            x => x.Context is null ? null : new SafeNamePair(x.Context, x.Context?.ToPascalCase()),
            x => new KeyPlural(new SafeNamePair(x.Key, x.Key.Replace("%d", "").Replace("{0}", "").ToPascalCase()),
                x.Plural is null ? null : new SafeNamePair(x.Plural, x.Plural.Replace("%d", "").Replace("{0}", "").ToPascalCase())));
        Config = cfg;
    }

    protected override string Str()
    {
        return string.Join("\n", Locs().Concat(Keys()));

        IEnumerable<string> Locs()
        {
            yield return $"LOCS:";
            foreach (var loc in this.Locs)
                yield return $" - {loc}";
        }

        IEnumerable<string> Keys()
        {
            yield return "KEYS:";
            foreach (var key in this.Keys)
                yield return $" - {key}";
        }
    }
}
