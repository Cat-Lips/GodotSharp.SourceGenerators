using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class PauseNotifyEventsTests : Node, ITest
{
    [Notify] public partial int Value { get; set; }

    void ITest.ReadyTests()
    {
        var changedTriggered = false;
        var changingTriggered = false;
        ValueChanged += () => changedTriggered = true;
        ValueChanging += () => changingTriggered = true;

        InitValue(7);
        changedTriggered.Should().BeFalse();
        changingTriggered.Should().BeFalse();

        Value = 7;
        changedTriggered.Should().BeFalse();
        changingTriggered.Should().BeFalse();

        Value = 3;
        changedTriggered.Should().BeTrue();
        changingTriggered.Should().BeTrue();

        changedTriggered = false;
        changingTriggered = false;
        PauseValueEvents = true;
        Value = 7;
        PauseValueEvents = false;
        changedTriggered.Should().BeFalse();
        changingTriggered.Should().BeFalse();

        Value = 7;
        changedTriggered.Should().BeFalse();
        changingTriggered.Should().BeFalse();

        Value = 3;
        changedTriggered.Should().BeTrue();
        changingTriggered.Should().BeTrue();
    }
}
