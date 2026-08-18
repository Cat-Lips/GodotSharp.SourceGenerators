using System.Reflection;

namespace GodotSharp.SourceGenerators.CancellationSourceExtensions;

internal static class Resources
{
    private const string cancellationSourceTemplate = "GodotSharp.SourceGenerators.CancellationSourceExtensions.CancellationSourceTemplate.scriban";
    public static readonly string CancellationSourceTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(cancellationSourceTemplate);
}
