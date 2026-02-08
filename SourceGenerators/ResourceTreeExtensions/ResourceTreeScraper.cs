using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.ResourceTreeExtensions;

using MyTree = Tree<ResourceTreeNode>;
using MyTreeNode = TreeNode<ResourceTreeNode>;

internal static class ResourceTreeScraper
{
    public static MyTree GetResourceTree(Compilation compilation, string gdRoot, string source, IResourceTreeConfig cfg)
    {
        Log.Debug($"Scanning {source} [{cfg}]");
        var res = GD.RES(source, gdRoot);
        var root = new ResourceTreeRoot(res, cfg.ShowDirPaths);
        var tree = new MyTree(root);
        Log.Debug($" - {root}");
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
                    var node = new ResourceTreeDir(name, res, cfg.ShowDirPaths);
                    var next = new MyTreeNode(node, parent);
                    Log.Debug($" - {node}");
                    ScanDir(dir, next);

                    if (next.HasChildren)
                        parent.Children.Add(next);
                }
            }

            void ScanFiles()
            {
                const string _UID = "UID";
                const string _RAW = "RAW";

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
                    var show = cfg.UseResPaths;
                    GetResource(out var res);
                    name = name.ToPascalCase();
                    type = cfg.UseGdLoad ? type : null;
                    if (type is not null) type = compilation.GetFullName(type);
                    var node = new ResourceTreeFile(name, res, type, show);
                    Log.Debug($" - {node}");
                    parent.Add(node);

                    void GetResource(out string res)
                    {
                        switch (type)
                        {
                            case _UID: type = null; show = true; res = UID.Get(file); break;
                            case _RAW: type = null; show = true; res = GD.RES(file, gdRoot); break;
                            default: res = GD.RES(file, gdRoot); break;
                        }
                    }
                }

                void AddExports(string[] exports, string type)
                {
                    type = cfg.UseGdLoad ? type : null;
                    if (type is not null) type = compilation.GetFullName(type);

                    foreach (var res in exports)
                    {
                        var name = Path.GetFileName(res).ToPascalCase();
                        var node = new ResourceTreeFile(name, res, type, cfg.UseResPaths);
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
                        ".res" => "Resource",
                        ".tscn" or ".scn" => "PackedScene",
                        ".mesh" => "Mesh",
                        ".multimesh" => "MultiMesh",
                        ".meshlib" => "MeshLibrary",
                        ".material" => "Material",
                        ".gdshader" => "Shader",
                        ".atlastex" => "AtlasTexture",
                        ".fontdata" => "Font",
                        ".theme" => "Theme",
                        ".anim" => "Animation",
                        ".occ" => "Occluder3D",
                        ".shape" => "Shape3D",
                        ".phymat" => "PhysicsMaterial",
                        ".json" => "JSON",
                        ".cs" => "CSharpScript",
                        ".gd" => "GDScript",
                        ".uid" => cfg.Uid ? _UID : null,
                        _ => null
                    };

                    string TryGetTypeFromImportFile(ref string[] exports)
                    {
                        var importFile = $"{file}.import";
                        return File.Exists(importFile) ? MiniImportScraper.GetType(importFile, out exports) : null;
                    }

                    string TryGetTypeFromXtrasLookup()
                        => cfg.Xtras.Contains(Path.GetExtension(file).TrimStart('.')) ? _RAW : null;
                }
            }
        }
    }
}
