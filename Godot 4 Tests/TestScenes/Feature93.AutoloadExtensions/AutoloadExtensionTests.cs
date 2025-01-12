using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class AutoloadExtensionTests : Node, ITest
{
    static AutoloadExtensionTests()
        => RunTestsStatic();

    void ITest.InitTests()
        => RunTests(this, IsInsideTree: false);

    void ITest.EnterTests()
        => RunTests(this, IsInsideTree: true);

    void ITest.ReadyTests()
        => RunTests(this, IsInsideTree: true);

    void ITest.ExitTests()
        => RunTests(this, IsInsideTree: false);

    private static void RunTests(Node self = null, bool? IsInsideTree = null)
    {
        TypeTest();
        ValueTest();
        EnsureIsInTree();

        static void TypeTest()
        {
            static Type TypeOf<T>(T _) => typeof(T);

            TypeOf(Autoload.AutoloadScene).Should().Be<Control>();
            TypeOf(Autoload.AutoloadSceneCS).Should().Be<AutoloadSceneCS>();
            TypeOf(Autoload.AutoloadSceneGD).Should().Be<Control>();

            TypeOf(Autoload.InheritedScene).Should().Be<Control>();
            TypeOf(Autoload.InheritedSceneCS).Should().Be<InheritedSceneCS>();
            TypeOf(Autoload.InheritedSceneGD).Should().Be<Control>();

            TypeOf(Autoload.AutoloadScriptCS).Should().Be<AutoloadScript>();
            TypeOf(Autoload.AutoloadScriptGD).Should().Be<Control>();

            TypeOf(Autoload.InheritedScript1).Should().Be<Node>(); // Can't currently resolve if extending by class name
            TypeOf(Autoload.InheritedScript2).Should().Be<Node3D>();
            TypeOf(Autoload.InheritedScript3).Should().Be<GpuParticles3D>();

            //TypeOf(Autoload.namedAutoLoad1).Should().Be<Control>();
            //TypeOf(Autoload.namedAutoLoad2).Should().Be<Control>();
            TypeOf(Autoload.NamedAutoLoad1).Should().Be<Control>();
            TypeOf(Autoload.NamedAutoLoad2).Should().Be<Control>();
        }

        static void ValueTest()
        {
            Autoload.AutoloadScene.Should().NotBeNull().And.BeOfType<Control>();
            Autoload.AutoloadSceneCS.Should().NotBeNull().And.BeOfType<AutoloadSceneCS>();
            Autoload.AutoloadSceneGD.Should().NotBeNull().And.BeOfType<Control>();

            Autoload.InheritedScene.Should().NotBeNull().And.BeOfType<Control>();
            Autoload.InheritedSceneCS.Should().NotBeNull().And.BeOfType<InheritedSceneCS>();
            Autoload.InheritedSceneGD.Should().NotBeNull().And.BeOfType<Control>();

            Autoload.AutoloadScriptCS.Should().NotBeNull().And.BeOfType<AutoloadScript>();
            Autoload.AutoloadScriptGD.Should().NotBeNull().And.BeOfType<Control>();

            Autoload.InheritedScript1.Should().NotBeNull().And.BeOfType<Node2D>();
            Autoload.InheritedScript2.Should().NotBeNull().And.BeOfType<Node3D>();
            Autoload.InheritedScript3.Should().NotBeNull().And.BeOfType<GpuParticles3D>();

            //Autoload.namedAutoLoad1.Should().NotBeNull().And.BeOfType<Control>();
            //Autoload.namedAutoLoad2.Should().NotBeNull().And.BeOfType<Control>();
            Autoload.NamedAutoLoad1.Should().NotBeNull().And.BeOfType<Control>();
            Autoload.NamedAutoLoad2.Should().NotBeNull().And.BeOfType<Control>();
        }

        void EnsureIsInTree()
            => self?.IsInsideTree().Should().Be(IsInsideTree.Value);
    }

    private static void RunTestsStatic()
    {
        if (!Engine.IsEditorHint())
        {
            try
            {
                RunTests();
            }
            catch (AssertionFailedException e)
            {
                GD.Print(e);
            }
        }
    }
}
