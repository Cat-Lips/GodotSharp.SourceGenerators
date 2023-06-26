using Godot;

namespace GodotTests.TestScenes
{
    [SceneTree]
    public partial class Test3Arg : Control
    {
        public int A { get; private set; }
        public int? B { get; private set; }
        public int? C { get; private set; }
        [Export] public int X { get; set; }

        [OnInstantiate]
        private void Initialise(int a, int? b, int? c = null)
        {
            A = a;
            B = b;
            C = c;
        }
    }
}
