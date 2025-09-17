using System.Text.RegularExpressions;

namespace GodotSharp.SourceGenerators.AnimNamesExtensions;

internal static class AnimNamesScraper
{
    private const string AnimLibRegexStr = @"^&""(?<Name>.+)"":";
    private const string SpriteFramesRegexStr = @"^""name"": &""(?<Name>.+)""";
    private static readonly Regex AnimLibRegex = new(AnimLibRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
    private static readonly Regex SpriteFramesRegex = new(SpriteFramesRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    public static IEnumerable<string> GetAnimNames(string source, string csFile)
    {
        Log.Debug($"Scraping {source} [Compiling {csFile}]");

        return MatchAnimNames(source).Distinct();

        static IEnumerable<string> MatchAnimNames(string source)
        {
            var ext = Path.GetExtension(source);
            var tres = ext is ".tres";
            var tscn = ext is ".tscn";

            var found = false;
            foreach (var line in File.ReadLines(source).Where(line => line != string.Empty))
            {
                Log.Debug($"Line: {line}");

                if (tres && line is "[resource]" ||
                    tscn && line.StartsWith("[sub_resource "))
                {
                    found = true;
                    continue;
                }

                if (found)
                {
                    if (TryMatchName(line, out var name))
                        yield return name;

                    if (tscn && line.StartsWith("[node "))
                        yield break;
                }
            }

            static bool TryMatchName(string line, out string name)
            {
                var match = AnimLibRegex.Match(line);
                if (match.Success)
                {
                    Log.Debug($" - AnimLib {AnimLibRegex.GetGroupsAsStr(match)}");
                    name = match.Groups["Name"].Value;
                    return true;
                }

                match = SpriteFramesRegex.Match(line);
                if (match.Success)
                {
                    Log.Debug($" - SpriteFrames {SpriteFramesRegex.GetGroupsAsStr(match)}");
                    name = match.Groups["Name"].Value;
                    return true;
                }

                name = default;
                return false;
            }
        }
    }
}
