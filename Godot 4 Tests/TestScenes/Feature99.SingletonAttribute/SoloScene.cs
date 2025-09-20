using FluentAssertions;
using Godot;

namespace GodotTests.TestScenes;

[Singleton, SceneTree]
public partial class SoloScene : Node
{
    [GodotOverride]
    private void OnReady()
    {
        NestedNode.Should().NotBeNull();
        _.MainNode.NestedNode.Should().NotBeNull();
    }

    public override partial void _Ready();
}
