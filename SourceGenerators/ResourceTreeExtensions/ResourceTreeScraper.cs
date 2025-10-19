using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.ResourceTreeExtensions;

internal static class ResourceTreeScraper
{
    private const string ImportFileTypeRegexStr = @"^type=""(?<Type>.*?)""$";
    private const string TResTypeRegexStr = @"^\[gd_resource type=""(?<Type>.*?)""";
    private const string InvalidIdentifierRegexStr = @"[^\p{Cf}\p{L}\p{Mc}\p{Mn}\p{Nd}\p{Nl}\p{Pc}]";
    private const string InvalidIdentifierStartRegexStr = @"^[^\p{L}\p{Nl}_]";

    private static Regex ImportFileTypeRegex = new(ImportFileTypeRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
    private static Regex TResTypeFileTypeRegex = new(TResTypeRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
    private static Regex InvalidIdentifierRegex = new(InvalidIdentifierRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
    private static Regex InvalidIdentifierStartRegex = new(InvalidIdentifierStartRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    public static Tree<ResourceTreeNode> GetNodes(Compilation compilation, string className, string gdRoot)
    {
        Log.Debug($"Scraping {gdRoot} for resources");

        Tree<ResourceTreeNode> sceneTree = new(new(className));

        ScrapeDirectory(gdRoot, sceneTree);

        void ScrapeDirectory(string path, TreeNode<ResourceTreeNode> parent)
        {
            HashSet<string> usedNames = [parent.Value.Name];

            foreach (var dir in Directory.EnumerateDirectories(path)
                         // Exclude e.g. .godot and .vs
                         .Where(x => !new DirectoryInfo(x).Attributes.HasFlag(FileAttributes.Hidden)))
            {
                var name = SanitizeName(Path.GetFileName(dir));
                usedNames.Add(name);
                ScrapeDirectory(dir, parent.Add(new(name)));
            }

            foreach (var file in Directory.EnumerateFiles(path))
            {
                if ((GetTypeFromExtension(file)
                     ?? GetTypeFromImportFile(file)) is { } type)
                {
                    var name = SanitizeName(Path.GetFileNameWithoutExtension(file));
                    if (usedNames.Contains(name))
                    {
                        name = SanitizeName(Path.GetFileName(file));
                        while (usedNames.Contains(name))
                        {
                            name = '_' + name;
                        }
                    }
                    usedNames.Add(name);

                    parent.Add(new ResourceTreeLeafNode(name, type, GD.GetResourcePath(file, gdRoot)));
                }

                static string GetTypeFromExtension(string file)
                {
                    return Path.GetExtension(file).ToLowerInvariant() switch
                    {
                        ".tscn" or ".scn" => "PackedScene",
                        ".cs" => "CSharpScript",
                        ".gd" => "GDScript",
                        ".tres" => TResTypeFileTypeRegex.Match(File.ReadLines(file).First()).Groups["Type"].Value,
                        _ => null,
                    };
                }

                static string GetTypeFromImportFile(string file)
                {
                    file += ".import";

                    if (!File.Exists(file)) return null;

                    foreach (var line in File.ReadLines(file))
                    {
                        var match = ImportFileTypeRegex.Match(line);
                        if (match.Success)
                            return match.Groups["Type"].Value;
                    }

                    return null;
                }
            }

            string SanitizeName(string name)
            {
                name = InvalidIdentifierRegex.Replace(name, "_");

                if (InvalidIdentifierStartRegex.IsMatch(name))
                    name = "_" + name;

                // Prevent conflicts with keywords
                if (name.All(char.IsLower))
                    name = char.ToUpperInvariant(name[0]) + name[1..];

                return name;
            }
        }

        return sceneTree;
    }
}
