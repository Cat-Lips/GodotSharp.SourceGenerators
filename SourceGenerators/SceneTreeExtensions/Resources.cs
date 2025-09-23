using System.Reflection;

namespace GodotSharp.SourceGenerators.SceneTreeExtensions;

internal static class Resources
{
    private const string sceneTreeTemplate = "GodotSharp.SourceGenerators.SceneTreeExtensions.SceneTreeTemplate.scriban";
    public static readonly string SceneTreeTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(sceneTreeTemplate);

    public static readonly string ITscnFilePath = @"
#if GODOT4_OR_GREATER
namespace Godot;

public interface ITscnFilePath
{
    static abstract string TscnFilePath { get; }
}
#endif".Trim();
}
