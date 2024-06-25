using FluentAssertions;
using Godot;

namespace NRT.Tests
{
    [SceneTree]
    public partial class Main : Node
    {
        [GodotOverride]
        private void OnReady()
        {
            // Setup
            var nullableWithNull = TestWithNullableNRT.Instantiate(null);
            var nullableWithNotNull = TestWithNullableNRT.Instantiate("not null");
            var nonNullableWithEmpty = TestWithNonNullableNRT.Instantiate(string.Empty); // (should not compile with null)
            var nonNullableWithNotEmpty = TestWithNonNullableNRT.Instantiate("not empty");

            TestInstantiate();
            TestNotify();
            TestNotifyWithAction();

            // Teardown
            nullableWithNull.Free();
            nullableWithNotNull.Free();
            nonNullableWithEmpty.Free();
            nonNullableWithNotEmpty.Free();

            GD.Print("TEST PASS OK");

            GetTree().Quit();

            void TestInstantiate()
            {
                nullableWithNull.InstantiateValue.Should().Be(null);
                nullableWithNotNull.InstantiateValue.Should().Be("not null");
                nonNullableWithEmpty.InstantiateValue.Should().Be(string.Empty);
                nonNullableWithNotEmpty.InstantiateValue.Should().Be("not empty");
            }

            void TestNotify()
            {
                nullableWithNull.NotifyTest.Should().Be(null);
                nullableWithNotNull.NotifyTest.Should().Be(null);
                nonNullableWithEmpty.NotifyTest.Should().Be(string.Empty);
                nonNullableWithNotEmpty.NotifyTest.Should().Be(string.Empty);

                nullableWithNull.NotifyTest = null;
                nullableWithNotNull.NotifyTest = "not null";
                nonNullableWithEmpty.NotifyTest = string.Empty; // (should not compile with null)
                nonNullableWithNotEmpty.NotifyTest = "not empty";

                nullableWithNull.NotifyTest.Should().Be(null);
                nullableWithNotNull.NotifyTest.Should().Be("not null");
                nonNullableWithEmpty.NotifyTest.Should().Be(string.Empty);
                nonNullableWithNotEmpty.NotifyTest.Should().Be("not empty");
            }

            void TestNotifyWithAction()
            {
                nullableWithNull.NotifyActionValue.Should().Be(null);
                nullableWithNotNull.NotifyActionValue.Should().Be(null);
                nonNullableWithEmpty.NotifyActionValue.Should().Be(string.Empty);
                nonNullableWithNotEmpty.NotifyActionValue.Should().Be(string.Empty);

                nullableWithNull.NotifyTestWithAction.Should().Be(null);
                nullableWithNotNull.NotifyTestWithAction.Should().Be(null);
                nonNullableWithEmpty.NotifyTestWithAction.Should().Be(string.Empty);
                nonNullableWithNotEmpty.NotifyTestWithAction.Should().Be(string.Empty);

                nullableWithNull.NotifyTestWithAction = null;
                nullableWithNotNull.NotifyTestWithAction = "not null";
                nonNullableWithEmpty.NotifyTestWithAction = string.Empty; // (should not compile with null)
                nonNullableWithNotEmpty.NotifyTestWithAction = "not empty";

                nullableWithNull.NotifyTestWithAction.Should().Be(null);
                nullableWithNotNull.NotifyTestWithAction.Should().Be("not null");
                nonNullableWithEmpty.NotifyTestWithAction.Should().Be(string.Empty);
                nonNullableWithNotEmpty.NotifyTestWithAction.Should().Be("not empty");

                nullableWithNull.NotifyActionValue.Should().Be(null);
                nullableWithNotNull.NotifyActionValue.Should().Be("not null");
                nonNullableWithEmpty.NotifyActionValue.Should().Be(string.Empty);
                nonNullableWithNotEmpty.NotifyActionValue.Should().Be("not empty");
            }
        }
    }
}
