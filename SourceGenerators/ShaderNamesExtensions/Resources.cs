using System.Reflection;

namespace GodotSharp.SourceGenerators.ShaderNamesExtensions;

internal static class Resources
{
    private const string shaderNamesTemplate = "GodotSharp.SourceGenerators.ShaderNamesExtensions.ShaderNamesTemplate.scriban";
    public static readonly string ShaderNamesTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(shaderNamesTemplate);
}
