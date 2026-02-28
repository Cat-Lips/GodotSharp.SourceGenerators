using System.Text.RegularExpressions;

namespace GodotSharp.SourceGenerators.ShaderExtensions;

internal static class VisualShaderScraper
{
    #region Regex

    private static class X
    {
        public const string Type = @"(?<type>\w+)";
        public const string Name = @"(?<name>\w+)";
        public const string Dflt = @"(?<dflt>.+)";
    }

    private const string VisualTypeRegexStr = @$"^\[sub_resource type=""VisualShaderNode{X.Type}Parameter""";
    private const string VisualNameRegexStr = @$"^parameter_name = ""{X.Name}""";
    private const string VisualDfltRegexStr = @$"^default_value = {X.Dflt}$";

    private static readonly Regex VisualTypeRegex = new(VisualTypeRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
    private static readonly Regex VisualNameRegex = new(VisualNameRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
    private static readonly Regex VisualDfltRegex = new(VisualDfltRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    #endregion

    internal record ShaderUniform(string Type, string Name, string Default, bool IsEnum);

    public static IEnumerable<ShaderUniform> GetUniforms(string shader)
    {
        Log.Debug($"Scraping VisualShader: {shader}");

        string type = null;
        string name = null;
        string dflt = null;
        var isEnum = false;

        foreach (var line in File.ReadLines(shader))
        {
            Log.Debug($"Line: {line}");

            if (type is null)
            {
                var match = VisualTypeRegex.Match(line);
                if (match.Success)
                {
                    Log.Debug($" - VisualType {VisualTypeRegex.GetGroupsAsStr(match)}");
                    type = match.Groups["type"].Value;
                    continue;
                }
            }

            else if (name is null)
            {
                var match = VisualNameRegex.Match(line);
                if (match.Success)
                {
                    Log.Debug($" - VisualName {VisualNameRegex.GetGroupsAsStr(match)}");
                    name = match.Groups["name"].Value;
                    continue;
                }
            }

            else if (dflt is null)
            {
                if (line.StartsWith("enum_names = "))
                {
                    Log.Debug($" - (enum)");
                    isEnum = true;
                    continue;
                }

                var match = VisualDfltRegex.Match(line);
                if (match.Success)
                {
                    Log.Debug($" - Dflt {VisualDfltRegex.GetGroupsAsStr(match)}");
                    dflt = match.Groups["dflt"].Value;
                    continue;
                }
            }

            if (line is "")
            {
                if (type is not null)
                {
                    var v = new ShaderUniform(type, name, dflt, isEnum);
                    Log.Debug($" - {v}");
                    yield return v;
                }

                type = null;
                name = null;
                dflt = null;
                isEnum = false;
                continue;
            }

            else if (line is "[resource]")
            {
                Log.Debug(" - End detected, stopping scrape");
                yield break;
            }
        }
    }
}
