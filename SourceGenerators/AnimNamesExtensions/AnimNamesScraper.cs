using System.Text.RegularExpressions;

namespace GodotSharp.SourceGenerators.AnimNamesExtensions;

internal static class AnimNamesScraper
{
    private const string AnimLibRegexStr = @"^&""(?<Name>.+)"":";
    private const string SpriteFramesRegexStr = @"^""name"": &""(?<Name>.+)""";
    private static readonly Regex AnimLibRegex = new(AnimLibRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
    private static readonly Regex SpriteFramesRegex = new(SpriteFramesRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    public static IEnumerable<string> GetAnimNames(string resPath, string csFile)
    {
        Log.Debug($"Scraping {resPath} [Compiling {csFile}]");

        return MatchAnimNames(resPath);

        static IEnumerable<string> MatchAnimNames(string resPath)
        {
            var found = false;
            foreach (var line in File.ReadLines(resPath).Where(line => line != string.Empty))
            {
                Log.Debug($"Line: {line}");

                if (line is "[resource]")
                {
                    found = true;
                    continue;
                }

                if (found)
                {
                    if (TryMatchName(line, out var name))
                        yield return name;
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
