using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class NotifyPartialPropertiesTest : Node, ITest
{
    [Notify] public partial int Value { get; set; }
    [Notify] public virtual partial int VirtualValue { get; set; }

    [Notify] public partial int ValueWithGetModifier { internal get; set; }
    [Notify] public partial int ValueWithSetModifier { get; protected set; }

    [Notify] public partial int ReadOnlyValue { get; }
    [Notify] public partial int WriteOnlyValue { set; }

    private void SetReadOnlyValue(int value) => _readOnlyValue.Set(value);
    private int GetWriteOnlyValue() => _writeOnlyValue.Get();

    void ITest.InitTests()
    {
        var isChanged = false;
        var isChanging = false;
        var _isChanged = false;
        var _isChanging = false;

        TestValue();
        TestVirtualValue();
        TestValueWithGetModifier();
        TestValueWithSetModifier();
        TestReadOnlyValue();
        TestWriteOnlyValue();

        void TestValue()
        {
            InitValue(3);
            Value.Should().Be(3);
            isChanged.Should().BeFalse();
            isChanging.Should().BeFalse();
            _isChanged.Should().BeFalse();
            _isChanging.Should().BeFalse();

            ValueChanged += OnChanged;
            ValueChanging += OnChanging;
            _value.Changed += _OnChanged;
            _value.Changing += _OnChanging;

            Value = 7;
            Value.Should().Be(7);
            isChanged.Should().BeTrue();
            isChanging.Should().BeTrue();
            _isChanged.Should().BeTrue();
            _isChanging.Should().BeTrue();

            ResetFlags();
        }

        void TestVirtualValue()
        {
            InitVirtualValue(3);
            VirtualValue.Should().Be(3);
            isChanged.Should().BeFalse();
            isChanging.Should().BeFalse();
            _isChanged.Should().BeFalse();
            _isChanging.Should().BeFalse();

            VirtualValueChanged += OnChanged;
            VirtualValueChanging += OnChanging;
            _virtualValue.Changed += _OnChanged;
            _virtualValue.Changing += _OnChanging;

            VirtualValue = 7;
            VirtualValue.Should().Be(7);
            isChanged.Should().BeTrue();
            isChanging.Should().BeTrue();
            _isChanged.Should().BeTrue();
            _isChanging.Should().BeTrue();

            ResetFlags();
        }

        void TestValueWithGetModifier()
        {
            InitValueWithGetModifier(3);
            ValueWithGetModifier.Should().Be(3);
            isChanged.Should().BeFalse();
            isChanging.Should().BeFalse();
            _isChanged.Should().BeFalse();
            _isChanging.Should().BeFalse();

            ValueWithGetModifierChanged += OnChanged;
            ValueWithGetModifierChanging += OnChanging;
            _valueWithGetModifier.Changed += _OnChanged;
            _valueWithGetModifier.Changing += _OnChanging;

            ValueWithGetModifier = 7;
            ValueWithGetModifier.Should().Be(7);
            isChanged.Should().BeTrue();
            isChanging.Should().BeTrue();
            _isChanged.Should().BeTrue();
            _isChanging.Should().BeTrue();

            ResetFlags();
        }

        void TestValueWithSetModifier()
        {
            InitValueWithSetModifier(3);
            ValueWithSetModifier.Should().Be(3);
            isChanged.Should().BeFalse();
            isChanging.Should().BeFalse();
            _isChanged.Should().BeFalse();
            _isChanging.Should().BeFalse();

            ValueWithSetModifierChanged += OnChanged;
            ValueWithSetModifierChanging += OnChanging;
            _valueWithSetModifier.Changed += _OnChanged;
            _valueWithSetModifier.Changing += _OnChanging;

            ValueWithSetModifier = 7;
            ValueWithSetModifier.Should().Be(7);
            isChanged.Should().BeTrue();
            isChanging.Should().BeTrue();
            _isChanged.Should().BeTrue();
            _isChanging.Should().BeTrue();

            ResetFlags();
        }

        void TestReadOnlyValue()
        {
            InitReadOnlyValue(3);
            ReadOnlyValue.Should().Be(3);
            isChanged.Should().BeFalse();
            isChanging.Should().BeFalse();
            _isChanged.Should().BeFalse();
            _isChanging.Should().BeFalse();

            ReadOnlyValueChanged += OnChanged;
            ReadOnlyValueChanging += OnChanging;
            _readOnlyValue.Changed += _OnChanged;
            _readOnlyValue.Changing += _OnChanging;

            SetReadOnlyValue(7);
            ReadOnlyValue.Should().Be(7);
            isChanged.Should().BeTrue();
            isChanging.Should().BeTrue();
            _isChanged.Should().BeTrue();
            _isChanging.Should().BeTrue();

            ResetFlags();
        }

        void TestWriteOnlyValue()
        {
            InitWriteOnlyValue(3);
            GetWriteOnlyValue().Should().Be(3);
            isChanged.Should().BeFalse();
            isChanging.Should().BeFalse();
            _isChanged.Should().BeFalse();
            _isChanging.Should().BeFalse();

            WriteOnlyValueChanged += OnChanged;
            WriteOnlyValueChanging += OnChanging;
            _writeOnlyValue.Changed += _OnChanged;
            _writeOnlyValue.Changing += _OnChanging;

            WriteOnlyValue = 7;
            GetWriteOnlyValue().Should().Be(7);
            isChanged.Should().BeTrue();
            isChanging.Should().BeTrue();
            _isChanged.Should().BeTrue();
            _isChanging.Should().BeTrue();

            ResetFlags();
        }

        void OnChanged() => isChanged = true;
        void OnChanging() => isChanging = true;
        void _OnChanged() => _isChanged = true;
        void _OnChanging() => _isChanging = true;

        void ResetFlags()
        {
            isChanged = false;
            isChanging = false;
            _isChanged = false;
            _isChanging = false;
        }
    }

    #region GeneratedCode

    //public partial int Value
    //{
    //    get => _value.Get();
    //    set => _value.Set(value);
    //}

    //public virtual partial int VirtualValue
    //{
    //    get => _virtualValue.Get();
    //    set => _virtualValue.Set(value);
    //}

    //public partial int ValueWithGetModifier
    //{
    //    internal get => _valueWithGetModifier.Get();
    //    set => _valueWithGetModifier.Set(value);
    //}

    //public partial int ValueWithSetModifier
    //{
    //    get => _valueWithSetModifier.Get();
    //    protected set => _valueWithSetModifier.Set(value);
    //}

    //public partial int ReadOnlyValue
    //{
    //    get => _readOnlyValue.Get();
    //}

    //public partial int WriteOnlyValue
    //{
    //    set => _writeOnlyValue.Set(value);
    //}

    #endregion
}
