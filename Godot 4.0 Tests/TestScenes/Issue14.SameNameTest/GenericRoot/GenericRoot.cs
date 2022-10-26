using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes
{
    [SceneTree]
    internal partial class GenericRoot : GenericRoot<int>, ITest
    {
        void ITest.InitTests()
        {

        }
    }
}
