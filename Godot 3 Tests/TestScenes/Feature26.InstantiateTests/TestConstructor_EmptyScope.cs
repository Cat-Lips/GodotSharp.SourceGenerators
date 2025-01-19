using Godot;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class TestConstructor_EmptyScope : Control
{
	[OnInstantiate(ctor: "")]
	private void Init() { }
}
