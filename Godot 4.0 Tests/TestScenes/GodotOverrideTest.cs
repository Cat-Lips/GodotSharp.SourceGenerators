using FluentAssertions;
using Godot;
using Godot.Collections;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes
{
    [SceneTree]
    internal partial class GodotOverrideTest : Node, ITest
    {
        public override partial void _EnterTree();
        public override partial void _Ready();
        [GodotOverride] public override partial void _Process(double _); // Can also be declared on partial override
        public override partial void _ExitTree();

        public override partial Variant _Get(StringName property);
        public override partial bool _Set(StringName property, Variant value);
        public override partial Array<Dictionary> _GetPropertyList();

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

        private void OnProcess(double _)
            => onProcess = true;

        [GodotOverride]
        private void OnExitTree()
            => onExitTree = true;

        [GodotOverride] private Variant OnGet(StringName property) => base._Get(property);
        [GodotOverride] private bool OnSet(StringName property, Variant value) => base._Set(property, value);
        [GodotOverride] private Array<Dictionary> OnGetPropertyList() => base._GetPropertyList();

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
