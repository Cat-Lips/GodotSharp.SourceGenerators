using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.AutoloadExtensions;

using Autoload = (string Name, string Type);

internal static class AutoloadScraper
{
    private const string AutoloadRegexStr = @"^(?<Name>.+?)=""\*res:\/\/(?<Path>.+?)""$";
    private static readonly Regex AutoloadRegex = new(AutoloadRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    public static IEnumerable<Autoload> GetAutoloads(Compilation compilation, string csFile, string gdRoot)
    {
        var gdFile = GD.GetProjectFile(csFile, gdRoot);
        Log.Debug($"Scraping {gdFile} [Compiling {csFile}]");
        gdRoot ??= Path.GetDirectoryName(gdFile);

        return MatchAutoloads(gdFile);

        IEnumerable<Autoload> MatchAutoloads(string gdFile)
        {
            var found = false;
            foreach (var line in File.ReadLines(gdFile)
                .Where(line => line is not "" && !line.StartsWith(";")))
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
                        yield return (name, TryGetType(path) ?? "Node");
                    else if (line.StartsWith("["))
                        yield break;
                }
            }

            bool TryMatchAutoload(string line, out string name, out string path)
            {
                var match = AutoloadRegex.Match(line);
                if (match.Success)
                {
                    Log.Debug($" - AutoloadRegex {AutoloadRegex.GetGroupsAsStr(match)}");
                    name = match.Groups["Name"].Value;
                    path = match.Groups["Path"].Value;
                    return true;
                }

                name = null;
                path = null;
                return false;
            }

            string TryGetType(string path)
            {
                return Path.GetExtension(path) switch
                {
                    ".gd" => MiniGdScraper.TryGetType(compilation, gdRoot, path),
                    ".cs" => MiniCsScraper.TryGetType(compilation, gdRoot, path),
                    ".tscn" => MiniTscnScraper.TryGetType(compilation, gdRoot, path),
                    _ => null,
                };
            }
        }
    }

    private static class MiniGdScraper
    {
        private const string ExtendsRegexStr1 = @"extends\s+""(?<Script>.+?)""";
        private const string ExtendsRegexStr2 = @"extends\s+(?<Type>\w+)";

        private static readonly Regex ExtendsRegex1 = new(ExtendsRegexStr1, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static readonly Regex ExtendsRegex2 = new(ExtendsRegexStr2, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        public static string TryGetType(Compilation compilation, string gdRoot, string path)
        {
            var file = Path.Combine(gdRoot, path);
            Log.Debug($">>> Scraping type from gd: {file}");

            string type = null;
            foreach (var line in File.ReadLines(file)
                .Where(line => line is not "" && !line.StartsWith("#")))
            {
                Log.Debug($"Line: {line}");

                if (TryMatchExtends(line, ref type)) break;
                if (line.StartsWith("class_name")) continue;

                break;
            }

            type = GetValidType(type);
            Log.Debug($"<<< {type}");
            return type;

            bool TryMatchExtends(string line, ref string type)
            {
                var match = ExtendsRegex1.Match(line);
                if (match.Success)
                {
                    Log.Debug($" - ExtendsRegex1 {ExtendsRegex1.GetGroupsAsStr(match)}");
                    var script = match.Groups["Script"].Value;
                    type = script.StartsWith("res://")
                        ? TryGetType(compilation, gdRoot, script.Replace("res://", ""))
                        : TryGetType(compilation, gdRoot, Path.Combine(Path.GetDirectoryName(path), script));
                    return true;
                }

                match = ExtendsRegex2.Match(line);
                if (match.Success)
                {
                    Log.Debug($" - ExtendsRegex2 {ExtendsRegex2.GetGroupsAsStr(match)}");
                    type = match.Groups["Type"].Value;
                    return true;
                }

                return false;
            }

            string GetValidType(string type)
            {
                if (type is null)
                {
                    Log.Debug(" - Type: Node (default)");
                    return "Node";
                }

                var csType = compilation.GetValidType(type);
                if (csType != type)
                {
                    Log.Debug($" - Type: {csType} (from {type})");
                    return csType;
                }

                Log.Debug($" - Type: {type}");
                return type;
            }
        }
    }

    private static class MiniCsScraper
    {
        public static string TryGetType(Compilation compilation, string gdRoot, string path)
        {
            var file = Path.Combine(gdRoot, path);
            Log.Debug($">>> Scraping type from cs: {file}");
            var name = Path.GetFileNameWithoutExtension(file); // Cheating?
            var type = compilation.GetFullName(name, file);
            Debug.Assert(type is not null);
            type = GetValidType(type);
            Log.Debug($"<<< {type}");
            return type;

            static string GetValidType(string type)
            {
                if (type is null)
                {
                    Log.Debug(" - Type: Node (default)");
                    return "Node";
                }

                Log.Debug($" - Type: {type}");
                return type;
            }
        }
    }

    private static class MiniTscnScraper
    {
        private const string ResourceRegexStrGD3 = @"^\[ext_resource path=""res://(?<Path>.*?)"" type=""(?<Type>PackedScene|Script)"" id=(?<Id>.*?)\]$";
        private const string ResourceRegexStrGD4 = @"^\[ext_resource type=""(?<Type>PackedScene|Script)"".+?path=""res://(?<Path>.*?)"".+?id=""(?<Id>.*?)""\]$";
        private const string RootNodeRegexStr = @"^\[node name=""(?<Name>.*?)"" type=""(?<Type>.*?)""\]$";
        private const string InheritRegexStrGD3 = @"^\[node name=""(?<Name>.*?)"" instance=ExtResource\( (?<ResId>.*?) \)\]$";
        private const string InheritRegexStrGD4 = @"^\[node name=""(?<Name>.*?)"" instance=ExtResource\(""(?<ResId>.*?)""\)\]$";
        private const string ScriptRegexStrGD3 = @"^script = ExtResource\( (?<ResId>.*?) \)$";
        private const string ScriptRegexStrGD4 = @"^script = ExtResource\(""(?<ResId>.*?)""\)$";

        private static readonly Regex ResourceRegexGD3 = new(ResourceRegexStrGD3, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static readonly Regex ResourceRegexGD4 = new(ResourceRegexStrGD4, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static readonly Regex RootNodeRegex = new(RootNodeRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static readonly Regex InheritRegexGD3 = new(InheritRegexStrGD3, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static readonly Regex InheritRegexGD4 = new(InheritRegexStrGD4, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static readonly Regex ScriptRegexGD3 = new(ScriptRegexStrGD3, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static readonly Regex ScriptRegexGD4 = new(ScriptRegexStrGD4, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        public static string TryGetType(Compilation compilation, string gdRoot, string tscn)
        {
            var file = Path.Combine(gdRoot, tscn);
            Log.Debug($">>> Scraping type from tscn: {file}");

            string type = null;
            var doneRes = false;
            var doneRoot = false;
            var doneValues = false;
            Dictionary<string, string> scenes = [];
            Dictionary<string, string> scripts = [];
            foreach (var line in File.ReadLines(file)
                .Skip(1).Where(line => line is not ""))
            {
                Log.Debug($"Line: {line}");

                if (TryMatchRes(line)) continue;
                if (TryMatchRoot(line, ref type)) continue;
                if (TryMatchValues(line, ref type)) continue;
                break;
            }

            type = GetValidType(type);
            Log.Debug($"<<< {type}");
            return type;

            bool TryMatchRes(string line)
            {
                if (doneRes) return false;

                var match = ResourceRegexGD4.Match(line);
                if (match.Success)
                {
                    Log.Debug($" - ResourceRegexGD4 {ResourceRegexGD4.GetGroupsAsStr(match)}");
                    var type = match.Groups["Type"].Value;
                    var path = match.Groups["Path"].Value;
                    var id = match.Groups["Id"].Value;
                    if (type is "PackedScene") scenes.Add(id, path);
                    else if (type is "Script") scripts.Add(id, path);
                    return true;
                }

                match = ResourceRegexGD3.Match(line);
                if (match.Success)
                {
                    Log.Debug($" - ResourceRegexGD3 {ResourceRegexGD3.GetGroupsAsStr(match)}");
                    var type = match.Groups["Type"].Value;
                    var path = match.Groups["Path"].Value;
                    var id = match.Groups["Id"].Value;
                    if (type is "PackedScene") scenes.Add(id, path);
                    else if (type is "Script") scripts.Add(id, path);
                    return true;
                }

                doneRes = true;
                return false;
            }

            bool TryMatchRoot(string line, ref string type)
            {
                if (doneRoot) return false;

                var match = RootNodeRegex.Match(line);
                if (match.Success)
                {
                    Log.Debug($" - RootNodeRegex {RootNodeRegex.GetGroupsAsStr(match)}");
                    var name = match.Groups["Name"].Value;
                    type = match.Groups["Type"].Value;
                    Log.Debug($" - Type: {type} (from root node)");
                    doneRoot = true;
                    return true;
                }

                match = InheritRegexGD4.Match(line);
                if (match.Success)
                {
                    Log.Debug($" - InheritRegexGD4 {InheritRegexGD4.GetGroupsAsStr(match)}");
                    var name = match.Groups["Name"].Value;
                    var id = match.Groups["ResId"].Value;
                    type = TryGetType(compilation, gdRoot, scenes[id]);
                    Log.Debug($" - Type: {type} (from inherited root)");
                    doneRoot = true;
                    return true;
                }

                match = InheritRegexGD3.Match(line);
                if (match.Success)
                {
                    Log.Debug($" - InheritRegexGD3 {InheritRegexGD3.GetGroupsAsStr(match)}");
                    var name = match.Groups["Name"].Value;
                    var id = match.Groups["ResId"].Value;
                    type = TryGetType(compilation, gdRoot, scenes[id]);
                    Log.Debug($" - Type: {type} (from inherited root)");
                    doneRoot = true;
                    return true;
                }

                return true; // Not matched yet
            }

            bool TryMatchValues(string line, ref string type)
            {
                if (doneValues) return false;

                if (line.StartsWith("["))
                {
                    doneValues = true;
                    return false; // No script
                }

                if (TryMatchScript(line, ref type))
                {
                    doneValues = true;
                    return false; // Done
                }

                return true;

                bool TryMatchScript(string line, ref string type)
                {
                    var match = ScriptRegexGD4.Match(line);
                    if (match.Success)
                    {
                        Log.Debug($" - ScriptRegexGD4 {ScriptRegexGD4.GetGroupsAsStr(match)}");
                        var id = match.Groups["ResId"].Value;
                        type = TryGetType(scripts[id]);
                        return true;
                    }

                    match = ScriptRegexGD3.Match(line);
                    if (match.Success)
                    {
                        Log.Debug($" - ScriptRegexGD3 {ScriptRegexGD3.GetGroupsAsStr(match)}");
                        var id = match.Groups["ResId"].Value;
                        type = TryGetType(scripts[id]);
                        return true;
                    }

                    return false;

                    string TryGetType(string path)
                    {
                        return Path.GetExtension(path) switch
                        {
                            ".gd" => MiniGdScraper.TryGetType(compilation, gdRoot, path),
                            ".cs" => MiniCsScraper.TryGetType(compilation, gdRoot, path),
                            _ => null,
                        };
                    }
                }
            }

            string GetValidType(string type)
            {
                if (type is null)
                {
                    Log.Debug($" - Type: Node (unknown)");
                    return "Node";
                }

                Log.Debug($" - Type: {type} (final)");
                return type;
            }
        }
    }
}
