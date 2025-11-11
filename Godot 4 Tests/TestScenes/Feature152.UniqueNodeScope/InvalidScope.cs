using Godot;
using GodotSharp.SourceGenerators;

namespace GodotTests.TestScenes;

[SceneTree(uqScope: Scope.None)]
public partial class InvalidScope : Node
{
    //public partial Node Node1 { get; }
    private partial Node Node2 { get; }
    protected partial Node Node3 { get; }
}
