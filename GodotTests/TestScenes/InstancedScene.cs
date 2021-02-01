using FluentAssertions;
using Godot;
using GodotTests.Utilities;

namespace GodotTests.TestScenes
{
    [SceneTree]
    public abstract partial class InstancedScene : Control, ITest
    {
        void ITest.InitTests()
            => _.InstancedScene.Should().BeOfType<RootScene>().And.NotBeNull();
    }
}
