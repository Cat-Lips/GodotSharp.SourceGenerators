using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes
{
	[SceneTree]
	internal partial class InheritingSceneWithoutScript : Control, ITest
	{
		void ITest.InitTests()
			=> _.Label.Text.Should().Be("Scene Without Script");
	}
}
