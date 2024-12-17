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
			MyInput.MoveLeft1.Should().Be("MoveLeft1");
			MyInput.MoveLeft2.Should().Be("Move Left 2");
			MyInput.MoveLeft3.Should().Be("Move_Left 3");
			MyInput.MoveLeft4.Should().Be("Move-Left 4");
			MyInput.Move.Left5.Should().Be("Move.Left 5");
			MyInput.MoveLeft6.Should().Be("move left 6");
			MyInput.MoveLeft7.Should().Be("move_left 7");
			MyInput.MoveLeft8.Should().Be("move-left 8");
			MyInput.Move.Left9.Should().Be("move.left 9");
			MyInput._InvalidChars10.Should().Be("'? - InvalidChars 10 ");
			MyInput._中文UnicodeChars11.Should().Be("中文 - UnicodeChars 11 ");
			MyInput._中文._Combined12.Should().Be("中文 . '? - Combined 12 ");
			MyInput._9InvalidStartChar13.Should().Be("9 - InvalidStartChar 13 ");
			MyInput.InvalidStartChar14.Should().Be("_ - InvalidStartChar 14 ");
			MyInput.InvalidStartChar15.Should().Be("- - InvalidStartChar 15 ");
			MyInput.InvalidStartChar16.Should().Be("  - InvalidStartChar 16 ");
			MyInput.ActionWithInput.Should().Be("ActionWithInput");
			MyInput.Nested.Action.Should().Be("Nested.Action");
			MyInput.Nested.DoubleNestedAction.Should().Be("Nested.Double.Nested.Action"); // ie, no double nesting

			// As above, so below

			MyStaticInput.MoveLeft1.Should().Be("MoveLeft1");
			MyStaticInput.MoveLeft2.Should().Be("Move Left 2");
			MyStaticInput.MoveLeft3.Should().Be("Move_Left 3");
			MyStaticInput.MoveLeft4.Should().Be("Move-Left 4");
			MyStaticInput.Move.Left5.Should().Be("Move.Left 5");
			MyStaticInput.MoveLeft6.Should().Be("move left 6");
			MyStaticInput.MoveLeft7.Should().Be("move_left 7");
			MyStaticInput.MoveLeft8.Should().Be("move-left 8");
			MyStaticInput.Move.Left9.Should().Be("move.left 9");
			MyStaticInput._InvalidChars10.Should().Be("'? - InvalidChars 10 ");
			MyStaticInput._中文UnicodeChars11.Should().Be("中文 - UnicodeChars 11 ");
			MyStaticInput._中文._Combined12.Should().Be("中文 . '? - Combined 12 ");
			MyStaticInput._9InvalidStartChar13.Should().Be("9 - InvalidStartChar 13 ");
			MyStaticInput.InvalidStartChar14.Should().Be("_ - InvalidStartChar 14 ");
			MyStaticInput.InvalidStartChar15.Should().Be("- - InvalidStartChar 15 ");
			MyStaticInput.InvalidStartChar16.Should().Be("  - InvalidStartChar 16 ");
			MyStaticInput.ActionWithInput.Should().Be("ActionWithInput");
			MyStaticInput.Nested.Action.Should().Be("Nested.Action");
			MyStaticInput.Nested.DoubleNestedAction.Should().Be("Nested.Double.Nested.Action"); // ie, no double nesting
		}
	}
}
