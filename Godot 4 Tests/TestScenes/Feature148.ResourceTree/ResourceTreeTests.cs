using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;
using GodotSharp.SourceGenerators.ResourceTreeExtensions;
using GodotTests.TestScenes.ResourceTreeTestAssets;

namespace GodotTests.TestScenes;
#region Test Cases (dflt)

[ResourceTree(".", xclude: ["TestScenes"])]
public static partial class RootRes;

[ResourceTree("Assets")]
public static partial class AbsoluteRes;

[ResourceTree("Resources")]
public static partial class RelativeRes;

[ResourceTree(res: Res.Scenes)]
public static partial class ResWithScenes;

[ResourceTree("Resources", res: Res.Scripts)]
public static partial class ResWithScripts;

[ResourceTree("Resources", xtras: ["csv", "cfg", "txt", "zip"])]
public static partial class ResWithXtras;

[ResourceTree("Resources", res: Res.Uid)]
public static partial class ResWithUid;

#endregion
#region Test Cases (load)

[ResourceTree("/", Res.Load, xclude: ["TestScenes"])]
public static partial class RootResWithLoad;

[ResourceTree("Assets", Res.Load)]
public static partial class AbsoluteResWithLoad;

[ResourceTree("Resources", Res.Load)]
public static partial class RelativeResWithLoad;

[ResourceTree(res: Res.Scenes | Res.Load)]
public static partial class ResWithScenesWithLoad;

[ResourceTree("Resources", res: Res.Scripts | Res.Load)]
public static partial class ResWithScriptsWithLoad;

[ResourceTree("Resources", res: Res.Load, xtras: ["csv", "cfg", "txt", "zip"])]
public static partial class ResWithXtrasWithLoad;

[ResourceTree("Resources", res: Res.Uid | Res.Load)]
public static partial class ResWithUidWithLoad;

