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
			_.NamedNode.Get().Should().BeSameAs(GetNode("NamedNode")).And.BeSameAs(GetNode("%NamedNode"));
			_.NamedNode.NestedNamedNode.Should().BeSameAs(GetNode("NamedNode/NestedNamedNode")).And.BeSameAs(GetNode("%NestedNamedNode"));

			_.Node.NamedNode.Get().Should().BeSameAs(GetNode("Node/NamedNode")).And.NotBeSameAs(GetNode("%NamedNode"));
			_.Node.NamedNode.NestedNamedNode.Should().BeSameAs(GetNode("Node/NamedNode/NestedNamedNode")).And.NotBeSameAs(GetNode("%NestedNamedNode"));

			NamedNode.Should().BeSameAs(GetNode("%NamedNode")).And.NotBeSameAs(GetNode("Node/NamedNode"));
			NestedNamedNode.Should().BeSameAs(GetNode("%NestedNamedNode")).And.NotBeSameAs(GetNode("Node/NamedNode/NestedNamedNode"));
		}
	}
}
