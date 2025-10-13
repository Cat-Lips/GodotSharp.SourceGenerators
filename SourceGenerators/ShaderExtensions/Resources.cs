using System.Reflection;

namespace GodotSharp.SourceGenerators.ShaderExtensions;

internal static class Resources
{
    private const string shaderTemplate = "GodotSharp.SourceGenerators.ShaderExtensions.ShaderTemplate.scriban";
    public static readonly string ShaderTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(shaderTemplate);
}
