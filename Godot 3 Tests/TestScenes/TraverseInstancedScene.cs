using System.Linq;
using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree(traverseInstancedScenes: true)]
internal abstract partial class TraverseInstancedScene : Control, ITest
{
	void ITest.InitTests()
	{
		_.InstancedScene.Get().Should().BeOfType<RootScene>().And.NotBeNull();

		// RootScene.InitTests
		_.InstancedScene.Label1.Text.Should().Be("Label1.0");
		_.InstancedScene.Label2.Text.Should().Be("Label2.0");
		_.InstancedScene.Layout.Label1.Text.Should().Be("Label1.1");
		_.InstancedScene.Layout.Label2.Text.Should().Be("Label2.1");
		_.InstancedScene.Layout.Layout.Label1.Text.Should().Be("Label1.2");
		_.InstancedScene.Layout.Layout.Label2.Text.Should().Be("Label2.2");
		_.InstancedScene.Layout.Layout.Layout.Label1.Text.Should().Be("Label1.3");
		_.InstancedScene.Layout.Layout.Layout.Label2.Text.Should().Be("Label2.3");

		_.InstancedScene.Layout.Get().Should().Be(GetNode("InstancedScene/Layout"));
		_.InstancedScene.Layout.Layout.Get().Should().Be(GetNode("InstancedScene/Layout/Layout"));
		_.InstancedScene.Layout.Layout.Layout.Get().Should().Be(GetNode("InstancedScene/Layout/Layout/Layout"));

		_.InstancedScene.GetType().GetProperties().Select(x => x.Name)
			.Should().HaveCount(3)
			.And.BeEquivalentTo(new[] { "Label1", "Label2", "Layout" });
	}
}
