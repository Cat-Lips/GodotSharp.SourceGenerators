using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes
{
    [SceneTree]
    public partial class InputMapAttributeTests : Node, ITest
    {
        void ITest.InitTests()
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
        }
    }
}
