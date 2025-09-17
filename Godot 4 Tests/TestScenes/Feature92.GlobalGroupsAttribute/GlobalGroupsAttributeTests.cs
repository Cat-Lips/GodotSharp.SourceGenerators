using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[GlobalGroups]
public static partial class GRP;

[SceneTree, GlobalGroups]
public partial class GlobalGroupsAttributeTests : Node, ITest
{
    [GlobalGroups]
    private static partial class _GRP;

    void ITest.InitTests()
    {
        Group1.Should().Be((StringName)"Group1");
        Group2.Should().Be((StringName)"Group2");
        GRP.Group1.Should().Be((StringName)"Group1");
        GRP.Group2.Should().Be((StringName)"Group2");
        _GRP.Group1.Should().Be((StringName)"Group1");
        _GRP.Group2.Should().Be((StringName)"Group2");
    }
}
