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
    {
        if (!Engine.IsEditorHint())
        {
            try
            {
                TypeTest();
                ValueTest();
                NameTest();
            }
            catch (AssertionFailedException e)
            {
                GD.Print(e);
            }
        }
    }

    void ITest.InitTests()
    {
        TypeTest();
        ValueTest();
        NameTest();
        InTree(false);
    }

    void ITest.EnterTests()
    {
        TypeTest();
        ValueTest();
        NameTest();
        InTree(true);
    }

    void ITest.ReadyTests()
    {
        TypeTest();
        ValueTest();
        NameTest();
        InTree(true);
    }

    void ITest.ExitTests()
    {
        TypeTest();
        ValueTest();
        NameTest();
        InTree(false);
    }

    private static void TypeTest()
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

        TypeOf(Autoload.namedAutoLoad1).Should().Be<Control>();
        TypeOf(Autoload.namedAutoLoad2).Should().Be<Control>();
    }

    private static void ValueTest()
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

        Autoload.namedAutoLoad1.Should().NotBeNull().And.BeOfType<Control>();
        Autoload.namedAutoLoad2.Should().NotBeNull().And.BeOfType<Control>();
    }

    private static void NameTest()
    {
        GD.Print(Autoload.AutoloadScene.Name);
        GD.Print(Autoload.AutoloadSceneCS.Name);
        GD.Print(Autoload.AutoloadSceneGD.Name);

        GD.Print(Autoload.InheritedScene.Name);
        GD.Print(Autoload.InheritedSceneCS.Name);
        GD.Print(Autoload.InheritedSceneGD.Name);

        GD.Print(Autoload.AutoloadScriptCS.Name);
        GD.Print(Autoload.AutoloadScriptGD.Name);

        GD.Print(Autoload.InheritedScript1.Name);
        GD.Print(Autoload.InheritedScript2.Name);
        GD.Print(Autoload.InheritedScript3.Name);

        GD.Print(Autoload.namedAutoLoad1.Name);
        GD.Print(Autoload.namedAutoLoad2.Name);

        //Autoload.AutoloadScene.Name.Should().Be("Groot");
        //Autoload.AutoloadSceneCS.Name.Should().Be("GrootCS");
        //Autoload.AutoloadSceneGD.Name.Should().Be("GrootGD");
        //Autoload.namedAutoLoad1.Name.Should().Be("Groot1");
        //Autoload.namedAutoLoad2.Name.Should().Be("Groot2");
    }

    private void InTree(bool expected)
        => IsInsideTree().Should().Be(expected);
}
