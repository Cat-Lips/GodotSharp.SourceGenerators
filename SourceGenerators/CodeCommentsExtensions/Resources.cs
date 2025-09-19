using System.Reflection;

namespace GodotSharp.SourceGenerators.CodeCommentsExtensions;

internal static class Resources
{
    private const string codeCommentsTemplate = "GodotSharp.SourceGenerators.CodeCommentsExtensions.CodeCommentsTemplate.scriban";
    public static readonly string CodeCommentsTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(codeCommentsTemplate);
}
