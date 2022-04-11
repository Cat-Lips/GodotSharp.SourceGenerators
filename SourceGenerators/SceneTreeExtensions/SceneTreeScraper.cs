using System.Diagnostics;
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
        private static string resPath;

        public static Tree<SceneTreeNode> GetNodes(Compilation compilation, string tscnFile)
        {
            Log.Debug();
            Log.Debug($"Scraping {tscnFile} [CacheCount: {sceneTreeCache.Count}]");

            var phase = Phase.ResourceScan;
            var resources = new Dictionary<string, string>();

            SceneTreeNode curNode = null;
            Tree<SceneTreeNode> sceneTree = new();
            Dictionary<string, TreeNode<SceneTreeNode>> nodeLookup = new();
            sceneTreeCache[tscnFile.Replace("\\", "/")] = sceneTree;

            foreach (var line in File.ReadLines(tscnFile).Skip(2))
            {
                Log.Debug($"Line: {line}");

                if (line is "")
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

                        if (IsRootNode())
                        {
                            phase = Phase.ScanToNextNode;
                            return;
                        }

                        var nodePath = GetNodePath(parentPath, nodeName);
                        GetNodeType(ref nodeType);
                        GetNode(out curNode);

                        phase = Phase.ScriptScan;

                        bool IsRootNode()
                        {
                            // Root nodes have no parent path
                            if (parentPath is not "")
                                return false;

                            // Scene inheritance has parent scene on root node
                            if (resourceId is not "")
                            {
                                var resource = resources[resourceId];
                                resPath ??= GetResPath(resource);
                                Log.Debug($" - InheritedScene: {resPath + resource}");
                                var parentScene = sceneTreeCache[resPath + resource];
                                parentScene.Traverse(CopyNode); // Full copy to accommodate changes

                                void CopyNode(TreeNode<SceneTreeNode> x)
                                {
                                    var parentPath = x.Parent?.Value.Path ?? ".";
                                    var node = new SceneTreeNode(x.Value.Name, x.Value.Type, x.Value.Path);

                                    AddNode(parentPath, node);
                                }
                            }

                            return true;
                        }

                        void GetNodeType(ref string nodeType)
                        {
                            // Instanced scene has resource id
                            if (resourceId is not "")
                            {
                                var resource = resources[resourceId];
                                var resourceName = Path.GetFileNameWithoutExtension(resource);
                                nodeType = compilation.GetFullName(resourceName); // Assumes type name == scene name
                                Log.Debug($" - InstancedScene: {nodePath} ({nodeType})");
                            }
                        }

                        void GetNode(out SceneTreeNode node)
                        {
                            // Inherited component has no type (only present if modified)
                            if (nodeType is "")
                            {
                                node = nodeLookup[nodePath].Value;
                                Log.Debug($" - InheritedNode: {node}");
                                return;
                            }

                            node = new(nodeName.Replace("-", "_"), nodeType, nodePath);
                            AddNode(parentPath, node);
                        }

                        void AddNode(string parentPath, SceneTreeNode node)
                        {
                            if (parentPath is ".")
                            {
                                var treeNode = new TreeNode<SceneTreeNode>(node, null);
                                nodeLookup.Add(node.Path, treeNode);
                                sceneTree.Nodes.Add(treeNode);
                            }
                            else
                            {
                                var treeNode = nodeLookup[parentPath].Add(node);
                                nodeLookup.Add(node.Path, treeNode);
                            }
                        }

                        string GetResPath(string resource)
                            => sceneTreeCache.Keys.Single(x => x.EndsWith(resource))[..^resource.Length];

                        static string GetNodePath(string parentPath, string nodeName)
                            => parentPath is "." ? nodeName : $"{parentPath}/{nodeName}";
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
                    Debug.Assert(match.Success);
                    if (match.Groups["Type"].Value is "Script" or "PackedScene")
                    {
                        Log.Debug($"Matched Resource: {ResourceRegex.GetGroupsAsStr(match)}");
                        resources.Add(match.Groups["Id"].Value, match.Groups["Path"].Value);
                    }
                }
            }

            return sceneTree;
        }
    }
}
