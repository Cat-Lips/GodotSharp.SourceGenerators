using Godot;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class TestConstructor_PrivateScope : Control
{
    [OnInstantiate(ctor: "private")]
    private void Init() { }
}
