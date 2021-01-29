using FluentAssertions;
using Godot;
using GodotTests.Utilities;

namespace GodotTests.TestScenes
{
    [SceneTree("SceneForScriptWithDifferentName.tscn")]
    public abstract partial class ScriptForSceneWithDifferentName : Control, ITest
    {
        public int RequiredFrames => 1;

        private bool GodotOverride_WithArgs_WasCalled;
        private bool GodotOverride_WithNoArgs_WasCalled;

        [GodotOverride] private void OnEnterTree() => GodotOverride_WithNoArgs_WasCalled = true;
        [GodotOverride] private void OnProcess(float _) => GodotOverride_WithArgs_WasCalled = true;

        void ITest.ProcessTests()
        {
            GodotOverride_WithArgs_WasCalled.Should().BeTrue();
            GodotOverride_WithNoArgs_WasCalled.Should().BeTrue();
        }

        void ITest.InitTests()
        {
            _.Layout._Label_1_.Text.Should().Be("Label1");
            _.Layout._Label_2_.Text.Should().Be("Label2");

            _.Layout.Get().Should().Be(GetNode("Layout"));
        }
    }
}
