using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.AutoloadExtensions
{
    internal static class AutoloadScraper
    {
        private const string AutoloadRegexStr = @"^""?(?<Name>.+?)""?=""\*(?<Path>.+?)""$";
        private static readonly Regex AutoloadRegex = new(AutoloadRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        public static IEnumerable<AutoloadNode> GetAutoloads(Compilation compilation, string csFile, string gdRoot)
        {
            var gdFile = GD.GetProjectFile(csFile, gdRoot);
            Log.Debug($"Scraping {gdFile} [Compiling {csFile}]");

            return MatchAutoloads(compilation, gdFile);

            static IEnumerable<AutoloadNode> MatchAutoloads(Compilation compilation, string gdFile)
            {
                var matchingInput = false;
                foreach (var line in File.ReadLines(gdFile).Where(line => line != string.Empty))
                {
                    Log.Debug($"Line: {line}");

                    if (line is "[autoload]")
                    {
                        matchingInput = true;
                        continue;
                    }

                    if (matchingInput)
                    {
                        if (TryMatchInput(line, out var autoload))
                            yield return autoload;
                        else if (line.StartsWith("["))
                            yield break;
                    }
                }

                bool TryMatchInput(string line, out AutoloadNode autoload)
                {
                    var match = AutoloadRegex.Match(line);
                    if (match.Success)
                    {
                        Log.Debug($" - Autoload {AutoloadRegex.GetGroupsAsStr(match)}");
                        var name = match.Groups["Name"].Value;
                        var resource = match.Groups["Path"].Value;
                        var type = compilation.GetFullName(name, resource);
                        if (type != null)
                        {
                            autoload = new AutoloadNode(name, type);
                            return true;
                        }
                    }

                    autoload = null;
                    return false;
                }
            }
        }
    }
}
