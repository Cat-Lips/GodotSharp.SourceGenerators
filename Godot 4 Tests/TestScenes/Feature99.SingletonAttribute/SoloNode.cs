using Godot;

namespace GodotTests.TestScenes;

[Singleton]
public partial class SoloNode : Node;

[Singleton(nameof(MyInit))]
public partial class SoloNodeWithInitOverride : Node
{
    public bool MyInitCalled { get; private set; }
    private void MyInit() => MyInitCalled = true;
}
