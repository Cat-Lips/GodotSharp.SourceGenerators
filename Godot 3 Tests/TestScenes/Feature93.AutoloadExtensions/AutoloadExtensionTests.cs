using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[Autoload]
public static partial class MyAutoloads;

[Autoload]
[AutoloadRename("NamedAutoLoad1", "namedAutoLoad1")]
[AutoloadRename("NamedAutoLoad2", "namedAutoLoad2")]
public static partial class MyAutoloadsWithRenames;

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

            TypeOf(MyAutoloads.AutoloadScene).Should().Be<Control>();
            TypeOf(MyAutoloads.AutoloadSceneCS).Should().Be<AutoloadSceneCS>();
            TypeOf(MyAutoloads.AutoloadSceneGD).Should().Be<Control>();

            TypeOf(MyAutoloads.InheritedScene).Should().Be<Control>();
            TypeOf(MyAutoloads.InheritedSceneCS).Should().Be<InheritedSceneCS>();
            TypeOf(MyAutoloads.InheritedSceneGD).Should().Be<Control>();

            TypeOf(MyAutoloads.AutoloadScriptCS).Should().Be<AutoloadScript>();
            TypeOf(MyAutoloads.InheritedScriptCS).Should().Be<InheritedScript>();

            TypeOf(MyAutoloads.AutoloadScriptGD1).Should().Be<Node2D>();
            TypeOf(MyAutoloads.AutoloadScriptGD2).Should().Be<Spatial>();
            TypeOf(MyAutoloads.AutoloadScriptGD3).Should().Be<Container>();

            TypeOf(MyAutoloads.InheritedScriptGD1).Should().Be<Node>(); // Can't currently resolve if extending by class name
            TypeOf(MyAutoloads.InheritedScriptGD2).Should().Be<Spatial>();
            TypeOf(MyAutoloads.InheritedScriptGD3).Should().Be<Container>();

            TypeOf(MyAutoloads.namedAutoLoad1).Should().Be<Control>();
            TypeOf(MyAutoloads.namedAutoLoad2).Should().Be<Control>();

            // As above, so below (except those so marked)
            TypeOf(MyAutoloadsWithRenames.AutoloadScene).Should().Be<Control>();
            TypeOf(MyAutoloadsWithRenames.AutoloadSceneCS).Should().Be<AutoloadSceneCS>();
            TypeOf(MyAutoloadsWithRenames.AutoloadSceneGD).Should().Be<Control>();

            TypeOf(MyAutoloadsWithRenames.InheritedScene).Should().Be<Control>();
            TypeOf(MyAutoloadsWithRenames.InheritedSceneCS).Should().Be<InheritedSceneCS>();
            TypeOf(MyAutoloadsWithRenames.InheritedSceneGD).Should().Be<Control>();

            TypeOf(MyAutoloadsWithRenames.AutoloadScriptCS).Should().Be<AutoloadScript>();
            TypeOf(MyAutoloadsWithRenames.InheritedScriptCS).Should().Be<InheritedScript>();

            TypeOf(MyAutoloadsWithRenames.AutoloadScriptGD1).Should().Be<Node2D>();
            TypeOf(MyAutoloadsWithRenames.AutoloadScriptGD2).Should().Be<Spatial>();
            TypeOf(MyAutoloadsWithRenames.AutoloadScriptGD3).Should().Be<Container>();

            TypeOf(MyAutoloadsWithRenames.InheritedScriptGD1).Should().Be<Node>(); // Can't currently resolve if extending by class name
            TypeOf(MyAutoloadsWithRenames.InheritedScriptGD2).Should().Be<Spatial>();
            TypeOf(MyAutoloadsWithRenames.InheritedScriptGD3).Should().Be<Container>();

            TypeOf(MyAutoloadsWithRenames.NamedAutoLoad1).Should().Be<Control>(); // Renamed!
            TypeOf(MyAutoloadsWithRenames.NamedAutoLoad2).Should().Be<Control>(); // Renamed!
        }

        static void ValueTest()
        {
            MyAutoloads.AutoloadScene.Should().NotBeNull().And.BeOfType<Control>();
            MyAutoloads.AutoloadSceneCS.Should().NotBeNull().And.BeOfType<AutoloadSceneCS>();
            MyAutoloads.AutoloadSceneGD.Should().NotBeNull().And.BeOfType<Control>();

            MyAutoloads.InheritedScene.Should().NotBeNull().And.BeOfType<Control>();
            MyAutoloads.InheritedSceneCS.Should().NotBeNull().And.BeOfType<InheritedSceneCS>();
            MyAutoloads.InheritedSceneGD.Should().NotBeNull().And.BeOfType<Control>();

            MyAutoloads.AutoloadScriptCS.Should().NotBeNull().And.BeOfType<AutoloadScript>();
            MyAutoloads.InheritedScriptCS.Should().NotBeNull().And.BeOfType<InheritedScript>();

            MyAutoloads.AutoloadScriptGD1.Should().NotBeNull().And.BeOfType<Node2D>();
            MyAutoloads.AutoloadScriptGD2.Should().NotBeNull().And.BeOfType<Spatial>();
            MyAutoloads.AutoloadScriptGD3.Should().NotBeNull().And.BeOfType<Container>();

            MyAutoloads.InheritedScriptGD1.Should().NotBeNull().And.BeOfType<Node2D>();
            MyAutoloads.InheritedScriptGD2.Should().NotBeNull().And.BeOfType<Spatial>();
            MyAutoloads.InheritedScriptGD3.Should().NotBeNull().And.BeOfType<Container>();

            MyAutoloads.namedAutoLoad1.Should().NotBeNull().And.BeOfType<Control>();
            MyAutoloads.namedAutoLoad2.Should().NotBeNull().And.BeOfType<Control>();

            // As above, so below (except those so marked)
            MyAutoloadsWithRenames.AutoloadScene.Should().NotBeNull().And.BeOfType<Control>();
            MyAutoloadsWithRenames.AutoloadSceneCS.Should().NotBeNull().And.BeOfType<AutoloadSceneCS>();
            MyAutoloadsWithRenames.AutoloadSceneGD.Should().NotBeNull().And.BeOfType<Control>();

            MyAutoloadsWithRenames.InheritedScene.Should().NotBeNull().And.BeOfType<Control>();
            MyAutoloadsWithRenames.InheritedSceneCS.Should().NotBeNull().And.BeOfType<InheritedSceneCS>();
            MyAutoloadsWithRenames.InheritedSceneGD.Should().NotBeNull().And.BeOfType<Control>();

            MyAutoloadsWithRenames.AutoloadScriptCS.Should().NotBeNull().And.BeOfType<AutoloadScript>();
            MyAutoloadsWithRenames.InheritedScriptCS.Should().NotBeNull().And.BeOfType<InheritedScript>();

            MyAutoloadsWithRenames.AutoloadScriptGD1.Should().NotBeNull().And.BeOfType<Node2D>();
            MyAutoloadsWithRenames.AutoloadScriptGD2.Should().NotBeNull().And.BeOfType<Spatial>();
            MyAutoloadsWithRenames.AutoloadScriptGD3.Should().NotBeNull().And.BeOfType<Container>();

            MyAutoloadsWithRenames.InheritedScriptGD1.Should().NotBeNull().And.BeOfType<Node2D>();
            MyAutoloadsWithRenames.InheritedScriptGD2.Should().NotBeNull().And.BeOfType<Spatial>();
            MyAutoloadsWithRenames.InheritedScriptGD3.Should().NotBeNull().And.BeOfType<Container>();

            MyAutoloadsWithRenames.NamedAutoLoad1.Should().NotBeNull().And.BeOfType<Control>(); // Renamed!
            MyAutoloadsWithRenames.NamedAutoLoad2.Should().NotBeNull().And.BeOfType<Control>(); // Renamed!
        }

        void EnsureIsInTree()
            => self?.IsInsideTree().Should().Be(IsInsideTree.Value);
    }

    private static void RunTestsStatic()
    {
        if (!Engine.EditorHint)
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
