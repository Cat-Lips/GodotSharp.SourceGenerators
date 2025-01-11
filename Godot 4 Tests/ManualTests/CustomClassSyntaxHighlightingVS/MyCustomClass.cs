using Godot;

namespace GodotTests.ManualTests.CustomClassSyntaxHighlightingVS;

public partial class MyCustomClass : Node
{
    public override partial void _EnterTree();

    [GodotOverride]
    private void OnEnterTree()
    {

    }
}
