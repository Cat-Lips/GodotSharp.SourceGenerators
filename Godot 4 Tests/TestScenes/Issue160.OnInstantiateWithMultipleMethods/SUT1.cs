using Godot;

namespace GodotTests.TestScenes.Issue160;

public partial class SUT1 : Node
{
    public int A;
    public int B;
    public bool X;

    [OnInstantiate] private void Init(int a) => A = a;
    [OnInstantiate(true)] private void Init(int a, int b) { A = a; B = b; }
    [OnInstantiate(false)] private void Init() => X = true;
}
