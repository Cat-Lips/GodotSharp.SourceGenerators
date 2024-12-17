using Godot;

namespace GodotTests.TestScenes
{
	[SceneTree]
	public partial class Test2Arg : Control
	{
		public int A { get; private set; }
		public int B { get; private set; }
		[Export] public int X { get; set; }

		[OnInstantiate]
		private void Initialise(int a, int b)
		{
			A = a;
			B = b;
		}
	}
}
