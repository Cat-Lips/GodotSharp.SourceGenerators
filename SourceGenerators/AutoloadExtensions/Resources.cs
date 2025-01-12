using System.Reflection;

namespace GodotSharp.SourceGenerators.AutoloadExtensions;

internal static class Resources
{
    private const string autoloadTemplate = "GodotSharp.SourceGenerators.AutoloadExtensions.AutoloadTemplate.sbncs";
    public static readonly string AutoloadTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(autoloadTemplate);

    public static readonly string AutoloadExtensions = @"

#if GODOT

namespace Godot;

[Autoload]
public static partial class Autoload
{
}

#endif

".Trim();
}
