using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes
{
    [SceneTree]
    internal abstract partial class GodotNotifyTests : Node, ITest
    {
        [Notify]
        private int Value1
        {
            get => _value1.Get();
            set => _value1.Set(value);
        }

        [Notify]
        private int Value2
        {
            get => _value2.Get();
            set => _value2.Set(value);
        }

        protected GodotNotifyTests()
        {
            Value1 = 3;
            Value2 = 7;
        }

        void ITest.InitTests()
        {
            Value1.Should().Be(3);
            Value2.Should().Be(7);

            Value1 = 5;

            Value1.Should().Be(5);
            Value2.Should().Be(7);

            var value1ChangeCount = 0;
            var value2ChangeCount = 0;
            Value1Changed += OnValue1Changed;
            Value2Changed += () => ++value2ChangeCount;

            Value1 = 5;
            value1ChangeCount.Should().Be(0);

            Value1 = 6;
            value1ChangeCount.Should().Be(1);

            Value1 = 6;
            value1ChangeCount.Should().Be(1);

            Value1 = 7;
            value1ChangeCount.Should().Be(2);

            Value1Changed -= OnValue1Changed;

            Value1 = 8;
            value1ChangeCount.Should().Be(2);

            Value1Changed += OnValue1Changed;

            Value1 = 8;
            value1ChangeCount.Should().Be(2);

            Value1 = 9;
            value1ChangeCount.Should().Be(3);

            Value1.Should().Be(9);
            Value2.Should().Be(7);
            value2ChangeCount.Should().Be(0);

            void OnValue1Changed()
                => ++value1ChangeCount;
        }
    }
}
