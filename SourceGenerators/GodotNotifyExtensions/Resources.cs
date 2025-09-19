using System.Reflection;

namespace GodotSharp.SourceGenerators.GodotNotifyExtensions;

internal static class Resources
{
    private const string godotNotifyTemplate = "GodotSharp.SourceGenerators.GodotNotifyExtensions.GodotNotifyTemplate.scriban";
    public static readonly string GodotNotifyTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(godotNotifyTemplate);
}
