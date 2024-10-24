using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes
{
    [SceneTree("SceneWithChildren.tscn")]
    public partial class DescendantNodeAccessingSceneTree : Node, ITest
    {
        void ITest.InitTests()
        {
            var someNodeFromSceneTree = _.TopLevelChild1.NestedChild11;
            someNodeFromSceneTree.Should().NotBeNull();
        }
    }
}
