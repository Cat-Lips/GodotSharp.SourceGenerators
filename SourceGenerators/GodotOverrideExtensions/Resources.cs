using System.Reflection;

namespace GodotSharp.SourceGenerators.GodotOverrideExtensions;

internal static class Resources
{
    private const string godotOverrideTemplate = "GodotSharp.SourceGenerators.GodotOverrideExtensions.GodotOverrideTemplate.scriban";
    public static readonly string GodotOverrideTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(godotOverrideTemplate);
}
