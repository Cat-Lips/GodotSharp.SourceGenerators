using System.Reflection;

namespace GodotSharp.SourceGenerators.SceneTreeExtensions
{
    internal static class Resources
    {
        private const string sceneTreeTemplate = "GodotSharp.SourceGenerators.SceneTreeExtensions.SceneTreeTemplate.sbncs";
        private const string sceneTreeNodeTemplate = "GodotSharp.SourceGenerators.SceneTreeExtensions.SceneTreeNodeTemplate.sbncs";
        public static readonly string SceneTreeTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(sceneTreeTemplate);
        public static readonly string SceneTreeNodeTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(sceneTreeNodeTemplate);
    }
}
