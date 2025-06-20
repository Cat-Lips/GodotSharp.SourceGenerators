//#define LOG

using System;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

using SUT = (
    uint ExpectedMask,
    int[] ExpectedBits,
    (Light2D Node, int[] GeneratedBits) Light2D,
    (Camera Node, int[] GeneratedBits) Camera3D,
    (CanvasItem Node, int[] GeneratedBits) Render2D,
    (VisualInstance Node, int[] GeneratedBits) Render3D,
    (CollisionObject2D Node, int[] GeneratedBits) Physics2D,
    (CollisionObject Node, int[] GeneratedBits) Physics3D,
    (Navigation2D Node, int[] GeneratedBits) Navigation2D,
    (Navigation Node, int[] GeneratedBits) Navigation3D);

[SceneTree]
public partial class LayerNamesIndexTest : Node, ITest
{
    private SUT WithLayer1Set => (0b_0000_0001, [1],
        (_.WithLayer1Set.Light2D, [MyLayers.Render2D.TestLayer1]),
        (_.WithLayer1Set.Camera3D, [MyLayers.Render3D.TestLayer1]),
        (_.WithLayer1Set.Render2D, [MyLayers.Render2D.TestLayer1]),
        (_.WithLayer1Set.Render3D, [MyLayers.Render3D.TestLayer1]),
        (_.WithLayer1Set.Physics2D, [MyLayers.Physics2D.TestLayer1]),
        (_.WithLayer1Set.Physics3D, [MyLayers.Physics3D.TestLayer1]),
        (_.WithLayer1Set.Navigation2D, [MyLayers.Navigation2D.TestLayer1]),
        (_.WithLayer1Set.Navigation3D, [MyLayers.Navigation3D.TestLayer1]));

    private SUT WithLayer3Set => (0b_0000_0100, [3],
        (_.WithLayer3Set.Light2D, [MyLayers.Render2D.TestLayer3]),
        (_.WithLayer3Set.Camera3D, [MyLayers.Render3D.TestLayer3]),
        (_.WithLayer3Set.Render2D, [MyLayers.Render2D.TestLayer3]),
        (_.WithLayer3Set.Render3D, [MyLayers.Render3D.TestLayer3]),
        (_.WithLayer3Set.Physics2D, [MyLayers.Physics2D.TestLayer3]),
        (_.WithLayer3Set.Physics3D, [MyLayers.Physics3D.TestLayer3]),
        (_.WithLayer3Set.Navigation2D, [MyLayers.Navigation2D.TestLayer3]),
        (_.WithLayer3Set.Navigation3D, [MyLayers.Navigation3D.TestLayer3]));

    private SUT WithLayer57Set => (0b_0101_0000, [5, 7],
        (_.WithLayer57Set.Light2D, [MyLayers.Render2D.TestLayer5, MyLayers.Render2D.TestLayer7]),
        (_.WithLayer57Set.Camera3D, [MyLayers.Render3D.TestLayer5, MyLayers.Render3D.TestLayer7]),
        (_.WithLayer57Set.Render2D, [MyLayers.Render2D.TestLayer5, MyLayers.Render2D.TestLayer7]),
        (_.WithLayer57Set.Render3D, [MyLayers.Render3D.TestLayer5, MyLayers.Render3D.TestLayer7]),
        (_.WithLayer57Set.Physics2D, [MyLayers.Physics2D.TestLayer5, MyLayers.Physics2D.TestLayer7]),
        (_.WithLayer57Set.Physics3D, [MyLayers.Physics3D.TestLayer5, MyLayers.Physics3D.TestLayer7]),
        (_.WithLayer57Set.Navigation2D, [MyLayers.Navigation2D.TestLayer5, MyLayers.Navigation2D.TestLayer7]),
        (_.WithLayer57Set.Navigation3D, [MyLayers.Navigation3D.TestLayer5, MyLayers.Navigation3D.TestLayer7]));

    private SUT WithNoLayerSet => (0b_0000_0000, [],
        (_.WithNoLayerSet.Light2D, []),
        (_.WithNoLayerSet.Camera3D, []),
        (_.WithNoLayerSet.Render2D, []),
        (_.WithNoLayerSet.Render3D, []),
        (_.WithNoLayerSet.Physics2D, []),
        (_.WithNoLayerSet.Physics3D, []),
        (_.WithNoLayerSet.Navigation2D, []),
        (_.WithNoLayerSet.Navigation3D, []));

