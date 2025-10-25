using System.Reflection;

namespace GodotSharp.SourceGenerators.AutoEnumExtensions;

internal static class Resources
{
    private const string autoEnumTemplate = "GodotSharp.SourceGenerators.AutoEnumExtensions.AutoEnumTemplate.scriban";
    public static readonly string AutoEnumTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(autoEnumTemplate);

    private const string autoEnumClassTemplate = "GodotSharp.SourceGenerators.AutoEnumExtensions.AutoEnumClassTemplate.scriban";
    public static readonly string AutoEnumClassTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(autoEnumClassTemplate);
}
