using System.Reflection;
using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class UniqueNodeScopeTest : Node, ITest
{
    private partial Node Node2 { get; }
    protected partial Node Node3 { get; }

    void ITest.ReadyTests()
    {
        const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        GetType().GetProperty(nameof(Node1), flags).GetMethod.IsPublic.Should().BeTrue();
        GetType().GetProperty(nameof(Node2), flags).GetMethod.IsPrivate.Should().BeTrue();
        GetType().GetProperty(nameof(Node3), flags).GetMethod.IsFamily.Should().BeTrue();
    }
}
