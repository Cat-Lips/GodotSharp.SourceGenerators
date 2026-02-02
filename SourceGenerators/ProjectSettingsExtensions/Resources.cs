using System.Reflection;

namespace GodotSharp.SourceGenerators.ProjectSettingsExtensions;

internal static class Resources
{
    private const string projectSettingsTemplate = "GodotSharp.SourceGenerators.ProjectSettingsExtensions.ProjectSettingsTemplate.scriban";
    public static readonly string ProjectSettingsTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(projectSettingsTemplate);
}