    void ITest.InitTests()
    {
        Test(WithLayer1Set, nameof(WithLayer1Set));
        Test(WithLayer3Set, nameof(WithLayer3Set));
        Test(WithLayer57Set, nameof(WithLayer57Set));
        Test(WithNoLayerSet, nameof(WithNoLayerSet));

        static void Test(SUT sut, string TestGroup)
        {
            LogTest(TestGroup, sut);

            Test2D();
            Test3D();

            void Test2D()
            {
                // WARNING: No GetMask/LayerBit function in Godot 3.6!!!
                //TestBits(sut.Light2D.Node.GetItemCullMaskBit, sut.Light2D.Node.RangeItemCullMask, sut.Light2D.GeneratedBits, "Light2D.RangeItemCullMask");
                //TestBits(sut.Light2D.Node.GetItemShadowCullMaskBit, sut.Light2D.Node.ShadowItemCullMask, sut.Light2D.GeneratedBits, "Light2D.ShadowItemCullMask");
                //TestBits(sut.Render2D.Node.GetVisibilityLayerBit, sut.Render2D.Node.LightMask, sut.Render2D.GeneratedBits, "Render2D.LightMask");
                //TestBits(sut.Physics2D.Node.GetVisibilityLayerBit, sut.Physics2D.Node.LightMask, sut.Render2D.GeneratedBits, "Physics2D.LightMask");
                //TestBits(sut.Navigation2D.Node.GetVisibilityLayerBit, sut.Navigation2D.Node.LightMask, sut.Render2D.GeneratedBits, "Navigation2D.LightMask");

                // WARNING: All GetMask/LayerBit functions are offset by 1 in Godot 3.6!!!
                TestBits(x => sut.Physics2D.Node.GetCollisionLayerBit(x - 1), sut.Physics2D.Node.CollisionLayer, sut.Physics2D.GeneratedBits, "Physics2D.CollisionLayer");
                TestBits(x => sut.Physics2D.Node.GetCollisionMaskBit(x - 1), sut.Physics2D.Node.CollisionMask, sut.Physics2D.GeneratedBits, "Physics2D.CollisionMask");

                // WARNING: No GetMask/LayerBit function in Godot 3.6!!!
                //TestBits(sut.Navigation2D.Node.GetNavigationLayerBit, sut.Navigation2D.Node.NavigationLayers, sut.Navigation2D.GeneratedBits, "Navigation2D.NavigationLayers");
            }

            void Test3D()
            {
                // WARNING: All GetMask/LayerBit functions are offset by 1 in Godot 3.6!!!
                TestBits(x => sut.Camera3D.Node.GetCullMaskBit(x - 1), sut.Camera3D.Node.CullMask, sut.Camera3D.GeneratedBits, "Camera3D.CullMask");
                TestBits(x => sut.Render3D.Node.GetLayerMaskBit(x - 1), sut.Render3D.Node.Layers, sut.Render3D.GeneratedBits, "Render3D.Layers");

                // WARNING: All GetMask/LayerBit functions are offset by 1 in Godot 3.6!!!
                TestBits(x => sut.Physics3D.Node.GetCollisionLayerBit(x - 1), sut.Physics3D.Node.CollisionLayer, sut.Physics3D.GeneratedBits, "Physics3D.CollisionLayer");
                TestBits(x => sut.Physics3D.Node.GetCollisionMaskBit(x - 1), sut.Physics3D.Node.CollisionMask, sut.Physics3D.GeneratedBits, "Physics3D.CollisionMask");

                // WARNING: No GetMask/LayerBit function in Godot 3.6!!!
                //TestBits(sut.Navigation3D.Node.GetNavigationLayerBit, sut.Navigation3D.Node.NavigationLayers, sut.Navigation3D.GeneratedBits, "Navigation3D.NavigationLayers");
            }

            void TestBits(Func<int, bool> func, uint layer, int[] generated, string LayerName)
            {
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
