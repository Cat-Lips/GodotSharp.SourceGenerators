using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class LayerNamesNameTest : Node, ITest
{
    void ITest.InitTests()
    {
        NameTest();
        IndexTest();

        void NameTest()
        {
            MyLayers.Navigation3D.WithSpaces12.Should().Be(12);            // layer_12="With Spaces 12"
            MyLayers.Navigation3D._With_Invalid_Chars_13.Should().Be(13);  // layer_13="[With \"Invalid\" Chars]?*+. 13"
            MyLayers.Navigation3D.WithLeadingSpace14.Should().Be(14);      // layer_14=" With Leading Space 14"
            MyLayers.Navigation3D.WithLeading15.Should().Be(15);           // layer_15="_ With Leading _ 15"
            MyLayers.Navigation3D.WithLeading16.Should().Be(16);           // layer_16="- With Leading - 16"
            MyLayers.Navigation3D._7WithLeadingNumeric17.Should().Be(17);  // layer_17="7 With Leading Numeric 17"
            MyLayers.Navigation3D._WithLeading_18.Should().Be(18);         // layer_18=". With Leading . 18"
            MyLayers.Navigation3D._中文WithLeadingUnicode19.Should().Be(19); // layer_19="中文 With Leading Unicode 19"

            MyLayers.Navigation3D.Mask.WithSpaces12.Should().Be(1 << 12);
            MyLayers.Navigation3D.Mask._With_Invalid_Chars_13.Should().Be(1 << 13);
            MyLayers.Navigation3D.Mask.WithLeadingSpace14.Should().Be(1 << 14);
            MyLayers.Navigation3D.Mask.WithLeading15.Should().Be(1 << 15);
            MyLayers.Navigation3D.Mask.WithLeading16.Should().Be(1 << 16);
            MyLayers.Navigation3D.Mask._7WithLeadingNumeric17.Should().Be(1 << 17);
            MyLayers.Navigation3D.Mask._WithLeading_18.Should().Be(1 << 18);
            MyLayers.Navigation3D.Mask._中文WithLeadingUnicode19.Should().Be(1 << 19);

            // As above, so below

            MyStaticLayers.Navigation3D.WithSpaces12.Should().Be(12);            // layer_12="With Spaces 12"
            MyStaticLayers.Navigation3D._With_Invalid_Chars_13.Should().Be(13);  // layer_13="[With \"Invalid\" Chars]?*+. 13"
            MyStaticLayers.Navigation3D.WithLeadingSpace14.Should().Be(14);      // layer_14=" With Leading Space 14"
            MyStaticLayers.Navigation3D.WithLeading15.Should().Be(15);           // layer_15="_ With Leading _ 15"
            MyStaticLayers.Navigation3D.WithLeading16.Should().Be(16);           // layer_16="- With Leading - 16"
            MyStaticLayers.Navigation3D._7WithLeadingNumeric17.Should().Be(17);  // layer_17="7 With Leading Numeric 17"
            MyStaticLayers.Navigation3D._WithLeading_18.Should().Be(18);         // layer_18=". With Leading . 18"
            MyStaticLayers.Navigation3D._中文WithLeadingUnicode19.Should().Be(19); // layer_19="中文 With Leading Unicode 19"

            MyStaticLayers.Navigation3D.Mask.WithSpaces12.Should().Be(1 << 12);
            MyStaticLayers.Navigation3D.Mask._With_Invalid_Chars_13.Should().Be(1 << 13);
            MyStaticLayers.Navigation3D.Mask.WithLeadingSpace14.Should().Be(1 << 14);
            MyStaticLayers.Navigation3D.Mask.WithLeading15.Should().Be(1 << 15);
            MyStaticLayers.Navigation3D.Mask.WithLeading16.Should().Be(1 << 16);
            MyStaticLayers.Navigation3D.Mask._7WithLeadingNumeric17.Should().Be(1 << 17);
            MyStaticLayers.Navigation3D.Mask._WithLeading_18.Should().Be(1 << 18);
            MyStaticLayers.Navigation3D.Mask._中文WithLeadingUnicode19.Should().Be(1 << 19);
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
                MyLayers.Render2D.TestLayer1.Should().Be(1);
                MyLayers.Render2D.TestLayer3.Should().Be(3);
                MyLayers.Render2D.TestLayer5.Should().Be(5);
                MyLayers.Render2D.TestLayer7.Should().Be(7);
                MyLayers.Render2D.Mask.TestLayer1.Should().Be(1 << 1);
                MyLayers.Render2D.Mask.TestLayer3.Should().Be(1 << 3);
                MyLayers.Render2D.Mask.TestLayer5.Should().Be(1 << 5);
                MyLayers.Render2D.Mask.TestLayer7.Should().Be(1 << 7);

                // As above, so below

                MyStaticLayers.Render2D.TestLayer1.Should().Be(1);
                MyStaticLayers.Render2D.TestLayer3.Should().Be(3);
                MyStaticLayers.Render2D.TestLayer5.Should().Be(5);
                MyStaticLayers.Render2D.TestLayer7.Should().Be(7);
                MyStaticLayers.Render2D.Mask.TestLayer1.Should().Be(1 << 1);
                MyStaticLayers.Render2D.Mask.TestLayer3.Should().Be(1 << 3);
                MyStaticLayers.Render2D.Mask.TestLayer5.Should().Be(1 << 5);
                MyStaticLayers.Render2D.Mask.TestLayer7.Should().Be(1 << 7);
            }

            void TestRender3D()
            {
                MyLayers.Render3D.TestLayer1.Should().Be(1);
                MyLayers.Render3D.TestLayer3.Should().Be(3);
                MyLayers.Render3D.TestLayer5.Should().Be(5);
                MyLayers.Render3D.TestLayer7.Should().Be(7);
                MyLayers.Render3D.Mask.TestLayer1.Should().Be(1 << 1);
                MyLayers.Render3D.Mask.TestLayer3.Should().Be(1 << 3);
                MyLayers.Render3D.Mask.TestLayer5.Should().Be(1 << 5);
                MyLayers.Render3D.Mask.TestLayer7.Should().Be(1 << 7);

                // As above, so below

                MyStaticLayers.Render3D.TestLayer1.Should().Be(1);
                MyStaticLayers.Render3D.TestLayer3.Should().Be(3);
                MyStaticLayers.Render3D.TestLayer5.Should().Be(5);
                MyStaticLayers.Render3D.TestLayer7.Should().Be(7);
                MyStaticLayers.Render3D.Mask.TestLayer1.Should().Be(1 << 1);
                MyStaticLayers.Render3D.Mask.TestLayer3.Should().Be(1 << 3);
                MyStaticLayers.Render3D.Mask.TestLayer5.Should().Be(1 << 5);
                MyStaticLayers.Render3D.Mask.TestLayer7.Should().Be(1 << 7);
            }

            void TestPhysics2D()
            {
                MyLayers.Physics2D.TestLayer1.Should().Be(1);
                MyLayers.Physics2D.TestLayer3.Should().Be(3);
                MyLayers.Physics2D.TestLayer5.Should().Be(5);
                MyLayers.Physics2D.TestLayer7.Should().Be(7);
                MyLayers.Physics2D.Mask.TestLayer1.Should().Be(1 << 1);
                MyLayers.Physics2D.Mask.TestLayer3.Should().Be(1 << 3);
                MyLayers.Physics2D.Mask.TestLayer5.Should().Be(1 << 5);
                MyLayers.Physics2D.Mask.TestLayer7.Should().Be(1 << 7);

                // As above, so below

                MyStaticLayers.Physics2D.TestLayer1.Should().Be(1);
                MyStaticLayers.Physics2D.TestLayer3.Should().Be(3);
                MyStaticLayers.Physics2D.TestLayer5.Should().Be(5);
                MyStaticLayers.Physics2D.TestLayer7.Should().Be(7);
                MyStaticLayers.Physics2D.Mask.TestLayer1.Should().Be(1 << 1);
                MyStaticLayers.Physics2D.Mask.TestLayer3.Should().Be(1 << 3);
                MyStaticLayers.Physics2D.Mask.TestLayer5.Should().Be(1 << 5);
                MyStaticLayers.Physics2D.Mask.TestLayer7.Should().Be(1 << 7);
            }

            void TestPhysics3D()
            {
                MyLayers.Physics3D.TestLayer1.Should().Be(1);
                MyLayers.Physics3D.TestLayer3.Should().Be(3);
                MyLayers.Physics3D.TestLayer5.Should().Be(5);
                MyLayers.Physics3D.TestLayer7.Should().Be(7);
                MyLayers.Physics3D.Mask.TestLayer1.Should().Be(1 << 1);
                MyLayers.Physics3D.Mask.TestLayer3.Should().Be(1 << 3);
                MyLayers.Physics3D.Mask.TestLayer5.Should().Be(1 << 5);
                MyLayers.Physics3D.Mask.TestLayer7.Should().Be(1 << 7);

                // As above, so below

                MyStaticLayers.Physics3D.TestLayer1.Should().Be(1);
                MyStaticLayers.Physics3D.TestLayer3.Should().Be(3);
                MyStaticLayers.Physics3D.TestLayer5.Should().Be(5);
                MyStaticLayers.Physics3D.TestLayer7.Should().Be(7);
                MyStaticLayers.Physics3D.Mask.TestLayer1.Should().Be(1 << 1);
                MyStaticLayers.Physics3D.Mask.TestLayer3.Should().Be(1 << 3);
                MyStaticLayers.Physics3D.Mask.TestLayer5.Should().Be(1 << 5);
                MyStaticLayers.Physics3D.Mask.TestLayer7.Should().Be(1 << 7);
            }

            void TestNavigation2D()
            {
                MyLayers.Navigation2D.TestLayer1.Should().Be(1);
                MyLayers.Navigation2D.TestLayer3.Should().Be(3);
                MyLayers.Navigation2D.TestLayer5.Should().Be(5);
                MyLayers.Navigation2D.TestLayer7.Should().Be(7);
                MyLayers.Navigation2D.Mask.TestLayer1.Should().Be(1 << 1);
                MyLayers.Navigation2D.Mask.TestLayer3.Should().Be(1 << 3);
                MyLayers.Navigation2D.Mask.TestLayer5.Should().Be(1 << 5);
                MyLayers.Navigation2D.Mask.TestLayer7.Should().Be(1 << 7);

                // As above, so below

                MyStaticLayers.Navigation2D.TestLayer1.Should().Be(1);
                MyStaticLayers.Navigation2D.TestLayer3.Should().Be(3);
                MyStaticLayers.Navigation2D.TestLayer5.Should().Be(5);
                MyStaticLayers.Navigation2D.TestLayer7.Should().Be(7);
                MyStaticLayers.Navigation2D.Mask.TestLayer1.Should().Be(1 << 1);
                MyStaticLayers.Navigation2D.Mask.TestLayer3.Should().Be(1 << 3);
                MyStaticLayers.Navigation2D.Mask.TestLayer5.Should().Be(1 << 5);
                MyStaticLayers.Navigation2D.Mask.TestLayer7.Should().Be(1 << 7);
            }

            void TestNavigation3D()
            {
                MyLayers.Navigation3D.TestLayer1.Should().Be(1);
                MyLayers.Navigation3D.TestLayer3.Should().Be(3);
                MyLayers.Navigation3D.TestLayer5.Should().Be(5);
                MyLayers.Navigation3D.TestLayer7.Should().Be(7);
                MyLayers.Navigation3D.Mask.TestLayer1.Should().Be(1 << 1);
                MyLayers.Navigation3D.Mask.TestLayer3.Should().Be(1 << 3);
                MyLayers.Navigation3D.Mask.TestLayer5.Should().Be(1 << 5);
                MyLayers.Navigation3D.Mask.TestLayer7.Should().Be(1 << 7);

                // As above, so below

                MyStaticLayers.Navigation3D.TestLayer1.Should().Be(1);
                MyStaticLayers.Navigation3D.TestLayer3.Should().Be(3);
                MyStaticLayers.Navigation3D.TestLayer5.Should().Be(5);
                MyStaticLayers.Navigation3D.TestLayer7.Should().Be(7);
                MyStaticLayers.Navigation3D.Mask.TestLayer1.Should().Be(1 << 1);
                MyStaticLayers.Navigation3D.Mask.TestLayer3.Should().Be(1 << 3);
                MyStaticLayers.Navigation3D.Mask.TestLayer5.Should().Be(1 << 5);
                MyStaticLayers.Navigation3D.Mask.TestLayer7.Should().Be(1 << 7);
            }
        }
    }
}
