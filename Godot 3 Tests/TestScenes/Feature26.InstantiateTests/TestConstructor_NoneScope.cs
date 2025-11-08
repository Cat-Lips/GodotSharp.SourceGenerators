using Godot;
using GodotSharp.SourceGenerators;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class TestConstructor_NoneScope : Control
{
    [OnInstantiate(ctor: Scope.None)]
    private void Init() { }
}
