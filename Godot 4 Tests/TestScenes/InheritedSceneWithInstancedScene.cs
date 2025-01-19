using System.Linq;
using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
internal partial class InheritedSceneWithInstancedScene : RootScene, ITest
{
    void ITest.InitTests()
    {
        // InstancedScene.InitTests
        _.InstancedScene.Should().BeOfType<RootScene>().And.NotBeNull();

        // InheritedScene.InitTests
        _.Local_Layout.Label1.Text.Should().Be("Label1.Local");
        _.Local_Layout.Label2.Text.Should().Be("Label2.Local");

        _.Local_Layout.Get().Should().Be(GetNode("Local-Layout"));

        // RootScene.InitTests
        _.Label1.Text.Should().Be("Label1.0");
        _.Label2.Text.Should().Be("Label2.0");
        _.Layout.Label1.Text.Should().Be("Label1.1");
        _.Layout.Label2.Text.Should().Be("Label2.1");
        _.Layout.Layout.Label1.Text.Should().Be("Label1.2");
        _.Layout.Layout.Label2.Text.Should().Be("Label2.2");
        _.Layout.Layout.Layout.Label1.Text.Should().Be("Label1.3");
        _.Layout.Layout.Layout.Label2.Text.Should().Be("Label2.3");

        _.Layout.Get().Should().Be(GetNode("Layout"));
        _.Layout.Layout.Get().Should().Be(GetNode("Layout/Layout"));
        _.Layout.Layout.Layout.Get().Should().Be(GetNode("Layout/Layout/Layout"));

        _.GetType().GetProperties().Select(x => x.Name)
            .Should().HaveCount(5)
            .And.BeEquivalentTo(new[] { "Label1", "Label2", "Layout", "Local_Layout", "InstancedScene" });
    }
}
