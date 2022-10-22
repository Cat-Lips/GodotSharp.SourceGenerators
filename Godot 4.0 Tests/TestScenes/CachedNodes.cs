using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes
{
    [SceneTree]
    internal partial class CachedNodes : Control, ITest
    {
        public override partial void _EnterTree();
        public override partial void _Process(double _1);

        public int RequiredFrames => 1;

        private bool GodotOverride_WithArgs_WasCalled;
        private bool GodotOverride_WithNoArgs_WasCalled;

        [GodotOverride] protected void OnEnterTree() => GodotOverride_WithNoArgs_WasCalled = true;
        [GodotOverride] protected void OnProcess(double _) => GodotOverride_WithArgs_WasCalled = true;

        void ITest.ProcessTests()
        {
            GodotOverride_WithArgs_WasCalled.Should().BeTrue();
            GodotOverride_WithNoArgs_WasCalled.Should().BeTrue();
        }

        void ITest.InitTests()
        {
            Node c1, c2, c3;
            c1 = c2 = c3 = null;

            CacheSceneTreeElements();
            RemoveChild(_.Layout.Get());
            TestThatSceneTreeElementsAreCached();

            void CacheSceneTreeElements()
                => Test(true);

            void TestThatSceneTreeElementsAreCached()
                => Test(false);

            void Test(bool cache)
            {
                _.Label1.Text.Should().Be("Label1.0");
                _.Label2.Text.Should().Be("Label2.0");
                _.Layout.Label1.Text.Should().Be("Label1.1");
                _.Layout.Label2.Text.Should().Be("Label2.1");
                _.Layout.Layout.Label1.Text.Should().Be("Label1.2");
                _.Layout.Layout.Label2.Text.Should().Be("Label2.2");
                _.Layout.Layout.Layout.Label1.Text.Should().Be("Label1.3");
                _.Layout.Layout.Layout.Label2.Text.Should().Be("Label2.3");

                if (cache)
                {
                    c1 = _.Layout.Get();
                    c2 = _.Layout.Layout.Get();
                    c3 = _.Layout.Layout.Layout.Get();

                    c1.Should().Be(GetNode("Layout")).And.NotBeNull();
                    c2.Should().Be(GetNode("Layout/Layout")).And.NotBeNull();
                    c3.Should().Be(GetNode("Layout/Layout/Layout")).And.NotBeNull();
                }
                else
                {
                    _.Layout.Get().Should().Be(c1);
                    _.Layout.Layout.Get().Should().Be(c2);
                    _.Layout.Layout.Layout.Get().Should().Be(c3);

                    GetNodeOrNull("Layout").Should().BeNull();
                    GetNodeOrNull("Layout/Layout").Should().BeNull();
                    GetNodeOrNull("Layout/Layout/Layout").Should().BeNull();
                }
            }
        }
    }
}
