using Godot;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class TestConstructor_DefaultProtectedScope : Control
{
	[OnInstantiate]
	private void Init() { }
}
