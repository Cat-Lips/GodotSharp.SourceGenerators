using System.Text.RegularExpressions;

namespace GodotSharp.SourceGenerators.AudioBusExtensions;

internal static class AudioBusScraper
{
    private const string AudioBusRegexStr = @"^bus/(?<Id>\d+)/name = &""(?<Name>.+)""";
    private static readonly Regex AudioBusRegex = new(AudioBusRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    public static IEnumerable<BusInfo> GetBusInfo(string source)
    {
        Log.Debug($"Scraping {source}");
        return MatchAudioBus(source).Prepend(new(0, "Master"));

        static IEnumerable<BusInfo> MatchAudioBus(string source)
        {
            var found = false;
            foreach (var line in File.ReadLines(source))
            {
                Log.Debug($"Line: {line}");

                if (line is "")
                    continue;

                if (line is "[resource]")
                {
                    found = true;
                    continue;
                }

                if (found)
                {
                    if (TryMatchAudioBus(line, out var id, out var name))
                        yield return new(id, name);
                }
            }

            static bool TryMatchAudioBus(string line, out int id, out string name)
            {
                var match = AudioBusRegex.Match(line);
                if (match.Success)
                {
                    Log.Debug($" - AudioBus {AudioBusRegex.GetGroupsAsStr(match)}");
                    id = int.Parse(match.Groups["Id"].Value);
                    name = match.Groups["Name"].Value;
                    return true;
                }

                id = default;
                name = default;
                return false;
            }
        }
    }
}
