using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes
{
    [SceneTree]
    internal partial class GenericRootTest : Control, ITest
    {
        void ITest.InitTests()
            => GenericRoot.Should().BeOfType<GenericRoot>();
    }
}
