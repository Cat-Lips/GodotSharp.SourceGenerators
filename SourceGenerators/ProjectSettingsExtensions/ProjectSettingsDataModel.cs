using Godot;
using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.ProjectSettingsExtensions;

internal class ProjectSettingsDataModel(INamedTypeSymbol symbol, ProjectSettingsAttribute cfg) : ClassDataModel(symbol)
{
    public bool Gravity { get; } = cfg.Gravity is not Generate.None;
    public bool GravityGet2D { get; } = cfg.Gravity.HasFlag(Generate.Get2D);
    public bool GravitySet2D { get; } = cfg.Gravity.HasFlag(Generate.Set2D);
    public bool GravityGet3D { get; } = cfg.Gravity.HasFlag(Generate.Get3D);
    public bool GravitySet3D { get; } = cfg.Gravity.HasFlag(Generate.Set3D);

    protected override string Str()
    {
        return string.Join(", ", Options());

        IEnumerable<string> Options()
        {
            yield return $"Gravity: {cfg.Gravity} [{string.Join(", ", GravityOptions())}]";

            IEnumerable<string> GravityOptions()
            {
                if (Gravity) yield return nameof(Gravity);
                if (GravityGet2D) yield return nameof(GravityGet2D);
                if (GravitySet2D) yield return nameof(GravitySet2D);
                if (GravityGet3D) yield return nameof(GravityGet3D);
                if (GravitySet3D) yield return nameof(GravitySet3D);
            }
        }
    }
}
