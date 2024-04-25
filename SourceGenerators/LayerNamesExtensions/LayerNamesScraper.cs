using System.Text.RegularExpressions;
using LayerNameData = (string Category, uint LayerValue, string MemberName);

namespace GodotSharp.SourceGenerators.LayerNamesExtensions
{
    internal static class LayerNamesScraper
    {
        private const string LayerNameRegexStr = @"^(?<Category>\w+)/layer_(?<Value>\d+)=""(?<Name>\w+)""";
        private static readonly Regex LayerNameRegex = new(LayerNameRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        private const string LayerCategoryRegexStr = @"^(?<Number>\d+\w*)_(?<Name>\w+)";
        private static readonly Regex LayerCategoryRegex = new(LayerCategoryRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        public static IEnumerable<LayerNameData> GetLayerNames(string csFile, string gdRoot)
        {
            var gdFile = GD.GetProjectFile(csFile, gdRoot);
            Log.Debug($"Scraping {gdFile} [Compiling {csFile}]");

            return MatchLayerNames(gdFile);

            static IEnumerable<LayerNameData> MatchLayerNames(string gdFile)
            {
                var matchingLayerName = false;
                foreach (var line in File.ReadLines(gdFile).Where(line => line != string.Empty))
                {
                    Log.Debug($"Line: {line}");

                    if (line is "[layer_names]")
                    {
                        matchingLayerName = true;
                        continue;
                    }

                    if (matchingLayerName)
                    {
                        if (TryMatchLayerName(line, out var layerNameInfo))
                            yield return layerNameInfo;
                        else if (line.StartsWith("["))
                            yield break;
                    }
                }

                static bool TryMatchLayerName(string line, out LayerNameData layerNameInfo)
                {
                    var match = LayerNameRegex.Match(line);
                    if (match.Success)
                    {
                        Log.Debug($" - LayerName {LayerNameRegex.GetGroupsAsStr(match)}");
                        layerNameInfo = new LayerNameData
                        {
                            Category = match.Groups["Category"].Value,
                            LayerValue = uint.Parse(match.Groups["Value"].Value) - 1u, // Layer value starts from 0 but the text starts from 1.
                            MemberName = match.Groups["Name"].Value
                        };
                        match = LayerCategoryRegex.Match(layerNameInfo.Category);
                        if (match.Success)
                        {
                             layerNameInfo.Category = $"{match.Groups["Name"].Value}_{match.Groups["Number"].Value}";
                        }
                        return true;
                    }

                    layerNameInfo = default;
                    return false;
                }
            }
        }
    }
}
