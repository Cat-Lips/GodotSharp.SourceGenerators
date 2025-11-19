using System.Text.RegularExpressions;

namespace GodotSharp.SourceGenerators.ShaderGlobalsExtensions;

internal static class ShaderGlobalsScraper
{
    private static readonly Regex BeginGlobalRegex = new(@"^""?(?<Name>.+?)""?={$");
    private static readonly Regex EndGlobalRegex = new("^}$");
    private static readonly Regex TypeRegex = new(@"^""type"": ""(?<Type>.+)"",?$");
    private static readonly Regex DefaultRegex = new(@"^""value"": (?<Default>.+?),?$");

    public record ShaderGlobal(string Type, string Name, string Default);

    public static IEnumerable<ShaderGlobal> GetShaderGlobals(string csFile, string gdRoot)
    {
        var gdFile = GD.GetProjectFile(csFile, gdRoot);
        Log.Debug($"Scraping {gdFile} [Compiling {csFile}]");

        return MatchShaderConstants(gdFile);

        static IEnumerable<ShaderGlobal> MatchShaderConstants(string gdFile)
        {
            var found = false;
            string name = null;
            string type = null;
            string @default = null;
            foreach (var line in File.ReadLines(gdFile).Where(line => line != string.Empty))
            {
                Log.Debug($"Line: {line}");


                if (line is "[shader_globals]")
                {
                    found = true;
                    continue;
                }

                if (found)
                {
                    if (name != null)
                    {
                        if (EndGlobalRegex.IsMatch(line))
                        {
                            if (type is null)
                            {
                                Log.Warn($" - Ignoring shader global without type {name}");
                                continue;
                            }

                            yield return new(type, name, @default);

                            name = null;
                            type = null;
                            @default = null;
                        }
                        else if (TypeRegex.Match(line) is { Success: true } typeMatch)
                        {
                            if (type != null)
                                Log.Warn($" - Duplicate type {type} for shader global {name}");

                            type = typeMatch.Groups["Type"].Value;
                        }
                        else if (DefaultRegex.Match(line) is { Success: true } defaultMatch)
                        {
                            if (@default != null)
                                Log.Warn($" - Duplicate default {@defaultMatch} for shader global {name}");

                            @default = defaultMatch.Groups["Default"].Value;
                        }
                    }
                    else
                    {
                        if (line.StartsWith("["))
                            yield break;
                        else if (BeginGlobalRegex.Match(line) is { Success: true } beginMatch)
                            name = beginMatch.Groups["Name"].Value;
                    }
                }
            }
        }
    }
}
