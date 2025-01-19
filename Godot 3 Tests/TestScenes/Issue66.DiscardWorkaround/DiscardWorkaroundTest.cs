using System.Collections.Generic;
using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree(root: "CustomRootToken")]
public partial class DiscardWorkaroundTest : Node, ITest
{
	void ITest.InitTests()
	{
		_ = GetValue();
		CustomRootToken.Get().Should().Be(this);
		foreach ((_, _) in new Dictionary<int, float>()) { }

		static float GetValue() => 7;
	}
}
