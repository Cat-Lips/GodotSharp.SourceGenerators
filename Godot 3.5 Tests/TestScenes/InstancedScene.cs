using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes
{
    [SceneTree]
    public abstract partial class InstancedScene : Control, ITest
    {
        void ITest.InitTests()
            => _.InstancedScene.Should().BeOfType<RootScene>().And.NotBeNull();
    }
}
