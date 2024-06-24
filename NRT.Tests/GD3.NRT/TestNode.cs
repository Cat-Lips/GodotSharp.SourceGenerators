using Godot;

namespace NRT.Tests
{
    [SceneTree]
    public partial class TestNode : Node
    {
        //[Notify] public string? NotifyTest { get; set; }

        [OnInstantiate] private void OnInstantiateTest(string? _) { }
    }
}
