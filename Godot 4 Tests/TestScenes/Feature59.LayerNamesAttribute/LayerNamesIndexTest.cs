//#define LOG

using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

using SUT = (
    uint ExpectedMask,
    int[] ExpectedBits,
    (Viewport Node, int[] GeneratedBits) Cull2D,
    (Camera3D Node, int[] GeneratedBits) Cull3D,
    (CanvasItem Node, int[] GeneratedBits) Render2D,
    (VisualInstance3D Node, int[] GeneratedBits) Render3D,
    (CollisionObject2D Node, int[] GeneratedBits) Physics2D,
    (CollisionObject3D Node, int[] GeneratedBits) Physics3D,
    (NavigationRegion2D Node, int[] GeneratedBits) Navigation2D,
    (NavigationRegion3D Node, int[] GeneratedBits) Navigation3D,
    (NavigationAgent2D Node, int[] GeneratedBits) Avoidance2D,
    (NavigationAgent3D Node, int[] GeneratedBits) Avoidance3D);

[SceneTree]
public partial class LayerNamesIndexTest : Node, ITest
{
    private SUT WithLayer1Set => (0b_0000_0001, [1],
        (_.WithLayer1Set.Cull2D, [MyLayers.Render2D.TestLayer1]),
        (_.WithLayer1Set.Cull3D, [MyLayers.Render3D.TestLayer1]),
        (_.WithLayer1Set.Render2D, [MyLayers.Render2D.TestLayer1]),
        (_.WithLayer1Set.Render3D, [MyLayers.Render3D.TestLayer1]),
        (_.WithLayer1Set.Physics2D, [MyLayers.Physics2D.TestLayer1]),
        (_.WithLayer1Set.Physics3D, [MyLayers.Physics3D.TestLayer1]),
        (_.WithLayer1Set.Navigation2D, [MyLayers.Navigation2D.TestLayer1]),
        (_.WithLayer1Set.Navigation3D, [MyLayers.Navigation3D.TestLayer1]),
        (_.WithLayer1Set.Avoidance2D, [MyLayers.Avoidance.TestLayer1]),
        (_.WithLayer1Set.Avoidance3D, [MyLayers.Avoidance.TestLayer1]));

    private SUT WithLayer3Set => (0b_0000_0100, [3],
        (_.WithLayer3Set.Cull2D, [MyLayers.Render2D.TestLayer3]),
        (_.WithLayer3Set.Cull3D, [MyLayers.Render3D.TestLayer3]),
        (_.WithLayer3Set.Render2D, [MyLayers.Render2D.TestLayer3]),
        (_.WithLayer3Set.Render3D, [MyLayers.Render3D.TestLayer3]),
        (_.WithLayer3Set.Physics2D, [MyLayers.Physics2D.TestLayer3]),
        (_.WithLayer3Set.Physics3D, [MyLayers.Physics3D.TestLayer3]),
        (_.WithLayer3Set.Navigation2D, [MyLayers.Navigation2D.TestLayer3]),
        (_.WithLayer3Set.Navigation3D, [MyLayers.Navigation3D.TestLayer3]),
        (_.WithLayer3Set.Avoidance2D, [MyLayers.Avoidance.TestLayer3]),
        (_.WithLayer3Set.Avoidance3D, [MyLayers.Avoidance.TestLayer3]));

    private SUT WithLayer57Set => (0b_0101_0000, [5, 7],
        (_.WithLayer57Set.Cull2D, [MyLayers.Render2D.TestLayer5, MyLayers.Render2D.TestLayer7]),
        (_.WithLayer57Set.Cull3D, [MyLayers.Render3D.TestLayer5, MyLayers.Render3D.TestLayer7]),
        (_.WithLayer57Set.Render2D, [MyLayers.Render2D.TestLayer5, MyLayers.Render2D.TestLayer7]),
        (_.WithLayer57Set.Render3D, [MyLayers.Render3D.TestLayer5, MyLayers.Render3D.TestLayer7]),
        (_.WithLayer57Set.Physics2D, [MyLayers.Physics2D.TestLayer5, MyLayers.Physics2D.TestLayer7]),
        (_.WithLayer57Set.Physics3D, [MyLayers.Physics3D.TestLayer5, MyLayers.Physics3D.TestLayer7]),
        (_.WithLayer57Set.Navigation2D, [MyLayers.Navigation2D.TestLayer5, MyLayers.Navigation2D.TestLayer7]),
        (_.WithLayer57Set.Navigation3D, [MyLayers.Navigation3D.TestLayer5, MyLayers.Navigation3D.TestLayer7]),
        (_.WithLayer57Set.Avoidance2D, [MyLayers.Avoidance.TestLayer5, MyLayers.Avoidance.TestLayer7]),
        (_.WithLayer57Set.Avoidance3D, [MyLayers.Avoidance.TestLayer5, MyLayers.Avoidance.TestLayer7]));

    private SUT WithNoLayerSet => (0b_0000_0000, [],
        (_.WithNoLayerSet.Cull2D, []),
        (_.WithNoLayerSet.Cull3D, []),
        (_.WithNoLayerSet.Render2D, []),
        (_.WithNoLayerSet.Render3D, []),
        (_.WithNoLayerSet.Physics2D, []),
        (_.WithNoLayerSet.Physics3D, []),
        (_.WithNoLayerSet.Navigation2D, []),
        (_.WithNoLayerSet.Navigation3D, []),
        (_.WithNoLayerSet.Avoidance2D, []),
        (_.WithNoLayerSet.Avoidance3D, []));

