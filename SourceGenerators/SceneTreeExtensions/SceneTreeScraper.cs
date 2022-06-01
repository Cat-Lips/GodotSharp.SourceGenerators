using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.SceneTreeExtensions
{
    internal static class SceneTreeScraper
    {
        private enum Phase
        {
            NodeScan,
            ScriptScan,
            ResourceScan,
            ScanToNextNode,
        }

        private const string NodeRegexStr = @"^\[node name=""(?<Name>.*?)""( type=""(?<Type>.*?)"")?( parent=""(?<Parent>.*?)"")?( index=""(?<Index>.*?)"")?( instance=ExtResource\( (?<Id>\d*))?";
        private const string ScriptRegexStr = @"^script = ExtResource\( (?<Id>\d*)";
        private const string ResourceRegexStr = @"^\[ext_resource path=""res:/(?<Path>.*)"" type=""(?<Type>.*)"" id=(?<Id>\d*)";

        private static readonly Regex NodeRegex = new(NodeRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static readonly Regex ScriptRegex = new(ScriptRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static readonly Regex ResourceRegex = new(ResourceRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        private static readonly Dictionary<string, Tree<SceneTreeNode>> sceneTreeCache = new();
        private static string _resPath = null;

        public static Tree<SceneTreeNode> GetNodes(Compilation compilation, string tscnFile, bool traverseInstancedScenes)
        {
            Log.Debug();
            tscnFile = tscnFile.Replace("\\", "/");
            Log.Debug($"Scraping {tscnFile} [CacheCount: {sceneTreeCache.Count}]");

            var phase = Phase.ResourceScan;
            var resources = new Dictionary<string, string>();

            var first = true;
            SceneTreeNode curNode = null;
            Tree<SceneTreeNode> sceneTree = null;
            Dictionary<string, TreeNode<SceneTreeNode>> nodeLookup = new();

            foreach (var line in File.ReadLines(tscnFile).Skip(2))
            {
                Log.Debug($"Line: {line}");

                if (first)
                {
                    first = false;
                    if (line.StartsWith("[node"))
                        phase = Phase.NodeScan;
                }
                else if (line is "")
                {
                    phase = Phase.NodeScan;
                    continue;
                }

                Match match = null;

                switch (phase)
                {
                    case Phase.NodeScan: NodeScan(); break;
                    case Phase.ScriptScan: ScriptScan(); break;
                    case Phase.ResourceScan: ResourceScan(); break;
                }

                void NodeScan()
                {
                    match = NodeRegex.Match(line);
                    if (match.Success)
                    {
                        Log.Debug($"Matched Node: {NodeRegex.GetGroupsAsStr(match)}");
                        var nodeName = match.Groups["Name"].Value;
                        var nodeType = match.Groups["Type"].Value;
                        var parentPath = match.Groups["Parent"].Value;
                        var resourceId = match.Groups["Id"].Value;

                        var nodePath = GetNodePath();
                        var safeNodeName = nodeName.Replace("-", "_");

                        AddNode(safeNodeName, nodePath);

                        phase = Phase.ScriptScan;

                        void AddNode(string nodeName, string nodePath)
                        {
                            if (IsRootNode())
                            {
                                if (HasResource()) // Inherited Scene
                                {
                                    var resource = GetResource();
                                    Log.Debug($" - InheritedScene: {resource}");
                                    if (!sceneTreeCache.TryGetValue(resource, out var parentScene))
                                        parentScene = GetNodes(compilation, resource, traverseInstancedScenes);

                                    parentScene.Traverse(x =>
                                    {
                                        if (x.IsRoot)
                                            AddNode(curNode = new SceneTreeNode(nodeName, x.Value.Type, x.Value.Path));
                                        else
                                            AddNode(new SceneTreeNode(x.Value.Name, x.Value.Type, x.Value.Path), x.Parent.IsRoot ? "." : x.Parent.Value.Path);
                                    });
                                }
                                else // Root Node (normal)
                                {
                                    AddNode(curNode = new SceneTreeNode(nodeName, nodeType, nodePath), parentPath);
                                    Log.Debug($" - RootNode: {curNode}");
                                }
                            }
                            else if (HasResource()) // Instanced Scene
                            {
                                var resource = GetResource();
                                Log.Debug($" - InstancedScene: {resource}");
                                if (!sceneTreeCache.TryGetValue(resource, out var instancedScene))
                                    instancedScene = GetNodes(compilation, resource, traverseInstancedScenes);

                                if (traverseInstancedScenes)
                                {
                                    instancedScene.Traverse(x =>
                                    {
                                        if (x.IsRoot)
                                            AddNode(curNode = new SceneTreeNode(nodeName, x.Value.Type, nodePath), parentPath);
                                        else
                                            AddNode(new SceneTreeNode(x.Value.Name, x.Value.Type, $"{nodePath}/{x.Value.Path}"), x.Parent.IsRoot ? nodePath : $"{nodePath}/{x.Parent.Value.Path}");
                                    });
                                }
                                else
                                {
                                    AddNode(curNode = new SceneTreeNode(nodeName, instancedScene.Value.Type, nodePath), parentPath);
                                }
                            }
                            else if (nodeType is "") // Inherited Node (already added, potentially modified)
                            {
                                curNode = nodeLookup[nodePath].Value;
                                Log.Debug($" - InheritedNode: {curNode}");
                            }
                            else // Node (normal)
                            {
                                AddNode(curNode = new SceneTreeNode(nodeName, nodeType, nodePath), parentPath);
                                Log.Debug($" - Node: {curNode}");
                            }

                            void AddNode(SceneTreeNode node, string parentPath = null)
                            {
                                if (sceneTree is null)
                                    nodeLookup.Add(".", sceneTree = new(node));
                                else
                                    nodeLookup.Add(node.Path, nodeLookup[parentPath].Add(node));
                            }
                        }

                        bool IsRootNode()
                            => parentPath is "";

                        bool IsChildNode()
                            => parentPath is ".";

                        bool HasResource()
                            => resourceId is not "";

                        string GetResource()
                        {
                            var resource = resources[resourceId];
                            return GetResPath(resource) + resource;

                            string GetResPath(string resource)
                            {
                                return _resPath is null || !tscnFile.StartsWith(_resPath)
                                    ? _resPath = TryGetFromSceneCache() ?? TryGetFromFileSystem() : _resPath;

                                string TryGetFromSceneCache()
                                    => sceneTreeCache.Keys.FirstOrDefault(x => x.EndsWith(resource))?[..^resource.Length];

                                string TryGetFromFileSystem()
                                {
                                    const string GodotProjectFile = "project.godot";
                                    var tscnFolder = Path.GetDirectoryName(tscnFile);

                                    while (tscnFolder is not null)
                                    {
                                        if (File.Exists($"{tscnFolder}/{GodotProjectFile}"))
                                            return tscnFolder;

                                        tscnFolder = Path.GetDirectoryName(tscnFolder);
                                    }

                                    throw new Exception($"Could not find {GodotProjectFile} in path {Path.GetDirectoryName(tscnFile)}");
                                }
                            }
                        }

                        string GetNodePath()
                            => IsRootNode() ? "" : IsChildNode() ? nodeName : $"{parentPath}/{nodeName}";
                    }
                }

                void ScriptScan()
                {
                    match = ScriptRegex.Match(line);
                    if (match.Success)
                    {
                        Log.Debug($"Matched Script: {ScriptRegex.GetGroupsAsStr(match)}");
                        var resource = resources[match.Groups["Id"].Value];
                        var name = Path.GetFileNameWithoutExtension(resource);
                        curNode.Type = compilation.GetFullName(name);
                        Log.Debug($" - {curNode}");

                        phase = Phase.ScanToNextNode;
                    }
                }

                void ResourceScan()
                {
                    match = ResourceRegex.Match(line);
                    if (match.Success && match.Groups["Type"].Value is "Script" or "PackedScene")
                    {
                        Log.Debug($"Matched Resource: {ResourceRegex.GetGroupsAsStr(match)}");
                        resources.Add(match.Groups["Id"].Value, match.Groups["Path"].Value);
                    }
                }
            }

            sceneTreeCache[tscnFile] = sceneTree;
            return sceneTree;
        }
    }
}
