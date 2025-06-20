using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

using SUT = (
    CanvasItem Render2D,
    VisualInstance Render3D,
    CollisionObject2D Physics2D,
    CollisionObject Physics3D,
    Navigation2D Navigation2D,
    Navigation Navigation3D,
    NavigationAgent2D Avoidance2D,
    NavigationAgent Avoidance3D);

[SceneTree]
public partial class LayerNamesIndexTest : Node, ITest
{
    private SUT WithLayer1Set => (
        _.WithLayer1Set.Render2D, _.WithLayer1Set.Render3D,
        _.WithLayer1Set.Physics2D, _.WithLayer1Set.Physics3D,
        _.WithLayer1Set.Navigation2D, _.WithLayer1Set.Navigation3D,
        _.WithLayer1Set.Avoidance2D, _.WithLayer1Set.Avoidance3D);

    private SUT WithLayer2Set => (
        _.WithLayer2Set.Render2D, _.WithLayer2Set.Render3D,
        _.WithLayer2Set.Physics2D, _.WithLayer2Set.Physics3D,
        _.WithLayer2Set.Navigation2D, _.WithLayer2Set.Navigation3D,
        _.WithLayer2Set.Avoidance2D, _.WithLayer2Set.Avoidance3D);

    private SUT WithLayer12Set => (
        _.WithLayer12Set.Render2D, _.WithLayer12Set.Render3D,
        _.WithLayer12Set.Physics2D, _.WithLayer12Set.Physics3D,
        _.WithLayer12Set.Navigation2D, _.WithLayer12Set.Navigation3D,
        _.WithLayer12Set.Avoidance2D, _.WithLayer12Set.Avoidance3D);

    private SUT WithNoLayerSet => (
        _.WithNoLayerSet.Render2D, _.WithNoLayerSet.Render3D,
        _.WithNoLayerSet.Physics2D, _.WithNoLayerSet.Physics3D,
        _.WithNoLayerSet.Navigation2D, _.WithNoLayerSet.Navigation3D,
        _.WithNoLayerSet.Avoidance2D, _.WithNoLayerSet.Avoidance3D);

    void ITest.InitTests()
    {
        Test(WithLayer1Set, 1, nameof(WithLayer1Set));
        Test(WithLayer2Set, 2, nameof(WithLayer2Set));
        Test(WithLayer12Set, 3, nameof(WithLayer12Set));
        Test(WithNoLayerSet, 0, nameof(WithNoLayerSet));

        static void Test(SUT sut, uint x, string name)
        {
            Test2D();
            Test3D();

            void Test2D()
            {
                sut.Render2D.LightMask.Should().Be((int)x, because: name);
                //sut.Render2D.VisibilityLayer.Should().Be(x, because: name);

                sut.Physics2D.LightMask.Should().Be((int)x, because: name);
                //sut.Physics2D.VisibilityLayer.Should().Be(x, because: name);
                sut.Physics2D.CollisionLayer.Should().Be(x, because: name);
                sut.Physics2D.CollisionMask.Should().Be(x, because: name);

                sut.Navigation2D.LightMask.Should().Be((int)x, because: name);
                //sut.Navigation2D.VisibilityLayer.Should().Be(x, because: name);
                sut.Navigation2D.NavigationLayers.Should().Be(x, because: name);

                sut.Avoidance2D.NavigationLayers.Should().Be(x, because: name);
                //sut.Avoidance2D.AvoidanceLayers.Should().Be(x, because: name);
                //sut.Avoidance2D.AvoidanceMask.Should().Be(x, because: name);
            }

            void Test3D()
            {
                sut.Render3D.Layers.Should().Be(x, because: name);

                sut.Physics3D.CollisionLayer.Should().Be(x, because: name);
                sut.Physics3D.CollisionMask.Should().Be(x, because: name);

                sut.Navigation3D.NavigationLayers.Should().Be(x, because: name);

                sut.Avoidance3D.NavigationLayers.Should().Be(x, because: name);
                //sut.Avoidance3D.AvoidanceLayers.Should().Be(x, because: name);
                //sut.Avoidance3D.AvoidanceMask.Should().Be(x, because: name);
            }
        }
    }
}
