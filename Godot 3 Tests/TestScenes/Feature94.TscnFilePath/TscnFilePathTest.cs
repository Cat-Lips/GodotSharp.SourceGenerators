using FluentAssertions;
using FluentAssertions.Execution;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class TscnFilePathTest : Node, ITest
{
    private static readonly string ExpectedTscnFilePath = "res://TestScenes/Feature94.TscnFilePath/TscnFilePathTest.tscn";

    static TscnFilePathTest()
        => RunTestsStatic();

    void ITest.InitTests()
        => RunTests(this, IsInsideTree: false);

    void ITest.EnterTests()
        => RunTests(this, IsInsideTree: true);

    void ITest.ReadyTests()
        => RunTests(this, IsInsideTree: true);

    void ITest.ExitTests()
        => RunTests(this, IsInsideTree: false);

    private static void RunTests(Node self = null, bool? IsInsideTree = null)
    {
        ValueTest();
        EnsureIsInTree();

        void ValueTest()
        {
            TscnFilePathTest.TscnFilePath.Should().Be(ExpectedTscnFilePath);
            self?.Filename.Should().Be(TscnFilePathTest.TscnFilePath);
        }

        void EnsureIsInTree()
            => self?.IsInsideTree().Should().Be(IsInsideTree.Value);
    }

    private static void RunTestsStatic()
    {
        if (!Engine.EditorHint)
        {
            try
            {
                RunTests();
            }
            catch (AssertionFailedException e)
            {
                GD.Print(e);
            }
        }
    }
}
