using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree/*, AnimNames*/] // TODO
public partial class AnimationAttributeTest : Node, ITest
{
    void ITest.InitTests()
    {
        MySpriteFrames.Idle.Should().Be((StringName)"Idle");
        MyAnimLib0.Idle.Should().Be((StringName)"Idle");
        MyAnimLib1.Idle.Should().Be((StringName)"Idle");
        MyAnimLib2.SharedAnim.Should().Be((StringName)"SharedAnim");
    }
}
