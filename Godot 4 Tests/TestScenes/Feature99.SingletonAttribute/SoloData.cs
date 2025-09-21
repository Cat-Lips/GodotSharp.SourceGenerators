using Godot;

namespace GodotTests.TestScenes;

[Singleton]
public partial class SoloData;

[Singleton]
public partial class SoloDataWithInit
{
    public bool InitCalled { get; private set; }
    private void Init() => InitCalled = true;
}
