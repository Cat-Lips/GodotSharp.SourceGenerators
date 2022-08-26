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

        [GodotOverride]
        private void OnReady()
            => onReady = true;

        [GodotOverride]
        private void OnProcess(float _)
            => onProcess = true;

        [GodotOverride]
        private void OnExitTree()
            => onExitTree = true;

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

        protected override void RunEnterTest()
        {
            base.RunEnterTest();

            onEnterTree.Should().BeTrue(); onEnterTree = false;
            onReady.Should().BeFalse();
            onProcess.Should().BeFalse();
            onExitTree.Should().BeFalse();
        }

        protected override void RunReadyTest()
        {
            base.RunReadyTest();

            onEnterTree.Should().BeFalse();
            onReady.Should().BeTrue(); onReady = false;
            onProcess.Should().BeFalse();
            onExitTree.Should().BeFalse();
        }

        protected override void RunProcessTest()
        {
            base.RunProcessTest();

            onEnterTree.Should().BeFalse();
            onReady.Should().BeFalse();
            onProcess.Should().BeTrue(); onProcess = false;
            onExitTree.Should().BeFalse();
        }

        protected override void RunExitTest()
        {
            base.RunExitTest();

            onEnterTree.Should().BeFalse();
            onReady.Should().BeFalse();
            onProcess.Should().BeFalse();
            onExitTree.Should().BeTrue(); onExitTree = false;
        }
    }
}
