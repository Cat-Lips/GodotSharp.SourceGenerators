using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes
{
    [SceneTree]
    internal abstract partial class GenericRootTest : Control, ITest
    {
        void ITest.InitTests()
            => GenericRoot.Should().BeOfType<GenericRoot>();
    }
}
