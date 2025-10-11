using System;
using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class InstantiableAttributeTests : Node, ITest
{
    void ITest.InitTests()
    {
        TestDefaultNoInit.New().Should().NotBeNull().And.BeOfType<TestDefaultNoInit>();
        TestCustomNoInit.New1().Should().NotBeNull().And.BeOfType<TestCustomNoInit>();

        TestDefaultWithInit.New().InitCalled.Should().BeTrue();
        TestCustomWithInit.New1().InitCalled.Should().BeTrue();

        Vector2 b1 = default;
        Vector2? b2 = default;

        TestDefaultWithInit.New(default(char)).InitCalled.Should().BeTrue();
        TestDefaultWithInit.New(default(char?)).InitCalled.Should().BeTrue();
        TestDefaultWithInit.New(default(float)).InitCalled.Should().BeTrue();
        TestDefaultWithInit.New(default(float?)).InitCalled.Should().BeTrue();
        TestDefaultWithInit.New(default(sbyte)).InitCalled.Should().BeTrue();
        TestDefaultWithInit.New(default(sbyte?)).InitCalled.Should().BeTrue();
        TestDefaultWithInit.New(default(InnerEnum)).InitCalled.Should().BeTrue();
        TestDefaultWithInit.New(default(InnerEnum?)).InitCalled.Should().BeTrue();
        TestDefaultWithInit.New(default, ref b1, out var c1).InitCalled.Should().BeTrue();
        TestDefaultWithInit.New(default, ref b2, out var c2).InitCalled.Should().BeTrue();
        TestDefaultWithInit.New(default, default(DateTime)).InitCalled.Should().BeTrue();
        TestDefaultWithInit.New(default, default(DateTime?)).InitCalled.Should().BeTrue();
        TestDefaultWithInit.New(default((DateTime, TimeSpan))).InitCalled.Should().BeTrue();
        TestDefaultWithInit.New(default((DateTime, TimeSpan)?)).InitCalled.Should().BeTrue();

        TestCustomWithInit.New1(default(char)).InitCalled.Should().BeTrue();
        TestCustomWithInit.New1(default(char?)).InitCalled.Should().BeTrue();
        TestCustomWithInit.New1(default(float)).InitCalled.Should().BeTrue();
        TestCustomWithInit.New1(default(float?)).InitCalled.Should().BeTrue();
        TestCustomWithInit.New1(default(sbyte)).InitCalled.Should().BeTrue();
        TestCustomWithInit.New1(default(sbyte?)).InitCalled.Should().BeTrue();
        TestCustomWithInit.New1(default(InnerEnum)).InitCalled.Should().BeTrue();
        TestCustomWithInit.New1(default(InnerEnum?)).InitCalled.Should().BeTrue();
        TestCustomWithInit.New1(default, ref b1, out c1).InitCalled.Should().BeTrue();
        TestCustomWithInit.New1(default, ref b2, out c2).InitCalled.Should().BeTrue();
        TestCustomWithInit.New1(default, default(DateTime)).InitCalled.Should().BeTrue();
        TestCustomWithInit.New1(default, default(DateTime?)).InitCalled.Should().BeTrue();
        TestCustomWithInit.New1(default((DateTime, TimeSpan))).InitCalled.Should().BeTrue();
        TestCustomWithInit.New1(default((DateTime, TimeSpan)?)).InitCalled.Should().BeTrue();
    }

    public enum InnerEnum
    {
        A = 1,
        B = 2,
        C = 3,
    }

    public class InnerData
    {
        public int A { get; } = 1;
        public int B { get; } = 2;
        public int C { get; } = 3;
    }
}
