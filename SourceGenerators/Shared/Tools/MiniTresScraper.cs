using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators;

internal static class MiniTresScraper
{
    private const string ResourceToken = "[resource]";
    private const string BaseTypeRegexStr = @"^\[gd_resource type=""(?<BaseType>.+?)""";
    private const string ScriptPathRegexStr = @"^\[ext_resource type=""Script"".+?path=""(?<Path>.+?)"".+?id=""(?<Id>.+?)""";
    private const string ScriptUsageRegexStr = @"^script = ExtResource(""(?<Id>.+?)"")";
    private static readonly Regex BaseTypeRegex = new(BaseTypeRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
    private static readonly Regex ScriptPathRegex = new(ScriptPathRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
    private static readonly Regex ScriptUsageRegex = new(ScriptUsageRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    public static string GetType(Compilation compilation, string tres)
    {
        Log.Debug($">>> GetType {tres}");
        var type = GetType();
        Log.Debug($"<<< {type ?? "<null>"}");
        return type;

        string GetType()
        {
            string baseType = null;
            string classType = null;
            var resourceFound = false;
            Dictionary<string, string> scripts = [];
            foreach (var line in File.ReadLines(tres))
            {
                Log.Debug($"Line: {line}");

                if (ScanHeader(line)) continue;

                if (line is "") continue;
                resourceFound |= line is ResourceToken;

                if (ScanExternals(line)) continue;
                if (ScanProperties(line)) continue;
            }

            return classType ?? baseType;

            bool ScanHeader(string line)
            {
                if (baseType is null)
                {
                    var match = BaseTypeRegex.Match(line);
                    if (!match.Success) throw new Exception($"Expected gd_resource header on first line!\n - Line: {line}\n - TRES: {tres}");
                    Log.Debug($" - BaseTypeRegex {BaseTypeRegex.GetGroupsAsStr(match)}");
                    baseType = match.Groups["BaseType"].Value;
                    return true;
                }

                return false;
            }

            bool ScanExternals(string line)
            {
                if (!resourceFound)
                {
                    var match = ScriptPathRegex.Match(line);
                    if (match.Success)
                    {
                        Log.Debug($" - ScriptPathRegex {ScriptPathRegex.GetGroupsAsStr(match)}");
                        var path = match.Groups["Path"].Value;
                        var id = match.Groups["Id"].Value;
                        if (path.EndsWith(".cs"))
                            scripts.Add(id, path);
                        return true;
                    }
                }

                return false;
            }

            bool ScanProperties(string line)
            {
                if (resourceFound)
                {
                    var match = ScriptUsageRegex.Match(line);
                    if (match.Success)
                    {
                        Log.Debug($" - ScriptUsageRegex {ScriptUsageRegex.GetGroupsAsStr(match)}");
                        var id = match.Groups["Id"].Value;
                        var script = scripts.Get(id);
                        if (script is not null)
                            classType = MiniCsScraper.GetType(compilation, script);
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
