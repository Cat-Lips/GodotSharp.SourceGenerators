using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes
{
    [SceneTree]
    internal abstract partial class GodotOverrideInheritanceTest : GodotOverrideTest, ITest
    {
        private bool onEnterTree;
        private bool onReady;
        private bool onProcess;
        private bool onExitTree;

        [GodotOverride]
        private void OnEnterTree()
            => onEnterTree = true;

        [GodotOverride(true)]
        private void OnReady()
            => onReady = true;

        [GodotOverride(true)]
        private void OnProcess(double _)
            => onProcess = true;

        [GodotOverride]
        private void OnExitTree()
            => onExitTree = true;

        [GodotOverride] private Variant OnGet(StringName property) => base._Get(property);
        [GodotOverride] private bool OnSet(StringName property, Variant value) => base._Set(property, value);
        [GodotOverride(true)] private Godot.Collections.Array<Godot.Collections.Dictionary> OnGetPropertyList() => base._GetPropertyList();

        void ITest.InitTests() => RunInitTest();
        void ITest.EnterTests() => RunEnterTest();
        void ITest.ReadyTests() => RunReadyTest();
        void ITest.ProcessTests() => RunProcessTest();
        void ITest.ExitTests() => RunExitTest();

        protected override void RunInitTest()
        {
            base.RunInitTest();

            onEnterTree.Should().BeFalse();
            onReady.Should().BeFalse();
            onProcess.Should().BeFalse();
            onExitTree.Should().BeFalse();
        }

        protected override void RunEnterTest(bool _ = false)
        {
            base.RunEnterTest();

            onEnterTree.Should().BeTrue(); onEnterTree = false;
            onReady.Should().BeFalse();
            onProcess.Should().BeFalse();
            onExitTree.Should().BeFalse();
        }

        protected override void RunReadyTest(bool _ = false)
        {
            base.RunReadyTest(false);

            onEnterTree.Should().BeFalse();
            onReady.Should().BeTrue(); onReady = false;
            onProcess.Should().BeFalse();
            onExitTree.Should().BeFalse();
        }

        protected override void RunProcessTest(bool _ = false)
        {
            base.RunProcessTest(false);

            onEnterTree.Should().BeFalse();
            onReady.Should().BeFalse();
            onProcess.Should().BeTrue(); onProcess = false;
            onExitTree.Should().BeFalse();
        }

        protected override void RunExitTest(bool _ = false)
        {
            base.RunExitTest();

            onEnterTree.Should().BeFalse();
            onReady.Should().BeFalse();
            onProcess.Should().BeFalse();
            onExitTree.Should().BeTrue(); onExitTree = false;
        }
    }
}
