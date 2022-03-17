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
        }

        private const string NodeRegexStr = @"^\[node name=""(?<Name>.*)"" type=""(?<Type>.*)"" parent=""(?<Parent>.*?)""";
        private const string ScriptRegexStr = @"^script = ExtResource\( (?<Id>\d*)";
        private const string ResourceRegexStr = @"^\[ext_resource path=""(?<Path>.*)"" type=""(?<Type>.*)"" id=(?<Id>\d*)";
        private const string SceneInstanceRegexStr = @"^\[node name=""(?<Name>.*)"" parent=""(?<Parent>.*?)"" instance=ExtResource\( (?<Id>\d*)";

        private static readonly Regex NodeRegex = new(NodeRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static readonly Regex ScriptRegex = new(ScriptRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static readonly Regex ResourceRegex = new(ResourceRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static readonly Regex SceneInstanceRegex = new(SceneInstanceRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        public static ICollection<SceneTreeNode> GetNodes(Compilation compilation, string tscnFile)
        {
            Log.Debug();
            Log.Debug($"Scraping {tscnFile}");

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
                Log.Debug($"Line: {line}");

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
                        AddNode(match.Groups["Type"].Value);
                        return;
                    }

                    match = SceneInstanceRegex.Match(line);
                    if (match.Success)
                    {
                        Log.Debug($"Matched SceneInstance: {SceneInstanceRegex.GetGroupsAsStr(match)}");
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

                    match = ScriptRegex.Match(line);
                    if (match.Success)
                    {
                        Log.Debug($"Matched Script: {ScriptRegex.GetGroupsAsStr(match)}");
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

            return sceneTree.Children;
        }
    }
}
