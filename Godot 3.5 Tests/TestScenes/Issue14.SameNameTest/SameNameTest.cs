using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes
{
	[SceneTree]
	internal abstract partial class SameNameTest : Control, ITest
	{
		void ITest.InitTests()
		{
			_.SameName_GlobalNamespace.Should().BeOfType<global::SameName>();
			_.SameName_SameNamespace.Should().BeOfType<SameName>();
			_.SameName_ChildNamespace.Should().BeOfType<ChildNamespace.SameName>();

			SameName_GlobalNamespace.Should().BeOfType<global::SameName>();
			SameName_SameNamespace.Should().BeOfType<SameName>();
			SameName_ChildNamespace.Should().BeOfType<ChildNamespace.SameName>();
		}
	}
}
