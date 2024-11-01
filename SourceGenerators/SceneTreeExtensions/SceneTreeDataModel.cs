using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.SceneTreeExtensions
{
    internal class SceneTreeDataModel
    {
        public string SceneTreeClassName { get; }
        public Tree<SceneTreeNode> SceneTree { get; }
        public List<SceneTreeNode> UniqueNodes { get; }

        public SceneTreeDataModel(
            Compilation compilation,
            string tscnFile,
            bool traverseInstancedScenes)
        {
            SceneTreeClassName = tscnFile;
            (SceneTree, UniqueNodes) = SceneTreeScraper.GetNodes(compilation, tscnFile, traverseInstancedScenes);
        }

        public override string ToString()
        {
            return string.Join("\n", Parts());

            IEnumerable<string> Parts()
            {
                yield return SceneTree.ToString().TrimEnd();
                yield return $"SceneTreeClassname: {SceneTreeClassName}";

                if (UniqueNodes.Any())
                    yield return $"\nUniqueNodes:\n - {string.Join("\n - ", UniqueNodes)}";
            }
        }
    }
}
