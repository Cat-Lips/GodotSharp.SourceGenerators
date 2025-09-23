using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[AudioBus]
public static partial class AudioBus;

[AudioBus("Resources/custom_bus_layout")]
public static partial class AudioBusWithRelativePath;

[AudioBus("TestScenes/Feature111.AudioBusAttribute/Resources/custom_bus_layout")]
public static partial class AudioBusWithAbsolutePath;

//[AudioBus("no_path")] // Should not compile
//public static partial class AudioBusWithInvalidPath;

[SceneTree]
public partial class AudioBusAttributeTest : Node, ITest
{
    [AudioBus]
    public static partial class _AudioBus;

    [AudioBus("Resources/custom_bus_layout")]
    public static partial class _AudioBusWithRelativePath;

    [AudioBus("TestScenes/Feature111.AudioBusAttribute/Resources/custom_bus_layout")]
    public static partial class _AudioBusWithAbsolutePath;

    //[AudioBus("Resources/no_path")] // Should not compile
    //public static partial class _AudioBusWithInvalidPath;

    void ITest.InitTests()
    {
        AudioBus.Master.Should().Be((StringName)"Master");
        AudioBus.Music.Should().Be((StringName)"Music");
        AudioBus.Fx.Should().Be((StringName)"FX");

        AudioBus.MasterId.Should().Be(0);
        AudioBus.MusicId.Should().Be(1);
        AudioBus.FxId.Should().Be(2);

        AudioBusWithRelativePath.Master.Should().Be((StringName)"Master");
        AudioBusWithRelativePath.X.Should().Be((StringName)"X");
        AudioBusWithRelativePath.Y.Should().Be((StringName)"Y");
        AudioBusWithRelativePath.Z.Should().Be((StringName)"Z");

        AudioBusWithRelativePath.MasterId.Should().Be(0);
        AudioBusWithRelativePath.XId.Should().Be(1);
        AudioBusWithRelativePath.YId.Should().Be(2);
        AudioBusWithRelativePath.ZId.Should().Be(3);

        AudioBusWithAbsolutePath.Master.Should().Be((StringName)"Master");
        AudioBusWithAbsolutePath.X.Should().Be((StringName)"X");
        AudioBusWithAbsolutePath.Y.Should().Be((StringName)"Y");
        AudioBusWithAbsolutePath.Z.Should().Be((StringName)"Z");

        AudioBusWithAbsolutePath.MasterId.Should().Be(0);
        AudioBusWithAbsolutePath.XId.Should().Be(1);
        AudioBusWithAbsolutePath.YId.Should().Be(2);
        AudioBusWithAbsolutePath.ZId.Should().Be(3);

        _AudioBus.Master.Should().Be((StringName)"Master");
        _AudioBus.Music.Should().Be((StringName)"Music");
        _AudioBus.Fx.Should().Be((StringName)"FX");

        _AudioBus.MasterId.Should().Be(0);
        _AudioBus.MusicId.Should().Be(1);
        _AudioBus.FxId.Should().Be(2);

        _AudioBusWithRelativePath.Master.Should().Be((StringName)"Master");
        _AudioBusWithRelativePath.X.Should().Be((StringName)"X");
        _AudioBusWithRelativePath.Y.Should().Be((StringName)"Y");
        _AudioBusWithRelativePath.Z.Should().Be((StringName)"Z");

        _AudioBusWithRelativePath.MasterId.Should().Be(0);
        _AudioBusWithRelativePath.XId.Should().Be(1);
        _AudioBusWithRelativePath.YId.Should().Be(2);
        _AudioBusWithRelativePath.ZId.Should().Be(3);

        _AudioBusWithAbsolutePath.Master.Should().Be((StringName)"Master");
        _AudioBusWithAbsolutePath.X.Should().Be((StringName)"X");
        _AudioBusWithAbsolutePath.Y.Should().Be((StringName)"Y");
        _AudioBusWithAbsolutePath.Z.Should().Be((StringName)"Z");

        _AudioBusWithAbsolutePath.MasterId.Should().Be(0);
        _AudioBusWithAbsolutePath.XId.Should().Be(1);
        _AudioBusWithAbsolutePath.YId.Should().Be(2);
        _AudioBusWithAbsolutePath.ZId.Should().Be(3);
    }
}
