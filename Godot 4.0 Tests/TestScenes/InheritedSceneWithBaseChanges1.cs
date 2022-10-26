using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes
{
    [SceneTree]
    internal partial class InheritedSceneWithBaseChanges1 : InheritedScene, ITest
    {
        void ITest.InitTests()
        {
            _.Label.Text.Should().Be("(new)");

            // InheritedScene.InitTests (with changes)
            _.Local_Layout.Label1.Text.Should().Be("Label1.Local");
            _.Local_Layout.Label2.Text.Should().Be("Label2.Local (modified)");
            _.Local_Layout.Label3.Text.Should().Be("Label3.Local (added)");

            _.Local_Layout.Get().Should().Be(GetNode("Local-Layout"));

            // RootScene.InitTests (with changes)
            _.Label1.Text.Should().Be("Label1.0");
            _.Label2.Text.Should().Be("Label2.0");
            _.Layout.Label1.Text.Should().Be("Label1.1");
            _.Layout.Label2.Text.Should().Be("Label2.1");
            _.Layout.Layout.Label1.GetType().Should().Be(typeof(InheritedSceneTypeOverride));
            _.Layout.Layout.Label2.Text.Should().Be("Label2.2");
            _.Layout.Layout.Layout.Label1.Text.Should().Be("Label1.3");
            _.Layout.Layout.Layout.Label2.Text.Should().Be("Label2.3 (modified)");
            _.Layout.Layout.Layout.Label3.Text.Should().Be("Label3.3 (added)");

            _.Layout.Get().Should().Be(GetNode("Layout"));
            _.Layout.Layout.Get().Should().Be(GetNode("Layout/Layout"));
            _.Layout.Layout.Layout.Get().Should().Be(GetNode("Layout/Layout/Layout"));

            _.GetType().GetProperties().Select(x => x.Name)
                .Should().HaveCount(5)
                .And.BeEquivalentTo(new[] { "Label1", "Label2", "Layout", "Local_Layout", "Label" });
        }
    }
}
