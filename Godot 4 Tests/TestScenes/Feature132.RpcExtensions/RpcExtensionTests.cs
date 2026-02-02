using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class RpcExtensionTests : Node, ITest
{
    public enum EnumTest { B, C, D, E }

    private string LastCall { get; set; }

    [Rpc(CallLocal = true)]
    public void My0()
        => LastCall = "0";

    [Rpc(CallLocal = true)]
    private void My1(int a)
        => LastCall = $"1|{a}";

    [Rpc(CallLocal = true)]
    protected void My2(int a, float b)
        => LastCall = $"2|{a}|{b}";

    [Rpc(CallLocal = true)]
    public void My3(int a, float b, EnumTest c)
        => LastCall = $"3|{a}|{b}|{c}";

    [Rpc(CallLocal = true)]
    private void My4(int a = 4, float b = 4.4f, EnumTest c = EnumTest.D)
        => LastCall = $"4|{a}|{b}|{c}";

    [Rpc(CallLocal = false)]
    protected void MyX()
        => LastCall = "X";

    private const BindingFlags All = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

    void ITest.ReadyTests()
    {
        var myMethods = GetType().GetMethods(All)
            .Where(x => x.Name.StartsWith("My"))
            .ToDictionary(x => x.Name, x => x.Attributes & MethodAttributes.MemberAccessMask);

        myMethods.Keys.Should().BeEquivalentTo([
            "My0", "My1", "My2", "My3", "My4", "MyX",
            "My0Rpc", "My1Rpc", "My2Rpc", "My3Rpc", "My4Rpc", "MyXRpc",
            "My0RpcId", "My1RpcId", "My2RpcId", "My3RpcId", "My4RpcId", "MyXRpcId"]);

        foreach (var name in new[] { "My0", "My1", "My2", "My3", "My4", "MyX" })
        {
            var expected = myMethods[name];
            myMethods[name + "Rpc"].Should().Be(expected);
            myMethods[name + "RpcId"].Should().Be(expected);
        }

        Test(My0Rpc, "0");
        Test(() => My1Rpc(1), "1|1");
        Test(() => My2Rpc(2, 2.2f), "2|2|2.2");
        Test(() => My3Rpc(3, 3.3f, EnumTest.C), "3|3|3.3|C");
        Test(() => My4Rpc(), "4|4|4.4|D");
        Test(MyXRpc, null);

        Test(() => My0RpcId(1), "0");
        Test(() => My1RpcId(1, 1), "1|1");
        Test(() => My2RpcId(1, 2, 2.2f), "2|2|2.2");
        Test(() => My3RpcId(1, 3, 3.3f, EnumTest.C), "3|3|3.3|C");
        Test(() => My4RpcId(1), "4|4|4.4|D");

        // Godot logs (and returns) error if calling self when CallLocal is false (uncomment to test)
        //Test(() => MyXRpcId(1), null, ok: false);

        // Godot logs errors for unknown peer id, but doesn't return error! (uncomment to test)
        //Test(() => My0RpcId(2), null/*, ok: false*/);
        //Test(() => My1RpcId(2, 1), null/*, ok: false*/);
        //Test(() => My2RpcId(2, 2, 2.2f), null/*, ok: false*/);
        //Test(() => My3RpcId(2, 3, 3.3f, EnumTest.C), null/*, ok: false*/);
        //Test(() => My4RpcId(2), null/*, ok: false*/);
        //Test(() => MyXRpcId(2), null/*, ok: false*/);

        void Test(Func<Error> sut, string expected, bool ok = true, [CallerArgumentExpression(nameof(sut))] string test = null)
        {
            LastCall = null;
            var err = sut();
            LastCall.Should().Be(expected, test);
            if (ok) err.Should().Be(Error.Ok, test);
            else err.Should().NotBe(Error.Ok, test);
        }
    }
}
