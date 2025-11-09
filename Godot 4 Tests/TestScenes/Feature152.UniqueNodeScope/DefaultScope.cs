using Godot;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class DefaultScope : Node
{
    //public partial Node Node1 { get; }
    private partial Node Node2 { get; }
    protected partial Node Node3 { get; }
}
