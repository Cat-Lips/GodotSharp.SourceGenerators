using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[ResourceTree(".")]
public static partial class RootRes;

[ResourceTree("Assets")]
public static partial class AbsoluteRes;

[ResourceTree("Resources")]
public static partial class RelativeRes;

[ResourceTree(scenes: true)]
public static partial class ResWithScenes;

[ResourceTree(scripts: true)]
public static partial class ResWithScripts;

[ResourceTree(xtras: ["csv", "cfg", "txt", "zip"])]
public static partial class ResWithXtras;

[ResourceTree(uid: true)]
public static partial class ResWithUID;

//[ResourceTree("Invalid")]
//public static partial class InvalidRes;

[SceneTree]
public partial class ResourceTreeTests : Node, ITest
{
    void ITest.InitTests()
    {
        TestRootRes();
        TestAbsoluteRes();
        TestRelativeRes();
        TestResWithScenes();
        TestResWithScripts();
        TestResWithXtras();
        TestResWithUID();

        static void TestRootRes()
        {
            typeof(RootRes).ShouldConsistOf(
                Properties: ["DefaultBusLayoutTres"],
                NestedTypes: ["Assets", "TestScenes"]);
            RootRes.DefaultBusLayoutTres.Should().BeOfType<AudioBusLayout>();

            typeof(RootRes.Assets).ShouldConsistOf(
                Properties: ["IconSvg"],
                NestedTypes: ["Tr"]);
            RootRes.Assets.IconSvg.Should().BeOfType<CompressedTexture2D>();

            typeof(RootRes.Assets.Tr).ShouldConsistOf(
                Properties: ["TrDeTranslation", "TrEnTranslation", "TrFrTranslation", "TrJpTranslation"]);
            RootRes.Assets.Tr.TrDeTranslation.Should().BeOfType<OptimizedTranslation>();
            RootRes.Assets.Tr.TrEnTranslation.Should().BeOfType<OptimizedTranslation>();
            RootRes.Assets.Tr.TrFrTranslation.Should().BeOfType<OptimizedTranslation>();
            RootRes.Assets.Tr.TrJpTranslation.Should().BeOfType<OptimizedTranslation>();
        }

        static void TestAbsoluteRes()
        {
            typeof(AbsoluteRes).ShouldConsistOf(
                Properties: ["IconSvg"],
                NestedTypes: ["Tr"]);
            AbsoluteRes.IconSvg.Should().BeOfType<CompressedTexture2D>();

            typeof(AbsoluteRes.Tr).ShouldConsistOf(
                Properties: ["TrDeTranslation", "TrEnTranslation", "TrFrTranslation", "TrJpTranslation"]);
            AbsoluteRes.Tr.TrDeTranslation.Should().BeOfType<OptimizedTranslation>();
            AbsoluteRes.Tr.TrEnTranslation.Should().BeOfType<OptimizedTranslation>();
            AbsoluteRes.Tr.TrFrTranslation.Should().BeOfType<OptimizedTranslation>();
            AbsoluteRes.Tr.TrJpTranslation.Should().BeOfType<OptimizedTranslation>();
        }

        static void TestRelativeRes()
        {
            typeof(RelativeRes).ShouldConsistOf(
                Properties: ["IconSvg"]);
            AbsoluteRes.IconSvg.Should().BeOfType<CompressedTexture2D>();
        }

        static void TestResWithScenes()
        {
            typeof(ResWithScenes).ShouldConsistOf(
                Properties: ["ResourceTreeTestsTscn"],
                NestedTypes: ["Resources"]);
            ResWithScenes.ResourceTreeTestsTscn.Should().BeOfType<PackedScene>();

            typeof(ResWithScenes.Resources).ShouldConsistOf(
                Properties: ["IconSvg"]);
            ResWithScenes.Resources.IconSvg.Should().BeOfType<CompressedTexture2D>();
        }

        static void TestResWithScripts()
        {
            typeof(ResWithScripts).ShouldConsistOf(
                Properties: ["ResourceTreeTestsCs"],
                NestedTypes: ["Resources"]);
            ResWithScripts.ResourceTreeTestsCs.Should().BeOfType<CSharpScript>();

            typeof(ResWithScripts.Resources).ShouldConsistOf(
                Properties: ["IconSvg", "MyResGd"]);
            ResWithScripts.Resources.IconSvg.Should().BeOfType<CompressedTexture2D>();
            ResWithScripts.Resources.MyResGd.Should().BeOfType<GDScript>();
        }

        static void TestResWithXtras()
        {
            typeof(ResWithXtras).ShouldConsistOf(
                NestedTypes: ["Resources"]);

            typeof(ResWithXtras.Resources).ShouldConsistOf(
                Properties: ["IconSvg", "MyResCfg", "MyResCsv", "MyResTxt"]);
            ResWithXtras.Resources.IconSvg.Should().BeOfType<CompressedTexture2D>();
            ResWithXtras.Resources.MyResCfg.Should().Be("res://TestScenes/Feature148.ResourceTree/Resources/MyRes.cfg");
            ResWithXtras.Resources.MyResCsv.Should().Be("res://TestScenes/Feature148.ResourceTree/Resources/MyRes.csv");
            ResWithXtras.Resources.MyResTxt.Should().Be("res://TestScenes/Feature148.ResourceTree/Resources/MyRes.txt");
            FileAccess.FileExists(ResWithXtras.Resources.MyResCfg).Should().BeTrue();
            FileAccess.FileExists(ResWithXtras.Resources.MyResCsv).Should().BeTrue();
            FileAccess.FileExists(ResWithXtras.Resources.MyResTxt).Should().BeTrue();
        }

        static void TestResWithUID()
        {
            typeof(ResWithUID).ShouldConsistOf(
                Properties: ["ResourceTreeTestsCsUid"],
                NestedTypes: ["Resources"]);
            ResWithUID.ResourceTreeTestsCsUid.Should().Be("uid://tyjsxc2njtw2");

            typeof(ResWithUID.Resources).ShouldConsistOf(
                Properties: ["IconSvg", "MyResGdUid"]);
            ResWithUID.Resources.IconSvg.Should().BeOfType<CompressedTexture2D>();
            ResWithUID.Resources.MyResGdUid.Should().Be("uid://sho6tst545eo");
        }
    }
}
