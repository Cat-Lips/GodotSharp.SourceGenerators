using Godot;
using GodotSharp.SourceGenerators;

namespace GodotTests.TestScenes.Issue160;

public partial class SUT2 : Node
{
    public int A;
    public int B;
    public bool X;

    [OnInstantiate(Scope.Private)] private void Init(int a) => A = a;
    [OnInstantiate(true)] private void Init(int a, int b) { A = a; B = b; }
    [OnInstantiate(false)] private void Init() => X = true;
}
