using Godot;

namespace GodotTests.ManualTests.CustomClassSyntaxHighlightingVS
{
    public abstract partial class MyCustomClass : Node
    {
        [GodotOverride]
        private void OnEnterTree()
        {

        }
    }
}
