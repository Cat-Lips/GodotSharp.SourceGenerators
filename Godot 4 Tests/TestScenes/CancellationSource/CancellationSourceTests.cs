using System.Threading;
using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
[CancellationSource]
public partial class CancellationSourceTests : Node, ITest
{
    private CancellationToken capturedToken;

    void ITest.InitTests()
    {
        Cts.Should().BeNull();
    }

    void ITest.EnterTests()
    {
        // After entering tree, CTS should be created
        Cts.Should().NotBeNull();
        Cts.Token.IsCancellationRequested.Should().BeFalse();
        Cts.Token.CanBeCanceled.Should().BeTrue();

        // Capture token to verify it gets cancelled on exit
        capturedToken = Cts.Token;
    }

    void ITest.ExitTests()
    {
        // After exiting tree, the captured token should be cancelled
        capturedToken.IsCancellationRequested.Should().BeTrue();

        // The property should now return null
        Cts.Should().BeNull();
    }
}
