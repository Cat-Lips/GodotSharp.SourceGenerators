using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class InputMapAttributeTests : Node, ITest
{
    void ITest.InitTests()
    {
        TestDefault();
        TestCustom();
        TestInput();

        static void TestDefault()
        {
            MyInput.MoveLeft1.Should().Be((StringName)"MoveLeft1");
            MyInput.MoveLeft2.Should().Be((StringName)"Move Left 2");
            MyInput.MoveLeft3.Should().Be((StringName)"Move_Left 3");
            MyInput.MoveLeft4.Should().Be((StringName)"Move-Left 4");
            MyInput.Move.Left5.Should().Be((StringName)"Move.Left 5");
            MyInput.MoveLeft6.Should().Be((StringName)"move left 6");
            MyInput.MoveLeft7.Should().Be((StringName)"move_left 7");
            MyInput.MoveLeft8.Should().Be((StringName)"move-left 8");
            MyInput.Move.Left9.Should().Be((StringName)"move.left 9");
            MyInput._InvalidChars10.Should().Be((StringName)"'? - InvalidChars 10 ");
            MyInput._中文UnicodeChars11.Should().Be((StringName)"中文 - UnicodeChars 11 ");
            MyInput._中文._Combined12.Should().Be((StringName)"中文 . '? - Combined 12 ");
            MyInput._9InvalidStartChar13.Should().Be((StringName)"9 - InvalidStartChar 13 ");
            MyInput.InvalidStartChar14.Should().Be((StringName)"_ - InvalidStartChar 14 ");
            MyInput.InvalidStartChar15.Should().Be((StringName)"- - InvalidStartChar 15 ");
            MyInput.InvalidStartChar16.Should().Be((StringName)"  - InvalidStartChar 16 ");
            MyInput.ActionWithInput.Should().Be((StringName)"ActionWithInput");
            MyInput.Nested.Action.Should().Be((StringName)"Nested.Action");
            MyInput.Nested.DoubleNestedAction.Should().Be((StringName)"Nested.Double.Nested.Action"); // ie, no double nesting

            // As above, so below

            MyStaticInput.MoveLeft1.Should().Be((StringName)"MoveLeft1");
            MyStaticInput.MoveLeft2.Should().Be((StringName)"Move Left 2");
            MyStaticInput.MoveLeft3.Should().Be((StringName)"Move_Left 3");
            MyStaticInput.MoveLeft4.Should().Be((StringName)"Move-Left 4");
            MyStaticInput.Move.Left5.Should().Be((StringName)"Move.Left 5");
            MyStaticInput.MoveLeft6.Should().Be((StringName)"move left 6");
            MyStaticInput.MoveLeft7.Should().Be((StringName)"move_left 7");
            MyStaticInput.MoveLeft8.Should().Be((StringName)"move-left 8");
            MyStaticInput.Move.Left9.Should().Be((StringName)"move.left 9");
            MyStaticInput._InvalidChars10.Should().Be((StringName)"'? - InvalidChars 10 ");
            MyStaticInput._中文UnicodeChars11.Should().Be((StringName)"中文 - UnicodeChars 11 ");
            MyStaticInput._中文._Combined12.Should().Be((StringName)"中文 . '? - Combined 12 ");
            MyStaticInput._9InvalidStartChar13.Should().Be((StringName)"9 - InvalidStartChar 13 ");
            MyStaticInput.InvalidStartChar14.Should().Be((StringName)"_ - InvalidStartChar 14 ");
            MyStaticInput.InvalidStartChar15.Should().Be((StringName)"- - InvalidStartChar 15 ");
            MyStaticInput.InvalidStartChar16.Should().Be((StringName)"  - InvalidStartChar 16 ");
            MyStaticInput.ActionWithInput.Should().Be((StringName)"ActionWithInput");
            MyStaticInput.Nested.Action.Should().Be((StringName)"Nested.Action");
            MyStaticInput.Nested.DoubleNestedAction.Should().Be((StringName)"Nested.Double.Nested.Action"); // ie, no double nesting
        }

        static void TestCustom()
        {
            MyGameInput.MoveLeft1.Action.Should().Be((StringName)"MoveLeft1");
            MyGameInput.MoveLeft2.Action.Should().Be((StringName)"Move Left 2");
            MyGameInput.MoveLeft3.Action.Should().Be((StringName)"Move_Left 3");
            MyGameInput.MoveLeft4.Action.Should().Be((StringName)"Move-Left 4");
            MyGameInput.Move.Left5.Action.Should().Be((StringName)"Move.Left 5");
            MyGameInput.MoveLeft6.Action.Should().Be((StringName)"move left 6");
            MyGameInput.MoveLeft7.Action.Should().Be((StringName)"move_left 7");
            MyGameInput.MoveLeft8.Action.Should().Be((StringName)"move-left 8");
            MyGameInput.Move.Left9.Action.Should().Be((StringName)"move.left 9");
            MyGameInput._InvalidChars10.Action.Should().Be((StringName)"'? - InvalidChars 10 ");
            MyGameInput._中文UnicodeChars11.Action.Should().Be((StringName)"中文 - UnicodeChars 11 ");
            MyGameInput._中文._Combined12.Action.Should().Be((StringName)"中文 . '? - Combined 12 ");
            MyGameInput._9InvalidStartChar13.Action.Should().Be((StringName)"9 - InvalidStartChar 13 ");
            MyGameInput.InvalidStartChar14.Action.Should().Be((StringName)"_ - InvalidStartChar 14 ");
            MyGameInput.InvalidStartChar15.Action.Should().Be((StringName)"- - InvalidStartChar 15 ");
            MyGameInput.InvalidStartChar16.Action.Should().Be((StringName)"  - InvalidStartChar 16 ");
            MyGameInput.ActionWithInput.Action.Should().Be((StringName)"ActionWithInput");
            MyGameInput.Nested.Action.Action.Should().Be((StringName)"Nested.Action");
            MyGameInput.Nested.DoubleNestedAction.Action.Should().Be((StringName)"Nested.Double.Nested.Action"); // ie, no double nesting

            // As above, so below

            MyStaticGameInput.MoveLeft1.Action.Should().Be((StringName)"MoveLeft1");
            MyStaticGameInput.MoveLeft2.Action.Should().Be((StringName)"Move Left 2");
            MyStaticGameInput.MoveLeft3.Action.Should().Be((StringName)"Move_Left 3");
            MyStaticGameInput.MoveLeft4.Action.Should().Be((StringName)"Move-Left 4");
            MyStaticGameInput.Move.Left5.Action.Should().Be((StringName)"Move.Left 5");
            MyStaticGameInput.MoveLeft6.Action.Should().Be((StringName)"move left 6");
            MyStaticGameInput.MoveLeft7.Action.Should().Be((StringName)"move_left 7");
            MyStaticGameInput.MoveLeft8.Action.Should().Be((StringName)"move-left 8");
            MyStaticGameInput.Move.Left9.Action.Should().Be((StringName)"move.left 9");
            MyStaticGameInput._InvalidChars10.Action.Should().Be((StringName)"'? - InvalidChars 10 ");
            MyStaticGameInput._中文UnicodeChars11.Action.Should().Be((StringName)"中文 - UnicodeChars 11 ");
            MyStaticGameInput._中文._Combined12.Action.Should().Be((StringName)"中文 . '? - Combined 12 ");
            MyStaticGameInput._9InvalidStartChar13.Action.Should().Be((StringName)"9 - InvalidStartChar 13 ");
            MyStaticGameInput.InvalidStartChar14.Action.Should().Be((StringName)"_ - InvalidStartChar 14 ");
            MyStaticGameInput.InvalidStartChar15.Action.Should().Be((StringName)"- - InvalidStartChar 15 ");
            MyStaticGameInput.InvalidStartChar16.Action.Should().Be((StringName)"  - InvalidStartChar 16 ");
            MyStaticGameInput.ActionWithInput.Action.Should().Be((StringName)"ActionWithInput");
            MyStaticGameInput.Nested.Action.Action.Should().Be((StringName)"Nested.Action");
            MyStaticGameInput.Nested.DoubleNestedAction.Action.Should().Be((StringName)"Nested.Double.Nested.Action"); // ie, no double nesting
        }

        static void TestInput()
        {
            TestInput(MyGameInput.MoveLeft1);
            TestInput(MyGameInput.MoveLeft2);
            TestInput(MyGameInput.MoveLeft3);
            TestInput(MyGameInput.MoveLeft4);
            TestInput(MyGameInput.Move.Left5);
            TestInput(MyGameInput.MoveLeft6);
            TestInput(MyGameInput.MoveLeft7);
            TestInput(MyGameInput.MoveLeft8);
            TestInput(MyGameInput.Move.Left9);
            TestInput(MyGameInput._InvalidChars10);
            TestInput(MyGameInput._中文UnicodeChars11);
            TestInput(MyGameInput._中文._Combined12);
            TestInput(MyGameInput._9InvalidStartChar13);
            TestInput(MyGameInput.InvalidStartChar14);
            TestInput(MyGameInput.InvalidStartChar15);
            TestInput(MyGameInput.InvalidStartChar16);
            TestInput(MyGameInput.ActionWithInput);
            TestInput(MyGameInput.Nested.Action);
            TestInput(MyGameInput.Nested.DoubleNestedAction); // ie, no double nesting

            // As above, so below

            //TestInput(MyStaticGameInput.MoveLeft1);
            //TestInput(MyStaticGameInput.MoveLeft2);
            //TestInput(MyStaticGameInput.MoveLeft3);
            //TestInput(MyStaticGameInput.MoveLeft4);
            //TestInput(MyStaticGameInput.Move.Left5);
            //TestInput(MyStaticGameInput.MoveLeft6);
            //TestInput(MyStaticGameInput.MoveLeft7);
            //TestInput(MyStaticGameInput.MoveLeft8);
            //TestInput(MyStaticGameInput.Move.Left9);
            //TestInput(MyStaticGameInput._InvalidChars10);
            //TestInput(MyStaticGameInput._中文UnicodeChars11);
            //TestInput(MyStaticGameInput._中文._Combined12);
            //TestInput(MyStaticGameInput._9InvalidStartChar13);
            //TestInput(MyStaticGameInput.InvalidStartChar14);
            //TestInput(MyStaticGameInput.InvalidStartChar15);
            //TestInput(MyStaticGameInput.InvalidStartChar16);
            //TestInput(MyStaticGameInput.ActionWithInput);
            //TestInput(MyStaticGameInput.Nested.Action);
            //TestInput(MyStaticGameInput.Nested.DoubleNestedAction); // ie, no double nesting

            static void TestInput(GameInput x)
            {
                x.IsPressed().Should().BeFalse(because: $"Default [{x.Action}]");
                x.IsJustPressed().Should().BeFalse(because: $"Default [{x.Action}]");
                x.IsJustReleased().Should().BeFalse(because: $"Default [{x.Action}]");
                x.GetStrength().Should().Be(0, because: $"Default [{x.Action}]");

                Input.ActionPress(x.Action, .7f);

                x.IsPressed().Should().BeTrue(because: $"ActionPress: {x.Action}");
                x.IsJustPressed().Should().BeTrue(because: $"ActionPress: {x.Action}");
                x.IsJustReleased().Should().BeFalse(because: $"ActionPress: {x.Action}");
                x.GetStrength().Should().Be(.7f, because: $"ActionPress: {x.Action}");

                Input.ActionRelease(x.Action);

                x.IsPressed().Should().BeFalse(because: $"ActionRelease: {x.Action}");
                //x.IsJustPressed().Should().BeFalse(because: $"ActionRelease: {x.Action}"); // Godot Bug?
                x.IsJustReleased().Should().BeTrue(because: $"ActionRelease: {x.Action}");
                x.GetStrength().Should().Be(0f, because: $"ActionRelease: {x.Action}");
            }
        }
    }
}
