using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

[SceneTree]
internal partial class RootSceneWithNoNamespace : Control, ITest
{
    public override partial void _EnterTree();
    public override partial void _Process(double _);

    public int RequiredFrames => 1;

    private bool GodotOverride_WithArgs_WasCalled;
    private bool GodotOverride_WithNoArgs_WasCalled;

    [GodotOverride] protected virtual void OnEnterTree() => GodotOverride_WithNoArgs_WasCalled = true;
    [GodotOverride] protected virtual void OnProcess(double _) => GodotOverride_WithArgs_WasCalled = true;

    void ITest.ProcessTests()
    {
        GodotOverride_WithArgs_WasCalled.Should().BeTrue();
        GodotOverride_WithNoArgs_WasCalled.Should().BeTrue();
    }

    void ITest.InitTests()
    {
        _.Layout.Label1.Text.Should().Be("Label1");
        _.Layout.Label2.Text.Should().Be("Label2");

        _.Layout.Get().Should().Be(GetNode("Layout"));
    }
}
