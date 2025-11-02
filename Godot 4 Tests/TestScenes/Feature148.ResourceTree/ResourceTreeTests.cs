using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;
using GodotSharp.SourceGenerators.ResourceTreeExtensions;
using GodotTests.TestScenes.ResourceTreeTestAssets;

namespace GodotTests.TestScenes;

#region Test Cases

[ResourceTree("", Res.Load, xclude: ["TestScenes"])]
public static partial class RootResWithLoad;

[ResourceTree("", Res.ResPaths, xclude: ["TestScenes"])]
public static partial class RootResWithResPaths;

[ResourceTree("/", Res.DirPaths, xclude: ["TestScenes"])]
public static partial class RootResWithDirPaths;

[ResourceTree(".", Res.Load | Res.ResPaths, xclude: ["TestScenes"])]
public static partial class RootResWithLoadAndResPaths;

[ResourceTree("Assets", Res.DirPaths)]
public static partial class AbsoluteRes;

[ResourceTree("Resources", Res.DirPaths)]
public static partial class RelativeRes;

[ResourceTree("Resources", res: Res.Default | Res.AllIn, xtras: ["csv", "cfg", "txt", "zip"])]
public static partial class ResTypes;

[ResourceTree(res: Res.Default | Res.Scenes)]
public static partial class ResTypesWithScenes;

//[ResourceTree("Invalid")]
//public static partial class InvalidRes;

#endregion

[SceneTree]
public partial class ResourceTreeTests : Node, ITest
{
    void ITest.InitTests()
    {
        TestRootRes();
        TestAbsoluteRes();
        TestRelativeRes();
        TestResTypes();

        static void TestRootRes()
        {
            TestRootResWithLoad();
            TestRootResWithResPaths();
            TestRootResWithDirPaths();
            TestRootResWithLoadAndResPaths();

            void TestRootResWithLoad()
            {
                typeof(RootResWithLoad).ShouldConsistOf(Properties: ["DefaultBusLayoutTres"], NestedTypes: ["Assets"]);
                typeof(RootResWithLoad.Assets).ShouldConsistOf(Properties: ["IconSvg"], NestedTypes: ["Tr"]);
                typeof(RootResWithLoad.Assets.Tr).ShouldConsistOf(Properties: ["TrEnTranslation", "TrFrTranslation", "TrDeTranslation", "TrJpTranslation"]);
                RootResWithLoad.DefaultBusLayoutTres.Should().BeOfType<AudioBusLayout>().And.NotBeNull();
                RootResWithLoad.Assets.IconSvg.Should().BeOfType<CompressedTexture2D>().And.NotBeNull();
                RootResWithLoad.Assets.Tr.TrEnTranslation.Should().BeOfType<OptimizedTranslation>().And.NotBeNull();
            }

            void TestRootResWithResPaths()
            {
                typeof(RootResWithResPaths).ShouldConsistOf(Properties: ["DefaultBusLayoutTres"], NestedTypes: ["Assets"]);
                typeof(RootResWithResPaths.Assets).ShouldConsistOf(Properties: ["IconSvg"], NestedTypes: ["Tr"]);
                typeof(RootResWithResPaths.Assets.Tr).ShouldConsistOf(Properties: ["TrEnTranslation", "TrFrTranslation", "TrDeTranslation", "TrJpTranslation"]);
                RootResWithResPaths.DefaultBusLayoutTres.Should().Be((StringName)"res://default_bus_layout.tres");
                RootResWithResPaths.Assets.IconSvg.Should().Be((StringName)"res://Assets/icon.svg");
                RootResWithResPaths.Assets.Tr.TrEnTranslation.Should().Be((StringName)"res://Assets/tr/tr.en.translation");
            }

            void TestRootResWithDirPaths()
            {
                typeof(RootResWithDirPaths).ShouldConsistOf(Properties: ["ResPath"], NestedTypes: ["Assets"]);
                typeof(RootResWithDirPaths.Assets).ShouldConsistOf(Properties: ["ResPath"], NestedTypes: ["Tr"]);
                typeof(RootResWithDirPaths.Assets.Tr).ShouldConsistOf(Properties: ["ResPath"]);
                RootResWithDirPaths.ResPath.Should().Be((StringName)"res://");
                RootResWithDirPaths.Assets.ResPath.Should().Be((StringName)"res://Assets");
                RootResWithDirPaths.Assets.Tr.ResPath.Should().Be((StringName)"res://Assets/tr");
            }

            void TestRootResWithLoadAndResPaths()
            {
                typeof(RootResWithLoadAndResPaths).ShouldConsistOf(NestedTypes: ["Assets", "DefaultBusLayoutTres"]);
                typeof(RootResWithLoadAndResPaths.Assets).ShouldConsistOf(NestedTypes: ["Tr", "IconSvg"]);
                typeof(RootResWithLoadAndResPaths.Assets.Tr).ShouldConsistOf(NestedTypes: ["TrEnTranslation", "TrFrTranslation", "TrDeTranslation", "TrJpTranslation"]);
                typeof(RootResWithLoadAndResPaths.Assets.IconSvg).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
                typeof(RootResWithLoadAndResPaths.DefaultBusLayoutTres).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
                typeof(RootResWithLoadAndResPaths.Assets.Tr.TrEnTranslation).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);

                RootResWithLoadAndResPaths.Assets.IconSvg.ResPath.Should().Be((StringName)"res://Assets/icon.svg");
                RootResWithLoadAndResPaths.DefaultBusLayoutTres.ResPath.Should().Be((StringName)"res://default_bus_layout.tres");
                RootResWithLoadAndResPaths.Assets.Tr.TrEnTranslation.ResPath.Should().Be((StringName)"res://Assets/tr/tr.en.translation");

                RootResWithLoadAndResPaths.Assets.IconSvg.Load().Should().BeOfType<CompressedTexture2D>().And.NotBeNull();
                RootResWithLoadAndResPaths.DefaultBusLayoutTres.Load().Should().BeOfType<AudioBusLayout>().And.NotBeNull();
                RootResWithLoadAndResPaths.Assets.Tr.TrEnTranslation.Load().Should().BeOfType<OptimizedTranslation>().And.NotBeNull();
            }
        }

        static void TestAbsoluteRes()
            => AbsoluteRes.ResPath.Should().Be((StringName)"res://Assets");

        static void TestRelativeRes()
            => RelativeRes.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources");

        static void TestResTypes()
        {
            ResTypes.MyCsShaderCs.Should().BeOfType<CSharpScript>().And.NotBeNull();
            ResTypes.MyCsShaderCsUid.Should().Be((StringName)"uid://dmex1g7fv35a");
            ResTypes.MyCsShaderTres.Should().BeOfType<MyCsShader>().And.NotBeNull();

            ResTypes.MyGdShaderGd.Should().BeOfType<GDScript>().And.NotBeNull();
            ResTypes.MyGdShaderGdUid.Should().Be((StringName)"uid://dwdevd03t3rpx");
            ResTypes.MyGdShaderTres.Should().BeOfType<ShaderMaterial>().And.NotBeNull();

            ResTypes.Xtras._3DModelTxt.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/xtras/3DModel.txt");
            ResTypes.Xtras.Model3DTxt.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/xtras/Model3D.txt");
            ResTypes.Xtras.MyResCfg.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/xtras/MyRes.cfg");
            ResTypes.Xtras.MyResCsv.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/xtras/MyRes.csv");

            ResTypesWithScenes.ResourceTreeTestsTscn.Should().BeOfType<PackedScene>().And.NotBeNull();
        }
    }
}
