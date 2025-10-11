using System;
using System.Collections.Generic;
using Godot;

namespace GodotTests.TestScenes;

using TestData = InstantiableAttributeTests.InnerData;
using TestEnum = InstantiableAttributeTests.InnerEnum;

[Instantiable("Init1", "New1", null)]
public partial class TestCustomWithInit : Node
{
    public bool InitCalled { get; private set; }

    private void Init1() => InitCalled = true;

    private void Init1(char a = 'a', string b = "b", bool c = true) => InitCalled = true;
    private void Init1(char? a = 'a', string b = "b", bool? c = true) => InitCalled = true;

    private void Init1(float a = .1f, double b = .1, decimal c = .1m) => InitCalled = true;
    private void Init1(float? a = .1f, double? b = .1, decimal? c = .1m) => InitCalled = true;

    private void Init1(sbyte a = 1, byte b = 1, short c = 1, ushort d = 1,
        int e = 1, uint f = 1, long g = 1, ulong h = 1) => InitCalled = true;
    private void Init1(sbyte? a = 1, byte? b = 1, short? c = 1, ushort? d = 1,
        int? e = 1, uint? f = 1, long? g = 1, ulong? h = 1) => InitCalled = true;

    private void Init1(TestEnum a = TestEnum.C, TestData b = null, Action<int> c = null) => InitCalled = true;
    private void Init1(TestEnum? a = TestEnum.C, TestData b = null, Action<int> c = null) => InitCalled = true;

    private void Init1(in Vector2 a, ref Vector2 b, out bool c, params string[] d) => c = InitCalled = true;
    private void Init1(in Vector2? a, ref Vector2? b, out bool? c, params IEnumerable<string> d) => c = InitCalled = true;

    private void Init1(in Vector2 a = default, in DateTime b = default, in TimeSpan c = default) => InitCalled = true;
    private void Init1(in Vector2? a = default, in DateTime? b = default, in TimeSpan? c = default) => InitCalled = true;

    private void Init1(in (DateTime, TimeSpan) a = default, in (DateTime X, TimeSpan Y) b = default) => InitCalled = true;
    private void Init1(in (DateTime, TimeSpan)? a = default, in (DateTime X, TimeSpan Y)? b = default) => InitCalled = true;
}
