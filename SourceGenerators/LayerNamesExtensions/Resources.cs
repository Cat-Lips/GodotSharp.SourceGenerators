using System.Reflection;

namespace GodotSharp.SourceGenerators.LayerNamesExtensions;

internal static class Resources
{
    private const string layerNamesTemplate = "GodotSharp.SourceGenerators.LayerNamesExtensions.LayerNamesTemplate.sbncs";
    public static readonly string LayerNamesTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(layerNamesTemplate);
}
