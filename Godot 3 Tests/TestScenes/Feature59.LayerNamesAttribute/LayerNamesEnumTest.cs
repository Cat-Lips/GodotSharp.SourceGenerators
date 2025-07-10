using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class LayerNamesEnumTest : Node, ITest
{
    void ITest.InitTests()
    {
        NameTest();
        IndexTest();

        void NameTest()
        {
            ((int)MyLayers.Navigation3DLayer.WithSpaces12).Should().Be(12);            // layer_12="With Spaces 12"
            ((int)MyLayers.Navigation3DLayer._With_Invalid_Chars_13).Should().Be(13);  // layer_13="[With \"Invalid\" Chars]?*+. 13"
            ((int)MyLayers.Navigation3DLayer.WithLeadingSpace14).Should().Be(14);      // layer_14=" With Leading Space 14"
            ((int)MyLayers.Navigation3DLayer.WithLeading15).Should().Be(15);           // layer_15="_ With Leading _ 15"
            ((int)MyLayers.Navigation3DLayer.WithLeading16).Should().Be(16);           // layer_16="- With Leading - 16"
            ((int)MyLayers.Navigation3DLayer._7WithLeadingNumeric17).Should().Be(17);  // layer_17="7 With Leading Numeric 17"
            ((int)MyLayers.Navigation3DLayer._WithLeading_18).Should().Be(18);         // layer_18=". With Leading . 18"
            ((int)MyLayers.Navigation3DLayer._中文WithLeadingUnicode19).Should().Be(19); // layer_19="中文 With Leading Unicode 19"

            ((uint)MyLayers.Navigation3DLayerMask.WithSpaces12).Should().Be(1 << (12 - 1));
            ((uint)MyLayers.Navigation3DLayerMask._With_Invalid_Chars_13).Should().Be(1 << (13 - 1));
            ((uint)MyLayers.Navigation3DLayerMask.WithLeadingSpace14).Should().Be(1 << (14 - 1));
            ((uint)MyLayers.Navigation3DLayerMask.WithLeading15).Should().Be(1 << (15 - 1));
            ((uint)MyLayers.Navigation3DLayerMask.WithLeading16).Should().Be(1 << (16 - 1));
            ((uint)MyLayers.Navigation3DLayerMask._7WithLeadingNumeric17).Should().Be(1 << (17 - 1));
            ((uint)MyLayers.Navigation3DLayerMask._WithLeading_18).Should().Be(1 << (18 - 1));
            ((uint)MyLayers.Navigation3DLayerMask._中文WithLeadingUnicode19).Should().Be(1 << (19 - 1));

            // As above, so below

            ((uint)MyStaticLayers.Navigation3DLayer.WithSpaces12).Should().Be(12);            // layer_12="With Spaces 12"
            ((uint)MyStaticLayers.Navigation3DLayer._With_Invalid_Chars_13).Should().Be(13);  // layer_13="[With \"Invalid\" Chars]?*+. 13"
            ((uint)MyStaticLayers.Navigation3DLayer.WithLeadingSpace14).Should().Be(14);      // layer_14=" With Leading Space 14"
            ((uint)MyStaticLayers.Navigation3DLayer.WithLeading15).Should().Be(15);           // layer_15="_ With Leading _ 15"
            ((uint)MyStaticLayers.Navigation3DLayer.WithLeading16).Should().Be(16);           // layer_16="- With Leading - 16"
            ((uint)MyStaticLayers.Navigation3DLayer._7WithLeadingNumeric17).Should().Be(17);  // layer_17="7 With Leading Numeric 17"
            ((uint)MyStaticLayers.Navigation3DLayer._WithLeading_18).Should().Be(18);         // layer_18=". With Leading . 18"
            ((uint)MyStaticLayers.Navigation3DLayer._中文WithLeadingUnicode19).Should().Be(19); // layer_19="中文 With Leading Unicode 19"

            ((uint)MyStaticLayers.Navigation3DLayerMask.WithSpaces12).Should().Be(1 << (12 - 1));
            ((uint)MyStaticLayers.Navigation3DLayerMask._With_Invalid_Chars_13).Should().Be(1 << (13 - 1));
            ((uint)MyStaticLayers.Navigation3DLayerMask.WithLeadingSpace14).Should().Be(1 << (14 - 1));
            ((uint)MyStaticLayers.Navigation3DLayerMask.WithLeading15).Should().Be(1 << (15 - 1));
            ((uint)MyStaticLayers.Navigation3DLayerMask.WithLeading16).Should().Be(1 << (16 - 1));
            ((uint)MyStaticLayers.Navigation3DLayerMask._7WithLeadingNumeric17).Should().Be(1 << (17 - 1));
            ((uint)MyStaticLayers.Navigation3DLayerMask._WithLeading_18).Should().Be(1 << (18 - 1));
            ((uint)MyStaticLayers.Navigation3DLayerMask._中文WithLeadingUnicode19).Should().Be(1 << (19 - 1));

        }

        void IndexTest()
        {
            TestRender2D();
            TestRender3D();
            TestPhysics2D();
            TestPhysics3D();
            TestNavigation2D();
            TestNavigation3D();

            void TestRender2D()
            {
                ((int)MyLayers.Render2DLayer.TestLayer1).Should().Be(1);
                ((int)MyLayers.Render2DLayer.TestLayer3).Should().Be(3);
                ((int)MyLayers.Render2DLayer.TestLayer5).Should().Be(5);
                ((int)MyLayers.Render2DLayer.TestLayer7).Should().Be(7);
                ((int)MyLayers.Render2DLayerMask.TestLayer1).Should().Be(1 << (1 - 1));
                ((int)MyLayers.Render2DLayerMask.TestLayer3).Should().Be(1 << (3 - 1));
                ((int)MyLayers.Render2DLayerMask.TestLayer5).Should().Be(1 << (5 - 1));
                ((int)MyLayers.Render2DLayerMask.TestLayer7).Should().Be(1 << (7 - 1));

                // As above, so below

                ((uint)MyStaticLayers.Render2DLayer.TestLayer1).Should().Be(1);
                ((uint)MyStaticLayers.Render2DLayer.TestLayer3).Should().Be(3);
                ((uint)MyStaticLayers.Render2DLayer.TestLayer5).Should().Be(5);
                ((uint)MyStaticLayers.Render2DLayer.TestLayer7).Should().Be(7);
                ((uint)MyStaticLayers.Render2DLayerMask.TestLayer1).Should().Be(1 << (1 - 1));
                ((uint)MyStaticLayers.Render2DLayerMask.TestLayer3).Should().Be(1 << (3 - 1));
                ((uint)MyStaticLayers.Render2DLayerMask.TestLayer5).Should().Be(1 << (5 - 1));
                ((uint)MyStaticLayers.Render2DLayerMask.TestLayer7).Should().Be(1 << (7 - 1));
            }

            void TestRender3D()
            {
                ((int)MyLayers.Render3DLayer.TestLayer1).Should().Be(1);
                ((int)MyLayers.Render3DLayer.TestLayer3).Should().Be(3);
                ((int)MyLayers.Render3DLayer.TestLayer5).Should().Be(5);
                ((int)MyLayers.Render3DLayer.TestLayer7).Should().Be(7);
                ((int)MyLayers.Render3DLayerMask.TestLayer1).Should().Be(1 << (1 - 1));
                ((int)MyLayers.Render3DLayerMask.TestLayer3).Should().Be(1 << (3 - 1));
                ((int)MyLayers.Render3DLayerMask.TestLayer5).Should().Be(1 << (5 - 1));
                ((int)MyLayers.Render3DLayerMask.TestLayer7).Should().Be(1 << (7 - 1));

                // As above, so below

                ((uint)MyStaticLayers.Render3DLayer.TestLayer1).Should().Be(1);
                ((uint)MyStaticLayers.Render3DLayer.TestLayer3).Should().Be(3);
                ((uint)MyStaticLayers.Render3DLayer.TestLayer5).Should().Be(5);
                ((uint)MyStaticLayers.Render3DLayer.TestLayer7).Should().Be(7);
                ((uint)MyStaticLayers.Render3DLayerMask.TestLayer1).Should().Be(1 << (1 - 1));
                ((uint)MyStaticLayers.Render3DLayerMask.TestLayer3).Should().Be(1 << (3 - 1));
                ((uint)MyStaticLayers.Render3DLayerMask.TestLayer5).Should().Be(1 << (5 - 1));
                ((uint)MyStaticLayers.Render3DLayerMask.TestLayer7).Should().Be(1 << (7 - 1));
            }

            void TestPhysics2D()
            {
                ((int)MyLayers.Physics2DLayer.TestLayer1).Should().Be(1);
                ((int)MyLayers.Physics2DLayer.TestLayer3).Should().Be(3);
                ((int)MyLayers.Physics2DLayer.TestLayer5).Should().Be(5);
                ((int)MyLayers.Physics2DLayer.TestLayer7).Should().Be(7);
                ((int)MyLayers.Physics2DLayerMask.TestLayer1).Should().Be(1 << (1 - 1));
                ((int)MyLayers.Physics2DLayerMask.TestLayer3).Should().Be(1 << (3 - 1));
                ((int)MyLayers.Physics2DLayerMask.TestLayer5).Should().Be(1 << (5 - 1));
                ((int)MyLayers.Physics2DLayerMask.TestLayer7).Should().Be(1 << (7 - 1));

                // As above, so below

                ((uint)MyStaticLayers.Physics2DLayer.TestLayer1).Should().Be(1);
                ((uint)MyStaticLayers.Physics2DLayer.TestLayer3).Should().Be(3);
                ((uint)MyStaticLayers.Physics2DLayer.TestLayer5).Should().Be(5);
                ((uint)MyStaticLayers.Physics2DLayer.TestLayer7).Should().Be(7);
                ((uint)MyStaticLayers.Physics2DLayerMask.TestLayer1).Should().Be(1 << (1 - 1));
                ((uint)MyStaticLayers.Physics2DLayerMask.TestLayer3).Should().Be(1 << (3 - 1));
                ((uint)MyStaticLayers.Physics2DLayerMask.TestLayer5).Should().Be(1 << (5 - 1));
                ((uint)MyStaticLayers.Physics2DLayerMask.TestLayer7).Should().Be(1 << (7 - 1));
            }

            void TestPhysics3D()
            {
                ((int)MyLayers.Physics3DLayer.TestLayer1).Should().Be(1);
                ((int)MyLayers.Physics3DLayer.TestLayer3).Should().Be(3);
                ((int)MyLayers.Physics3DLayer.TestLayer5).Should().Be(5);
                ((int)MyLayers.Physics3DLayer.TestLayer7).Should().Be(7);
                ((int)MyLayers.Physics3DLayerMask.TestLayer1).Should().Be(1 << (1 - 1));
                ((int)MyLayers.Physics3DLayerMask.TestLayer3).Should().Be(1 << (3 - 1));
                ((int)MyLayers.Physics3DLayerMask.TestLayer5).Should().Be(1 << (5 - 1));
                ((int)MyLayers.Physics3DLayerMask.TestLayer7).Should().Be(1 << (7 - 1));

                // As above, so below

                ((uint)MyStaticLayers.Physics3DLayer.TestLayer1).Should().Be(1);
                ((uint)MyStaticLayers.Physics3DLayer.TestLayer3).Should().Be(3);
                ((uint)MyStaticLayers.Physics3DLayer.TestLayer5).Should().Be(5);
                ((uint)MyStaticLayers.Physics3DLayer.TestLayer7).Should().Be(7);
                ((uint)MyStaticLayers.Physics3DLayerMask.TestLayer1).Should().Be(1 << (1 - 1));
                ((uint)MyStaticLayers.Physics3DLayerMask.TestLayer3).Should().Be(1 << (3 - 1));
                ((uint)MyStaticLayers.Physics3DLayerMask.TestLayer5).Should().Be(1 << (5 - 1));
                ((uint)MyStaticLayers.Physics3DLayerMask.TestLayer7).Should().Be(1 << (7 - 1));
            }

            void TestNavigation2D()
            {
                ((int)MyLayers.Navigation2DLayer.TestLayer1).Should().Be(1);
                ((int)MyLayers.Navigation2DLayer.TestLayer3).Should().Be(3);
                ((int)MyLayers.Navigation2DLayer.TestLayer5).Should().Be(5);
                ((int)MyLayers.Navigation2DLayer.TestLayer7).Should().Be(7);
                ((int)MyLayers.Navigation2DLayerMask.TestLayer1).Should().Be(1 << (1 - 1));
                ((int)MyLayers.Navigation2DLayerMask.TestLayer3).Should().Be(1 << (3 - 1));
                ((int)MyLayers.Navigation2DLayerMask.TestLayer5).Should().Be(1 << (5 - 1));
                ((int)MyLayers.Navigation2DLayerMask.TestLayer7).Should().Be(1 << (7 - 1));

                // As above, so below

                ((uint)MyStaticLayers.Navigation2DLayer.TestLayer1).Should().Be(1);
                ((uint)MyStaticLayers.Navigation2DLayer.TestLayer3).Should().Be(3);
                ((uint)MyStaticLayers.Navigation2DLayer.TestLayer5).Should().Be(5);
                ((uint)MyStaticLayers.Navigation2DLayer.TestLayer7).Should().Be(7);
                ((uint)MyStaticLayers.Navigation2DLayerMask.TestLayer1).Should().Be(1 << (1 - 1));
                ((uint)MyStaticLayers.Navigation2DLayerMask.TestLayer3).Should().Be(1 << (3 - 1));
                ((uint)MyStaticLayers.Navigation2DLayerMask.TestLayer5).Should().Be(1 << (5 - 1));
                ((uint)MyStaticLayers.Navigation2DLayerMask.TestLayer7).Should().Be(1 << (7 - 1));
            }

            void TestNavigation3D()
            {
                ((int)MyLayers.Navigation3DLayer.TestLayer1).Should().Be(1);
                ((int)MyLayers.Navigation3DLayer.TestLayer3).Should().Be(3);
                ((int)MyLayers.Navigation3DLayer.TestLayer5).Should().Be(5);
                ((int)MyLayers.Navigation3DLayer.TestLayer7).Should().Be(7);
                ((int)MyLayers.Navigation3DLayerMask.TestLayer1).Should().Be(1 << (1 - 1));
                ((int)MyLayers.Navigation3DLayerMask.TestLayer3).Should().Be(1 << (3 - 1));
                ((int)MyLayers.Navigation3DLayerMask.TestLayer5).Should().Be(1 << (5 - 1));
                ((int)MyLayers.Navigation3DLayerMask.TestLayer7).Should().Be(1 << (7 - 1));

                // As above, so below

                ((uint)MyStaticLayers.Navigation3DLayer.TestLayer1).Should().Be(1);
                ((uint)MyStaticLayers.Navigation3DLayer.TestLayer3).Should().Be(3);
                ((uint)MyStaticLayers.Navigation3DLayer.TestLayer5).Should().Be(5);
                ((uint)MyStaticLayers.Navigation3DLayer.TestLayer7).Should().Be(7);
                ((uint)MyStaticLayers.Navigation3DLayerMask.TestLayer1).Should().Be(1 << (1 - 1));
                ((uint)MyStaticLayers.Navigation3DLayerMask.TestLayer3).Should().Be(1 << (3 - 1));
                ((uint)MyStaticLayers.Navigation3DLayerMask.TestLayer5).Should().Be(1 << (5 - 1));
                ((uint)MyStaticLayers.Navigation3DLayerMask.TestLayer7).Should().Be(1 << (7 - 1));
            }
        }
    }
}
