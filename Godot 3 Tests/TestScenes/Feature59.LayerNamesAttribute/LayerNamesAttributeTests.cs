using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes
{
    [SceneTree]
    public partial class LayerNamesAttributeTests : Node, ITest
    {
        void ITest.InitTests()
        {
            MyLayers.Render2D.Layer0.Should().Be(0);
            MyLayers.Render3D.Layer0.Should().Be(0);
            MyLayers.Physics2D.Layer0.Should().Be(0);
            MyLayers.Physics3D.Layer0.Should().Be(0);
            MyLayers.Navigation2D.Layer0.Should().Be(0);
            MyLayers.Navigation3D.Layer0.Should().Be(0);
            //MyLayers.Avoidance.Layer0.Should().Be(0);

            MyLayers.Render2D.Mask.Layer0.Should().Be(1 << 0);
            MyLayers.Render3D.Mask.Layer0.Should().Be(1 << 0);
            MyLayers.Physics2D.Mask.Layer0.Should().Be(1 << 0);
            MyLayers.Physics3D.Mask.Layer0.Should().Be(1 << 0);
            MyLayers.Navigation2D.Mask.Layer0.Should().Be(1 << 0);
            MyLayers.Navigation3D.Mask.Layer0.Should().Be(1 << 0);
            //MyLayers.Avoidance.Mask.Layer0.Should().Be(1 << 0);

            MyLayers.Render2D.WithSpaces1.Should().Be(1);             // "With Spaces 1"
            MyLayers.Render2D._With_Invalid_Chars_2.Should().Be(2);   // "[With \"Invalid\" Chars]?*+. 2 "
            MyLayers.Render2D.WithLeadingSpace3.Should().Be(3);       // " With Leading Space 3 "
            MyLayers.Render2D.WithLeading4.Should().Be(4);            // "_ With Leading _ 4 "
            MyLayers.Render2D.WithLeading5.Should().Be(5);            // "- With Leading - 5 "
            MyLayers.Render2D._9WithLeadingNumeric6.Should().Be(6);   // "9 With Leading Numeric 6 "
            MyLayers.Render2D._WithLeading_7.Should().Be(7);          // ". With Leading . 7 "
            MyLayers.Render2D._中文WithLeadingUnicode8.Should().Be(8); // "中文 With Leading Unicode 8 "

            MyLayers.Render2D.Mask.WithSpaces1.Should().Be(1 << 1);
            MyLayers.Render2D.Mask._With_Invalid_Chars_2.Should().Be(1 << 2);
            MyLayers.Render2D.Mask.WithLeadingSpace3.Should().Be(1 << 3);
            MyLayers.Render2D.Mask.WithLeading4.Should().Be(1 << 4);
            MyLayers.Render2D.Mask.WithLeading5.Should().Be(1 << 5);
            MyLayers.Render2D.Mask._9WithLeadingNumeric6.Should().Be(1 << 6);
            MyLayers.Render2D.Mask._WithLeading_7.Should().Be(1 << 7);
            MyLayers.Render2D.Mask._中文WithLeadingUnicode8.Should().Be(1 << 8);

            // As above, so below

            MyStaticLayers.Render2D.Layer0.Should().Be(0);
            MyStaticLayers.Render3D.Layer0.Should().Be(0);
            MyStaticLayers.Physics2D.Layer0.Should().Be(0);
            MyStaticLayers.Physics3D.Layer0.Should().Be(0);
            MyStaticLayers.Navigation2D.Layer0.Should().Be(0);
            MyStaticLayers.Navigation3D.Layer0.Should().Be(0);
            //MyStaticLayers.Avoidance.Layer0.Should().Be(0);

            MyStaticLayers.Render2D.Mask.Layer0.Should().Be(1 << 0);
            MyStaticLayers.Render3D.Mask.Layer0.Should().Be(1 << 0);
            MyStaticLayers.Physics2D.Mask.Layer0.Should().Be(1 << 0);
            MyStaticLayers.Physics3D.Mask.Layer0.Should().Be(1 << 0);
            MyStaticLayers.Navigation2D.Mask.Layer0.Should().Be(1 << 0);
            MyStaticLayers.Navigation3D.Mask.Layer0.Should().Be(1 << 0);
            //MyStaticLayers.Avoidance.Mask.Layer0.Should().Be(1 << 0);

            MyStaticLayers.Render2D.WithSpaces1.Should().Be(1);             // "With Spaces 1"
            MyStaticLayers.Render2D._With_Invalid_Chars_2.Should().Be(2);   // "[With \"Invalid\" Chars]?*+. 2 "
            MyStaticLayers.Render2D.WithLeadingSpace3.Should().Be(3);       // " With Leading Space 3 "
            MyStaticLayers.Render2D.WithLeading4.Should().Be(4);            // "_ With Leading _ 4 "
            MyStaticLayers.Render2D.WithLeading5.Should().Be(5);            // "- With Leading - 5 "
            MyStaticLayers.Render2D._9WithLeadingNumeric6.Should().Be(6);   // "9 With Leading Numeric 6 "
            MyStaticLayers.Render2D._WithLeading_7.Should().Be(7);          // ". With Leading . 7 "
            MyStaticLayers.Render2D._中文WithLeadingUnicode8.Should().Be(8); // "中文 With Leading Unicode 8 "

            MyStaticLayers.Render2D.Mask.WithSpaces1.Should().Be(1 << 1);
            MyStaticLayers.Render2D.Mask._With_Invalid_Chars_2.Should().Be(1 << 2);
            MyStaticLayers.Render2D.Mask.WithLeadingSpace3.Should().Be(1 << 3);
            MyStaticLayers.Render2D.Mask.WithLeading4.Should().Be(1 << 4);
            MyStaticLayers.Render2D.Mask.WithLeading5.Should().Be(1 << 5);
            MyStaticLayers.Render2D.Mask._9WithLeadingNumeric6.Should().Be(1 << 6);
            MyStaticLayers.Render2D.Mask._WithLeading_7.Should().Be(1 << 7);
            MyStaticLayers.Render2D.Mask._中文WithLeadingUnicode8.Should().Be(1 << 8);
        }
    }
}
