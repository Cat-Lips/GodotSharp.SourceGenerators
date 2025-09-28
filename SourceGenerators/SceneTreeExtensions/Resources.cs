using System.Reflection;

namespace GodotSharp.SourceGenerators.SceneTreeExtensions;

internal static class Resources
{
    private const string sceneTreeTemplate = "GodotSharp.SourceGenerators.SceneTreeExtensions.SceneTreeTemplate.scriban";
    public static readonly string SceneTreeTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(sceneTreeTemplate);

    public static readonly string ISceneTree = @"
#if GODOT4_OR_GREATER
namespace Godot;

public partial interface ISceneTree
{
    static abstract string TscnFilePath { get; }
}
#endif".Trim();

    public static readonly string IInstantiable = @"
#if GODOT4_OR_GREATER
namespace Godot;

public partial interface IInstantiable<T> where T : class, IInstantiable<T>, ISceneTree
{
    static T Instantiate() => GD.Load<PackedScene>(T.TscnFilePath).Instantiate<T>();
}

public partial interface IInstantiable
{
    static T Instantiate<T>() where T : class, ISceneTree
        => GD.Load<PackedScene>(T.TscnFilePath).Instantiate<T>();
}

public static partial class Instantiator
{
    public static T Instantiate<T>() where T : class, ISceneTree
        => GD.Load<PackedScene>(T.TscnFilePath).Instantiate<T>();
}
#endif".Trim();
}
