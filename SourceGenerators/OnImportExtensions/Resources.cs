using System.Reflection;

namespace GodotSharp.SourceGenerators.OnImportExtensions
{
    internal static class Resources
    {
        private const string onImportTemplate = "GodotSharp.SourceGenerators.OnImportExtensions.OnImportTemplate.sbncs";
        public static readonly string OnImportTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(onImportTemplate);
    }
}
