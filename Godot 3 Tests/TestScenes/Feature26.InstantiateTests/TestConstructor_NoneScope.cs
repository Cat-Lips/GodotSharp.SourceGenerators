using Godot;

namespace GodotTests.TestScenes
{
    [SceneTree]
    public partial class TestConstructor_NoneScope : Control
    {
        [OnInstantiate(ctor: "none")]
        private void Init() { }
    }
}
