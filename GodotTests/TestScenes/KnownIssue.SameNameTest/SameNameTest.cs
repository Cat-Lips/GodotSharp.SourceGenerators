using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes
{
    [SceneTree]
    internal abstract partial class SameNameTest : Control, ITest
    {
        void ITest.InitTests()
        {
            SameName0.Should().BeOfType<SameName>();
            SameName1.Should().BeOfType<Namespace1.SameName>();
            SameName2.Should().BeOfType<Namespace2.SameName>();
        }
    }
}
