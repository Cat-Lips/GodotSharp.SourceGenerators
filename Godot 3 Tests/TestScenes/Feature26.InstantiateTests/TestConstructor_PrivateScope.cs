using Godot;
using GodotSharp.SourceGenerators;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class TestConstructor_PrivateScope : Control
{
    [OnInstantiate(ctor: Scope.Private)]
    private void Init() { }
}
