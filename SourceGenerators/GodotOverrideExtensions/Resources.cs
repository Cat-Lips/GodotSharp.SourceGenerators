using System.Reflection;

namespace GodotSharp.SourceGenerators.GodotOverrideExtensions;

internal static class Resources
{
    private const string godotOverrideTemplate = "GodotSharp.SourceGenerators.GodotOverrideExtensions.GodotOverrideTemplate.sbncs";
    public static readonly string GodotOverrideTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(godotOverrideTemplate);
}
