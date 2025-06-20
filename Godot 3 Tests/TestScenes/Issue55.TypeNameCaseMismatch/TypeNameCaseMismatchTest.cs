using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class TypeNameCaseMismatchTest : Node, ITest
{
    void ITest.InitTests()
    {
        // NB: There is no case mismatch in GD3
    }
}
