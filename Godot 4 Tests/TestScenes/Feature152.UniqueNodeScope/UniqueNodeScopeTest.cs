using System;
using System.Reflection;
using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class UniqueNodeScopeTest : Node, ITest
{
    void ITest.ReadyTests()
    {
        const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        Test(typeof(CustomScope));
        Test(typeof(DefaultScope));
        Test(typeof(InvalidScope));

        static void Test(Type sut)
        {
            sut.GetProperty("Node1", flags).GetMethod.IsPublic.Should().BeTrue();
            sut.GetProperty("Node2", flags).GetMethod.IsPrivate.Should().BeTrue();
            sut.GetProperty("Node3", flags).GetMethod.IsFamily.Should().BeTrue();
        }
    }
}
