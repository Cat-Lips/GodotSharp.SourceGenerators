using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class UnsupportedInheritedOverride : Spatial, ITest
{
    void ITest.InitTests()
    {
        // If it compiles, test passes
    }
}
