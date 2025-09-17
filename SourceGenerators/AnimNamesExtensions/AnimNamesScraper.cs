using System.Text.RegularExpressions;

namespace GodotSharp.SourceGenerators.AnimNamesExtensions;

internal static class AnimNamesScraper
{
    private enum AnimType
    {
        AnimLib,
        SpriteFrames,
    }

    private const string TscnNode = @"[node ";
    private const string AnimLibTres = @"[gd_resource type=""AnimationLibrary""";
    private const string AnimLibTscn = @"[sub_resource type=""AnimationLibrary""";
    private const string SpriteFramesTres = @"[gd_resource type=""SpriteFrames""";
    private const string SpriteFramesTscn = @"[sub_resource type=""SpriteFrames""";

    private const string AnimLibRegexStr = @"^&""(?<Name>.+)"":";
    private const string SpriteFramesRegexStr = @"^""name"": &""(?<Name>.+)""";
    private static readonly Regex AnimLibRegex = new(AnimLibRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
    private static readonly Regex SpriteFramesRegex = new(SpriteFramesRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    public static IEnumerable<string> GetAnimNames(string source, string csFile)
    {
        Log.Debug($"Scraping {source} [Compiling {csFile}]");
        return MatchAnimNames(source).Distinct();

        static IEnumerable<string> MatchAnimNames(string source)
        {
            var ext = Path.GetExtension(source);
            return ext switch
            {
                ".tres" => MatchAnimNamesTres(),
                ".tscn" => MatchAnimNamesTscn(),
                _ => throw new Exception($"Unsupported [AnimNames] extension {ext} in {source}"),
            };

            IEnumerable<string> MatchAnimNamesTres()
            {
                var first = true;
                var found = false;
                var type = (AnimType?)null;
                foreach (var line in File.ReadLines(source))
                {
                    Log.Debug($"Line: {line}");

                    if (first)
                    {
                        first = false;
                        type = line.StartsWith(AnimLibTres) ? AnimType.AnimLib
                             : line.StartsWith(SpriteFramesTres) ? AnimType.SpriteFrames
                             : throw new Exception($"Unsupported [AnimNames] format {line} in {source}");
                        continue;
                    }

                    if (!found)
                    {
                        found = line is "[resource]";
                        continue;
                    }

                    if (type is AnimType.AnimLib && TryMatchAnimLib(line, out var name) ||
                        type is AnimType.SpriteFrames && TryMatchSpriteFrames(line, out name))
                    {
                        yield return name;
                    }
                }
            }

            IEnumerable<string> MatchAnimNamesTscn()
            {
                var type = (AnimType?)null;
                foreach (var line in File.ReadLines(source))
                {
                    Log.Debug($"Line: {line}");

                    if (type is null)
                    {
                        type = line.StartsWith(AnimLibTscn) ? AnimType.AnimLib
                             : line.StartsWith(SpriteFramesTscn) ? AnimType.SpriteFrames
                             : null;
                        continue;
                    }

                    if (line == string.Empty)
                    {
                        type = null;
                        continue;
                    }

                    if (line.StartsWith(TscnNode))
                        yield break;

                    if (type is AnimType.AnimLib && TryMatchAnimLib(line, out var name) ||
                        type is AnimType.SpriteFrames && TryMatchSpriteFrames(line, out name))
                    {
                        yield return name;
                    }
                }
            }

            static bool TryMatchAnimLib(string line, out string name)
            {
                var match = AnimLibRegex.Match(line);
                if (match.Success)
                {
                    Log.Debug($" - AnimLib {AnimLibRegex.GetGroupsAsStr(match)}");
                    name = match.Groups["Name"].Value;
                    return true;
                }

                name = default;
                return false;
            }

            static bool TryMatchSpriteFrames(string line, out string name)
            {
                var match = SpriteFramesRegex.Match(line);
                if (match.Success)
                {
                    Log.Debug($" - SpriteFrames {SpriteFramesRegex.GetGroupsAsStr(match)}");
                    name = match.Groups["Name"].Value;
                    return true;
                }

                name = default;
                return false;
            }
        }
    }
}
