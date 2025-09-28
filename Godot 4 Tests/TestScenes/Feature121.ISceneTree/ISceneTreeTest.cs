using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;
using GodotTests.ISceneTreeTests;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class ISceneTreeTest : Node, ITest
{
    private static readonly string ExpectedTscnFilePath = "res://TestScenes/Feature121.ISceneTree/ISceneTreeTest.tscn";

    void ITest.InitTests()
    {
        InterfaceTests();
        InstantiateTests();

        void InterfaceTests()
        {
            Test<ISceneTreeTest>();
            TscnFilePath.Should().Be(ExpectedTscnFilePath);
            GetType().GetInterface("ISceneTree").GetProperty("TscnFilePath").Should().NotBeNull();

            static void Test<T>() where T : ISceneTree
                => T.TscnFilePath.Should().Be(ExpectedTscnFilePath);
        }

        void InstantiateTests()
        {
            var scene1 = Instantiator.Instantiate<Scene1>();
            var scene2 = Instantiator.Instantiate<Scene2>();
            var scene3 = Instantiator.Instantiate<Scene3>();
            scene1.Should().NotBeNull().And.BeOfType<Scene1>();
            scene2.Should().NotBeNull().And.BeOfType<Scene2>();
            scene3.Should().NotBeNull().And.BeOfType<Scene3>();

            var scene1a = IInstantiable.Instantiate<Scene1>();
            var scene2a = IInstantiable.Instantiate<Scene2>();
            var scene3a = IInstantiable.Instantiate<Scene3>();
            scene1a.Should().NotBeNull().And.BeOfType<Scene1>();
            scene2a.Should().NotBeNull().And.BeOfType<Scene2>();
            scene3a.Should().NotBeNull().And.BeOfType<Scene3>();

            var scene3b = IInstantiable<Scene3>.Instantiate();
            scene3b.Should().NotBeNull().And.BeOfType<Scene3>();

            var scene2c = InstantiateC<Scene2>();
            scene2c.Should().NotBeNull().And.BeOfType<Scene2>();

            var scene3d = InstantiateD<Scene3>();
            scene3d.Should().NotBeNull().And.BeOfType<Scene3>();

            static T InstantiateC<T>() where T : class, ISceneTree, IInstantiable
                => IInstantiable.Instantiate<T>();

            static T InstantiateD<T>() where T : class, ISceneTree, IInstantiable<T>
                => IInstantiable<T>.Instantiate();
        }
    }
}
