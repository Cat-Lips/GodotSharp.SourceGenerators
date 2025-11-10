using Godot;
using GodotTests.TestScenes.Data;

namespace GodotTests.TestScenes
{
    [SceneTree]
    public partial class TestNamespaceArg : Control
    {
        [Export] public int X { get; set; }
        public MyData Data { get; private set; }

        [OnInstantiate]
        private void Initialise(MyData data)
            => Data = data;
    }
}

namespace GodotTests.TestScenes.Data
{
    public class MyData
    {
        public int A { get; } = 1;
        public int B { get; } = 2;
        public int C { get; } = 3;
    }
}
