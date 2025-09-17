using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class AnimationAttributeTest : Node, ITest
{
    [AnimNames]
    private partial class Anim;

    void ITest.InitTests()
    {
        TestAnimPlayerWithEmbeddedAnimLibs();
        TestAnimPlayerWithExternalAnimLibs();
        TestAnimSpriteWithEmbeddedSpriteFrames();
        TestAnimSpriteWithExternalSpriteFrames();

        void TestAnimPlayerWithEmbeddedAnimLibs()
        {
            Anim.Idle.Should().Be((StringName)"Idle");
            Anim.SharedAnim.Should().Be((StringName)"SharedAnim");
        }

        void TestAnimPlayerWithExternalAnimLibs()
        {
            MyAnimLib0.Idle.Should().Be((StringName)"Idle");
            MyAnimLib1.Idle.Should().Be((StringName)"Idle");
            MyAnimLib2.SharedAnim.Should().Be((StringName)"SharedAnim");
        }

        void TestAnimSpriteWithEmbeddedSpriteFrames()
            => Anim.Idle.Should().Be((StringName)"Idle");

        void TestAnimSpriteWithExternalSpriteFrames()
            => MySpriteFrames.Idle.Should().Be((StringName)"Idle");
    }
}
