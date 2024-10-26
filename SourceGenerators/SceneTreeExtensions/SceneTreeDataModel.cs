using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.SceneTreeExtensions
{
    internal class SceneTreeDataModel : ClassDataModel
    {
        public Tree<SceneTreeNode> SceneTree { get; }
        public List<SceneTreeNode> UniqueNodes { get; }
        public List<string> ScriptNodePaths { get; }
        public string TscnFile { get; }

        public SceneTreeDataModel(Compilation compilation, INamedTypeSymbol symbol, string tscnFile,
            bool traverseInstancedScenes) : base(symbol)
        {
            TscnFile = tscnFile;
            (SceneTree, UniqueNodes) =
                SceneTreeScraper.GetNodes(compilation, tscnFile, traverseInstancedScenes);

            var symbolName = symbol.FullName();
            ScriptNodePaths = SceneTree.Where(n => n.Value.Type == symbolName)
                .Select(n => n.Value.Path == string.Empty ? "." : n.Value.Path)
                .ToList();
        }

        protected override string Str()
        {
            return string.Join("\n", Parts());

            IEnumerable<string> Parts()
            {
                yield return SceneTree.ToString().TrimEnd();

                if (UniqueNodes.Any())
                    yield return $"\nUniqueNodes:\n - {string.Join("\n - ", UniqueNodes)}";

                yield return $"\nScriptNodePaths:\n - {string.Join("\n - ", ScriptNodePaths)}";
            }
        }
    }
}
