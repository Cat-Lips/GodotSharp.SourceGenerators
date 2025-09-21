using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class SingletonAttributeTests : Node, ITest
{
    [Singleton]
    private partial class SoloPrivate;

    void ITest.InitTests()
    {
        SoloData.Instance.Should().NotBeNull();
        SoloNode.Instance.Should().NotBeNull();
        SoloScene.Instance.Should().NotBeNull();
        SoloScene.Instance.NestedNode.Should().NotBeNull();
        SoloPrivate.Instance.Should().NotBeNull();
        SoloDataWithInit.Instance.Should().NotBeNull();
        SoloDataWithInit.Instance.InitCalled.Should().BeTrue();
        SoloNodeWithInitOverride.Instance.Should().NotBeNull();
        SoloNodeWithInitOverride.Instance.MyInitCalled.Should().BeTrue();

        // Should not compile
        //new SoloData();
        //new SoloNode();
        //new SoloScene();
        //new SoloPrivate();

        // Can't prevent this:
        var notSoloScene = (SoloScene)GD.Load<PackedScene>("res://TestScenes/Feature99.SingletonAttribute/SoloScene.tscn").Instantiate();
        notSoloScene.Should().NotBe(SoloScene.Instance);
    }
}
