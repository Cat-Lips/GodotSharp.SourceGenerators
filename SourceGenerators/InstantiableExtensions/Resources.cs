using System.Reflection;

namespace GodotSharp.SourceGenerators.InstantiableExtensions;

internal static class Resources
{
    private const string instantiableTemplate = "GodotSharp.SourceGenerators.InstantiableExtensions.InstantiableTemplate.scriban";
    public static readonly string InstantiableTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(instantiableTemplate);
}
