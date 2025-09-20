using System.Reflection;

namespace GodotSharp.SourceGenerators.SingletonExtensions;

internal static class Resources
{
    private const string singletonTemplate = "GodotSharp.SourceGenerators.SingletonExtensions.SingletonTemplate.scriban";
    public static readonly string SingletonTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(singletonTemplate);
}
