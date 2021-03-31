using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        }

        private const string NodeRegex = @"^\[node name=""(?<Name>.*)"" type=""(?<Type>.*)"" parent=""(?<Parent>.*?)""";
        private const string ScriptRegex = @"^script = ExtResource\( (?<Id>\d*)";
        private const string ResourceRegex = @"^\[ext_resource path=""(?<Path>.*)"" type=""(?<Type>.*)"" id=(?<Id>\d*)";
        private const string SceneInstanceRegex = @"^\[node name=""(?<Name>.*)"" parent=""(?<Parent>.*?)"" instance=ExtResource\( (?<Id>\d*)";

        public static ICollection<SceneTreeNode> GetNodes(Compilation compilation, string tscnFile)
        {
            // If present, use resource name as node type (script, scene)
            // Known Issue: For instanced scenes, scene and script must match
            //  (If required, will need to scrape instanced tscn to get script name)
            var resourceNames = new Dictionary<string, string>();

            // Scan resources (top of file) to get script/scene names
            var phase = Phase.ResourceScan;

            SceneTreeNode curNode = null;
            var sceneTree = new SceneTreeNode();
            foreach (var line in File.ReadLines(tscnFile).Skip(1).SkipWhile(x => x is "")) // Skip header
            {
                Match match = null;

                switch (phase)
                {
                    case Phase.NodeScan: NodeScan(); break;
                    case Phase.ScriptScan: ScriptScan(); break;
                    case Phase.ResourceScan: ResourceScan(); break;
                }

                void NodeScan()
                {
                    match = Regex.Match(line, NodeRegex, RegexOptions.Compiled);
                    if (match.Success)
                    {
                        AddNode(match.Groups["Type"].Value);
                        return;
                    }

                    match = Regex.Match(line, SceneInstanceRegex, RegexOptions.Compiled);
                    if (match.Success)
                    {
                        AddNode(compilation.GetFullName(resourceNames[match.Groups["Id"].Value]));
                        return;
                    }

                    void AddNode(string nodeType)
                    {
                        var nodeName = match.Groups["Name"].Value;
                        var parentPath = match.Groups["Parent"].Value;
                        var nodePath = parentPath is "." ? nodeName : $"{parentPath}/{nodeName}";
                        var nodeNames = nodePath.Replace("-", "_").Split('/');
                        curNode = sceneTree.Add(nodeNames, nodeType);
                        curNode.Path = nodePath;

                        // Once we've matched a node, scan to find script (to override type)
                        phase = Phase.ScriptScan;
                    }
                }

                void ScriptScan()
                {
                    if (line is "")
                    {
                        // End of node (no script) - Scan for next node
                        phase = Phase.NodeScan;
                        return;
                    }

                    match = Regex.Match(line, ScriptRegex, RegexOptions.Compiled);
                    if (match.Success)
                    {
                        curNode.Type = compilation.GetFullName(resourceNames[match.Groups["Id"].Value]);

                        // Found script (node type renamed) - Scan for next node
                        phase = Phase.NodeScan;
                    }
                }

                void ResourceScan()
                {
                    if (line is "")
                    {
                        // End of resources - Scan for nodes
                        phase = Phase.NodeScan;
                        return;
                    }

                    match = Regex.Match(line, ResourceRegex, RegexOptions.Compiled);
                    Debug.Assert(match.Success);
                    if (match.Groups["Type"].Value is "Script" or "PackedScene")
                    {
                        var resourceName = Path.GetFileNameWithoutExtension(match.Groups["Path"].Value);
                        resourceNames.Add(match.Groups["Id"].Value, resourceName);
                    }
                }
            }

            return sceneTree.Children;
        }
    }
}
