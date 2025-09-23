using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class ITscnFilePathTest : Node, ITest
{
    private static readonly string ExpectedTscnFilePath = "res://TestScenes/Feature121.ITscnFilePath/ITscnFilePathTest.tscn";

    void ITest.InitTests()
    {
        Test<ITscnFilePathTest>();
        TscnFilePath.Should().Be(ExpectedTscnFilePath);
        GetType().GetInterface("ITscnFilePath").GetProperty("TscnFilePath").Should().NotBeNull();
    }

    public static void Test<T>() where T : ITscnFilePath
        => T.TscnFilePath.Should().Be(ExpectedTscnFilePath);
}
