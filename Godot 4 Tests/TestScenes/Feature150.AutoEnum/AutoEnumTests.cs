using System;
using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[AutoEnum]
public enum MapType
{
    Outside,
    Corridor,
    Apartment,
}

[AutoEnum]
public partial class MapData
{
    private MapData() { }
    public static readonly MapData Outside = new();
    public static readonly MapData Corridor = new();
    public static readonly MapData Apartment = new();
    public static readonly object Invalid = new();
}

[SceneTree]
public partial class AutoEnumTests : Node, ITest
{
    [AutoEnum]
    public enum _MapType
    {
        Outside,
        Corridor,
        Apartment,
    }

    [AutoEnum]
    public partial class _MapData
    {
        private _MapData() { }
        public static readonly _MapData Outside = new();
        public static readonly _MapData Corridor = new();
        public static readonly _MapData Apartment = new();
        public static readonly object Invalid = new();
    }

    void ITest.ReadyTests()
    {
        MapType.Outside.Str().Should().Be(nameof(MapType.Outside));
        MapType.Corridor.Str().Should().Be(nameof(MapType.Corridor));
        MapType.Apartment.Str().Should().Be(nameof(MapType.Apartment));
        MapTypeStr.Parse(nameof(MapType.Outside)).Should().Be(MapType.Outside);
        MapTypeStr.Parse(nameof(MapType.Corridor)).Should().Be(MapType.Corridor);
        MapTypeStr.Parse(nameof(MapType.Apartment)).Should().Be(MapType.Apartment);

        MapData.Outside.ToEnum().Should().Be(MapData.Enum.Outside);
        MapData.Corridor.ToEnum().Should().Be(MapData.Enum.Corridor);
        MapData.Apartment.ToEnum().Should().Be(MapData.Enum.Apartment);
        MapData.FromEnum(MapData.Enum.Outside).Should().Be(MapData.Outside);
        MapData.FromEnum(MapData.Enum.Corridor).Should().Be(MapData.Corridor);
        MapData.FromEnum(MapData.Enum.Apartment).Should().Be(MapData.Apartment);

        MapData.Outside.ToStr().Should().Be(nameof(MapData.Outside));
        MapData.Corridor.ToStr().Should().Be(nameof(MapData.Corridor));
        MapData.Apartment.ToStr().Should().Be(nameof(MapData.Apartment));
        MapData.FromStr(nameof(MapData.Outside)).Should().Be(MapData.Outside);
        MapData.FromStr(nameof(MapData.Corridor)).Should().Be(MapData.Corridor);
        MapData.FromStr(nameof(MapData.Apartment)).Should().Be(MapData.Apartment);

        _MapType.Outside.Str().Should().Be(nameof(_MapType.Outside));
        _MapType.Corridor.Str().Should().Be(nameof(_MapType.Corridor));
        _MapType.Apartment.Str().Should().Be(nameof(_MapType.Apartment));
        _MapTypeStr.Parse(nameof(_MapType.Outside)).Should().Be(_MapType.Outside);
        _MapTypeStr.Parse(nameof(_MapType.Corridor)).Should().Be(_MapType.Corridor);
        _MapTypeStr.Parse(nameof(_MapType.Apartment)).Should().Be(_MapType.Apartment);

        _MapData.Outside.ToEnum().Should().Be(_MapData.Enum.Outside);
        _MapData.Corridor.ToEnum().Should().Be(_MapData.Enum.Corridor);
        _MapData.Apartment.ToEnum().Should().Be(_MapData.Enum.Apartment);
        _MapData.FromEnum(_MapData.Enum.Outside).Should().Be(_MapData.Outside);
        _MapData.FromEnum(_MapData.Enum.Corridor).Should().Be(_MapData.Corridor);
        _MapData.FromEnum(_MapData.Enum.Apartment).Should().Be(_MapData.Apartment);

        _MapData.Outside.ToStr().Should().Be(nameof(_MapData.Enum.Outside));
        _MapData.Corridor.ToStr().Should().Be(nameof(_MapData.Enum.Corridor));
        _MapData.Apartment.ToStr().Should().Be(nameof(_MapData.Enum.Apartment));
        _MapData.FromStr(nameof(_MapData.Enum.Outside)).Should().Be(_MapData.Outside);
        _MapData.FromStr(nameof(_MapData.Enum.Corridor)).Should().Be(_MapData.Corridor);
        _MapData.FromStr(nameof(_MapData.Enum.Apartment)).Should().Be(_MapData.Apartment);

        FluentActions.Invoking(() => MapTypeStr.Parse("Invalid")).Should().Throw<ArgumentOutOfRangeException>();
        FluentActions.Invoking(() => MapData.FromStr("Invalid")).Should().Throw<ArgumentOutOfRangeException>();
        FluentActions.Invoking(() => _MapTypeStr.Parse("Invalid")).Should().Throw<ArgumentOutOfRangeException>();
        FluentActions.Invoking(() => _MapData.FromStr("Invalid")).Should().Throw<ArgumentOutOfRangeException>();
    }
}

[AutoEnum]
public partial class MapNameWithNoMembers
{
    //public static readonly MapNameWithNoMembers Outside = new();
    //public static readonly MapNameWithNoMembers Corridor = new();
    //public static readonly MapNameWithNoMembers Apartment = new();
    private MapNameWithNoMembers() { }
}

[AutoEnum]
public enum MapTypeWithNoMembers
{
    //Outside,
    //Corridor,
    //Apartment,
}
