using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes
{
    [SceneTree]
    public partial class TypeNameCaseMismatchTest : Node, ITest
    {
        // Native name: GPUParticles3D
        // C# name:     GpuParticles3D
        void ITest.InitTests()
        {
            // If it compiles, test passes
        }
    }
}
