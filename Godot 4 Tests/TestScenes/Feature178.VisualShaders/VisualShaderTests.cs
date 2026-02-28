using System.Collections.Generic;
using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;
using GodotTests.TestScenes.SUT_ShaderAttribute;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class VisualShaderTests : Node, ITest
{
    public enum MyIntAsEnum
    {
        A,
        B,
        C,
    }

    private readonly Dictionary<string, object> MyVisualShaderExpected = new() {

        // DEFAULTS //

        {"MyColor.Default", Colors.Black},
        {"MyBool.Default", default(bool)},

        {"MyFloat.Default", default(float)},
        {"MyFloatWithRange.Default", default(float)},
        {"MyFloatWithRangeStep.Default", default(float)},

        {"MyInt.Default", default(int)},
        {"MyIntWithRange.Default", default(int)},
        {"MyIntWithRangeStep.Default", default(int)},
        {"MyIntAsEnum.Default", default(MyIntAsEnum)},
        {"MyIntAsNonEnum.Default", (int)default(MyIntAsEnum)},

        {"MyUInt.Default", default(int)},

        {"MyCubemap.Default", null},
        {"MyTexture.Default", null},
        {"MyTexture3D.Default", null},
        {"MyTextureArray.Default", null},

        {"MyCubemapWithOptions.Default", null},
        {"MyTextureWithOptions.Default", null},
        {"MyTexture3DWithOptions.Default", null},
        {"MyTextureArrayWithOptions.Default", null},
        {"MyTransform.Default", Transform3D.Identity},

        {"MyVector2.Default", Vector2.Zero},
        {"MyVector3.Default", Vector3.Zero},
        {"MyVector4.Default", Vector4.Zero},

        {"MyGlobalColor.Default", Colors.Black},
        {"MyInstanceColor.Default", Colors.Black},
        {"MyInstanceColorWithIndex.Default", Colors.Black},

        // VALUES //

        {"MyColor.Value", Colors.Red},
        {"MyBool.Value", true},

        {"MyFloat.Value", .1f},
        {"MyFloatWithRange.Value", .1f},
        {"MyFloatWithRangeStep.Value", .1f},

        {"MyInt.Value", 1},
        {"MyIntWithRange.Value", 1},
        {"MyIntWithRangeStep.Value", 1},
        {"MyIntAsEnum.Value", MyIntAsEnum.B},
        {"MyIntAsNonEnum.Value", (int)MyIntAsEnum.B},

        {"MyUInt.Value", 1},

        {"MyCubemap.Value", new Cubemap()},
        {"MyTexture.Value", new ImageTexture()},
        {"MyTexture3D.Value", new ImageTexture3D()},
        {"MyTextureArray.Value", new Texture2DArray()},

        {"MyCubemapWithOptions.Value", new Cubemap()},
        {"MyTextureWithOptions.Value", new ImageTexture()},
        {"MyTexture3DWithOptions.Value", new ImageTexture3D()},
        {"MyTextureArrayWithOptions.Value", new Texture2DArray()},
        {"MyTransform.Value", new Transform3D(Basis.Identity, Vector3.One)},

        {"MyVector2.Value", Vector2.One},
        {"MyVector3.Value", Vector3.One},
        {"MyVector4.Value", Vector4.One},

        {"MyGlobalColor.Value", Colors.Red},
        {"MyInstanceColor.Value", Colors.Red},
        {"MyInstanceColorWithIndex.Value", Colors.Red},
    };

    private readonly Dictionary<string, object> EmptyVisualShaderExpected = [];

    private readonly Dictionary<string, object> MyVisualShaderWithDefaultsExpected = new() {

        // DEFAULTS //

        {"MyColor.Default", Colors.Red},
        {"MyBool.Default", true},

        {"MyFloat.Default", .1f},
        {"MyFloatWithRange.Default", .1f},
        {"MyFloatWithRangeStep.Default", .1f},

        {"MyInt.Default", 1},
        {"MyIntWithRange.Default", 1},
        {"MyIntWithRangeStep.Default", 1},
        {"MyIntAsEnum.Default", MyIntAsEnum.B},
        {"MyIntAsNonEnum.Default", (int)MyIntAsEnum.B},

        {"MyUInt.Default", 1},

        {"MyCubemap.Default", null}, // No default
        {"MyTexture.Default", null}, // No default
        {"MyTexture3D.Default", null}, // No default
        {"MyTextureArray.Default", null}, // No default

        {"MyCubemapWithOptions.Default", null}, // No default
        {"MyTextureWithOptions.Default", null}, // No default
        {"MyTexture3DWithOptions.Default", null}, // No default
        {"MyTextureArrayWithOptions.Default", null}, // No default
        {"MyTransform.Default", new Transform3D(Basis.Identity, Vector3.One)},

        {"MyVector2.Default", Vector2.One},
        {"MyVector3.Default", Vector3.One},
        {"MyVector4.Default", Vector4.One},

        {"MyGlobalColor.Default", Colors.Red},
        {"MyInstanceColor.Default", Colors.Red},
        {"MyInstanceColorWithIndex.Default", Colors.Red},

        // VALUES //

        {"MyColor.Value", Colors.Black},
        {"MyBool.Value", default(bool)},

        {"MyFloat.Value", default(float)},
        {"MyFloatWithRange.Value", default(float)},
        {"MyFloatWithRangeStep.Value", default(float)},

        {"MyInt.Value", default(int)},
        {"MyIntWithRange.Value", default(int)},
        {"MyIntWithRangeStep.Value", default(int)},
        {"MyIntAsEnum.Value", default(MyIntAsEnum)},
        {"MyIntAsNonEnum.Value", (int)default(MyIntAsEnum)},

        {"MyUInt.Value", default(int)},

        {"MyCubemap.Value", null},
        {"MyTexture.Value", null},
        {"MyTexture3D.Value", null},
        {"MyTextureArray.Value", null},

        {"MyCubemapWithOptions.Value", null},
        {"MyTextureWithOptions.Value", null},
        {"MyTexture3DWithOptions.Value", null},
        {"MyTextureArrayWithOptions.Value", null},
        {"MyTransform.Value", Transform3D.Identity},

        {"MyVector2.Value", Vector2.Zero},
        {"MyVector3.Value", Vector3.Zero},
        {"MyVector4.Value", Vector4.Zero},

        {"MyGlobalColor.Value", Colors.Black},
        {"MyInstanceColor.Value", Colors.Black},
        {"MyInstanceColorWithIndex.Value", Colors.Black},
    };

    void ITest.ReadyTests()
    {
        RunGeneratedTests();
        RunNoGeneratedTests();
        RunImplicitOperatorTests();
        RunGeneratedTestsFromScene();

        void RunGeneratedTests()
        {
            MyVisualShader.RunTests(MyVisualShaderExpected);
            MyVisualShaderAsStatic.RunTests(MyVisualShaderExpected);
            MyVisualShaderAsResource.RunTests(MyVisualShaderExpected);
            MyVisualShaderAsShaderMaterial.RunTests(MyVisualShaderExpected);

            EmptyVisualShader.RunTests(EmptyVisualShaderExpected);
            EmptyVisualShaderAsStatic.RunTests(EmptyVisualShaderExpected);
            EmptyVisualShaderAsResource.RunTests(EmptyVisualShaderExpected);
            EmptyVisualShaderAsShaderMaterial.RunTests(EmptyVisualShaderExpected);

            MyVisualShaderWithDefaults.RunTests(MyVisualShaderWithDefaultsExpected);
            MyVisualShaderWithDefaultsAsStatic.RunTests(MyVisualShaderWithDefaultsExpected);
            MyVisualShaderWithDefaultsAsResource.RunTests(MyVisualShaderWithDefaultsExpected);
            MyVisualShaderWithDefaultsAsShaderMaterial.RunTests(MyVisualShaderWithDefaultsExpected);
        }

        void RunNoGeneratedTests()
        {
            typeof(MyVisualShader_NO_TEST).GetMethod("RunTests").Should().BeNull();
            typeof(MyVisualShaderAsStatic_NO_TEST).GetMethod("RunTests").Should().BeNull();
            typeof(MyVisualShaderAsResource_NO_TEST).GetMethod("RunTests").Should().BeNull();
            typeof(MyVisualShaderAsShaderMaterial_NO_TEST).GetMethod("RunTests").Should().BeNull();

            typeof(EmptyVisualShader_NO_TEST).GetMethod("RunTests").Should().BeNull();
            typeof(EmptyVisualShaderAsStatic_NO_TEST).GetMethod("RunTests").Should().BeNull();
            typeof(EmptyVisualShaderAsResource_NO_TEST).GetMethod("RunTests").Should().BeNull();
            typeof(EmptyVisualShaderAsShaderMaterial_NO_TEST).GetMethod("RunTests").Should().BeNull();

            typeof(MyVisualShaderWithDefaults_NO_TEST).GetMethod("RunTests").Should().BeNull();
            typeof(MyVisualShaderWithDefaultsAsStatic_NO_TEST).GetMethod("RunTests").Should().BeNull();
            typeof(MyVisualShaderWithDefaultsAsResource_NO_TEST).GetMethod("RunTests").Should().BeNull();
            typeof(MyVisualShaderWithDefaultsAsShaderMaterial_NO_TEST).GetMethod("RunTests").Should().BeNull();
        }

        void RunImplicitOperatorTests()
        {
            {
                var material = (ShaderMaterial)MeshWithMyVisualShader.MaterialOverride;
                TestImplicitShader(material, out var shader);
                TestImplicitMaterial(shader);

                void TestImplicitShader(MyVisualShader sut, out MyVisualShader @out)
                    => (@out = sut).Material.Should().Be(material);

                void TestImplicitMaterial(ShaderMaterial sut)
                    => sut.Should().Be(material);
            }

            {
                var material = (ShaderMaterial)MeshWithEmptyVisualShader.MaterialOverride;
                TestImplicitShader(material, out var shader);
                TestImplicitMaterial(shader);

                void TestImplicitShader(EmptyVisualShader sut, out EmptyVisualShader @out)
                    => (@out = sut).Material.Should().Be(material);

                void TestImplicitMaterial(ShaderMaterial sut)
                    => sut.Should().Be(material);
            }

            {
                var material = (ShaderMaterial)MeshWithMyVisualShaderWithDefaults.MaterialOverride;
                TestImplicitShader(material, out var shader);
                TestImplicitMaterial(shader);

                void TestImplicitShader(MyVisualShaderWithDefaults sut, out MyVisualShaderWithDefaults @out)
                    => (@out = sut).Material.Should().Be(material);

                void TestImplicitMaterial(ShaderMaterial sut)
                    => sut.Should().Be(material);
            }
        }

        void RunGeneratedTestsFromScene()
        {
            {
                var sutMyVisualShaderShaderMaterial = (ShaderMaterial)MeshWithMyVisualShader.MaterialOverride;
                MyVisualShader.RunTests(MyVisualShaderExpected, sutMyVisualShaderShaderMaterial);
                MyVisualShaderAsStatic.RunTests(MyVisualShaderExpected, sutMyVisualShaderShaderMaterial);
                MyVisualShaderAsResource.RunTests(MyVisualShaderExpected, sutMyVisualShaderShaderMaterial);
                //MyVisualShaderAsShaderMaterial.RunTests(MyVisualShaderExpected, sutMyVisualShaderShaderMaterial);

                var sutMyVisualShaderAsShaderMaterial = (MyVisualShaderAsShaderMaterial)MeshWithMyVisualShaderAsShaderMaterial.MaterialOverride;
                MyVisualShaderAsShaderMaterial.RunTests(MyVisualShaderExpected, sutMyVisualShaderAsShaderMaterial);
            }

            {
                var sutEmptyVisualShaderShaderMaterial = (ShaderMaterial)MeshWithEmptyVisualShader.MaterialOverride;
                EmptyVisualShader.RunTests(EmptyVisualShaderExpected, sutEmptyVisualShaderShaderMaterial);
                EmptyVisualShaderAsStatic.RunTests(EmptyVisualShaderExpected, sutEmptyVisualShaderShaderMaterial);
                EmptyVisualShaderAsResource.RunTests(EmptyVisualShaderExpected, sutEmptyVisualShaderShaderMaterial);
                //EmptyVisualShaderAsShaderMaterial.RunTests(EmptyVisualShaderExpected, sutEmptyVisualShaderShaderMaterial);

                var sutEmptyVisualShaderAsShaderMaterial = (EmptyVisualShaderAsShaderMaterial)MeshWithEmptyVisualShaderAsShaderMaterial.MaterialOverride;
                EmptyVisualShaderAsShaderMaterial.RunTests(EmptyVisualShaderExpected, sutEmptyVisualShaderAsShaderMaterial);
            }

            {
                // FIXME: VisualShader defaults not set when added to local/scene ShaderMaterial (no visible assignments in tscn)
                //var sutMyVisualShaderWithDefaultsShaderMaterial = (ShaderMaterial)MeshWithMyVisualShaderWithDefaults.MaterialOverride;
                //MyVisualShaderWithDefaults.RunTests(MyVisualShaderWithDefaultsExpected, sutMyVisualShaderWithDefaultsShaderMaterial);
                //MyVisualShaderWithDefaultsAsStatic.RunTests(MyVisualShaderWithDefaultsExpected, sutMyVisualShaderWithDefaultsShaderMaterial);
                //MyVisualShaderWithDefaultsAsResource.RunTests(MyVisualShaderWithDefaultsExpected, sutMyVisualShaderWithDefaultsShaderMaterial);
                ////MyVisualShaderWithDefaultsAsShaderMaterial.RunTests(MyVisualShaderWithDefaultsExpected, sutMyVisualShaderWithDefaultsShaderMaterial);

                // OK: VisualShader defaults are set when added to external ShaderMaterial (still no visible assignments in tres or tscn)
                var sutMyVisualShaderWithDefaultsAsShaderMaterial = (MyVisualShaderWithDefaultsAsShaderMaterial)MeshWithMyVisualShaderWithDefaultsAsShaderMaterial.MaterialOverride;
                MyVisualShaderWithDefaultsAsShaderMaterial.RunTests(MyVisualShaderWithDefaultsExpected, sutMyVisualShaderWithDefaultsAsShaderMaterial);
            }
        }
    }
}
