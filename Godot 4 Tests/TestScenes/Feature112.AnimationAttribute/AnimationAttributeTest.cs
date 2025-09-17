using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class AnimationAttributeTest : Node, ITest
{
    [AnimNames]
    private static partial class MyAnims;

    void ITest.InitTests()
    {
        TestEmbeddedAnimNames();
        TestExternalAnimNames();

        void TestEmbeddedAnimNames()
        {
            MyAnims.Idle1.Should().Be((StringName)"Idle1");
            MyAnims.Idle2.Should().Be((StringName)"Idle2");
            MyAnims.SharedAnim.Should().Be((StringName)"SharedAnim");
        }

        void TestExternalAnimNames()
        {
            MyAnimLib0.Idle1.Should().Be((StringName)"Idle1");
            MyAnimLib0.Idle2.Should().Be((StringName)"Idle2");
            MyAnimLib1.Idle1.Should().Be((StringName)"Idle1");
            MyAnimLib1.Idle2.Should().Be((StringName)"Idle2");
            MyAnimLib2.SharedAnim.Should().Be((StringName)"SharedAnim");
            MySpriteFrames.Idle1.Should().Be((StringName)"Idle1");
            MySpriteFrames.Idle2.Should().Be((StringName)"Idle2");
        }
    }
}