    void ITest.InitTests()
    {
        Test(WithLayer1Set);
        Test(WithLayer3Set);
        Test(WithLayer57Set);
        Test(WithNoLayerSet);

        static void Test(SUT sut, [CallerArgumentExpression(nameof(sut))] string TestGroup = null)
        {
            LogTest(TestGroup, sut);

            Test2D();
            Test3D();

            void Test2D()
            {
                // WARNING: VisibilityLayer/CullMask functions are still unsigned and offset by 1 in Godot 4!!!
                TestBits(x => sut.Cull2D.Node.GetCanvasCullMaskBit((uint)x - 1), sut.Cull2D.Node.CanvasCullMask, sut.Cull2D.GeneratedBits);
                TestBits(x => sut.Render2D.Node.GetVisibilityLayerBit((uint)x - 1), sut.Render2D.Node.VisibilityLayer, sut.Render2D.GeneratedBits);
                TestBits(x => sut.Physics2D.Node.GetVisibilityLayerBit((uint)x - 1), sut.Physics2D.Node.VisibilityLayer, sut.Render2D.GeneratedBits);
                TestBits(x => sut.Navigation2D.Node.GetVisibilityLayerBit((uint)x - 1), sut.Navigation2D.Node.VisibilityLayer, sut.Render2D.GeneratedBits);

                TestBits(sut.Physics2D.Node.GetCollisionLayerValue, sut.Physics2D.Node.CollisionLayer, sut.Physics2D.GeneratedBits);
                TestBits(sut.Physics2D.Node.GetCollisionMaskValue, sut.Physics2D.Node.CollisionMask, sut.Physics2D.GeneratedBits);

                TestBits(sut.Navigation2D.Node.GetNavigationLayerValue, sut.Navigation2D.Node.NavigationLayers, sut.Navigation2D.GeneratedBits);
                TestBits(sut.Avoidance2D.Node.GetNavigationLayerValue, sut.Avoidance2D.Node.NavigationLayers, sut.Navigation2D.GeneratedBits);

                TestBits(sut.Avoidance2D.Node.GetAvoidanceLayerValue, sut.Avoidance2D.Node.AvoidanceLayers, sut.Avoidance2D.GeneratedBits);
                TestBits(sut.Avoidance2D.Node.GetAvoidanceMaskValue, sut.Avoidance2D.Node.AvoidanceMask, sut.Avoidance2D.GeneratedBits);
            }

            void Test3D()
            {
                TestBits(sut.Cull3D.Node.GetCullMaskValue, sut.Cull3D.Node.CullMask, sut.Cull3D.GeneratedBits);
                TestBits(sut.Render3D.Node.GetLayerMaskValue, sut.Render3D.Node.Layers, sut.Render3D.GeneratedBits);

                TestBits(sut.Physics3D.Node.GetCollisionLayerValue, sut.Physics3D.Node.CollisionLayer, sut.Physics3D.GeneratedBits);
                TestBits(sut.Physics3D.Node.GetCollisionMaskValue, sut.Physics3D.Node.CollisionMask, sut.Physics3D.GeneratedBits);

                TestBits(sut.Navigation3D.Node.GetNavigationLayerValue, sut.Navigation3D.Node.NavigationLayers, sut.Navigation3D.GeneratedBits);
                TestBits(sut.Avoidance3D.Node.GetNavigationLayerValue, sut.Avoidance3D.Node.NavigationLayers, sut.Navigation3D.GeneratedBits);

                TestBits(sut.Avoidance3D.Node.GetAvoidanceLayerValue, sut.Avoidance3D.Node.AvoidanceLayers, sut.Avoidance3D.GeneratedBits);
                TestBits(sut.Avoidance3D.Node.GetAvoidanceMaskValue, sut.Avoidance3D.Node.AvoidanceMask, sut.Avoidance3D.GeneratedBits);
            }

            void TestBits(Func<int, bool> func, uint layer, int[] generated, [CallerArgumentExpression(nameof(layer))] string LayerName = null)
            {
                LayerName = LayerName.Replace("sut.", "").Replace(".Node.", ".");

                LogLayer(LayerName, layer);
                generated.Should().BeEquivalentTo(sut.ExpectedBits, $"{LayerName} {TestGroup}");

                for (var i = 1; i < 10; ++i)
                {
                    var expected = sut.ExpectedBits.Contains(i);
                    var because = $"{LayerName} {TestGroup} [bit: {i}, expected: {expected}]";

                    LogResult(i, func);
                    func(i).Should().Be(expected, because);
                }
            }

            [Conditional("LOG")]
            static void LogTest(string TestGroup, SUT sut)
                => GD.Print($"{TestGroup} [Expect: {Bits(sut.ExpectedMask)}]");

            [Conditional("LOG")]
            static void LogLayer(string LayerName, uint layer)
                => GD.Print($" - {LayerName} [Actual: {Bits(layer)}]");

            [Conditional("LOG")]
            static void LogResult(int i, Func<int, bool> func)
                => GD.Print($"  -- {i} => {func(i)}");

            static string Bits(uint bits)
                => $"{Convert.ToString(bits, 2).PadLeft(8, '0')} ({bits})";
        }
    }
}
