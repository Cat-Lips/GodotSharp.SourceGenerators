using Godot;
using GodotSharp.SourceGenerators;

namespace NRT.Tests;

[SceneTree, Instantiable(ctor: Scope.None)]
public partial class TestWithNonNullableNRT : Node
{
    public required string InstantiateValue1;
    public required string InstantiateValue2;
    public string NotifyActionValue = "";

    //public string NonInitialisedValue; // WARNING
    public required string NonInitialisedValue; // No warning

    [Notify]
    public string NotifyTest
    {
        get => _notifyTest.Get();
        set => _notifyTest.Set(value);
    }

    [Notify]
    public string NotifyTestWithAction
    {
        get => _notifyTestWithAction.Get();
        set => _notifyTestWithAction.Set(value, OnNotifyTestWithActionChanged);
    }

    [OnInstantiate(ctor: Scope.None)]
    private void OnInstantiateTest(string value1 = "", string value2 = default!)
    {
        InstantiateValue1 = value1;
        InstantiateValue2 = value2;
    }

    private void OnNotifyTestWithActionChanged()
        => NotifyActionValue = NotifyTestWithAction;

    private TestWithNonNullableNRT()
    {
        InitNotifyTest("");
        InitNotifyTestWithAction("");
    }

    private void Init(string value1 = "", string value2 = default!)
    {
        InstantiateValue1 = value1;
        InstantiateValue2 = value2;
    }
}
