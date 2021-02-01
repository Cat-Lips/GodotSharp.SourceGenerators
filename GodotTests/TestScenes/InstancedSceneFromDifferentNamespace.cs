using FluentAssertions;
using Godot;
using GodotTests.TestScenes.Script;
using GodotTests.Utilities;

namespace GodotTests.TestScenes
{
    [SceneTree]
    public abstract partial class InstancedSceneFromDifferentNamespace : Control, ITest
    {
        // Known Issue: tscn and script must have same name
        void ITest.InitTests()
            => _.InstancedScene.Should().BeOfType<SceneInDifferentNamespace>().And.NotBeNull();
    }
}
