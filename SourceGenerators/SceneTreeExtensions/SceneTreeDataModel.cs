using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.SceneTreeExtensions
{
    internal class SceneTreeDataModel : ClassDataModel
    {
        public Tree<SceneTreeNode> SceneTree { get; }
        public List<SceneTreeNode> UniqueNodes { get; }

        public SceneTreeDataModel(Compilation compilation, INamedTypeSymbol symbol, string tscnFile, bool traverseInstancedScenes) : base(symbol)
            => (SceneTree, UniqueNodes) = SceneTreeScraper.GetNodes(compilation, tscnFile, traverseInstancedScenes);

        protected override string Str()
        {
            return string.Join("\n", Parts());

            IEnumerable<string> Parts()
            {
                yield return SceneTree.ToString().TrimEnd();

                if (UniqueNodes.Any())
                    yield return $"\nUniqueNodes:\n - {string.Join("\n - ", UniqueNodes)}";
            }
        }
    }
}
