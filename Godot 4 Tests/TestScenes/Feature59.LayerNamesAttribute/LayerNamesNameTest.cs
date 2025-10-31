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
            MyLayers.Avoidance.WithSpaces12.Should().Be(12);            // layer_12="With Spaces 12"
            MyLayers.Avoidance.WithInvalidChars13.Should().Be(13);      // layer_13="[With \"Invalid\" Chars]?*+. 13"
            MyLayers.Avoidance.WithLeadingSpace14.Should().Be(14);      // layer_14=" With Leading Space 14"
            MyLayers.Avoidance.WithLeading15.Should().Be(15);           // layer_15="_ With Leading _ 15"
            MyLayers.Avoidance.WithLeading16.Should().Be(16);           // layer_16="- With Leading - 16"
            MyLayers.Avoidance._7WithLeadingNumeric17.Should().Be(17);  // layer_17="7 With Leading Numeric 17"
            MyLayers.Avoidance.WithLeading18.Should().Be(18);           // layer_18=". With Leading . 18"
            MyLayers.Avoidance.中文WithLeadingUnicode19.Should().Be(19); // layer_19="中文 With Leading Unicode 19"

            MyLayers.Avoidance.Mask.WithSpaces12.Should().Be(1 << (12 - 1));
            MyLayers.Avoidance.Mask.WithInvalidChars13.Should().Be(1 << (13 - 1));
            MyLayers.Avoidance.Mask.WithLeadingSpace14.Should().Be(1 << (14 - 1));
            MyLayers.Avoidance.Mask.WithLeading15.Should().Be(1 << (15 - 1));
            MyLayers.Avoidance.Mask.WithLeading16.Should().Be(1 << (16 - 1));
            MyLayers.Avoidance.Mask._7WithLeadingNumeric17.Should().Be(1 << (17 - 1));
            MyLayers.Avoidance.Mask.WithLeading18.Should().Be(1 << (18 - 1));
            MyLayers.Avoidance.Mask.中文WithLeadingUnicode19.Should().Be(1 << (19 - 1));

            // As above, so below

            MyStaticLayers.Avoidance.WithSpaces12.Should().Be(12);            // layer_12="With Spaces 12"
            MyStaticLayers.Avoidance.WithInvalidChars13.Should().Be(13);      // layer_13="[With \"Invalid\" Chars]?*+. 13"
            MyStaticLayers.Avoidance.WithLeadingSpace14.Should().Be(14);      // layer_14=" With Leading Space 14"
            MyStaticLayers.Avoidance.WithLeading15.Should().Be(15);           // layer_15="_ With Leading _ 15"
            MyStaticLayers.Avoidance.WithLeading16.Should().Be(16);           // layer_16="- With Leading - 16"
            MyStaticLayers.Avoidance._7WithLeadingNumeric17.Should().Be(17);  // layer_17="7 With Leading Numeric 17"
            MyStaticLayers.Avoidance.WithLeading18.Should().Be(18);           // layer_18=". With Leading . 18"
            MyStaticLayers.Avoidance.中文WithLeadingUnicode19.Should().Be(19); // layer_19="中文 With Leading Unicode 19"

            MyStaticLayers.Avoidance.Mask.WithSpaces12.Should().Be(1 << (12 - 1));
            MyStaticLayers.Avoidance.Mask.WithInvalidChars13.Should().Be(1 << (13 - 1));
            MyStaticLayers.Avoidance.Mask.WithLeadingSpace14.Should().Be(1 << (14 - 1));
            MyStaticLayers.Avoidance.Mask.WithLeading15.Should().Be(1 << (15 - 1));
            MyStaticLayers.Avoidance.Mask.WithLeading16.Should().Be(1 << (16 - 1));
            MyStaticLayers.Avoidance.Mask._7WithLeadingNumeric17.Should().Be(1 << (17 - 1));
            MyStaticLayers.Avoidance.Mask.WithLeading18.Should().Be(1 << (18 - 1));
            MyStaticLayers.Avoidance.Mask.中文WithLeadingUnicode19.Should().Be(1 << (19 - 1));
        }

        void IndexTest()
        {
            TestRender2D();
            TestRender3D();
            TestPhysics2D();
            TestPhysics3D();
            TestNavigation2D();
            TestNavigation3D();
            TestAvoidance();

            void TestRender2D()
            {
                MyLayers.Render2D.TestLayer1.Should().Be(1);
                MyLayers.Render2D.TestLayer3.Should().Be(3);
                MyLayers.Render2D.TestLayer5.Should().Be(5);
                MyLayers.Render2D.TestLayer7.Should().Be(7);
                MyLayers.Render2D.Mask.TestLayer1.Should().Be(1 << (1 - 1));
                MyLayers.Render2D.Mask.TestLayer3.Should().Be(1 << (3 - 1));
                MyLayers.Render2D.Mask.TestLayer5.Should().Be(1 << (5 - 1));
                MyLayers.Render2D.Mask.TestLayer7.Should().Be(1 << (7 - 1));

                // As above, so below

                MyStaticLayers.Render2D.TestLayer1.Should().Be(1);
                MyStaticLayers.Render2D.TestLayer3.Should().Be(3);
                MyStaticLayers.Render2D.TestLayer5.Should().Be(5);
                MyStaticLayers.Render2D.TestLayer7.Should().Be(7);
                MyStaticLayers.Render2D.Mask.TestLayer1.Should().Be(1 << (1 - 1));
                MyStaticLayers.Render2D.Mask.TestLayer3.Should().Be(1 << (3 - 1));
                MyStaticLayers.Render2D.Mask.TestLayer5.Should().Be(1 << (5 - 1));
                MyStaticLayers.Render2D.Mask.TestLayer7.Should().Be(1 << (7 - 1));
            }

            void TestRender3D()
            {
                MyLayers.Render3D.TestLayer1.Should().Be(1);
                MyLayers.Render3D.TestLayer3.Should().Be(3);
                MyLayers.Render3D.TestLayer5.Should().Be(5);
                MyLayers.Render3D.TestLayer7.Should().Be(7);
                MyLayers.Render3D.Mask.TestLayer1.Should().Be(1 << (1 - 1));
                MyLayers.Render3D.Mask.TestLayer3.Should().Be(1 << (3 - 1));
                MyLayers.Render3D.Mask.TestLayer5.Should().Be(1 << (5 - 1));
                MyLayers.Render3D.Mask.TestLayer7.Should().Be(1 << (7 - 1));

                // As above, so below

                MyStaticLayers.Render3D.TestLayer1.Should().Be(1);
                MyStaticLayers.Render3D.TestLayer3.Should().Be(3);
                MyStaticLayers.Render3D.TestLayer5.Should().Be(5);
                MyStaticLayers.Render3D.TestLayer7.Should().Be(7);
                MyStaticLayers.Render3D.Mask.TestLayer1.Should().Be(1 << (1 - 1));
                MyStaticLayers.Render3D.Mask.TestLayer3.Should().Be(1 << (3 - 1));
                MyStaticLayers.Render3D.Mask.TestLayer5.Should().Be(1 << (5 - 1));
                MyStaticLayers.Render3D.Mask.TestLayer7.Should().Be(1 << (7 - 1));
            }

            void TestPhysics2D()
            {
                MyLayers.Physics2D.TestLayer1.Should().Be(1);
                MyLayers.Physics2D.TestLayer3.Should().Be(3);
                MyLayers.Physics2D.TestLayer5.Should().Be(5);
                MyLayers.Physics2D.TestLayer7.Should().Be(7);
                MyLayers.Physics2D.Mask.TestLayer1.Should().Be(1 << (1 - 1));
                MyLayers.Physics2D.Mask.TestLayer3.Should().Be(1 << (3 - 1));
                MyLayers.Physics2D.Mask.TestLayer5.Should().Be(1 << (5 - 1));
                MyLayers.Physics2D.Mask.TestLayer7.Should().Be(1 << (7 - 1));

                // As above, so below

                MyStaticLayers.Physics2D.TestLayer1.Should().Be(1);
                MyStaticLayers.Physics2D.TestLayer3.Should().Be(3);
                MyStaticLayers.Physics2D.TestLayer5.Should().Be(5);
                MyStaticLayers.Physics2D.TestLayer7.Should().Be(7);
                MyStaticLayers.Physics2D.Mask.TestLayer1.Should().Be(1 << (1 - 1));
                MyStaticLayers.Physics2D.Mask.TestLayer3.Should().Be(1 << (3 - 1));
                MyStaticLayers.Physics2D.Mask.TestLayer5.Should().Be(1 << (5 - 1));
                MyStaticLayers.Physics2D.Mask.TestLayer7.Should().Be(1 << (7 - 1));
            }

            void TestPhysics3D()
            {
                MyLayers.Physics3D.TestLayer1.Should().Be(1);
                MyLayers.Physics3D.TestLayer3.Should().Be(3);
                MyLayers.Physics3D.TestLayer5.Should().Be(5);
                MyLayers.Physics3D.TestLayer7.Should().Be(7);
                MyLayers.Physics3D.Mask.TestLayer1.Should().Be(1 << (1 - 1));
                MyLayers.Physics3D.Mask.TestLayer3.Should().Be(1 << (3 - 1));
                MyLayers.Physics3D.Mask.TestLayer5.Should().Be(1 << (5 - 1));
                MyLayers.Physics3D.Mask.TestLayer7.Should().Be(1 << (7 - 1));

                // As above, so below

                MyStaticLayers.Physics3D.TestLayer1.Should().Be(1);
                MyStaticLayers.Physics3D.TestLayer3.Should().Be(3);
                MyStaticLayers.Physics3D.TestLayer5.Should().Be(5);
                MyStaticLayers.Physics3D.TestLayer7.Should().Be(7);
                MyStaticLayers.Physics3D.Mask.TestLayer1.Should().Be(1 << (1 - 1));
                MyStaticLayers.Physics3D.Mask.TestLayer3.Should().Be(1 << (3 - 1));
                MyStaticLayers.Physics3D.Mask.TestLayer5.Should().Be(1 << (5 - 1));
                MyStaticLayers.Physics3D.Mask.TestLayer7.Should().Be(1 << (7 - 1));
            }

            void TestNavigation2D()
            {
                MyLayers.Navigation2D.TestLayer1.Should().Be(1);
                MyLayers.Navigation2D.TestLayer3.Should().Be(3);
                MyLayers.Navigation2D.TestLayer5.Should().Be(5);
                MyLayers.Navigation2D.TestLayer7.Should().Be(7);
                MyLayers.Navigation2D.Mask.TestLayer1.Should().Be(1 << (1 - 1));
                MyLayers.Navigation2D.Mask.TestLayer3.Should().Be(1 << (3 - 1));
                MyLayers.Navigation2D.Mask.TestLayer5.Should().Be(1 << (5 - 1));
                MyLayers.Navigation2D.Mask.TestLayer7.Should().Be(1 << (7 - 1));

                // As above, so below

                MyStaticLayers.Navigation2D.TestLayer1.Should().Be(1);
                MyStaticLayers.Navigation2D.TestLayer3.Should().Be(3);
                MyStaticLayers.Navigation2D.TestLayer5.Should().Be(5);
                MyStaticLayers.Navigation2D.TestLayer7.Should().Be(7);
                MyStaticLayers.Navigation2D.Mask.TestLayer1.Should().Be(1 << (1 - 1));
                MyStaticLayers.Navigation2D.Mask.TestLayer3.Should().Be(1 << (3 - 1));
                MyStaticLayers.Navigation2D.Mask.TestLayer5.Should().Be(1 << (5 - 1));
                MyStaticLayers.Navigation2D.Mask.TestLayer7.Should().Be(1 << (7 - 1));
            }

            void TestNavigation3D()
            {
                MyLayers.Navigation3D.TestLayer1.Should().Be(1);
                MyLayers.Navigation3D.TestLayer3.Should().Be(3);
                MyLayers.Navigation3D.TestLayer5.Should().Be(5);
                MyLayers.Navigation3D.TestLayer7.Should().Be(7);
                MyLayers.Navigation3D.Mask.TestLayer1.Should().Be(1 << (1 - 1));
                MyLayers.Navigation3D.Mask.TestLayer3.Should().Be(1 << (3 - 1));
                MyLayers.Navigation3D.Mask.TestLayer5.Should().Be(1 << (5 - 1));
                MyLayers.Navigation3D.Mask.TestLayer7.Should().Be(1 << (7 - 1));

                // As above, so below

                MyStaticLayers.Navigation3D.TestLayer1.Should().Be(1);
                MyStaticLayers.Navigation3D.TestLayer3.Should().Be(3);
                MyStaticLayers.Navigation3D.TestLayer5.Should().Be(5);
                MyStaticLayers.Navigation3D.TestLayer7.Should().Be(7);
                MyStaticLayers.Navigation3D.Mask.TestLayer1.Should().Be(1 << (1 - 1));
                MyStaticLayers.Navigation3D.Mask.TestLayer3.Should().Be(1 << (3 - 1));
                MyStaticLayers.Navigation3D.Mask.TestLayer5.Should().Be(1 << (5 - 1));
                MyStaticLayers.Navigation3D.Mask.TestLayer7.Should().Be(1 << (7 - 1));
            }

            void TestAvoidance()
            {
                MyLayers.Avoidance.TestLayer1.Should().Be(1);
                MyLayers.Avoidance.TestLayer3.Should().Be(3);
                MyLayers.Avoidance.TestLayer5.Should().Be(5);
                MyLayers.Avoidance.TestLayer7.Should().Be(7);
                MyLayers.Avoidance.Mask.TestLayer1.Should().Be(1 << (1 - 1));
                MyLayers.Avoidance.Mask.TestLayer3.Should().Be(1 << (3 - 1));
                MyLayers.Avoidance.Mask.TestLayer5.Should().Be(1 << (5 - 1));
                MyLayers.Avoidance.Mask.TestLayer7.Should().Be(1 << (7 - 1));

                // As above, so below

                MyStaticLayers.Avoidance.TestLayer1.Should().Be(1);
                MyStaticLayers.Avoidance.TestLayer3.Should().Be(3);
                MyStaticLayers.Avoidance.TestLayer5.Should().Be(5);
                MyStaticLayers.Avoidance.TestLayer7.Should().Be(7);
                MyStaticLayers.Avoidance.Mask.TestLayer1.Should().Be(1 << (1 - 1));
                MyStaticLayers.Avoidance.Mask.TestLayer3.Should().Be(1 << (3 - 1));
                MyStaticLayers.Avoidance.Mask.TestLayer5.Should().Be(1 << (5 - 1));
                MyStaticLayers.Avoidance.Mask.TestLayer7.Should().Be(1 << (7 - 1));
            }
        }
    }
}
