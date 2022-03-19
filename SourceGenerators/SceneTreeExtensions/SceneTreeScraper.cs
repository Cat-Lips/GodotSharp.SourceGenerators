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

        private const string NodeRegexStr = @"^\[node name=""(?<Name>.*)"" type=""(?<Type>.*)"" parent=""(?<Parent>.*?)""";
        private const string ScriptRegexStr = @"^script = ExtResource\( (?<Id>\d*)";
        private const string ResourceRegexStr = @"^\[ext_resource path=""(?<Path>.*)"" type=""(?<Type>.*)"" id=(?<Id>\d*)";
        private const string SceneInstanceRegexStr = @"^\[node name=""(?<Name>.*)"" parent=""(?<Parent>.*?)"" instance=ExtResource\( (?<Id>\d*)";

        private static readonly Regex NodeRegex = new(NodeRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static readonly Regex ScriptRegex = new(ScriptRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static readonly Regex ResourceRegex = new(ResourceRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static readonly Regex SceneInstanceRegex = new(SceneInstanceRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        public static Tree<SceneTreeNode> GetNodes(Compilation compilation, string tscnFile)
        {
            Log.Debug();
            Log.Debug($"Scraping {tscnFile}");

            var phase = Phase.ResourceScan;
            var resourceNames = new Dictionary<string, string>();

            SceneTreeNode curNode = null;
            Tree<SceneTreeNode> sceneTree = new();
            Dictionary<string, TreeNode<SceneTreeNode>> nodeLookup = new();
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
                    if (MatchNode())
                    {
                        var nodeName = match.Groups["Name"].Value;
                        var nodeType = match.Groups["Type"].Value;
                        var parentPath = match.Groups["Parent"].Value;
                        var resourceId = match.Groups["Id"].Value;

                        var nodePath = GetNodePath(parentPath, nodeName);
                        SetNodeType(ref nodeType);
                        AddNode(out curNode);

                        phase = Phase.ScriptScan;

                        void SetNodeType(ref string nodeType)
                        {
                            if (resourceId is not "")
                            {
                                var resourceName = resourceNames[resourceId];
                                nodeType = compilation.GetFullName(resourceName); // Assumes type name == scene name
                                Log.Debug($" - InstancedScene: {nodePath} ({nodeType})");
                            }
                        }

                        void AddNode(out SceneTreeNode curNode)
                        {
                            curNode = new(nodeName.Replace("-", "_"), nodeType, nodePath);

                            if (parentPath is ".")
                            {
                                var treeNode = new TreeNode<SceneTreeNode>(curNode, null);
                                nodeLookup.Add(nodePath, treeNode);
                                sceneTree.Nodes.Add(treeNode);
                            }
                            else
                            {
                                var treeNode = nodeLookup[parentPath].Add(curNode);
                                nodeLookup.Add(nodePath, treeNode);
                            }
                        }

                        string GetNodePath(string parentPath, string nodeName)
                            => parentPath is "." ? nodeName : $"{parentPath}/{nodeName}";
                    }

                    bool MatchNode()
                    {
                        match = NodeRegex.Match(line);
                        if (match.Success)
                        {
                            Log.Debug($"Matched Node: {NodeRegex.GetGroupsAsStr(match)}");
                            return true;
                        }

                        match = SceneInstanceRegex.Match(line);
                        if (match.Success)
                        {
                            Log.Debug($"Matched Node: {SceneInstanceRegex.GetGroupsAsStr(match)}");
                            return true;
                        }

                        match = null;
                        return false;
                    }
                }

                void ScriptScan()
                {
                    match = ScriptRegex.Match(line);
                    if (match.Success)
                    {
                        Log.Debug($"Matched Script: {ScriptRegex.GetGroupsAsStr(match)}");
                        curNode.Type = compilation.GetFullName(resourceNames[match.Groups["Id"].Value]);
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
                        var resourceName = Path.GetFileNameWithoutExtension(match.Groups["Path"].Value);
                        resourceNames.Add(match.Groups["Id"].Value, resourceName);
                    }
                }
            }

            return sceneTree;
        }
    }
}
