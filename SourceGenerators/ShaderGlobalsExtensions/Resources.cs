using System.Reflection;

namespace GodotSharp.SourceGenerators.ShaderGlobalsExtensions;

internal static class Resources
{
    private const string shaderGlobalsTemplate = "GodotSharp.SourceGenerators.ShaderGlobalsExtensions.ShaderGlobalsTemplate.scriban";
    public static readonly string ShaderGlobalsTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(shaderGlobalsTemplate);
}
