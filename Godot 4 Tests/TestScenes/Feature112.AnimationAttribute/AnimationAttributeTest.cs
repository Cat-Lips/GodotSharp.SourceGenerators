using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree, AnimNames]
public partial class AnimationAttributeTest : Node, ITest
{
    [AnimNames]
    private static partial class MyAnims;

    [AnimNames]
    private partial class NestedAnims;

    void ITest.InitTests()
    {
        TestEmbeddedAnimNames();
        TestExternalAnimNames();

        void TestEmbeddedAnimNames()
        {
            MyAnims.Idle1.Should().Be((StringName)"Idle1");
            MyAnims.Idle2.Should().Be((StringName)"Idle2");
            MyAnims.SharedAnim.Should().Be((StringName)"SharedAnim");

            AnimName.Idle1.Should().Be((StringName)"Idle1");
            AnimName.Idle2.Should().Be((StringName)"Idle2");
            AnimName.SharedAnim.Should().Be((StringName)"SharedAnim");

            NestedAnims.AnimName.Idle1.Should().Be((StringName)"Idle1");
            NestedAnims.AnimName.Idle2.Should().Be((StringName)"Idle2");
            NestedAnims.AnimName.SharedAnim.Should().Be((StringName)"SharedAnim");
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

            NestedAnimLib0.AnimName.Idle1.Should().Be((StringName)"Idle1");
            NestedAnimLib0.AnimName.Idle2.Should().Be((StringName)"Idle2");
            NestedAnimLib1.AnimName.Idle1.Should().Be((StringName)"Idle1");
            NestedAnimLib1.AnimName.Idle2.Should().Be((StringName)"Idle2");
            NestedAnimLib2.AnimName.SharedAnim.Should().Be((StringName)"SharedAnim");
            NestedSpriteFrames.AnimName.Idle1.Should().Be((StringName)"Idle1");
            NestedSpriteFrames.AnimName.Idle2.Should().Be((StringName)"Idle2");
        }
    }
}
