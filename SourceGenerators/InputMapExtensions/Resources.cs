using System.Reflection;

namespace GodotSharp.SourceGenerators.InputMapExtensions;

internal static class Resources
{
    private const string inputMapTemplate = "GodotSharp.SourceGenerators.InputMapExtensions.InputMapTemplate.scriban";
    public static readonly string InputMapTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(inputMapTemplate);
}
