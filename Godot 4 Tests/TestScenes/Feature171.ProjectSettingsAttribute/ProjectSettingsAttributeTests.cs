using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;
using GodotSharp.SourceGenerators.ProjectSettingsExtensions;

namespace GodotTests.TestScenes;

[ProjectSettings]
public static partial class PRJ;

[ProjectSettings(Gravity: Generate.None)]
public static partial class PRJ_None;

[ProjectSettings(Gravity: Generate.Get2D)]
public static partial class PRJ_Get2D;

[ProjectSettings(Gravity: Generate.Get3D)]
public static partial class PRJ_Get3D;

[ProjectSettings(Gravity: Generate.GetSet2D)]
public static partial class PRJ_GetSet2D;

[ProjectSettings(Gravity: Generate.GetSet3D)]
public static partial class PRJ_GetSet3D;

[SceneTree]
public partial class ProjectSettingsAttributeTests : Node, ITest
{
    void ITest.ReadyTests()
    {
        TestAll();
        TestNone();
        TestGet2D();
        TestGet3D();
        TestGetSet2D();
        TestGetSet3D();

        void TestAll()
        {
            typeof(PRJ).ShouldConsistOf(NestedTypes: ["Default"], Properties: [
                "Gravity2D", "Gravity3D",
                "GravityVector2D", "GravityVector3D",
                "GravityIsPoint2D", "GravityIsPoint3D",
                "GravityPointUnitDist2D", "GravityPointUnitDist3D"]);
            typeof(PRJ.Default).ShouldConsistOf(
                Properties: [
                    "Gravity2D", "Gravity3D",
                    "GravityVector2D", "GravityVector3D"]);

            PRJ.Default.Gravity2D.Should().Be(162.5f);
            PRJ.Default.Gravity3D.Should().Be(1.625f);
            PRJ.Default.GravityVector2D.Should().Be(new Vector2(.1f, .2f));
            PRJ.Default.GravityVector3D.Should().Be(new Vector3(.1f, .2f, .3f));

            PRJ.Gravity2D.Should().Be(PRJ.Default.Gravity2D);
            PRJ.Gravity3D.Should().Be(PRJ.Default.Gravity3D);
            PRJ.GravityVector2D.Should().Be(PRJ.Default.GravityVector2D);
            PRJ.GravityVector3D.Should().Be(PRJ.Default.GravityVector3D);
            PRJ.GravityIsPoint2D.Should().Be(false);
            PRJ.GravityIsPoint3D.Should().Be(false);
            PRJ.GravityPointUnitDist2D.Should().Be(0f);
            PRJ.GravityPointUnitDist3D.Should().Be(0f);

            ActualGravity2D().Should().Be(PRJ.Gravity2D);
            ActualGravity3D().Should().Be(PRJ.Gravity3D);
            ActualGravityVector2D().Should().Be(PRJ.GravityVector2D);
            ActualGravityVector3D().Should().Be(PRJ.GravityVector3D);
            ActualGravityIsPoint2D().Should().Be(PRJ.GravityIsPoint2D);
            ActualGravityIsPoint3D().Should().Be(PRJ.GravityIsPoint3D);
            ActualGravityPointUnitDist2D().Should().Be(PRJ.GravityPointUnitDist2D);
            ActualGravityPointUnitDist3D().Should().Be(PRJ.GravityPointUnitDist3D);

            PRJ.Gravity2D = default;
            PRJ.Gravity3D = default;
            PRJ.GravityVector2D = default;
            PRJ.GravityVector3D = default;
            PRJ.GravityIsPoint2D = !PRJ.GravityIsPoint2D;
            PRJ.GravityIsPoint3D = !PRJ.GravityIsPoint3D;
            PRJ.GravityPointUnitDist2D = .7f;
            PRJ.GravityPointUnitDist3D = .7f;

            PRJ.Gravity2D.Should().Be(0f);
            PRJ.Gravity3D.Should().Be(0f);
            PRJ.GravityVector2D.Should().Be(Vector2.Zero);
            PRJ.GravityVector3D.Should().Be(Vector3.Zero);
            PRJ.GravityIsPoint2D.Should().Be(true);
            PRJ.GravityIsPoint3D.Should().Be(true);
            PRJ.GravityPointUnitDist2D.Should().Be(.7f);
            PRJ.GravityPointUnitDist3D.Should().Be(.7f);

            ActualGravity2D().Should().Be(PRJ.Gravity2D);
            ActualGravity3D().Should().Be(PRJ.Gravity3D);
            ActualGravityVector2D().Should().Be(PRJ.GravityVector2D);
            ActualGravityVector3D().Should().Be(PRJ.GravityVector3D);
            ActualGravityIsPoint2D().Should().Be(PRJ.GravityIsPoint2D);
            ActualGravityIsPoint3D().Should().Be(PRJ.GravityIsPoint3D);
            ActualGravityPointUnitDist2D().Should().Be(PRJ.GravityPointUnitDist2D);
            ActualGravityPointUnitDist3D().Should().Be(PRJ.GravityPointUnitDist3D);

            PRJ.Gravity2D = PRJ.Default.Gravity2D;
            PRJ.Gravity3D = PRJ.Default.Gravity3D;
            PRJ.GravityVector2D = PRJ.Default.GravityVector2D;
            PRJ.GravityVector3D = PRJ.Default.GravityVector3D;
            PRJ.GravityIsPoint2D = default;
            PRJ.GravityIsPoint3D = default;
            PRJ.GravityPointUnitDist2D = default;
            PRJ.GravityPointUnitDist3D = default;

            PRJ.Gravity2D.Should().Be(PRJ.Default.Gravity2D);
            PRJ.Gravity3D.Should().Be(PRJ.Default.Gravity3D);
            PRJ.GravityVector2D.Should().Be(PRJ.Default.GravityVector2D);
            PRJ.GravityVector3D.Should().Be(PRJ.Default.GravityVector3D);
            PRJ.GravityIsPoint2D.Should().Be(false);
            PRJ.GravityIsPoint3D.Should().Be(false);
            PRJ.GravityPointUnitDist2D.Should().Be(0f);
            PRJ.GravityPointUnitDist3D.Should().Be(0f);

            ActualGravity2D().Should().Be(PRJ.Gravity2D);
            ActualGravity3D().Should().Be(PRJ.Gravity3D);
            ActualGravityVector2D().Should().Be(PRJ.GravityVector2D);
            ActualGravityVector3D().Should().Be(PRJ.GravityVector3D);
            ActualGravityIsPoint2D().Should().Be(PRJ.GravityIsPoint2D);
            ActualGravityIsPoint3D().Should().Be(PRJ.GravityIsPoint3D);
            ActualGravityPointUnitDist2D().Should().Be(PRJ.GravityPointUnitDist2D);
            ActualGravityPointUnitDist3D().Should().Be(PRJ.GravityPointUnitDist3D);
        }

        void TestNone()
            => typeof(PRJ_None).ShouldConsistOf();

        void TestGet2D()
        {
            typeof(PRJ_Get2D).ShouldConsistOf(Properties: ["Gravity2D", "GravityVector2D"]);

            PRJ_Get2D.Gravity2D.Should().Be(162.5f);
            PRJ_Get2D.GravityVector2D.Should().Be(new Vector2(.1f, .2f));

            ActualGravity2D().Should().Be(PRJ_Get2D.Gravity2D);
            ActualGravityVector2D().Should().Be(PRJ_Get2D.GravityVector2D);
        }

        void TestGet3D()
        {
            typeof(PRJ_Get3D).ShouldConsistOf(Properties: ["Gravity3D", "GravityVector3D"]);

            PRJ_Get3D.Gravity3D.Should().Be(1.625f);
            PRJ_Get3D.GravityVector3D.Should().Be(new Vector3(.1f, .2f, .3f));

            ActualGravity3D().Should().Be(PRJ_Get3D.Gravity3D);
            ActualGravityVector3D().Should().Be(PRJ_Get3D.GravityVector3D);
        }

        void TestGetSet2D()
        {
            typeof(PRJ_GetSet2D).ShouldConsistOf(NestedTypes: ["Default"], Properties: [
                "Gravity2D", "GravityVector2D", "GravityIsPoint2D", "GravityPointUnitDist2D"]);
            typeof(PRJ_GetSet2D.Default).ShouldConsistOf(Properties: ["Gravity2D", "GravityVector2D"]);

            PRJ_GetSet2D.Default.Gravity2D.Should().Be(162.5f);
            PRJ_GetSet2D.Default.GravityVector2D.Should().Be(new Vector2(.1f, .2f));

            PRJ_GetSet2D.Gravity2D.Should().Be(PRJ_GetSet2D.Default.Gravity2D);
            PRJ_GetSet2D.GravityVector2D.Should().Be(PRJ_GetSet2D.Default.GravityVector2D);
            PRJ_GetSet2D.GravityIsPoint2D.Should().Be(false);
            PRJ_GetSet2D.GravityPointUnitDist2D.Should().Be(0f);

            ActualGravity2D().Should().Be(PRJ_GetSet2D.Gravity2D);
            ActualGravityVector2D().Should().Be(PRJ_GetSet2D.GravityVector2D);
            ActualGravityIsPoint2D().Should().Be(PRJ_GetSet2D.GravityIsPoint2D);
            ActualGravityPointUnitDist2D().Should().Be(PRJ_GetSet2D.GravityPointUnitDist2D);

            PRJ_GetSet2D.Gravity2D = default;
            PRJ_GetSet2D.GravityVector2D = default;
            PRJ_GetSet2D.GravityIsPoint2D = !PRJ_GetSet2D.GravityIsPoint2D;
            PRJ_GetSet2D.GravityPointUnitDist2D = .7f;

            PRJ_GetSet2D.Gravity2D.Should().Be(0f);
            PRJ_GetSet2D.GravityVector2D.Should().Be(Vector2.Zero);
            PRJ_GetSet2D.GravityIsPoint2D.Should().Be(true);
            PRJ_GetSet2D.GravityPointUnitDist2D.Should().Be(.7f);

            ActualGravity2D().Should().Be(PRJ_GetSet2D.Gravity2D);
            ActualGravityVector2D().Should().Be(PRJ_GetSet2D.GravityVector2D);
            ActualGravityIsPoint2D().Should().Be(PRJ_GetSet2D.GravityIsPoint2D);
            ActualGravityPointUnitDist2D().Should().Be(PRJ_GetSet2D.GravityPointUnitDist2D);

            PRJ_GetSet2D.Gravity2D = PRJ_GetSet2D.Default.Gravity2D;
            PRJ_GetSet2D.GravityVector2D = PRJ_GetSet2D.Default.GravityVector2D;
            PRJ_GetSet2D.GravityIsPoint2D = default;
            PRJ_GetSet2D.GravityPointUnitDist2D = default;

            PRJ_GetSet2D.Gravity2D.Should().Be(PRJ_GetSet2D.Default.Gravity2D);
            PRJ_GetSet2D.GravityVector2D.Should().Be(PRJ_GetSet2D.Default.GravityVector2D);
            PRJ_GetSet2D.GravityIsPoint2D.Should().Be(false);
            PRJ_GetSet2D.GravityPointUnitDist2D.Should().Be(0f);

            ActualGravity2D().Should().Be(PRJ_GetSet2D.Gravity2D);
            ActualGravityVector2D().Should().Be(PRJ_GetSet2D.GravityVector2D);
            ActualGravityIsPoint2D().Should().Be(PRJ_GetSet2D.GravityIsPoint2D);
            ActualGravityPointUnitDist2D().Should().Be(PRJ_GetSet2D.GravityPointUnitDist2D);
        }

        void TestGetSet3D()
        {
            typeof(PRJ_GetSet3D).ShouldConsistOf(NestedTypes: ["Default"], Properties: [
                "Gravity3D", "GravityVector3D", "GravityIsPoint3D", "GravityPointUnitDist3D"]);
            typeof(PRJ_GetSet3D.Default).ShouldConsistOf(Properties: ["Gravity3D", "GravityVector3D"]);

            PRJ_GetSet3D.Default.Gravity3D.Should().Be(1.625f);
            PRJ_GetSet3D.Default.GravityVector3D.Should().Be(new Vector3(.1f, .2f, .3f));

            PRJ_GetSet3D.Gravity3D.Should().Be(PRJ_GetSet3D.Default.Gravity3D);
            PRJ_GetSet3D.GravityVector3D.Should().Be(PRJ_GetSet3D.Default.GravityVector3D);
            PRJ_GetSet3D.GravityIsPoint3D.Should().Be(false);
            PRJ_GetSet3D.GravityPointUnitDist3D.Should().Be(0f);

            ActualGravity3D().Should().Be(PRJ_GetSet3D.Gravity3D);
            ActualGravityVector3D().Should().Be(PRJ_GetSet3D.GravityVector3D);
            ActualGravityIsPoint3D().Should().Be(PRJ_GetSet3D.GravityIsPoint3D);
            ActualGravityPointUnitDist3D().Should().Be(PRJ_GetSet3D.GravityPointUnitDist3D);

            PRJ_GetSet3D.Gravity3D = default;
            PRJ_GetSet3D.GravityVector3D = default;
            PRJ_GetSet3D.GravityIsPoint3D = !PRJ_GetSet3D.GravityIsPoint3D;
            PRJ_GetSet3D.GravityPointUnitDist3D = .7f;

            PRJ_GetSet3D.Gravity3D.Should().Be(0f);
            PRJ_GetSet3D.GravityVector3D.Should().Be(Vector3.Zero);
            PRJ_GetSet3D.GravityIsPoint3D.Should().Be(true);
            PRJ_GetSet3D.GravityPointUnitDist3D.Should().Be(.7f);

            ActualGravity3D().Should().Be(PRJ_GetSet3D.Gravity3D);
            ActualGravityVector3D().Should().Be(PRJ_GetSet3D.GravityVector3D);
            ActualGravityIsPoint3D().Should().Be(PRJ_GetSet3D.GravityIsPoint3D);
            ActualGravityPointUnitDist3D().Should().Be(PRJ_GetSet3D.GravityPointUnitDist3D);

            PRJ_GetSet3D.Gravity3D = PRJ_GetSet3D.Default.Gravity3D;
            PRJ_GetSet3D.GravityVector3D = PRJ_GetSet3D.Default.GravityVector3D;
            PRJ_GetSet3D.GravityIsPoint3D = default;
            PRJ_GetSet3D.GravityPointUnitDist3D = default;

            PRJ_GetSet3D.Gravity3D.Should().Be(PRJ_GetSet3D.Default.Gravity3D);
            PRJ_GetSet3D.GravityVector3D.Should().Be(PRJ_GetSet3D.Default.GravityVector3D);
            PRJ_GetSet3D.GravityIsPoint3D.Should().Be(false);
            PRJ_GetSet3D.GravityPointUnitDist3D.Should().Be(0f);

            ActualGravity3D().Should().Be(PRJ_GetSet3D.Gravity3D);
            ActualGravityVector3D().Should().Be(PRJ_GetSet3D.GravityVector3D);
            ActualGravityIsPoint3D().Should().Be(PRJ_GetSet3D.GravityIsPoint3D);
            ActualGravityPointUnitDist3D().Should().Be(PRJ_GetSet3D.GravityPointUnitDist3D);
        }

        float ActualGravity2D() => (float)PhysicsServer2D.AreaGetParam(GetViewport().FindWorld2D().Space, PhysicsServer2D.AreaParameter.Gravity);
        float ActualGravity3D() => (float)PhysicsServer3D.AreaGetParam(GetViewport().FindWorld3D().Space, PhysicsServer3D.AreaParameter.Gravity);
        Vector2 ActualGravityVector2D() => (Vector2)PhysicsServer2D.AreaGetParam(GetViewport().FindWorld2D().Space, PhysicsServer2D.AreaParameter.GravityVector);
        Vector3 ActualGravityVector3D() => (Vector3)PhysicsServer3D.AreaGetParam(GetViewport().FindWorld3D().Space, PhysicsServer3D.AreaParameter.GravityVector);
        bool ActualGravityIsPoint2D() => (bool)PhysicsServer2D.AreaGetParam(GetViewport().FindWorld2D().Space, PhysicsServer2D.AreaParameter.GravityIsPoint);
        bool ActualGravityIsPoint3D() => (bool)PhysicsServer3D.AreaGetParam(GetViewport().FindWorld3D().Space, PhysicsServer3D.AreaParameter.GravityIsPoint);
        float ActualGravityPointUnitDist2D() => (float)PhysicsServer2D.AreaGetParam(GetViewport().FindWorld2D().Space, PhysicsServer2D.AreaParameter.GravityPointUnitDistance);
        float ActualGravityPointUnitDist3D() => (float)PhysicsServer3D.AreaGetParam(GetViewport().FindWorld3D().Space, PhysicsServer3D.AreaParameter.GravityPointUnitDistance);
    }
}
