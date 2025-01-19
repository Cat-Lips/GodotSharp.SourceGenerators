using Godot;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class Test1Arg : Control
{
	public int A { get; private set; }
	[Export] public int X { get; set; }

	[OnInstantiate]
	private void Initialise(int a)
		=> A = a;
}
