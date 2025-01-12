//#define RENAME_TEST

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
            TypeOf(Autoload.InheritedScriptCS).Should().Be<InheritedScript>();

            TypeOf(Autoload.AutoloadScriptGD1).Should().Be<Node2D>();
            TypeOf(Autoload.AutoloadScriptGD2).Should().Be<Node3D>();
            TypeOf(Autoload.AutoloadScriptGD3).Should().Be<GpuParticles3D>();

            TypeOf(Autoload.InheritedScriptGD1).Should().Be<Node>(); // Can't currently resolve if extending by class name
            TypeOf(Autoload.InheritedScriptGD2).Should().Be<Node3D>();
            TypeOf(Autoload.InheritedScriptGD3).Should().Be<GpuParticles3D>();
#if RENAME_TEST
            TypeOf(Autoload.NamedAutoLoad1).Should().Be<Control>();
            TypeOf(Autoload.NamedAutoLoad2).Should().Be<Control>();
#else
            TypeOf(Autoload.namedAutoLoad1).Should().Be<Control>();
            TypeOf(Autoload.namedAutoLoad2).Should().Be<Control>();
#endif
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
            Autoload.InheritedScriptCS.Should().NotBeNull().And.BeOfType<InheritedScript>();

            Autoload.AutoloadScriptGD1.Should().NotBeNull().And.BeOfType<Node2D>();
            Autoload.AutoloadScriptGD2.Should().NotBeNull().And.BeOfType<Node3D>();
            Autoload.AutoloadScriptGD3.Should().NotBeNull().And.BeOfType<GpuParticles3D>();

            Autoload.InheritedScriptGD1.Should().NotBeNull().And.BeOfType<Node2D>();
            Autoload.InheritedScriptGD2.Should().NotBeNull().And.BeOfType<Node3D>();
            Autoload.InheritedScriptGD3.Should().NotBeNull().And.BeOfType<GpuParticles3D>();

#if RENAME_TEST
            Autoload.NamedAutoLoad1.Should().NotBeNull().And.BeOfType<Control>();
            Autoload.NamedAutoLoad2.Should().NotBeNull().And.BeOfType<Control>();
#else
            Autoload.namedAutoLoad1.Should().NotBeNull().And.BeOfType<Control>();
            Autoload.namedAutoLoad2.Should().NotBeNull().And.BeOfType<Control>();
#endif
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
