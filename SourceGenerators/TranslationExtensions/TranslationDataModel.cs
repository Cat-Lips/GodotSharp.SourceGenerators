using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.TranslationExtensions;

internal class TranslationDataModel : ClassDataModel
{
    public record SafeNamePair(string SafeName, string RawName);
    public record SafeNamePairWithArgCount(string SafeName, string RawName, int ArgCount);

    public readonly bool Xtras;
    public readonly SafeNamePair[] Locs;
    public readonly SafeNamePairWithArgCount[] Keys;

    public TranslationDataModel(INamedTypeSymbol symbol, string source, bool xtras) : base(symbol)
    {
        var data = CSVScraper.ParseCSV(source);
        Locs = [.. data.Locs.Select(x => new SafeNamePair(x.ToPascalCase(), x))];
        Keys = [.. data.Keys.Select(x => new SafeNamePairWithArgCount(x.Key.ToPascalCase(), x.Key, x.Args))];
        Xtras = xtras;
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
