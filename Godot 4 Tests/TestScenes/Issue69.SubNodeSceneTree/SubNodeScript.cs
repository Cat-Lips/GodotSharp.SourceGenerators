using System.Linq;
using FluentAssertions;
using Godot;

// Script with different name to scene

namespace GodotTests.TestScenes;

[SceneTree($"{nameof(SubNodeSceneTreeTest)}.tscn")]
public partial class SubNodeScript : Node
{
    // Same as SubNodeSceneTreeTest.RunTests()
    public void RunTests()
    {
        RootAccessTest();
        UniqueNameTest();
        NavigationTest();

        void RootAccessTest()
        {
            var root = _.Get();
            root.Should().NotBeNull();
            root.GetChildren().Should().Equal([_.Node1, _.Node2, _.Node3, _.Node4]);

            root = _; // implicit cast
            root.Should().NotBeNull();
            root.GetChildren().Should().Equal([_.Node1, _.Node2, _.Node3, _.Node4]);
        }

        void UniqueNameTest()
        {
            Node1.Should().NotBeNull().And.BeOfType<SubNodeSceneTreeTest>().And.NotBe(Node2);
            Node2.Should().NotBeNull().And.BeOfType<SubNodeSceneTreeTest>().And.NotBe(Node1);
            Node3.Should().NotBeNull().And.BeOfType<SubNodeScript>().And.NotBe(Node4);
            Node4.Should().NotBeNull().And.BeOfType<SubNodeScript>().And.NotBe(Node3);
        }

        void NavigationTest()
        {
            var node1 = _.Node1.Node1.Get();
            var node1Parent = _.Node1.Get();
            var node1Child = _.Node1.Node1.Node1;

            var node2 = _.Node2.Node2.Get();
            var node2Parent = _.Node2.Get();
            var node2Child = _.Node2.Node2.Node2;

            var node3 = _.Node3.Node3.Get();
            var node3Parent = _.Node3.Get();
            var node3Child = _.Node3.Node3.Node3;

            var node4 = _.Node4.Node4.Get();
            var node4Parent = _.Node4.Get();
            var node4Child = _.Node4.Node4.Node4;

            node1.Should().NotBeNull().And.BeOfType<SubNodeSceneTreeTest>().And.NotBe(node2);
            node2.Should().NotBeNull().And.BeOfType<SubNodeSceneTreeTest>().And.NotBe(node1);
            node3.Should().NotBeNull().And.BeOfType<SubNodeScript>().And.NotBe(node4);
            node4.Should().NotBeNull().And.BeOfType<SubNodeScript>().And.NotBe(node3);

            node1.GetParent().Should().Be(node1Parent);
            node2.GetParent().Should().Be(node2Parent);
            node3.GetParent().Should().Be(node3Parent);
            node4.GetParent().Should().Be(node4Parent);

            node1.GetChildren().Single().Should().Be(node1Child);
            node2.GetChildren().Single().Should().Be(node2Child);
            node3.GetChildren().Single().Should().Be(node3Child);
            node4.GetChildren().Single().Should().Be(node4Child);
        }
    }
}
