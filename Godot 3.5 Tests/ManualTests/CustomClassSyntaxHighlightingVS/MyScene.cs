using Godot;

namespace GodotTests.ManualTests.CustomClassSyntaxHighlightingVS
{
	[SceneTree]
	public partial class MyScene : Node
	{
		// Test passes if MyCustomClass is syntax highlighted in Visual Studio 2019 (with default settings)
		protected MyCustomClass classShouldBeSyntaxHighlighted; // Passes in Visual Studio 2022

		[GodotOverride]
		private void OnReady()
			=> classShouldBeSyntaxHighlighted = _.MyCustomClass;
	}
}
