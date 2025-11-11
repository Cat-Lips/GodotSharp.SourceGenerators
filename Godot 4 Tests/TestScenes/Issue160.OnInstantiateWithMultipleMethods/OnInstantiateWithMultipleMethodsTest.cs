using System.Linq;
using System.Reflection;
using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;
using GodotTests.TestScenes.Issue160;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class OnInstantiateWithMultipleMethodsTest : Node, ITest
{
    void ITest.ReadyTests()
    {
        TestSut1();
        TestSut2();

        static void TestSut1()
        {
            var a = SUT1.Instantiate(1);
            var b = SUT1.Instantiate(2, 3);
            var x = SUT1.Instantiate();

            a.A.Should().Be(1);
            a.B.Should().Be(default);
            a.X.Should().Be(default);

            b.A.Should().Be(2);
            b.B.Should().Be(3);
            b.X.Should().Be(default);

            x.A.Should().Be(default);
            x.B.Should().Be(default);
            x.X.Should().Be(true);

            const BindingFlags All = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            typeof(SUT1).GetConstructors(All).Single().IsFamily.Should().Be(true); // protected
        }

        static void TestSut2()
        {
            var a = SUT2.Instantiate(1);
            var b = SUT2.Instantiate(2, 3);
            var x = SUT2.Instantiate();

            a.A.Should().Be(1);
            a.B.Should().Be(default);
            a.X.Should().Be(default);

            b.A.Should().Be(2);
            b.B.Should().Be(3);
            b.X.Should().Be(default);

            x.A.Should().Be(default);
            x.B.Should().Be(default);
            x.X.Should().Be(true);

            const BindingFlags All = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            typeof(SUT2).GetConstructors(All).Single().IsPrivate.Should().Be(true); // this is the only change
        }
    }
}
