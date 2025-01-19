using Microsoft.CodeAnalysis;
using LayerNameValue = (string MemberName, uint LayerValue);

namespace GodotSharp.SourceGenerators.LayerNamesExtensions;

internal class LayerNamesDataModel : ClassDataModel
{
    public ILookup<string, LayerNameValue> Layers { get; }

    public LayerNamesDataModel(INamedTypeSymbol symbol, string csPath, string gdRoot) : base(symbol)
    {
        Layers = LayerNamesScraper
            .GetLayerNames(csPath, gdRoot)
            .ToLookup(
                x => Capitalise(x.Category),
                x => (x.LayerName.ToSafeName(), x.LayerValue));

        static string Capitalise(string name)
        {
            return name[^1] is 'd'
                ? $"{char.ToUpper(name[0])}{name[1..^1]}D"
                : $"{char.ToUpper(name[0])}{name[1..]}";
        }
    }

    protected override string Str()
    {
        return string.Join("\n", LayerLookup());

        IEnumerable<string> LayerLookup()
        {
            foreach (var lookup in Layers)
            {
                foreach (var (name, value) in lookup)
                    yield return $"ClassName: {lookup.Key}, MemberName: {name}, LayerValue: {value}";
            }
        }
    }
}
