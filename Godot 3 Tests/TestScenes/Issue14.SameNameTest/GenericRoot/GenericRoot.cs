using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
internal abstract partial class GenericRoot : GenericRoot<int>, ITest
{
    void ITest.InitTests()
    {

    }
}
