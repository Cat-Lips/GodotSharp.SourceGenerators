using System.Text.RegularExpressions;

namespace GodotSharp.SourceGenerators.GlobalGroupsExtensions;

internal static class GlobalGroupsScraper
{
    private const string GlobalGroupRegexStr = @"^(?<GroupName>\w+)=";
    private static readonly Regex GlobalGroupRegex = new(GlobalGroupRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    public static IEnumerable<string> GetGlobalGroups(string csFile, string gdRoot)
    {
        var gdFile = GD.GetProjectFile(csFile, gdRoot);
        Log.Debug($"Scraping {gdFile} [Compiling {csFile}]");

        return MatchGlobalGroups(gdFile);

        static IEnumerable<string> MatchGlobalGroups(string gdFile)
        {
            var found = false;
            foreach (var line in File.ReadLines(gdFile))
            {
                Log.Debug($"Line: {line}");

                if (line is "")
                    continue;

                if (line is "[global_group]")
                {
                    found = true;
                    continue;
                }

                if (found)
                {
                    if (TryMatchGroupName(line, out var name))
                        yield return name;
                    else if (line.StartsWith("["))
                        yield break;
                }
            }

            static bool TryMatchGroupName(string line, out string name)
            {
                var match = GlobalGroupRegex.Match(line);
                if (match.Success)
                {
                    Log.Debug($" - GlobalGroup {GlobalGroupRegex.GetGroupsAsStr(match)}");
                    name = match.Groups["GroupName"].Value;
                    return true;
                }

                name = default;
                return false;
            }
        }
    }
}
