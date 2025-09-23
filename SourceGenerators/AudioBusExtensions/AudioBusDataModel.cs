using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.AudioBusExtensions;

internal record BusInfo(int Id, string Name, string Label = null);

internal class AudioBusDataModel(INamedTypeSymbol symbol, string source) : ClassDataModel(symbol)
{
    public BusInfo[] BusInfo { get; } = [.. AudioBusScraper.GetBusInfo(source).Select(x => new BusInfo(x.Id, x.Name, x.Name.ToSafeName()))];

    protected override string Str() => string.Join<BusInfo>("\n", BusInfo);
}
