using System.Text.RegularExpressions;

namespace GodotSharp.SourceGenerators.AutoloadExtensions
{
    internal static class AutoloadScraper
    {
        private const string AutoloadRegexStr = @"^(?<Name>.+?)=""*(?<Path>.+?)""$";
        private static readonly Regex AutoloadRegex = new(AutoloadRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        public static IEnumerable<(string Name, string Path)> GetAutoloads(string csFile, string gdRoot)
        {
            var gdFile = GD.GetProjectFile(csFile, gdRoot);
            Log.Debug($"Scraping {gdFile} [Compiling {csFile}]");

            return MatchAutoloads(gdFile);

            static IEnumerable<(string Name, string Path)> MatchAutoloads(string gdFile)
            {
                var found = false;
                foreach (var line in File.ReadLines(gdFile).Where(line => line != string.Empty))
                {
                    Log.Debug($"Line: {line}");

                    if (line is "[autoload]")
                    {
                        found = true;
                        continue;
                    }

                    if (found)
                    {
                        if (TryMatchAutoload(line, out var name, out var path))
                            yield return (name, path);
                        else if (line.StartsWith("["))
                            yield break;
                    }
                }

                static bool TryMatchAutoload(string line, out string name, out string path)
                {
                    var match = AutoloadRegex.Match(line);
                    if (match.Success)
                    {
                        Log.Debug($" - Autoload {AutoloadRegex.GetGroupsAsStr(match)}");
                        name = match.Groups["Name"].Value;
                        path = match.Groups["Path"].Value;
                        return true;
                    }

                    name = null;
                    path = null;
                    return false;
                }
            }
        }
    }
}
