using System.Text.RegularExpressions;

namespace GodotSharp.SourceGenerators.ShaderGlobalsExtensions;

internal static class ShaderGlobalsScraper
{
    private const string NameRegexStr = @"^""?(?<Name>.+?)""?={$";
    private const string TypeRegexStr = @"^""type"": ""(?<Type>.+?)"",$";
    private const string DefaultRegexStr = @"^""value"": ""?(?<Default>.*?)""?$";

    private static readonly Regex NameRegex = new(NameRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
    private static readonly Regex TypeRegex = new(TypeRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
    private static readonly Regex DefaultRegex = new(DefaultRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    public record ShaderGlobal(string Name, string Type, string Default);

    public static IEnumerable<ShaderGlobal> GetShaderGlobals(string gdRoot)
    {
        var gdPrj = GD.PRJ(gdRoot);
        Log.Debug($"Scraping {gdPrj}");

        return MatchShaderGlobals(gdPrj);

        static IEnumerable<ShaderGlobal> MatchShaderGlobals(string gdFile)
        {
            var wip = false;
            var found = false;
            string name = null;
            string type = null;
            string dflt = null;
            foreach (var line in File.ReadLines(gdFile)
                .Where(line => line is not ""))
            {
                Log.Debug($"Line: {line}");

                if (line is "[shader_globals]")
                {
                    found = true;
                    continue;
                }

                if (found)
                {
                    if (MatchShaderGlobal(line))
                        yield return new(name, type, dflt.NullIfEmpty());
                    else if (!wip && line.StartsWith("["))
                        yield break;
                }
            }

            bool MatchShaderGlobal(string line)
            {
                if (name is null) { MatchName(); return false; }
                if (type is null) { MatchType(); return false; }
                if (dflt is null) { MatchDflt(); return true; }
                MatchEnd(); return false;

                void MatchName()
                {
                    if (NameRegex.Match(line) is { Success: true } match)
                    {
                        Log.Debug($" - Name {NameRegex.GetGroupsAsStr(match)}");
                        name = match.Groups["Name"].Value;
                        if (wip) EXPECTED("new ShaderGlobal");
                        if (type is not null) EXPECTED("name before type");
                        if (dflt is not null) EXPECTED("name before default");
                        wip = true;
                        return;
                    }

                    EXPECTED(NameRegexStr);
                }

                void MatchType()
                {
                    if (TypeRegex.Match(line) is { Success: true } match)
                    {
                        Log.Debug($" - Type {TypeRegex.GetGroupsAsStr(match)}");
                        type = match.Groups["Type"].Value;
                        if (!wip) EXPECTED("within ShaderGlobal");
                        if (name is null) EXPECTED("name before type");
                        if (dflt is not null) EXPECTED("type before default");
                        return;
                    }

                    EXPECTED(TypeRegexStr);
                }

                void MatchDflt()
                {
                    if (DefaultRegex.Match(line) is { Success: true } match)
                    {
                        Log.Debug($" - Default {DefaultRegex.GetGroupsAsStr(match)}");
                        dflt = match.Groups["Default"].Value;
                        if (!wip) EXPECTED("within ShaderGlobal");
                        if (name is null) EXPECTED("name before default");
                        if (type is null) EXPECTED("type before default");
                        return;
                    }

                    EXPECTED(DefaultRegexStr);
                }

                void MatchEnd()
                {
                    if (line is "}")
                    {
                        Log.Debug($" - END");
                        if (!wip) EXPECTED("within ShaderGlobal");
                        if (name is null) EXPECTED("name before end");
                        if (type is null) EXPECTED("type before end");
                        if (dflt is null) EXPECTED("default before end");
                        wip = false; name = null; type = null; dflt = null;
                        return;
                    }

                    EXPECTED("}");
                }

                void EXPECTED(string reason)
                    => throw new Exception($"Malformed ShaderGlobal [Expected: {reason}, Found: {line} - Name: {name}, Type: {type}, Default: {dflt}]");
            }
        }
    }
}
