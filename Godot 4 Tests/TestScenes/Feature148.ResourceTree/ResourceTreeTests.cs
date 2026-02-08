using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;
using GodotSharp.SourceGenerators;
using GodotTests.TestScenes.ResourceTreeTestAssets;

namespace GodotTests.TestScenes;

#region Test Cases

[ResourceTree("res://", ResG.LoadRes, xclude: ["TestScenes"])]
public static partial class RootResWithLoad;

[ResourceTree("res://", ResG.ResPaths, xclude: ["TestScenes"])]
public static partial class RootResWithResPaths;

[ResourceTree("res://", ResG.DirPaths, xclude: ["TestScenes"])]
public static partial class RootResWithDirPaths;

[ResourceTree("res://", ResG.LoadRes | ResG.ResPaths, xclude: ["TestScenes"])]
public static partial class RootResWithLoadAndResPaths;

[ResourceTree("/", ResG.DirPaths)]
public static partial class AbsoluteRes;
[ResourceTree("Assets", ResG.DirPaths)]
public static partial class AbsoluteResDir1;
[ResourceTree("/Assets", ResG.DirPaths)]
public static partial class AbsoluteResDir2;

[ResourceTree(null, ResG.DirPaths)]
public static partial class RelativeRes1;
[ResourceTree(".", ResG.DirPaths)]
public static partial class RelativeRes2;
[ResourceTree("", ResG.DirPaths)]
public static partial class RelativeRes3;
[ResourceTree("Resources", ResG.DirPaths)]
public static partial class RelativeResDir1;
[ResourceTree("./Resources", ResG.DirPaths)]
public static partial class RelativeResDir2;

[ResourceTree("Resources", resi: ResI.All, xtras: ["csv", "cfg", "txt", "zip"])]
public static partial class ResWithTypes;

