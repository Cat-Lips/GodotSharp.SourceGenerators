using FluentAssertions;
using Godot;

namespace GodotTests.TestScenes;

[SceneTree]
internal abstract partial class Scene : Control
{
	public void CheckMyLabel<TLabel>(string text)
	{
		MyLabel.Should().Be(_.MyLabel);
		MyLabel.Text.Should().Be(text);
		MyLabel.Should().BeOfType<TLabel>();
		_.MyLabel.Should().BeOfType<TLabel>();
	}
}
