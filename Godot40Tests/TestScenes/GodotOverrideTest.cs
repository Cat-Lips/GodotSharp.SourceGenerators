using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes
{
    [SceneTree]
    internal abstract partial class GodotOverrideTest : Node, ITest
    {
        private bool onEnterTree;
        private bool onReady;
        private bool onProcess;
        private bool onExitTree;

        public int RequiredFrames => 1;

        [GodotOverride]
        private void OnEnterTree()
            => onEnterTree = true;

        [GodotOverride]
        private void OnReady()
            => onReady = true;

        [GodotOverride]
        private void OnProcess(double _)
            => onProcess = true;

        [GodotOverride]
        private void OnExitTree()
            => onExitTree = true;

        [GodotOverride] private Variant OnGet(StringName property) => base._Get(property);
        [GodotOverride] private bool OnSet(StringName property, Variant value) => base._Set(property, value);
        [GodotOverride] private Godot.Collections.Array<Godot.Collections.Dictionary> OnGetPropertyList() => base._GetPropertyList();

        void ITest.InitTests() => RunInitTest();
        void ITest.EnterTests() => RunEnterTest();
        void ITest.ReadyTests() => RunReadyTest();
        void ITest.ProcessTests() => RunProcessTest();
        void ITest.ExitTests() => RunExitTest();

        protected virtual void RunInitTest()
        {
            onEnterTree.Should().BeFalse();
            onReady.Should().BeFalse();
            onProcess.Should().BeFalse();
            onExitTree.Should().BeFalse();
        }

        protected virtual void RunEnterTest(bool expected = true)
        {
            onEnterTree.Should().Be(expected); onEnterTree = false;
            onReady.Should().BeFalse();
            onProcess.Should().BeFalse();
            onExitTree.Should().BeFalse();
        }

        protected virtual void RunReadyTest(bool expected = true)
        {
            onEnterTree.Should().BeFalse();
            onReady.Should().Be(expected); onReady = false;
            onProcess.Should().BeFalse();
            onExitTree.Should().BeFalse();
        }

        protected virtual void RunProcessTest(bool expected = true)
        {
            onEnterTree.Should().BeFalse();
            onReady.Should().BeFalse();
            onProcess.Should().Be(expected); onProcess = false;
            onExitTree.Should().BeFalse();
        }

        protected virtual void RunExitTest(bool expected = true)
        {
            onEnterTree.Should().BeFalse();
            onReady.Should().BeFalse();
            onProcess.Should().BeFalse();
            onExitTree.Should().Be(expected); onExitTree = false;
        }
    }
}
