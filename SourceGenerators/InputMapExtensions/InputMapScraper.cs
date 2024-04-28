using System.Text.RegularExpressions;

namespace GodotSharp.SourceGenerators.InputMapExtensions
{
    internal static class InputMapScraper
    {
        private const string InputRegexStr = @"^""?(?<Input>.+?)""?=.*$";
        private static readonly Regex InputRegex = new(InputRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        public static IEnumerable<string> GetInputActions(string csFile, string gdRoot)
        {
            var gdFile = GD.GetProjectFile(csFile, gdRoot);
            Log.Debug($"Scraping {gdFile} [Compiling {csFile}]");

            return MatchInputActions(gdFile);

            static IEnumerable<string> MatchInputActions(string gdFile)
            {
                var found = false;
                foreach (var line in File.ReadLines(gdFile).Where(line => line != string.Empty))
                {
                    Log.Debug($"Line: {line}");

                    if (line is "[input]")
                    {
                        found = true;
                        continue;
                    }

                    if (found)
                    {
                        if (TryMatchInput(line, out var action))
                            yield return action;
                        else if (line.StartsWith("["))
                            yield break;
                    }
                }

                static bool TryMatchInput(string line, out string action)
                {
                    var match = InputRegex.Match(line);
                    if (match.Success)
                    {
                        Log.Debug($" - Input {InputRegex.GetGroupsAsStr(match)}");
                        action = match.Groups["Input"].Value;
                        return true;
                    }

                    action = null;
                    return false;
                }
            }
        }
    }
}
