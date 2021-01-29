using FluentAssertions;
using Godot;
using GodotTests.Utilities;

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
        }
    }
}
