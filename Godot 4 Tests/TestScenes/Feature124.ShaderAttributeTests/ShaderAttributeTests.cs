using System.Collections.Generic;
using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class ShaderAttributeTests : Node, ITest
{
    public enum MyShaderEnum
    {
        A = 30,
        B = 60,
        C = 200,
    }

    private readonly Dictionary<string, object> MyShaderExpected = new() {
        {"UniformCount", 38},

        // DEFAULTS //

        {"MyInt.Default", 0},
        {"MyUint.Default", 0},
        {"MyBool.Default", false},
        {"MyFloat.Default", .0f},

        // Shader will set matrices to Identity if no default provided
        {"MyMat2.Default", Transform2D.Identity},
        {"MyMat3.Default", Basis.Identity},
        {"MyMat4.Default", Transform3D.Identity},

        {"MyVec2.Default", Vector2.Zero},
        {"MyVec3.Default", Vector3.Zero},
        {"MyVec4.Default", Vector4.Zero},

        {"MyIvec2.Default", Vector2I.Zero},
        {"MyIvec3.Default", Vector3I.Zero},
        {"MyIvec4.Default", Vector4I.Zero},

        {"MyUvec2.Default", Vector2I.Zero},
        {"MyUvec3.Default", Vector3I.Zero},
        {"MyUvec4.Default", Vector4I.Zero},

        {"MyBvec2.Default", 0},
        {"MyBvec3.Default", 0},
        {"MyBvec4.Default", 0},

        // Shader will set colors to 0,0,0,1 if no default provided
        {"MyCol3.Default", new Color(0, 0, 0, 1)},
        {"MyCol4.Default", new Color(0, 0, 0, 1)},

        {"MyIntAsRange.Default", 0},
        {"MyFloatAsRange.Default", .0f},

        {"MyEnumAsInt1.Default", 0},
        {"MyEnumAsInt2.Default", 0},
        {"MyEnumAsEnum.Default", (MyShaderEnum)0},

        {"MySampler2D.Default", null},
        {"MyIsampler2D.Default", null},
        {"MyUsampler2D.Default", null},

        {"MySampler3D.Default", null},
        {"MyIsampler3D.Default", null},
        {"MyUsampler3D.Default", null},

        {"MySampler2DArray.Default", null},
        {"MyIsampler2DArray.Default", null},
        {"MyUsampler2DArray.Default", null},

        {"MySamplerCube.Default", null},
        {"MySamplerCubeArray.Default", null},
        {"MySamplerExternalOES.Default", null },

        // VALUES //

        {"MyInt.Value", 1},
        {"MyUint.Value", 1},
        {"MyBool.Value", true},
        {"MyFloat.Value", .1f},

        {"MyMat2.Value", new Transform2D()},
        {"MyMat3.Value", new Basis()},
        {"MyMat4.Value", new Transform3D()},

        {"MyVec2.Value", Vector2.One},
        {"MyVec3.Value", Vector3.One},
        {"MyVec4.Value", Vector4.One},

        {"MyIvec2.Value", Vector2I.One},
        {"MyIvec3.Value", Vector3I.One},
        {"MyIvec4.Value", Vector4I.One},

        {"MyUvec2.Value", Vector2I.One},
        {"MyUvec3.Value", Vector3I.One},
        {"MyUvec4.Value", Vector4I.One},

        {"MyBvec2.Value", 1},
        {"MyBvec3.Value", 1},
        {"MyBvec4.Value", 1},

        {"MyCol3.Value", Colors.Red},
        {"MyCol4.Value", Colors.Red},

        {"MyIntAsRange.Value", 1},
        {"MyFloatAsRange.Value", .1f},

        {"MyEnumAsInt1.Value", 1},
        {"MyEnumAsInt2.Value", 1},
        {"MyEnumAsEnum.Value", MyShaderEnum.B},

        {"MySampler2D.Value", GD.Load<Texture2D>("res://Assets/icon.svg")},
        {"MyIsampler2D.Value", GD.Load<Texture2D>("res://Assets/icon.svg")},
        {"MyUsampler2D.Value", GD.Load<Texture2D>("res://Assets/icon.svg")},

        {"MySampler3D.Value", new ImageTexture3D()},
        {"MyIsampler3D.Value", new ImageTexture3D()},
        {"MyUsampler3D.Value", new ImageTexture3D()},

        {"MySampler2DArray.Value", new Texture2DArray()},
        {"MyIsampler2DArray.Value", new Texture2DArray()},
        {"MyUsampler2DArray.Value", new Texture2DArray()},

        {"MySamplerCube.Value", new Cubemap()},
        {"MySamplerCubeArray.Value", new CubemapArray()},
        {"MySamplerExternalOES.Value", new ExternalTexture()},
    };

    private readonly Dictionary<string, object> EmptyShaderExpected = new() {
        {"UniformCount", 0},
    };

    private readonly Dictionary<string, object> MyShaderWithDefaultsExpected = new() {
        {"UniformCount", 41},

        // DEFAULTS //

        {"MyInt.Default", 1},
        {"MyUint.Default", 1},
        {"MyBool.Default", true},
        {"MyFloat.Default", .1f},

        // Not parsing matrices yet - default will always be Identity
        {"MyMat2.Default", Transform2D.Identity},
        {"MyMat3.Default", Basis.Identity},
        {"MyMat4.Default", Transform3D.Identity},

        {"MyVec2.Default", Vector2.One},
        {"MyVec3.Default", Vector3.One},
        {"MyVec4.Default", Vector4.One},

        {"MyIvec2.Default", Vector2I.One},
        {"MyIvec3.Default", Vector3I.One},
        {"MyIvec4.Default", Vector4I.One},

        {"MyUvec2.Default", Vector2I.One},
        {"MyUvec3.Default", Vector3I.One},
        {"MyUvec4.Default", Vector4I.One},

        {"MyBvec2.Default", 0b11},
        {"MyBvec3.Default", 0b111},
        {"MyBvec4.Default", 0b1111},

        {"MyCol3.Default", new Color(1, 1, 1, 1)},
        {"MyCol4.Default", new Color(1, 1, 1, 1)},

        {"MyIntAsRange.Default", 1},
        {"MyFloatAsRange.Default", .1f},

        {"MyEnumAsInt1.Default", 1},
        {"MyEnumAsInt60.Default", 60},
        {"MyEnumAsEnum1.Default", (MyShaderEnum)1},
        {"MyEnumAsEnum60.Default", MyShaderEnum.B},

        {"MyVec2WithScalarDefault.Default", Vector2.One},
        {"MyVec3WithScalarDefault.Default", Vector3.One},
        {"MyVec4WithScalarDefault.Default", Vector4.One},

        {"MyIvec2WithScalarDefault.Default", Vector2I.One},
        {"MyIvec3WithScalarDefault.Default", Vector3I.One},
        {"MyIvec4WithScalarDefault.Default", Vector4I.One},

        {"MyUvec2WithScalarDefault.Default", Vector2I.One},
        {"MyUvec3WithScalarDefault.Default", Vector3I.One},
        {"MyUvec4WithScalarDefault.Default", Vector4I.One},

        {"MyBvec2WithScalarDefault.Default", 0b11},
        {"MyBvec3WithScalarDefault.Default", 0b111},
        {"MyBvec4WithScalarDefault.Default", 0b1111},

        {"MyCol3WithScalarDefault.Default", new Color(1, 1, 1, 1)},
        {"MyCol4WithScalarDefault.Default", new Color(1, 1, 1, 1)},

        // VALUES //

        {"MyInt.Value", 0},
        {"MyUint.Value", 0},
        {"MyBool.Value", false},
        {"MyFloat.Value", .0f},

        {"MyMat2.Value", new Transform2D()},
        {"MyMat3.Value", new Basis()},
        {"MyMat4.Value", new Transform3D()},

        {"MyVec2.Value", Vector2.Zero},
        {"MyVec3.Value", Vector3.Zero},
        {"MyVec4.Value", Vector4.Zero},

        {"MyIvec2.Value", Vector2I.Zero},
        {"MyIvec3.Value", Vector3I.Zero},
        {"MyIvec4.Value", Vector4I.Zero},

        {"MyUvec2.Value", Vector2I.Zero},
        {"MyUvec3.Value", Vector3I.Zero},
        {"MyUvec4.Value", Vector4I.Zero},

        {"MyBvec2.Value", 0},
        {"MyBvec3.Value", 0},
        {"MyBvec4.Value", 0},

        {"MyCol3.Value", new Color()},
        {"MyCol4.Value", new Color()},

        {"MyIntAsRange.Value", 0},
        {"MyFloatAsRange.Value", .0f},

        {"MyEnumAsInt1.Value", 0},
        {"MyEnumAsInt60.Value", 0},
        {"MyEnumAsEnum1.Value", 0},
        {"MyEnumAsEnum60.Value", 0},

        {"MyVec2WithScalarDefault.Value", Vector2.Zero},
        {"MyVec3WithScalarDefault.Value", Vector3.Zero},
        {"MyVec4WithScalarDefault.Value", Vector4.Zero},

        {"MyIvec2WithScalarDefault.Value", Vector2I.Zero},
        {"MyIvec3WithScalarDefault.Value", Vector3I.Zero},
        {"MyIvec4WithScalarDefault.Value", Vector4I.Zero},

        {"MyUvec2WithScalarDefault.Value", Vector2I.Zero},
        {"MyUvec3WithScalarDefault.Value", Vector3I.Zero},
        {"MyUvec4WithScalarDefault.Value", Vector4I.Zero},

        {"MyBvec2WithScalarDefault.Value", 0},
        {"MyBvec3WithScalarDefault.Value", 0},
        {"MyBvec4WithScalarDefault.Value", 0},

        {"MyCol3WithScalarDefault.Value", new Color()},
        {"MyCol4WithScalarDefault.Value", new Color()},
    };

    void ITest.ReadyTests()
    {
        RunGeneratedTests();
        SuppressGeneratedTests();
        RunImplicitOperatorTests();
        RunGeneratedTestsFromScene();

        void RunGeneratedTests()
        {
            MyShader.RunTests(MyShaderExpected);
            MyShaderAsStatic.RunTests(MyShaderExpected);
            MyShaderAsResource.RunTests(MyShaderExpected);
            MyShaderAsShaderMaterial.RunTests(MyShaderExpected);

            EmptyShader.RunTests(EmptyShaderExpected);
            EmptyShaderAsStatic.RunTests(EmptyShaderExpected);
            EmptyShaderAsResource.RunTests(EmptyShaderExpected);
            EmptyShaderAsShaderMaterial.RunTests(EmptyShaderExpected);

            MyShaderWithDefaults.RunTests(MyShaderWithDefaultsExpected);
            MyShaderWithDefaultsAsStatic.RunTests(MyShaderWithDefaultsExpected);
            MyShaderWithDefaultsAsResource.RunTests(MyShaderWithDefaultsExpected);
            MyShaderWithDefaultsAsShaderMaterial.RunTests(MyShaderWithDefaultsExpected);

            typeof(MyShader).GetMethod("RunTests").Should().NotBeNull();
            typeof(MyShaderAsStatic).GetMethod("RunTests").Should().NotBeNull();
            typeof(MyShaderAsResource).GetMethod("RunTests").Should().NotBeNull();
            typeof(MyShaderAsShaderMaterial).GetMethod("RunTests").Should().NotBeNull();

            typeof(EmptyShader).GetMethod("RunTests").Should().NotBeNull();
            typeof(EmptyShaderAsStatic).GetMethod("RunTests").Should().NotBeNull();
            typeof(EmptyShaderAsResource).GetMethod("RunTests").Should().NotBeNull();
            typeof(EmptyShaderAsShaderMaterial).GetMethod("RunTests").Should().NotBeNull();

            typeof(MyShaderWithDefaults).GetMethod("RunTests").Should().NotBeNull();
            typeof(MyShaderWithDefaultsAsStatic).GetMethod("RunTests").Should().NotBeNull();
            typeof(MyShaderWithDefaultsAsResource).GetMethod("RunTests").Should().NotBeNull();
            typeof(MyShaderWithDefaultsAsShaderMaterial).GetMethod("RunTests").Should().NotBeNull();
        }

        void SuppressGeneratedTests()
        {
            //MyShader_NO_TEST.RunTests(MyShaderExpected);
            //MyShaderAsStatic_NO_TEST.RunTests(MyShaderExpected);
            //MyShaderAsResource_NO_TEST.RunTests(MyShaderExpected);
            //MyShaderAsShaderMaterial_NO_TEST.RunTests(MyShaderExpected);

            //EmptyShader_NO_TEST.RunTests(EmptyShaderExpected);
            //EmptyShaderAsStatic_NO_TEST.RunTests(EmptyShaderExpected);
            //EmptyShaderAsResource_NO_TEST.RunTests(EmptyShaderExpected);
            //EmptyShaderAsShaderMaterial_NO_TEST.RunTests(EmptyShaderExpected);

            //MyShaderWithDefaults_NO_TEST.RunTests(MyShaderWithDefaultsExpected);
            //MyShaderWithDefaultsAsStatic_NO_TEST.RunTests(MyShaderWithDefaultsExpected);
            //MyShaderWithDefaultsAsResource_NO_TEST.RunTests(MyShaderWithDefaultsExpected);
            //MyShaderWithDefaultsAsShaderMaterial_NO_TEST.RunTests(MyShaderWithDefaultsExpected);

            typeof(MyShader_NO_TEST).GetMethod("RunTests").Should().BeNull();
            typeof(MyShaderAsStatic_NO_TEST).GetMethod("RunTests").Should().BeNull();
            typeof(MyShaderAsResource_NO_TEST).GetMethod("RunTests").Should().BeNull();
            typeof(MyShaderAsShaderMaterial_NO_TEST).GetMethod("RunTests").Should().BeNull();

            typeof(EmptyShader_NO_TEST).GetMethod("RunTests").Should().BeNull();
            typeof(EmptyShaderAsStatic_NO_TEST).GetMethod("RunTests").Should().BeNull();
            typeof(EmptyShaderAsResource_NO_TEST).GetMethod("RunTests").Should().BeNull();
            typeof(EmptyShaderAsShaderMaterial_NO_TEST).GetMethod("RunTests").Should().BeNull();

            typeof(MyShaderWithDefaults_NO_TEST).GetMethod("RunTests").Should().BeNull();
            typeof(MyShaderWithDefaultsAsStatic_NO_TEST).GetMethod("RunTests").Should().BeNull();
            typeof(MyShaderWithDefaultsAsResource_NO_TEST).GetMethod("RunTests").Should().BeNull();
            typeof(MyShaderWithDefaultsAsShaderMaterial_NO_TEST).GetMethod("RunTests").Should().BeNull();
        }

        void RunImplicitOperatorTests()
        {
            {
                var material = (ShaderMaterial)MeshWithMyShader.MaterialOverride;
                TestImplicitShader(material, out var shader);
                TestImplicitMaterial(shader);

                void TestImplicitShader(MyShader sut, out MyShader @out)
                    => (@out = sut).Material.Should().Be(material);

                void TestImplicitMaterial(ShaderMaterial sut)
                    => sut.Should().Be(material);
            }

            {
                var material = (ShaderMaterial)MeshWithEmptyShader.MaterialOverride;
                TestImplicitShader(material, out var shader);
                TestImplicitMaterial(shader);

                void TestImplicitShader(EmptyShader sut, out EmptyShader @out)
                    => (@out = sut).Material.Should().Be(material);

                void TestImplicitMaterial(ShaderMaterial sut)
                    => sut.Should().Be(material);
            }

            {
                var material = (ShaderMaterial)MeshWithMyShaderWithDefaults.MaterialOverride;
                TestImplicitShader(material, out var shader);
                TestImplicitMaterial(shader);

                void TestImplicitShader(MyShaderWithDefaults sut, out MyShaderWithDefaults @out)
                    => (@out = sut).Material.Should().Be(material);

                void TestImplicitMaterial(ShaderMaterial sut)
                    => sut.Should().Be(material);
            }
        }

        void RunGeneratedTestsFromScene()
        {
            {
                var sutMyShaderShaderMaterial = (ShaderMaterial)MeshWithMyShader.MaterialOverride;
                MyShader.RunTests(MyShaderExpected, sutMyShaderShaderMaterial);
                MyShaderAsStatic.RunTests(MyShaderExpected, sutMyShaderShaderMaterial);
                MyShaderAsResource.RunTests(MyShaderExpected, sutMyShaderShaderMaterial);
                //MyShaderAsShaderMaterial.RunTests(MyShaderExpected, sutMyShaderShaderMaterial);

                var sutMyShaderAsShaderMaterial = (MyShaderAsShaderMaterial)MeshWithMyShaderAsShaderMaterial.MaterialOverride;
                MyShaderAsShaderMaterial.RunTests(MyShaderExpected, sutMyShaderAsShaderMaterial);
            }

            {
                var sutEmptyShaderShaderMaterial = (ShaderMaterial)MeshWithEmptyShader.MaterialOverride;
                EmptyShader.RunTests(EmptyShaderExpected, sutEmptyShaderShaderMaterial);
                EmptyShaderAsStatic.RunTests(EmptyShaderExpected, sutEmptyShaderShaderMaterial);
                EmptyShaderAsResource.RunTests(EmptyShaderExpected, sutEmptyShaderShaderMaterial);
                //EmptyShaderAsShaderMaterial.RunTests(EmptyShaderExpected, sutEmptyShaderShaderMaterial);

                var sutEmptyShaderAsShaderMaterial = (EmptyShaderAsShaderMaterial)MeshWithEmptyShaderAsShaderMaterial.MaterialOverride;
                EmptyShaderAsShaderMaterial.RunTests(EmptyShaderExpected, sutEmptyShaderAsShaderMaterial);
            }

            {
                var sutMyShaderWithDefaultsShaderMaterial = (ShaderMaterial)MeshWithMyShaderWithDefaults.MaterialOverride;
                MyShaderWithDefaults.RunTests(MyShaderWithDefaultsExpected, sutMyShaderWithDefaultsShaderMaterial);
                MyShaderWithDefaultsAsStatic.RunTests(MyShaderWithDefaultsExpected, sutMyShaderWithDefaultsShaderMaterial);
                MyShaderWithDefaultsAsResource.RunTests(MyShaderWithDefaultsExpected, sutMyShaderWithDefaultsShaderMaterial);
                //MyShaderWithDefaultsAsShaderMaterial.RunTests(MyShaderWithDefaultsExpected, sutMyShaderWithDefaultsShaderMaterial);

                var sutMyShaderWithDefaultsAsShaderMaterial = (MyShaderWithDefaultsAsShaderMaterial)MeshWithMyShaderWithDefaultsAsShaderMaterial.MaterialOverride;
                MyShaderWithDefaultsAsShaderMaterial.RunTests(MyShaderWithDefaultsExpected, sutMyShaderWithDefaultsAsShaderMaterial);
            }
        }
    }
}