[ResourceTree(resi: ResI.Scenes)]
public static partial class ResWithScenes;

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
                RootResWithResPaths.DefaultBusLayoutTres.Should().Be("res://default_bus_layout.tres");
                RootResWithResPaths.Assets.IconSvg.Should().Be("res://Assets/icon.svg");
                RootResWithResPaths.Assets.Tr.TrEnTranslation.Should().Be("res://Assets/tr/tr.en.translation");
            }

            void TestRootResWithDirPaths()
            {
                typeof(RootResWithDirPaths).ShouldConsistOf(Properties: ["ResPath"], NestedTypes: ["Assets"]);
                typeof(RootResWithDirPaths.Assets).ShouldConsistOf(Properties: ["ResPath"], NestedTypes: ["Tr"]);
                typeof(RootResWithDirPaths.Assets.Tr).ShouldConsistOf(Properties: ["ResPath"]);
                RootResWithDirPaths.ResPath.Should().Be("res://");
                RootResWithDirPaths.Assets.ResPath.Should().Be("res://Assets");
                RootResWithDirPaths.Assets.Tr.ResPath.Should().Be("res://Assets/tr");
            }

            void TestRootResWithLoadAndResPaths()
            {
                typeof(RootResWithLoadAndResPaths).ShouldConsistOf(NestedTypes: ["Assets", "DefaultBusLayoutTres"]);
                typeof(RootResWithLoadAndResPaths.Assets).ShouldConsistOf(NestedTypes: ["Tr", "IconSvg"]);
                typeof(RootResWithLoadAndResPaths.Assets.Tr).ShouldConsistOf(NestedTypes: ["TrEnTranslation", "TrFrTranslation", "TrDeTranslation", "TrJpTranslation"]);
                typeof(RootResWithLoadAndResPaths.Assets.IconSvg).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
                typeof(RootResWithLoadAndResPaths.DefaultBusLayoutTres).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
                typeof(RootResWithLoadAndResPaths.Assets.Tr.TrEnTranslation).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);

                RootResWithLoadAndResPaths.Assets.IconSvg.ResPath.Should().Be("res://Assets/icon.svg");
                RootResWithLoadAndResPaths.DefaultBusLayoutTres.ResPath.Should().Be("res://default_bus_layout.tres");
                RootResWithLoadAndResPaths.Assets.Tr.TrEnTranslation.ResPath.Should().Be("res://Assets/tr/tr.en.translation");

                RootResWithLoadAndResPaths.Assets.IconSvg.Load().Should().BeOfType<CompressedTexture2D>().And.NotBeNull();
                RootResWithLoadAndResPaths.DefaultBusLayoutTres.Load().Should().BeOfType<AudioBusLayout>().And.NotBeNull();
                RootResWithLoadAndResPaths.Assets.Tr.TrEnTranslation.Load().Should().BeOfType<OptimizedTranslation>().And.NotBeNull();
            }
        }

        static void TestAbsoluteRes()
        {
            AbsoluteRes.ResPath.Should().Be("res://");
            AbsoluteResDir1.ResPath.Should().Be("res://Assets");
            AbsoluteResDir2.ResPath.Should().Be("res://Assets");
        }

        static void TestRelativeRes()
        {
            RelativeRes1.ResPath.Should().Be("res://TestScenes/Feature148.ResourceTree");
            RelativeRes2.ResPath.Should().Be("res://TestScenes/Feature148.ResourceTree");
            RelativeRes3.ResPath.Should().Be("res://TestScenes/Feature148.ResourceTree");
            RelativeResDir1.ResPath.Should().Be("res://TestScenes/Feature148.ResourceTree/Resources");
            RelativeResDir2.ResPath.Should().Be("res://TestScenes/Feature148.ResourceTree/Resources");
        }

        static void TestResTypes()
        {
            ResWithTypes.MyCsShaderCs.Should().BeOfType<CSharpScript>().And.NotBeNull();
            ResWithTypes.MyCsShaderCsUid.Should().Be("uid://dmex1g7fv35a");
            ResWithTypes.MyCsShaderTres.Should().BeOfType<MyCsShader>().And.NotBeNull();

            ResWithTypes.MyGdShaderGd.Should().BeOfType<GDScript>().And.NotBeNull();
            ResWithTypes.MyGdShaderGdUid.Should().Be("uid://dwdevd03t3rpx");
            ResWithTypes.MyGdShaderTres.Should().BeOfType<ShaderMaterial>().And.NotBeNull();

            ResWithTypes.Xtras._3DModelTxt.Should().Be("res://TestScenes/Feature148.ResourceTree/Resources/xtras/3DModel.txt");
            ResWithTypes.Xtras.Model3DTxt.Should().Be("res://TestScenes/Feature148.ResourceTree/Resources/xtras/Model3D.txt");
            ResWithTypes.Xtras.MyResCfg.Should().Be("res://TestScenes/Feature148.ResourceTree/Resources/xtras/MyRes.cfg");
            ResWithTypes.Xtras.MyResCsv.Should().Be("res://TestScenes/Feature148.ResourceTree/Resources/xtras/MyRes.csv");

            ResWithScenes.ResourceTreeTestsTscn.Should().BeOfType<PackedScene>().And.NotBeNull();

            ResWithTypes.MyAnimationAnim.Should().BeOfType<Animation>().And.NotBeNull();
            ResWithTypes.MyAtlasTextureAtlastex.Should().BeOfType<AtlasTexture>().And.NotBeNull();
            ResWithTypes.MyFontFontdata.Should().BeOfType<Font>().And.NotBeNull();
            ResWithTypes.MyJsonJson.Should().BeOfType<Json>().And.NotBeNull();
            ResWithTypes.MyMaterialMaterial.Should().BeOfType<FogMaterial>().And.NotBeNull();
            ResWithTypes.MyMeshMesh.Should().BeOfType<ArrayMesh>().And.NotBeNull();
            ResWithTypes.MyMeshRes.Should().BeOfType<ArrayMesh>().And.NotBeNull();
            ResWithTypes.MyMeshLibraryMeshlib.Should().BeOfType<MeshLibrary>().And.NotBeNull();
            ResWithTypes.MyMultiMeshMultimesh.Should().BeOfType<MultiMesh>().And.NotBeNull();
            ResWithTypes.MyOccluderOcc.Should().BeOfType<Occluder3D>().And.NotBeNull();
            ResWithTypes.MyPhysicsMaterialPhymat.Should().BeOfType<PhysicsMaterial>().And.NotBeNull();
            ResWithTypes.MyShaderGdshader.Should().BeOfType<Shader>().And.NotBeNull();
            ResWithTypes.MyShapeShape.Should().BeOfType<Shape3D>().And.NotBeNull();
            ResWithTypes.MyThemeTheme.Should().BeOfType<Theme>().And.NotBeNull();
            // Meaningless type, can probably be ignored.
            //ResWithTypes.MyTranslationTranslation.Should().BeOfType<Translation>().And.NotBeNull();
        }
    }
}
