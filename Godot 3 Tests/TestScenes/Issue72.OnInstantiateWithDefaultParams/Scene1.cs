using Godot;

namespace GodotTests.TestScenes.Issue72;

public partial class Scene1 : Node
{
    public enum TestEnum { a, b, c }
    public struct TestStruct { public TestEnum x; }

    public int i1;
    public int? i2;
    public bool b1;
    public bool? b2;
    public float f1;
    public float? f2;
    public string s1;
    public string s2;
    public TestEnum e1;
    public TestEnum? e2;
    public TestStruct x1;
    public TestStruct? x2;
    public Scene2.TestEnum ee1;
    public Scene2.TestEnum? ee2;
    public Scene2.TestStruct xx1;
    public Scene2.TestStruct? xx2;

    [OnInstantiate]
    private void Init(
        int i1, int? i2,
        bool b1, bool? b2,
        float f1, float? f2,
        string s1, string s2,
        TestEnum e1, TestEnum? e2,
        in TestStruct x1, in TestStruct? x2,
        Scene2.TestEnum ee1, Scene2.TestEnum? ee2,
        in Scene2.TestStruct xx1, in Scene2.TestStruct? xx2)
    {
        this.i1 = i1;
        this.i2 = i2;
        this.b1 = b1;
        this.b2 = b2;
        this.f1 = f1;
        this.f2 = f2;
        this.s1 = s1;
        this.s2 = s2;
        this.e1 = e1;
        this.e2 = e2;
        this.x1 = x1;
        this.x2 = x2;
        this.ee1 = ee1;
        this.ee2 = ee2;
        this.xx1 = xx1;
        this.xx2 = xx2;
    }
}
