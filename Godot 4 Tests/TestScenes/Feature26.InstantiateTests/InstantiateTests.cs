using System.Linq;
using System.Reflection;
using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class InstantiateTests : Control, ITest
{
    void ITest.InitTests()
    {
        var sut0 = Test0Arg.Instantiate();
        var sut1 = Test1Arg.Instantiate(1);
        var sut2 = Test2Arg.Instantiate(1, 2);
        var sut3 = Test3Arg.Instantiate(1, 2, 3);
        var sut4 = TestNamespaceArg.Instantiate(new());

        sut0.X.Should().Be(7);
        sut1.A.Should().Be(1); sut1.X.Should().Be(7);
        sut2.A.Should().Be(1); sut2.B.Should().Be(2); sut2.X.Should().Be(7);
        sut3.A.Should().Be(1); sut3.B.Should().Be(2); sut3.C.Should().Be(3); sut3.X.Should().Be(7);
        sut4.Data.A.Should().Be(1); sut4.Data.B.Should().Be(2); sut4.Data.C.Should().Be(3); sut4.X.Should().Be(7);

        var sutNone = TestConstructor_NoneScope.Instantiate();
        var sutPrivate = TestConstructor_PrivateScope.Instantiate();
        var sutProtected = TestConstructor_DefaultProtectedScope.Instantiate();

        sutNone.GetType().GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Should().ContainSingle();
        sutNone.GetType().GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Single().IsPublic.Should().BeTrue();
        sutPrivate.GetType().GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Should().ContainSingle();
        sutPrivate.GetType().GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Single().IsPrivate.Should().BeTrue();
        sutProtected.GetType().GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Should().ContainSingle();
        sutProtected.GetType().GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Single().IsFamily.Should().BeTrue();
    }
}
