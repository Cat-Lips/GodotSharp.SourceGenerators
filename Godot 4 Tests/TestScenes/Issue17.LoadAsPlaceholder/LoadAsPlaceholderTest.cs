using System.Reflection;
using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
internal partial class LoadAsPlaceholderTest : Control, ITest
{
    void ITest.InitTests()
    {
        TestScene.Should().BeOfType<TestScene>();
        PlaceholderScene.Should().BeOfType<InstancePlaceholder>();

        var testSceneResource = typeof(TestScene).GetCustomAttribute<SceneTreeAttribute>().SceneFile;
        var placeholderSceneResource = ProjectSettings.GlobalizePath(PlaceholderScene.GetInstancePath()).Replace("/", "\\");
        placeholderSceneResource.Should().Be(testSceneResource);
    }
}
