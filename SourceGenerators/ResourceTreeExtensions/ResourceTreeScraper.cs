using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.ResourceTreeExtensions;

using MyTree = Tree<ResourceTreeNode>;
using MyTreeNode = TreeNode<ResourceTreeNode>;

internal static class ResourceTreeScraper
{
    public static MyTree GetResourceTree(Compilation compilation, string gdRoot, string source, IResourceTreeConfig cfg)
    {
        Log.Debug($"Scanning {source} [{cfg}]");
        var tree = new MyTree(new ResourceTreeRoot(GD.RES(source, gdRoot)));
        Log.Debug($" - {tree.Value}");
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
                    if (name is "addons") continue;
                    if (name.StartsWith(".")) continue;
                    if (cfg.Xclude.Contains(name)) continue;

                    name = name.ToPascalCase();
                    var res = GD.RES(dir, gdRoot);
                    var node = new ResourceTreeDir(name, res);
                    var next = new MyTreeNode(node, parent);
                    Log.Debug($" - {node}");
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

                    if (exports?.Length is null or 0)
                        AddFile(file, name, type);
                    else AddExports(exports, type);
                }

                void AddFile(string file, string name, string type)
                {
                    GetResource(out var res);
                    name = name.ToPascalCase();
                    type = cfg.Load ? type : null;
                    var node = new ResourceTreeFile(name, res, type);
                    Log.Debug($" - {node}");
                    parent.Add(node);

                    void GetResource(out string res)
                    {
                        switch (type)
                        {
                            case UID: type = null; res = MiniUidScraper.GetUid(file); break;
                            case RAW: type = null; res = GD.RES(file, gdRoot); break;
                            default: res = GD.RES(file, gdRoot); break;
                        }
                    }
                }

                void AddExports(string[] exports, string type)
                {
                    type = cfg.Load ? type : null;

                    foreach (var res in exports)
                    {
                        var name = Path.GetFileName(res).ToPascalCase();
                        var node = new ResourceTreeFile(name, res, type);
                        Log.Debug($" - {node}");
                        parent.Add(node);
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

                    string TryGetTypeFromImportFile(ref string[] exports)
                    {
                        var importFile = $"{file}.import";
                        return File.Exists(importFile) ? MiniImportScraper.GetType(importFile, out exports) : null;
                    }

                    string TryGetTypeFromXtrasLookup()
                        => cfg.Xtras.Contains(Path.GetExtension(file).TrimStart('.')) ? RAW : null;
                }
            }
        }
    }
}
