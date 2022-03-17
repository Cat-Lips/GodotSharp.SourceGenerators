using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes
{
    [SceneTree]
    internal abstract partial class InheritedScene : RootScene, ITest
    {
        void ITest.InitTests()
        {
            _.Local_Layout.Label1.Text.Should().Be("Label1.Local");
            _.Local_Layout.Label2.Text.Should().Be("Label2.Local");

            _.Local_Layout.Get().Should().Be(GetNode("Local-Layout"));

            // Inheritance should not expose private `_` of parent
            //var t = base._; // Should not compile
            _.GetType().GetProperties().Select(x => x.Name)
                .Should().HaveCount(1)
                .And.BeEquivalentTo(new[] { "Local_Layout" });
        }
    }
}
