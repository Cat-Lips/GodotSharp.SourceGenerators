using System.Reflection;

namespace GodotSharp.SourceGenerators.GodotNotifyExtensions
{
    internal static class Resources
    {
        private const string godotNotifyTemplate = "GodotSharp.SourceGenerators.GodotNotifyExtensions.GodotNotifyTemplate.sbncs";
        public static readonly string GodotNotifyTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(godotNotifyTemplate);
    }
}
