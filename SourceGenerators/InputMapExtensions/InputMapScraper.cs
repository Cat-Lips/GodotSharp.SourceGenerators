using System.Text.RegularExpressions;

namespace GodotSharp.SourceGenerators.InputMapExtensions
{
    internal static class InputMapScraper
    {
        private const string InputRegexStr = @"^""?(?<Input>.+?)""?=.*$";
        private static readonly Regex InputRegex = new(InputRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        public static List<(string GdAction, string CsMember)> GetInputActions(string csFile)
        {
            Log.Debug();
            var gdFile = GD.GetProjectFile(csFile);
            Log.Debug($"Scraping {gdFile} [Compiling {csFile}]");

            return MatchInputActions(gdFile).ToList();

            static IEnumerable<(string GdAction, string CsMember)> MatchInputActions(string gdFile)
            {
                var matchingInput = false;
                foreach (var line in File.ReadLines(gdFile).Where(line => line != string.Empty))
                {
                    Log.Debug($"Line: {line}");

                    if (line is "[input]")
                    {
                        matchingInput = true;
                        continue;
                    }

                    if (matchingInput)
                    {
                        if (TryMatchInput(line, out var gdAction, out var csMember))
                            yield return (gdAction, csMember);
                        else if (line.StartsWith("["))
                            yield break;
                    }
                }

                static bool TryMatchInput(string line, out string gdAction, out string csMember)
                {
                    var match = InputRegex.Match(line);
                    if (match.Success)
                    {
                        Log.Debug($" - Input {InputRegex.GetGroupsAsStr(match)}");
                        gdAction = match.Groups["Input"].Value;
                        csMember = gdAction.ToTitleCase().Replace(" ", "");
                        return true;
                    }

                    gdAction = null;
                    csMember = null;
                    return false;
                }
            }
        }
    }
}
