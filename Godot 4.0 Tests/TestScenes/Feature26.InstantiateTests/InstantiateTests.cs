using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes
{
	[SceneTree]
	public partial class InstantiateTests : Control, ITest
	{
		void ITest.InitTests()
		{
			var sut0 = Test0Arg.Instantiate();
			var sut1 = Test1Arg.Instantiate(1);
			var sut2 = Test2Arg.Instantiate(1, 2);
			var sut3 = Test3Arg.Instantiate(1, 2, 3);

			sut0.X.Should().Be(7);
			sut1.A.Should().Be(1); sut1.X.Should().Be(7);
			sut2.A.Should().Be(1); sut2.B.Should().Be(2); sut2.X.Should().Be(7);
			sut3.A.Should().Be(1); sut3.B.Should().Be(2); sut3.C.Should().Be(3); sut3.X.Should().Be(7);
		}
	}
}
