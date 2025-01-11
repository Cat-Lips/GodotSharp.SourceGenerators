using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;
using GodotTests.TestScenes.Script;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class InstancedSceneFromDifferentNamespace : Control, ITest
{
    // Known Issue: tscn and script must have same name
    void ITest.InitTests()
        => _.InstancedScene.Should().BeOfType<SceneInDifferentNamespace>().And.NotBeNull();
}
