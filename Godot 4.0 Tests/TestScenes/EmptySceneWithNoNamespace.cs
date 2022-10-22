using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

[SceneTree]
public partial class EmptySceneWithNoNamespace : Control, ITest
{
    public override partial void _EnterTree();
    public override partial void _Process(double _1);

    public int RequiredFrames => 1;

    private bool GodotOverride_WithArgs_WasCalled;
    private bool GodotOverride_WithNoArgs_WasCalled;

    [GodotOverride] public virtual void OnEnterTree() => GodotOverride_WithNoArgs_WasCalled = true;
    [GodotOverride] public virtual void OnProcess(double _) => GodotOverride_WithArgs_WasCalled = true;

    void ITest.ProcessTests()
    {
        GodotOverride_WithArgs_WasCalled.Should().BeTrue();
        GodotOverride_WithNoArgs_WasCalled.Should().BeTrue();
    }

    void ITest.InitTests()
        => _.GetType().GetProperties().Should().BeEmpty();
}
