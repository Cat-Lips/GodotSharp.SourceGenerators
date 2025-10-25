using System.Reflection;

namespace GodotSharp.SourceGenerators.ResourceTreeExtensions;

internal static class Resources
{
    private const string resourceTreeTemplate = "GodotSharp.SourceGenerators.ResourceTreeExtensions.ResourceTreeTemplate.scriban";
    public static readonly string ResourceTreeTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(resourceTreeTemplate);
}
