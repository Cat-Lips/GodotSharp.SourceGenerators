using Godot;
using GodotSharp.SourceGenerators;

namespace GodotTests.TestScenes;

[SceneTree(uqScope: Scope.Protected)]
public partial class CustomScope : Node
{
    public partial Node Node1 { get; }
    private partial Node Node2 { get; }
    //protected partial Node Node3 { get; }
}
