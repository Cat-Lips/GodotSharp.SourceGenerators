using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
internal abstract partial class InstancingSceneWithoutScript : Control, ITest
{
	void ITest.InitTests()
	{
		_.SceneWithoutScript1.GetScript().Should().BeNull();
		_.SceneWithoutScript1.Should().Be(GetNode("SceneWithoutScript1"));

		_.SceneWithoutScript2.GetScript().Should().BeNull();
		_.SceneWithoutScript2.Should().Be(GetNode("SceneWithoutScript2"));
	}
}
