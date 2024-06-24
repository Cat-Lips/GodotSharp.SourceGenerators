using Godot;

namespace NRT.Tests
{
    [SceneTree]
    public partial class Main : Node
    {
        [GodotOverride]
        private void OnReady()
        {
            AddChild(TestNode.Instantiate(null));
            AddChild(TestNode.Instantiate("not null"));

            GD.Print("TEST COMPLETE");
            GetTree().Quit();
        }
    }
}
