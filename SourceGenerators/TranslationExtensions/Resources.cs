using System.Reflection;

namespace GodotSharp.SourceGenerators.TranslationExtensions;

internal static class Resources
{
    private const string translationTemplate = "GodotSharp.SourceGenerators.TranslationExtensions.TranslationTemplate.scriban";
    public static readonly string TranslationTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(translationTemplate);
}
