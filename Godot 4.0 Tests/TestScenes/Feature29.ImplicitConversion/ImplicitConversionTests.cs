using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes
{
    [SceneTree]
    public partial class ImplicitConversionTests : Control, ITest
    {
        private Control l1;
        private Control l1_l2;
        private Control l2;
        private Control l2_l2;

        void ITest.InitTests()
        {
            l1 = _.L1.Get();
            l1_l2 = _.L1.L2;
            l2 = _.L2.Get();
            l2_l2 = _.L2.L2;

            l1.Should().NotBeNull().And.BeOfType<Control>();
            l1_l2.Should().NotBeNull().And.BeOfType<Control>();
            l2.Should().NotBeNull().And.BeOfType<Control>();
            l2_l2.Should().NotBeNull().And.BeOfType<Control>();
        }
    }
}
