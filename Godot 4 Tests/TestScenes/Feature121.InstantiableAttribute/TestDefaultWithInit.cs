using System;
using System.Collections.Generic;
using Godot;

namespace GodotTests.TestScenes;

using TestData = InstantiableAttributeTests.InnerData;
using TestEnum = InstantiableAttributeTests.InnerEnum;

[Instantiable]
public partial class TestDefaultWithInit : Node
{
    public bool InitCalled { get; private set; }

    private void Init() => InitCalled = true;

    private void Init(char a = default, string b = default, bool c = default) => InitCalled = true;
    private void Init(char? a = default, string b = default, bool? c = default) => InitCalled = true;

    private void Init(float a = default, double b = default, decimal c = default) => InitCalled = true;
    private void Init(float? a = default, double? b = default, decimal? c = default) => InitCalled = true;

    private void Init(sbyte a = default, byte b = default, short c = default, ushort d = default,
        int e = default, uint f = default, long g = default, ulong h = default) => InitCalled = true;
    private void Init(sbyte? a = default, byte? b = default, short? c = default, ushort? d = default,
        int? e = default, uint? f = default, long? g = default, ulong? h = default) => InitCalled = true;

    private void Init(TestEnum a = default, TestData b = default, Action c = default) => InitCalled = true;
    private void Init(TestEnum? a = default, TestData b = default, Action c = default) => InitCalled = true;

    private void Init(in Vector2 a, ref Vector2 b, out bool c, params string[] d) => c = InitCalled = true;
    private void Init(in Vector2? a, ref Vector2? b, out bool? c, params IEnumerable<string> d) => c = InitCalled = true;

    private void Init(in Vector2 a = default, in DateTime b = default, in TimeSpan c = default) => InitCalled = true;
    private void Init(in Vector2? a = default, in DateTime? b = default, in TimeSpan? c = default) => InitCalled = true;

    private void Init(in (DateTime, TimeSpan) a = default, in (DateTime X, TimeSpan Y) b = default) => InitCalled = true;
    private void Init(in (DateTime, TimeSpan)? a = default, in (DateTime X, TimeSpan Y)? b = default) => InitCalled = true;
}
