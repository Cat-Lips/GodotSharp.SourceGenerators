using FluentAssertions;
using Godot;

namespace GodotTests.TestScenes
{
	[SceneTree]
	internal partial class SceneWithEditableChildren : Control
	{
		public void Check<T1, T2>(string t1, string t2)
		{
			Child1.CheckMyLabel<T1>(t1);
			Child2.CheckMyLabel<T2>(t2);

			Child1.Should().Be(_.Child1);
			Child2.Should().Be(_.Child2);

			_.Child1.CheckMyLabel<T1>(t1);
			_.Child2.CheckMyLabel<T2>(t2);
		}
	}
}
