using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.SceneTreeExtensions
{
    internal class SceneTreeDataModel
    {
        public string ClassName { get; }
        public string Namespace { get; }
        public Tree<SceneTreeNode> SceneTree { get; }
        public List<SceneTreeNode> UniqueNodes { get; }

        public SceneTreeDataModel(
            Compilation compilation,
            CodeDependency.SceneTree sceneTree)
        {
            ClassName = sceneTree.ClassName;
            Namespace = sceneTree.Namespace;
            (SceneTree, UniqueNodes) = SceneTreeScraper.GetNodes(
                compilation,
                sceneTree.TscnFileName,
                sceneTree.TraverseInstancedScenes);
        }

        public override string ToString()
        {
            return string.Join("\n", Parts());

            IEnumerable<string> Parts()
            {
                yield return $"SceneTree: {SceneTree.ToString().TrimEnd()}";
                yield return $"Classname: {ClassName}";
                yield return $"Namespace: {Namespace}";

                if (UniqueNodes.Any())
                    yield return $"\nUniqueNodes:\n - {string.Join("\n - ", UniqueNodes)}";
            }
        }
    }
}
