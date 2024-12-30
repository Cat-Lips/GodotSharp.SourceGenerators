using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class AutoloadExtensionTests : Node, ITest
{
    void ITest.ReadyTests()
    {
        // FIXME: Currently only works if node in tree - Autoloads should be always accessible
        this.Autoloads().AutoloadScene.Should().BeOfType<Node>();
        this.Autoloads().AutoloadSceneCS.Should().BeOfType<AutoloadSceneCS>();
        this.Autoloads().AutoloadSceneGD.Should().BeOfType<Node>();
        this.Autoloads().AutoloadScriptCS.Should().BeOfType<AutoloadScript>();
        this.Autoloads().AutoloadScriptGD.Should().BeOfType<Node>();
    }
}
