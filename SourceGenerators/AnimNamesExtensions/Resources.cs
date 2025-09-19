using System.Reflection;

namespace GodotSharp.SourceGenerators.AnimNamesExtensions;

internal static class Resources
{
    private const string animNamesTemplate = "GodotSharp.SourceGenerators.AnimNamesExtensions.AnimNamesTemplate.scriban";
    public static readonly string AnimNamesTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(animNamesTemplate);
}
