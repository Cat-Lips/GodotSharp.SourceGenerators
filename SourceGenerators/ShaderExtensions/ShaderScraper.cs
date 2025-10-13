using System.Text.RegularExpressions;

namespace GodotSharp.SourceGenerators.ShaderExtensions;

internal static class ShaderScraper
{
    #region Regex

    private static class X
    {
        public const string Type = @"(?<type>\w+)";
        public const string Name = @"(?<name>\w+)";
        public const string Hint = @"(:\s*(?<hint>[^=;]+))?";
        public const string Dflt = @"(=\s*(?<dflt>[^;]+))?";
        public const string Cmnt = @"(//\s*(?<cmnt>.*))?";
    }

    private const string UniformRegexStr = @$"^uniform\s+{X.Type}\s+{X.Name}\s*{X.Hint}\s*{X.Dflt};\s*{X.Cmnt}$";
    private static readonly Regex UniformRegex = new(UniformRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    private const string FuncRegexStr = @$"^{X.Type}\s+{X.Name}\s*\(";
    private static readonly Regex FuncRegex = new(FuncRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    private const string ArgRegexStr = @$"^{X.Type}\((?<args>.*)\)";
    private static readonly Regex ArgRegex = new(ArgRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    #endregion

    internal record ShaderUniform(string Type, string Name, string Hint, string Default, string Args, string Comment);

    public static IEnumerable<ShaderUniform> GetUniforms(string shader)
    {
        Log.Debug($"Scraping {shader} [{UniformRegexStr}]");

        foreach (var line in File.ReadLines(shader))
        {
            Log.Debug($"Line: {line}");

            var match = UniformRegex.Match(line);
            if (match.Success)
            {
                Log.Debug($" - Uniform {UniformRegex.GetGroupsAsStr(match)}");
                var type = match.Groups["type"].Value;
                var name = match.Groups["name"].Value;
                var hint = match.Groups["hint"].Value.Trim().NullIfEmpty();
                var dflt = match.Groups["dflt"].Value.Trim().NullIfEmpty();
                var cmnt = match.Groups["cmnt"].Value.Trim().NullIfEmpty();
                var args = dflt is null ? null : ArgRegex.Match(dflt).Groups["args"].Value.Trim().NullIfEmpty();
                yield return new(type, name, hint, dflt, args, cmnt);
            }
            else if (FuncRegex.IsMatch(line))
            {
                Log.Debug(" - Function detected, stopping scrape");
                yield break;
            }
        }
    }
}
