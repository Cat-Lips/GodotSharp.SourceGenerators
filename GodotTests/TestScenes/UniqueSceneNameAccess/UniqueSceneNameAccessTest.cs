using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes
{
    [SceneTree]
    internal abstract partial class UniqueSceneNameAccessTest : Node, ITest
    {
        void ITest.InitTests()
        {
            _.NamedNode.Get().Should().BeSameAs(GetNode("NamedNode"));
            _.NamedNode.NestedNamedNode.Should().BeSameAs(GetNode("NamedNode/NestedNamedNode"));

            _.Node.NamedNode.Get().Should().BeSameAs(GetNode("Node/NamedNode"));
            _.Node.NamedNode.NestedNamedNode.Should().BeSameAs(GetNode("Node/NamedNode/NestedNamedNode"));

            //NamedNode.Should().BeSameAs(GetNode("NamedNode"));
            //NestedNamedNode.Should().BeSameAs(GetNode("NamedNode/NestedNamedNode"));

            //NamedNode.Should().NotBeSameAs(GetNode("Node/NamedNode"));
            //NestedNamedNode.Should().NotBeSameAs(GetNode("Node/NamedNode/NestedNamedNode"));
        }
    }
}
