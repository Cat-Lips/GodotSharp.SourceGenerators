using Godot;

namespace GodotTests.TestScenes
{
    [SceneTree]
    public partial class TestConstructor_NullScope : Control
    {
        [OnInstantiate(ctor: null)]
        private void Init() { }
    }
}