#endregion

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
            typeof(RootRes).ShouldConsistOf(Properties: ["ResPath", "DefaultBusLayoutTres"], NestedTypes: ["Assets"]);
            RootRes.ResPath.Should().Be((StringName)"res://");
            RootRes.DefaultBusLayoutTres.Should().Be((StringName)"res://default_bus_layout.tres");

            typeof(RootRes.Assets).ShouldConsistOf(Properties: ["ResPath", "IconSvg"], NestedTypes: ["Tr"]);
            RootRes.Assets.ResPath.Should().Be((StringName)"res://Assets");
            RootRes.Assets.IconSvg.Should().Be((StringName)"res://Assets/icon.svg");

            typeof(RootRes.Assets.Tr).ShouldConsistOf(Properties: ["ResPath", "TrDeTranslation", "TrEnTranslation", "TrFrTranslation", "TrJpTranslation"]);
            RootRes.Assets.Tr.ResPath.Should().Be((StringName)"res://Assets/tr");
            RootRes.Assets.Tr.TrDeTranslation.Should().Be((StringName)"res://Assets/tr/tr.de.translation");
            RootRes.Assets.Tr.TrEnTranslation.Should().Be((StringName)"res://Assets/tr/tr.en.translation");
            RootRes.Assets.Tr.TrFrTranslation.Should().Be((StringName)"res://Assets/tr/tr.fr.translation");
            RootRes.Assets.Tr.TrJpTranslation.Should().Be((StringName)"res://Assets/tr/tr.jp.translation");

            typeof(RootResWithLoad).ShouldConsistOf(Properties: ["ResPath"], NestedTypes: ["Assets", "DefaultBusLayoutTres"]);
            typeof(RootResWithLoad.DefaultBusLayoutTres).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
            RootResWithLoad.ResPath.Should().Be((StringName)"res://");
            RootResWithLoad.DefaultBusLayoutTres.ResPath.Should().Be((StringName)"res://default_bus_layout.tres");
            RootResWithLoad.DefaultBusLayoutTres.Load().Should().BeOfType<AudioBusLayout>().And.NotBeNull();

            typeof(RootResWithLoad.Assets).ShouldConsistOf(Properties: ["ResPath"], NestedTypes: ["Tr", "IconSvg"]);
            typeof(RootResWithLoad.Assets.IconSvg).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
            RootResWithLoad.Assets.ResPath.Should().Be((StringName)"res://Assets");
            RootResWithLoad.Assets.IconSvg.ResPath.Should().Be((StringName)"res://Assets/icon.svg");
            RootResWithLoad.Assets.IconSvg.Load().Should().BeOfType<CompressedTexture2D>().And.NotBeNull();

            typeof(RootResWithLoad.Assets.Tr).ShouldConsistOf(Properties: ["ResPath"], NestedTypes: ["TrDeTranslation", "TrEnTranslation", "TrFrTranslation", "TrJpTranslation"]);
            typeof(RootResWithLoad.Assets.Tr.TrDeTranslation).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
            typeof(RootResWithLoad.Assets.Tr.TrEnTranslation).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
            typeof(RootResWithLoad.Assets.Tr.TrFrTranslation).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
            typeof(RootResWithLoad.Assets.Tr.TrDeTranslation).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
            RootResWithLoad.Assets.Tr.ResPath.Should().Be((StringName)"res://Assets/tr");
            RootResWithLoad.Assets.Tr.TrDeTranslation.ResPath.Should().Be((StringName)"res://Assets/tr/tr.de.translation");
            RootResWithLoad.Assets.Tr.TrEnTranslation.ResPath.Should().Be((StringName)"res://Assets/tr/tr.en.translation");
            RootResWithLoad.Assets.Tr.TrFrTranslation.ResPath.Should().Be((StringName)"res://Assets/tr/tr.fr.translation");
            RootResWithLoad.Assets.Tr.TrJpTranslation.ResPath.Should().Be((StringName)"res://Assets/tr/tr.jp.translation");
            RootResWithLoad.Assets.Tr.TrDeTranslation.Load().Should().BeOfType<OptimizedTranslation>().And.NotBeNull();
            RootResWithLoad.Assets.Tr.TrEnTranslation.Load().Should().BeOfType<OptimizedTranslation>().And.NotBeNull();
            RootResWithLoad.Assets.Tr.TrFrTranslation.Load().Should().BeOfType<OptimizedTranslation>().And.NotBeNull();
            RootResWithLoad.Assets.Tr.TrJpTranslation.Load().Should().BeOfType<OptimizedTranslation>().And.NotBeNull();
        }

        static void TestAbsoluteRes()
        {
            typeof(AbsoluteRes).ShouldConsistOf(Properties: ["ResPath", "IconSvg"], NestedTypes: ["Tr"]);
            AbsoluteRes.ResPath.Should().Be((StringName)"res://Assets");
            AbsoluteRes.IconSvg.Should().Be((StringName)"res://Assets/icon.svg");

            typeof(AbsoluteRes.Tr).ShouldConsistOf(Properties: ["ResPath", "TrDeTranslation", "TrEnTranslation", "TrFrTranslation", "TrJpTranslation"]);
            AbsoluteRes.Tr.ResPath.Should().Be((StringName)"res://Assets/tr");
            AbsoluteRes.Tr.TrDeTranslation.Should().Be((StringName)"res://Assets/tr/tr.de.translation");
            AbsoluteRes.Tr.TrEnTranslation.Should().Be((StringName)"res://Assets/tr/tr.en.translation");
            AbsoluteRes.Tr.TrFrTranslation.Should().Be((StringName)"res://Assets/tr/tr.fr.translation");
            AbsoluteRes.Tr.TrJpTranslation.Should().Be((StringName)"res://Assets/tr/tr.jp.translation");

            typeof(AbsoluteResWithLoad).ShouldConsistOf(Properties: ["ResPath"], NestedTypes: ["Tr", "IconSvg"]);
            typeof(AbsoluteResWithLoad.IconSvg).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
            AbsoluteResWithLoad.ResPath.Should().Be((StringName)"res://Assets");
            AbsoluteResWithLoad.IconSvg.ResPath.Should().Be((StringName)"res://Assets/icon.svg");
            AbsoluteResWithLoad.IconSvg.Load().Should().BeOfType<CompressedTexture2D>().And.NotBeNull();

            typeof(AbsoluteResWithLoad.Tr).ShouldConsistOf(Properties: ["ResPath"], NestedTypes: ["TrDeTranslation", "TrEnTranslation", "TrFrTranslation", "TrJpTranslation"]);
            typeof(AbsoluteResWithLoad.Tr.TrDeTranslation).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
            typeof(AbsoluteResWithLoad.Tr.TrEnTranslation).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
            typeof(AbsoluteResWithLoad.Tr.TrFrTranslation).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
            typeof(AbsoluteResWithLoad.Tr.TrDeTranslation).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
            AbsoluteResWithLoad.Tr.ResPath.Should().Be((StringName)"res://Assets/tr");
            AbsoluteResWithLoad.Tr.TrDeTranslation.ResPath.Should().Be((StringName)"res://Assets/tr/tr.de.translation");
            AbsoluteResWithLoad.Tr.TrEnTranslation.ResPath.Should().Be((StringName)"res://Assets/tr/tr.en.translation");
            AbsoluteResWithLoad.Tr.TrFrTranslation.ResPath.Should().Be((StringName)"res://Assets/tr/tr.fr.translation");
            AbsoluteResWithLoad.Tr.TrJpTranslation.ResPath.Should().Be((StringName)"res://Assets/tr/tr.jp.translation");
            AbsoluteResWithLoad.Tr.TrDeTranslation.Load().Should().BeOfType<OptimizedTranslation>().And.NotBeNull();
            AbsoluteResWithLoad.Tr.TrEnTranslation.Load().Should().BeOfType<OptimizedTranslation>().And.NotBeNull();
            AbsoluteResWithLoad.Tr.TrFrTranslation.Load().Should().BeOfType<OptimizedTranslation>().And.NotBeNull();
            AbsoluteResWithLoad.Tr.TrJpTranslation.Load().Should().BeOfType<OptimizedTranslation>().And.NotBeNull();
        }

        static void TestRelativeRes()
        {
            typeof(RelativeRes).ShouldConsistOf(Properties: ["ResPath", "MyCsShaderTres", "MyGdShaderTres", "MyNoShaderTres"]);
            RelativeRes.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources");
            RelativeRes.MyCsShaderTres.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyCsShader.tres");
            RelativeRes.MyGdShaderTres.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyGdShader.tres");
            RelativeRes.MyNoShaderTres.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyNoShader.tres");

            typeof(RelativeResWithLoad).ShouldConsistOf(Properties: ["ResPath"], NestedTypes: ["MyCsShaderTres", "MyGdShaderTres", "MyNoShaderTres"]);
            typeof(RelativeResWithLoad.MyCsShaderTres).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
            typeof(RelativeResWithLoad.MyGdShaderTres).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
            typeof(RelativeResWithLoad.MyNoShaderTres).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
            RelativeResWithLoad.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources");
            RelativeResWithLoad.MyCsShaderTres.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyCsShader.tres");
            RelativeResWithLoad.MyGdShaderTres.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyGdShader.tres");
            RelativeResWithLoad.MyNoShaderTres.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyNoShader.tres");
            RelativeResWithLoad.MyCsShaderTres.Load().Should().BeOfType<MyCsShader>().And.NotBeNull();
            RelativeResWithLoad.MyGdShaderTres.Load().Should().BeOfType<ShaderMaterial>().And.NotBeNull();
            RelativeResWithLoad.MyNoShaderTres.Load().Should().BeOfType<ShaderMaterial>().And.NotBeNull();
        }

        static void TestResWithScenes()
        {
            typeof(ResWithScenes).ShouldConsistOf(Properties: ["ResPath", "ResourceTreeTestsTscn"], NestedTypes: ["Resources"]);
            ResWithScenes.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree");
            ResWithScenes.ResourceTreeTestsTscn.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/ResourceTreeTests.tscn");

            typeof(ResWithScenesWithLoad).ShouldConsistOf(Properties: ["ResPath"], NestedTypes: ["Resources", "ResourceTreeTestsTscn"]);
            typeof(ResWithScenesWithLoad.ResourceTreeTestsTscn).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
            ResWithScenesWithLoad.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree");
            ResWithScenesWithLoad.ResourceTreeTestsTscn.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/ResourceTreeTests.tscn");
            ResWithScenesWithLoad.ResourceTreeTestsTscn.Load().Should().BeOfType<PackedScene>().And.NotBeNull();
        }

        static void TestResWithScripts()
        {
            typeof(ResWithScripts).ShouldConsistOf(Properties: ["ResPath", "MyCsShaderCs", "MyCsShaderTres", "MyGdShaderGd", "MyGdShaderTres", "MyNoShaderTres"]);
            ResWithScripts.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources");
            ResWithScripts.MyCsShaderCs.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyCsShader.cs");
            ResWithScripts.MyGdShaderGd.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyGdShader.gd");
            ResWithScripts.MyCsShaderTres.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyCsShader.tres");
            ResWithScripts.MyGdShaderTres.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyGdShader.tres");
            ResWithScripts.MyNoShaderTres.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyNoShader.tres");

            typeof(ResWithScriptsWithLoad).ShouldConsistOf(Properties: ["ResPath"], NestedTypes: ["MyCsShaderCs", "MyCsShaderTres", "MyGdShaderGd", "MyGdShaderTres", "MyNoShaderTres"]);
            typeof(ResWithScriptsWithLoad.MyCsShaderCs).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
            typeof(ResWithScriptsWithLoad.MyGdShaderGd).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
            typeof(ResWithScriptsWithLoad.MyCsShaderTres).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
            typeof(ResWithScriptsWithLoad.MyGdShaderTres).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
            typeof(ResWithScriptsWithLoad.MyNoShaderTres).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
            ResWithScriptsWithLoad.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources");
            ResWithScriptsWithLoad.MyCsShaderCs.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyCsShader.cs");
            ResWithScriptsWithLoad.MyGdShaderGd.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyGdShader.gd");
            ResWithScriptsWithLoad.MyCsShaderTres.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyCsShader.tres");
            ResWithScriptsWithLoad.MyGdShaderTres.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyGdShader.tres");
            ResWithScriptsWithLoad.MyNoShaderTres.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyNoShader.tres");
            ResWithScriptsWithLoad.MyCsShaderCs.Load().Should().BeOfType<CSharpScript>().And.NotBeNull();
            ResWithScriptsWithLoad.MyGdShaderGd.Load().Should().BeOfType<GDScript>().And.NotBeNull();
            ResWithScriptsWithLoad.MyCsShaderTres.Load().Should().BeOfType<MyCsShader>().And.NotBeNull();
            ResWithScriptsWithLoad.MyGdShaderTres.Load().Should().BeOfType<ShaderMaterial>().And.NotBeNull();
            ResWithScriptsWithLoad.MyNoShaderTres.Load().Should().BeOfType<ShaderMaterial>().And.NotBeNull();
        }

        static void TestResWithXtras()
        {
            // Should be same as TestRelativeRes + xtras
            typeof(ResWithXtras).ShouldConsistOf(Properties: ["ResPath", "MyCsShaderTres", "MyGdShaderTres", "MyNoShaderTres"], NestedTypes: ["Xtras"]);
            ResWithXtras.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources");
            ResWithXtras.MyCsShaderTres.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyCsShader.tres");
            ResWithXtras.MyGdShaderTres.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyGdShader.tres");
            ResWithXtras.MyNoShaderTres.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyNoShader.tres");

            typeof(ResWithXtrasWithLoad).ShouldConsistOf(Properties: ["ResPath"], NestedTypes: ["Xtras", "MyCsShaderTres", "MyGdShaderTres", "MyNoShaderTres"]);
            typeof(ResWithXtrasWithLoad.MyCsShaderTres).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
            typeof(ResWithXtrasWithLoad.MyGdShaderTres).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
            typeof(ResWithXtrasWithLoad.MyNoShaderTres).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
            ResWithXtrasWithLoad.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources");
            ResWithXtrasWithLoad.MyCsShaderTres.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyCsShader.tres");
            ResWithXtrasWithLoad.MyGdShaderTres.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyGdShader.tres");
            ResWithXtrasWithLoad.MyNoShaderTres.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyNoShader.tres");
            ResWithXtrasWithLoad.MyCsShaderTres.Load().Should().BeOfType<MyCsShader>().And.NotBeNull();
            ResWithXtrasWithLoad.MyGdShaderTres.Load().Should().BeOfType<ShaderMaterial>().And.NotBeNull();
            ResWithXtrasWithLoad.MyNoShaderTres.Load().Should().BeOfType<ShaderMaterial>().And.NotBeNull();

            // Testing xtras
            typeof(ResWithXtras.Xtras).ShouldConsistOf(Properties: ["ResPath", "_3DModelTxt", "Model3DTxt", "MyResCfg", "MyResCsv"]);
            ResWithXtras.Xtras.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/xtras");
            ResWithXtras.Xtras._3DModelTxt.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/xtras/3DModel.txt");
            ResWithXtras.Xtras.Model3DTxt.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/xtras/Model3D.txt");
            ResWithXtras.Xtras.MyResCfg.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/xtras/MyRes.cfg");
            ResWithXtras.Xtras.MyResCsv.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/xtras/MyRes.csv");
            FileAccess.FileExists(ResWithXtras.Xtras._3DModelTxt).Should().BeTrue();
            FileAccess.FileExists(ResWithXtras.Xtras.Model3DTxt).Should().BeTrue();
            FileAccess.FileExists(ResWithXtras.Xtras.MyResCfg).Should().BeTrue();
            FileAccess.FileExists(ResWithXtras.Xtras.MyResCsv).Should().BeTrue();

            // No type for xtras, so should be same
            typeof(ResWithXtrasWithLoad.Xtras).ShouldConsistOf(Properties: ["ResPath", "_3DModelTxt", "Model3DTxt", "MyResCfg", "MyResCsv"]);
            ResWithXtrasWithLoad.Xtras.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/xtras");
            ResWithXtrasWithLoad.Xtras._3DModelTxt.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/xtras/3DModel.txt");
            ResWithXtrasWithLoad.Xtras.Model3DTxt.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/xtras/Model3D.txt");
            ResWithXtrasWithLoad.Xtras.MyResCfg.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/xtras/MyRes.cfg");
            ResWithXtrasWithLoad.Xtras.MyResCsv.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/xtras/MyRes.csv");
            FileAccess.FileExists(ResWithXtrasWithLoad.Xtras._3DModelTxt).Should().BeTrue();
            FileAccess.FileExists(ResWithXtrasWithLoad.Xtras.Model3DTxt).Should().BeTrue();
            FileAccess.FileExists(ResWithXtrasWithLoad.Xtras.MyResCfg).Should().BeTrue();
            FileAccess.FileExists(ResWithXtrasWithLoad.Xtras.MyResCsv).Should().BeTrue();
        }

        static void TestResWithUID()
        {
            // Should be same as TestRelativeRes + uid
            typeof(ResWithUid).ShouldConsistOf(Properties: ["ResPath", "MyCsShaderCsUid", "MyCsShaderTres", "MyGdShaderGdUid", "MyGdShaderTres", "MyNoShaderTres"]);
            ResWithUid.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources");
            ResWithUid.MyCsShaderTres.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyCsShader.tres");
            ResWithUid.MyGdShaderTres.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyGdShader.tres");
            ResWithUid.MyNoShaderTres.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyNoShader.tres");

            typeof(ResWithUidWithLoad).ShouldConsistOf(Properties: ["ResPath", "MyCsShaderCsUid", "MyGdShaderGdUid"], NestedTypes: ["MyCsShaderTres", "MyGdShaderTres", "MyNoShaderTres"]);
            typeof(ResWithUidWithLoad.MyCsShaderTres).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
            typeof(ResWithUidWithLoad.MyGdShaderTres).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
            typeof(ResWithUidWithLoad.MyNoShaderTres).ShouldConsistOf(Properties: ["ResPath"], Methods: ["Load"]);
            ResWithUidWithLoad.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources");
            ResWithUidWithLoad.MyCsShaderTres.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyCsShader.tres");
            ResWithUidWithLoad.MyGdShaderTres.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyGdShader.tres");
            ResWithUidWithLoad.MyNoShaderTres.ResPath.Should().Be((StringName)"res://TestScenes/Feature148.ResourceTree/Resources/MyNoShader.tres");
            ResWithUidWithLoad.MyCsShaderTres.Load().Should().BeOfType<MyCsShader>().And.NotBeNull();
            ResWithUidWithLoad.MyGdShaderTres.Load().Should().BeOfType<ShaderMaterial>().And.NotBeNull();
            ResWithUidWithLoad.MyNoShaderTres.Load().Should().BeOfType<ShaderMaterial>().And.NotBeNull();

            // Testing uid
            ResWithUid.MyCsShaderCsUid.Should().Be((StringName)"uid://dmex1g7fv35a");
            ResWithUid.MyGdShaderGdUid.Should().Be((StringName)"uid://dwdevd03t3rpx");

            // No type for uid, so should be same
            ResWithUidWithLoad.MyCsShaderCsUid.Should().Be((StringName)"uid://dmex1g7fv35a");
            ResWithUidWithLoad.MyGdShaderGdUid.Should().Be((StringName)"uid://dwdevd03t3rpx");
        }
    }
}
