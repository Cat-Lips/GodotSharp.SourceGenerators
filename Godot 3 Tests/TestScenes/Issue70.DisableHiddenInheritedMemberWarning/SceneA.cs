using Godot;

namespace GodotTests.TestScenes
{
	public partial class SceneA : Node
	{
		[OnInstantiate] // Success if compiles
		private void Init() { }
	}
}
