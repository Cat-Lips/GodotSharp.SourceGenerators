using Godot;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class Test0Arg : Control
{
    [Export] public int X { get; set; }
    public bool InitCalled { get; private set; }

    [OnInstantiate]
    private void Init()
        => InitCalled = true;
}
