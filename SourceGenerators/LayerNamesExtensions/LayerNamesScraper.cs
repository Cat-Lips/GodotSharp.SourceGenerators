using System.Text.RegularExpressions;
using LayerInfo = (string Category, string LayerName, uint LayerValue);

namespace GodotSharp.SourceGenerators.LayerNamesExtensions;

internal static class LayerNamesScraper
{
    private const string LayerRegexStr = @"^(?<Category>\w+)/layer_(?<LayerValue>\d+)=""(?<LayerName>.+)""";
    private static readonly Regex LayerRegex = new(LayerRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    public static IEnumerable<LayerInfo> GetLayerNames(string csFile, string? gdRoot)
    {
        var gdFile = GD.GetProjectFile(csFile, gdRoot);
        Log.Debug($"Scraping {gdFile} [Compiling {csFile}]");

        return MatchLayerNames(gdFile);

        static IEnumerable<LayerInfo> MatchLayerNames(string gdFile)
        {
            var found = false;
            foreach (var line in File.ReadLines(gdFile).Where(line => line != string.Empty))
            {
                Log.Debug($"Line: {line}");

                if (line is "[layer_names]")
                {
                    found = true;
                    continue;
                }

                if (found)
                {
                    if (TryMatchLayer(line, out var layer))
                        yield return layer;
                    else if (line.StartsWith("["))
                        yield break;
                }
            }

            static bool TryMatchLayer(string line, out LayerInfo layer)
            {
                var match = LayerRegex.Match(line);
                if (match.Success)
                {
                    Log.Debug($" - Layer {LayerRegex.GetGroupsAsStr(match)}");
                    layer = (
                        ReverseParts(match.Groups["Category"].Value),
                        match.Groups["LayerName"].Value,
                        uint.Parse(match.Groups["LayerValue"].Value) - 1);
                    return true;

                    static string ReverseParts(string str, char sep = '_')
                        => string.Join("", str.Split(sep).Reverse());
                }

                layer = default;
                return false;
            }
        }
    }
}
