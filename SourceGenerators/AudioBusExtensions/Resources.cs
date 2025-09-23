using System.Reflection;

namespace GodotSharp.SourceGenerators.AudioBusExtensions;

internal static class Resources
{
    private const string audioBusTemplate = "GodotSharp.SourceGenerators.AudioBusExtensions.AudioBusTemplate.scriban";
    public static readonly string AudioBusTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(audioBusTemplate);
}
