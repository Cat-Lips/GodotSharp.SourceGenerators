using System.Reflection;

namespace GodotSharp.SourceGenerators.GlobalGroupsExtensions;

internal static class Resources
{
    private const string globalGroupsTemplate = "GodotSharp.SourceGenerators.GlobalGroupsExtensions.GlobalGroupsTemplate.sbncs";
    public static readonly string GlobalGroupsTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(globalGroupsTemplate);
}
