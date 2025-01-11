using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
internal partial class AmbiguousTypeWithImplicitUsings : Node, ITest
{
    void ITest.InitTests()
        => _.AmbiguousType.Should().BeOfType<Godot.Timer>().And.NotBeNull();
}
