using System.Reflection;

namespace GodotSharp.SourceGenerators.OnInstantiateExtensions
{
    internal static class Resources
    {
        private const string onInstantiateTemplate = "GodotSharp.SourceGenerators.OnInstantiateExtensions.OnInstantiateTemplate.sbncs";
        public static readonly string OnInstantiateTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(onInstantiateTemplate);
    }
}
