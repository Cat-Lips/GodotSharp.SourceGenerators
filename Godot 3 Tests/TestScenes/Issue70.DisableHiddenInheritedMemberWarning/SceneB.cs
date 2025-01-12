using Godot;

namespace GodotTests.TestScenes;

public partial class SceneB : SceneA
{
	[OnInstantiate] // Success if compiles
	private void Init() { }
}
