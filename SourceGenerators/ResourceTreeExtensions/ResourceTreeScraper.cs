using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.ResourceTreeExtensions;

using MyTree = Tree<ResourceTreeNode>;
using MyTreeNode = TreeNode<ResourceTreeNode>;

internal static class ResourceTreeScraper
{
    public static MyTree GetResourceTree(Compilation compilation, string gdRoot, string source, IResourceTreeConfig cfg)
    {
        Log.Debug($"Scanning {source} [Scenes: {cfg.Scenes}, Scripts: {cfg.Scripts}, Uid: {cfg.Uid}, Xtras: {string.Join("|", cfg.Xtras)}]");

        var xtras = new HashSet<string>(cfg.Xtras.Select(x => x.TrimStart('.')));
        var tree = new MyTree(null);
        ScanDir(source, tree);
        return tree;

        void ScanDir(string path, MyTreeNode parent)
    {
            if (!Ignore())
            {
                ScanDirs();
                ScanFiles();
            }

            bool Ignore()
                => File.Exists(Path.Combine(path, ".gdignore"));

            void ScanDirs()
            {
                foreach (var dir in Directory.EnumerateDirectories(path))
                {
                    var name = Path.GetFileName(dir);
                    if (name.StartsWith(".")) continue;

                    name = name.ReplaceUnsafeChars();
                    Log.Debug($"Dir: {dir}, Name: {name}");
                    var next = new MyTreeNode(new ResourceTreeDir(name), parent);
                    ScanDir(dir, next);

                    if (next.HasChildren)
                        parent.Children.Add(next);
                }
            }

            void ScanFiles()
        {
                const string UID = "UID";
                const string RAW = "RAW";

                foreach (var file in Directory.EnumerateFiles(path))
            {
                    var name = Path.GetFileName(file);
                    if (name.StartsWith(".")) continue;

                    var type = TryGetType(file, out var exports);
                    if (type is null) continue;

                    if (!cfg.Scenes && type is "PackedScene") continue;
                    if (!cfg.Scripts && type is "CSharpScript" or "GDScript") continue;

                    Log.Debug($"File: {file}");

                    if (exports?.Length is null or 0)
                        AddFile(file, name, type);
                    else AddExports(exports, type);
            }

                void AddFile(string file, string name, string type)
            {
                    GetResource(out var resource);
                    name = name.ReplaceUnsafeChars().ToPascalCase();
                    Log.Debug($" - Name: {name}, Type: {type}, Resource: {resource}");
                    parent.Add(new ResourceTreeFile(name, type, resource));

                    void GetResource(out string resource)
                    {
                        switch (type)
                        {
                            case UID: type = null; resource = MiniUidScraper.GetUid(file); break;
                            case RAW: type = null; resource = GD.RES(file, gdRoot); break;
                            default: resource = GD.RES(file, gdRoot); break;
                        }
                        }
                    }
                    usedNames.Add(name);

                void AddExports(string[] exports, string type)
                {
                    foreach (var res in exports)
                    {
                        var name = Path.GetFileName(res).ReplaceUnsafeChars().ToPascalCase();
                        Log.Debug($" - Name: {name}, Type: {type}, Res: {res}");
                        parent.Add(new ResourceTreeFile(name, type, res));
                    }
                }

                string TryGetType(string file, out string[] exports)
                {
                    exports = null;
                    return TryGetTypeFromExtension() ??
                           TryGetTypeFromImportFile(ref exports) ??
                           TryGetTypeFromXtrasLookup();

                    string TryGetTypeFromExtension() => Path.GetExtension(file) switch
                    {
                        ".tres" => MiniTresScraper.GetType(compilation, file),
                        ".tscn" or ".scn" => "PackedScene",
                        ".uid" => cfg.Uid ? UID : null,
                        ".cs" => "CSharpScript",
                        ".gd" => "GDScript",
                        _ => null
                    };
                }

                static string GetTypeFromImportFile(string file)
                {
                    file += ".import";

                    if (!File.Exists(file)) return null;

                    string TryGetTypeFromImportFile(ref string[] exports)
                    {
                        var importFile = $"{file}.import";
                        return File.Exists(importFile) ? MiniImportScraper.GetType(importFile, out exports) : null;
                    }

                    string TryGetTypeFromXtrasLookup()
                        => xtras.Contains(Path.GetExtension(file).TrimStart('.')) ? RAW : null;
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
