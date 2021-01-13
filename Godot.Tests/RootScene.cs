using FluentAssertions;
using Godot;
using GodotTests.Utilities;

namespace GodotTests
{
    [SceneTree]
    internal partial class RootScene : Control, ITest
    {
        public int RequiredFrames => 1;

        private bool GodotOverride_WithArgs_WasCalled;
        private bool GodotOverride_WithNoArgs_WasCalled;

        [GodotOverride] protected void OnEnterTree() => GodotOverride_WithNoArgs_WasCalled = true;
        [GodotOverride] protected void OnProcess(float _) => GodotOverride_WithArgs_WasCalled = true;

        void ITest.ProcessTests()
        {
            GodotOverride_WithArgs_WasCalled.Should().BeTrue();
            GodotOverride_WithNoArgs_WasCalled.Should().BeTrue();
        }

        void ITest.InitTests()
        {
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
        }
    }
}
