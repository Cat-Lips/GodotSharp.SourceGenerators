using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Godot;
using GodotSharp.BuildingBlocks.TestRunner;

namespace GodotTests.TestScenes;

[SceneTree]
public partial class ShaderNamesAttributeTests : Node, ITest
{
    private const BindingFlags PublicStatic = BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly;
    private static readonly string[] NestedGodotTypes = ["MethodName", "PropertyName", "SignalName"];

    public enum MyShaderEnum
    {
        A = 30,
        B = 60,
        C = 200,
    }

    void ITest.ReadyTests()
    {
        RunMyShaderTest(RAW(MyShader.ResPath));
        RunEmptyShaderTest(RAW(EmptyShader.ResPath));
        RunMyShaderAsResourceTest(RES(typeof(MyShaderAsResource)));
        RunMyShaderWithDefaultsTest(RAW(MyShaderWithDefaults.ResPath));
        RunMyShaderWithDefaultsAsResourceTest(RES(typeof(MyShaderWithDefaultsAsResource)));

        static void RunMyShaderTest(ShaderMaterial mat)
        {
            var x = TEST_NAME(nameof(RunEmptyShaderTest));
            mat.Shader.ResourcePath.Should().Be(MyShader.ResPath, x);
            NestedTypes(typeof(MyShader)).Should().BeEquivalentTo(["Params", "Default"], x);
            DeclaredMethods(typeof(MyShader)).Count().Should().Be(MyShader.Params.All.Length * 3 + 1, x); // Get/Set/Reset + Reset
            DeclaredFields(typeof(MyShader.Params)).Count().Should().Be(MyShader.Params.All.Length + 1, x); // +1 => All
            DeclaredFields(typeof(MyShader.Default)).Count().Should().Be(MyShader.Params.All.Length, x);

            TestParams();
            TestDefaults();
            TestDefaultGet();
            TestSet();
            TestReset();

            void TestParams()
            {
                var x = TEST_NAME(nameof(TestParams));

                MyShader.Params.MyInt.Should().Be((StringName)"my_int", x);
                MyShader.Params.MyUint.Should().Be((StringName)"my_uint", x);
                MyShader.Params.MyBool.Should().Be((StringName)"my_bool", x);
                MyShader.Params.MyFloat.Should().Be((StringName)"my_float", x);

                MyShader.Params.MyMat2.Should().Be((StringName)"my_mat2", x);
                MyShader.Params.MyMat3.Should().Be((StringName)"my_mat3", x);
                MyShader.Params.MyMat4.Should().Be((StringName)"my_mat4", x);

                MyShader.Params.MyVec2.Should().Be((StringName)"my_vec2", x);
                MyShader.Params.MyVec3.Should().Be((StringName)"my_vec3", x);
                MyShader.Params.MyVec4.Should().Be((StringName)"my_vec4", x);

                MyShader.Params.MyIvec2.Should().Be((StringName)"my_ivec2", x);
                MyShader.Params.MyIvec3.Should().Be((StringName)"my_ivec3", x);
                MyShader.Params.MyIvec4.Should().Be((StringName)"my_ivec4", x);

                MyShader.Params.MyUvec2.Should().Be((StringName)"my_uvec2", x);
                MyShader.Params.MyUvec3.Should().Be((StringName)"my_uvec3", x);
                MyShader.Params.MyUvec4.Should().Be((StringName)"my_uvec4", x);

                MyShader.Params.MyBvec2.Should().Be((StringName)"my_bvec2", x);
                MyShader.Params.MyBvec3.Should().Be((StringName)"my_bvec3", x);
                MyShader.Params.MyBvec4.Should().Be((StringName)"my_bvec4", x);

                MyShader.Params.MyCol3.Should().Be((StringName)"my_col3", x);
                MyShader.Params.MyCol4.Should().Be((StringName)"my_col4", x);

                MyShader.Params.MyIntAsRange.Should().Be((StringName)"my_int_as_range", x);
                MyShader.Params.MyFloatAsRange.Should().Be((StringName)"my_float_as_range", x);

                MyShader.Params.MyEnumAsInt1.Should().Be((StringName)"my_enum_as_int1", x);
                MyShader.Params.MyEnumAsInt2.Should().Be((StringName)"my_enum_as_int2", x);
                MyShader.Params.MyEnumAsEnum.Should().Be((StringName)"my_enum_as_enum", x);

                MyShader.Params.MySampler2D.Should().Be((StringName)"my_sampler2D", x);
                MyShader.Params.MyIsampler2D.Should().Be((StringName)"my_isampler2D", x);
                MyShader.Params.MyUsampler2D.Should().Be((StringName)"my_usampler2D", x);

                MyShader.Params.MySampler3D.Should().Be((StringName)"my_sampler3D", x);
                MyShader.Params.MyIsampler3D.Should().Be((StringName)"my_isampler3D", x);
                MyShader.Params.MyUsampler3D.Should().Be((StringName)"my_usampler3D", x);

                MyShader.Params.MySampler2DArray.Should().Be((StringName)"my_sampler2DArray", x);
                MyShader.Params.MyIsampler2DArray.Should().Be((StringName)"my_isampler2DArray", x);
                MyShader.Params.MyUsampler2DArray.Should().Be((StringName)"my_usampler2DArray", x);

                MyShader.Params.MySamplerCube.Should().Be((StringName)"my_samplerCube", x);
                MyShader.Params.MySamplerCubeArray.Should().Be((StringName)"my_samplerCubeArray", x);
                MyShader.Params.MySamplerExternalOES.Should().Be((StringName)"my_samplerExternalOES", x);

                MyShader.Params.All.Should().BeEquivalentTo(
                [
                    MyShader.Params.MyInt,
                    MyShader.Params.MyUint,
                    MyShader.Params.MyBool,
                    MyShader.Params.MyFloat,

                    MyShader.Params.MyMat2,
                    MyShader.Params.MyMat3,
                    MyShader.Params.MyMat4,

                    MyShader.Params.MyVec2,
                    MyShader.Params.MyVec3,
                    MyShader.Params.MyVec4,

                    MyShader.Params.MyIvec2,
                    MyShader.Params.MyIvec3,
                    MyShader.Params.MyIvec4,

                    MyShader.Params.MyUvec2,
                    MyShader.Params.MyUvec3,
                    MyShader.Params.MyUvec4,

                    MyShader.Params.MyBvec2,
                    MyShader.Params.MyBvec3,
                    MyShader.Params.MyBvec4,

                    MyShader.Params.MyCol3,
                    MyShader.Params.MyCol4,

                    MyShader.Params.MyIntAsRange,
                    MyShader.Params.MyFloatAsRange,

                    MyShader.Params.MyEnumAsInt1,
                    MyShader.Params.MyEnumAsInt2,
                    MyShader.Params.MyEnumAsEnum,

                    MyShader.Params.MySampler2D,
                    MyShader.Params.MyIsampler2D,
                    MyShader.Params.MyUsampler2D,

                    MyShader.Params.MySampler3D,
                    MyShader.Params.MyIsampler3D,
                    MyShader.Params.MyUsampler3D,

                    MyShader.Params.MySampler2DArray,
                    MyShader.Params.MyIsampler2DArray,
                    MyShader.Params.MyUsampler2DArray,

                    MyShader.Params.MySamplerCube,
                    MyShader.Params.MySamplerCubeArray,
                    MyShader.Params.MySamplerExternalOES,
                ], x);
            }

            void TestDefaults()
            {
                var x = TEST_NAME(nameof(TestDefaults));

                MyShader.Default.MyInt.Should().Be(0, x);
                MyShader.Default.MyUint.Should().Be(0, x);
                MyShader.Default.MyBool.Should().Be(false, x);
                MyShader.Default.MyFloat.Should().Be(.0f, x);

                // Shader will set matrices to Identity if no default provided
                MyShader.Default.MyMat2.Should().Be(Transform2D.Identity, x);
                MyShader.Default.MyMat3.Should().Be(Basis.Identity, x);
                MyShader.Default.MyMat4.Should().Be(Transform3D.Identity, x);

                MyShader.Default.MyVec2.Should().Be(Vector2.Zero, x);
                MyShader.Default.MyVec3.Should().Be(Vector3.Zero, x);
                MyShader.Default.MyVec4.Should().Be(Vector4.Zero, x);

                MyShader.Default.MyIvec2.Should().Be(Vector2I.Zero, x);
                MyShader.Default.MyIvec3.Should().Be(Vector3I.Zero, x);
                MyShader.Default.MyIvec4.Should().Be(Vector4I.Zero, x);

                MyShader.Default.MyUvec2.Should().Be(Vector2I.Zero, x);
                MyShader.Default.MyUvec3.Should().Be(Vector3I.Zero, x);
                MyShader.Default.MyUvec4.Should().Be(Vector4I.Zero, x);

                MyShader.Default.MyBvec2.Should().Be(0, x);
                MyShader.Default.MyBvec3.Should().Be(0, x);
                MyShader.Default.MyBvec4.Should().Be(0, x);

                // Shader will set colors to 0,0,0,1 if no default provided
                MyShader.Default.MyCol3.Should().Be(new Color(0, 0, 0, 1), x);
                MyShader.Default.MyCol4.Should().Be(new Color(0, 0, 0, 1), x);

                MyShader.Default.MyIntAsRange.Should().Be(0, x);
                MyShader.Default.MyFloatAsRange.Should().Be(.0f, x);

                MyShader.Default.MyEnumAsInt1.Should().Be(0, x);
                MyShader.Default.MyEnumAsInt2.Should().Be(0, x);
                ((object)MyShader.Default.MyEnumAsEnum).Should().Be((MyShaderEnum)0, x);

                MyShader.Default.MySampler2D.Should().Be(null, x);
                MyShader.Default.MyIsampler2D.Should().Be(null, x);
                MyShader.Default.MyUsampler2D.Should().Be(null, x);

                MyShader.Default.MySampler3D.Should().Be(null, x);
                MyShader.Default.MyIsampler3D.Should().Be(null, x);
                MyShader.Default.MyUsampler3D.Should().Be(null, x);

                MyShader.Default.MySampler2DArray.Should().Be(null, x);
                MyShader.Default.MyIsampler2DArray.Should().Be(null, x);
                MyShader.Default.MyUsampler2DArray.Should().Be(null, x);

                MyShader.Default.MySamplerCube.Should().Be(null, x);
                MyShader.Default.MySamplerCubeArray.Should().Be(null, x);
                MyShader.Default.MySamplerExternalOES.Should().Be(null, x);
            }

            void TestDefaultGet(string x = null)
            {
                x ??= TEST_NAME(nameof(TestDefaultGet));

                MyShader.GetMyInt(mat).Should().Be(MyShader.Default.MyInt, x);
                MyShader.GetMyUint(mat).Should().Be(MyShader.Default.MyUint, x);
                MyShader.GetMyBool(mat).Should().Be(MyShader.Default.MyBool, x);
                MyShader.GetMyFloat(mat).Should().Be(MyShader.Default.MyFloat, x);

                MyShader.GetMyMat2(mat).Should().Be(MyShader.Default.MyMat2, x);
                MyShader.GetMyMat3(mat).Should().Be(MyShader.Default.MyMat3, x);
                MyShader.GetMyMat4(mat).Should().Be(MyShader.Default.MyMat4, x);

                MyShader.GetMyVec2(mat).Should().Be(MyShader.Default.MyVec2, x);
                MyShader.GetMyVec3(mat).Should().Be(MyShader.Default.MyVec3, x);
                MyShader.GetMyVec4(mat).Should().Be(MyShader.Default.MyVec4, x);

                MyShader.GetMyIvec2(mat).Should().Be(MyShader.Default.MyIvec2, x);
                MyShader.GetMyIvec3(mat).Should().Be(MyShader.Default.MyIvec3, x);
                MyShader.GetMyIvec4(mat).Should().Be(MyShader.Default.MyIvec4, x);

                MyShader.GetMyUvec2(mat).Should().Be(MyShader.Default.MyUvec2, x);
                MyShader.GetMyUvec3(mat).Should().Be(MyShader.Default.MyUvec3, x);
                MyShader.GetMyUvec4(mat).Should().Be(MyShader.Default.MyUvec4, x);

                MyShader.GetMyBvec2(mat).Should().Be(MyShader.Default.MyBvec2, x);
                MyShader.GetMyBvec3(mat).Should().Be(MyShader.Default.MyBvec3, x);
                MyShader.GetMyBvec4(mat).Should().Be(MyShader.Default.MyBvec4, x);

                MyShader.GetMyCol3(mat).Should().Be(MyShader.Default.MyCol3, x);
                MyShader.GetMyCol4(mat).Should().Be(MyShader.Default.MyCol4, x);

                MyShader.GetMyIntAsRange(mat).Should().Be(MyShader.Default.MyIntAsRange, x);
                MyShader.GetMyFloatAsRange(mat).Should().Be(MyShader.Default.MyFloatAsRange, x);

                MyShader.GetMyEnumAsInt1(mat).Should().Be(MyShader.Default.MyEnumAsInt1, x);
                MyShader.GetMyEnumAsInt2(mat).Should().Be(MyShader.Default.MyEnumAsInt2, x);
                MyShader.GetMyEnumAsEnum(mat).Should().Be(MyShader.Default.MyEnumAsEnum, x);

                MyShader.GetMySampler2D(mat).Should().Be(MyShader.Default.MySampler2D, x);
                MyShader.GetMyIsampler2D(mat).Should().Be(MyShader.Default.MyIsampler2D, x);
                MyShader.GetMyUsampler2D(mat).Should().Be(MyShader.Default.MyUsampler2D, x);

                MyShader.GetMySampler3D(mat).Should().Be(MyShader.Default.MySampler3D, x);
                MyShader.GetMyIsampler3D(mat).Should().Be(MyShader.Default.MyIsampler3D, x);
                MyShader.GetMyUsampler3D(mat).Should().Be(MyShader.Default.MyUsampler3D, x);

                MyShader.GetMySampler2DArray(mat).Should().Be(MyShader.Default.MySampler2DArray, x);
                MyShader.GetMyIsampler2DArray(mat).Should().Be(MyShader.Default.MyIsampler2DArray, x);
                MyShader.GetMyUsampler2DArray(mat).Should().Be(MyShader.Default.MyUsampler2DArray, x);

                MyShader.GetMySamplerCube(mat).Should().Be(MyShader.Default.MySamplerCube, x);
                MyShader.GetMySamplerCubeArray(mat).Should().Be(MyShader.Default.MySamplerCubeArray, x);
                MyShader.GetMySamplerExternalOES(mat).Should().Be(MyShader.Default.MySamplerExternalOES, x);
            }

            void TestSet()
            {
                var x = TEST_NAME(nameof(TestSet));

                MyShader.SetMyInt(mat, 1); MyShader.GetMyInt(mat).Should().Be(1, x);
                MyShader.SetMyUint(mat, 1); MyShader.GetMyUint(mat).Should().Be(1, x);
                MyShader.SetMyBool(mat, true); MyShader.GetMyBool(mat).Should().Be(true, x);
                MyShader.SetMyFloat(mat, .1f); MyShader.GetMyFloat(mat).Should().Be(.1f, x);

                MyShader.SetMyMat2(mat, new Transform2D()); MyShader.GetMyMat2(mat).Should().Be(new Transform2D(), x);
                MyShader.SetMyMat3(mat, new Basis()); MyShader.GetMyMat3(mat).Should().Be(new Basis(), x);
                MyShader.SetMyMat4(mat, new Transform3D()); MyShader.GetMyMat4(mat).Should().Be(new Transform3D(), x);

                MyShader.SetMyVec2(mat, Vector2.One); MyShader.GetMyVec2(mat).Should().Be(Vector2.One, x);
                MyShader.SetMyVec3(mat, Vector3.One); MyShader.GetMyVec3(mat).Should().Be(Vector3.One, x);
                MyShader.SetMyVec4(mat, Vector4.One); MyShader.GetMyVec4(mat).Should().Be(Vector4.One, x);

                MyShader.SetMyIvec2(mat, Vector2I.One); MyShader.GetMyIvec2(mat).Should().Be(Vector2I.One, x);
                MyShader.SetMyIvec3(mat, Vector3I.One); MyShader.GetMyIvec3(mat).Should().Be(Vector3I.One, x);
                MyShader.SetMyIvec4(mat, Vector4I.One); MyShader.GetMyIvec4(mat).Should().Be(Vector4I.One, x);

                MyShader.SetMyUvec2(mat, Vector2I.One); MyShader.GetMyUvec2(mat).Should().Be(Vector2I.One, x);
                MyShader.SetMyUvec3(mat, Vector3I.One); MyShader.GetMyUvec3(mat).Should().Be(Vector3I.One, x);
                MyShader.SetMyUvec4(mat, Vector4I.One); MyShader.GetMyUvec4(mat).Should().Be(Vector4I.One, x);

                MyShader.SetMyBvec2(mat, 1); MyShader.GetMyBvec2(mat).Should().Be(1, x);
                MyShader.SetMyBvec3(mat, 1); MyShader.GetMyBvec3(mat).Should().Be(1, x);
                MyShader.SetMyBvec4(mat, 1); MyShader.GetMyBvec4(mat).Should().Be(1, x);

                MyShader.SetMyCol3(mat, Colors.Red); MyShader.GetMyCol3(mat).Should().Be(Colors.Red, x);
                MyShader.SetMyCol4(mat, Colors.Red); MyShader.GetMyCol4(mat).Should().Be(Colors.Red, x);

                MyShader.SetMyIntAsRange(mat, 1); MyShader.GetMyIntAsRange(mat).Should().Be(1, x);
                MyShader.SetMyFloatAsRange(mat, .1f); MyShader.GetMyFloatAsRange(mat).Should().Be(.1f, x);

                MyShader.SetMyEnumAsInt1(mat, 1); MyShader.GetMyEnumAsInt1(mat).Should().Be(1, x);
                MyShader.SetMyEnumAsInt2(mat, 1); MyShader.GetMyEnumAsInt2(mat).Should().Be(1, x);
                MyShader.SetMyEnumAsEnum(mat, MyShaderEnum.B); MyShader.GetMyEnumAsEnum(mat).Should().Be(MyShaderEnum.B, x);

                var testTex2D = GD.Load<Texture2D>("res://Assets/icon.svg");
                var testTex3D = new ImageTexture3D();
                var testTex2DArray = new Texture2DArray();
                var cubemap = new Cubemap();
                var cubemapArray = new CubemapArray();
                var externalTexture = new ExternalTexture();

                MyShader.SetMySampler2D(mat, testTex2D); MyShader.GetMySampler2D(mat).Should().Be(testTex2D, x);
                MyShader.SetMyIsampler2D(mat, testTex2D); MyShader.GetMyIsampler2D(mat).Should().Be(testTex2D, x);
                MyShader.SetMyUsampler2D(mat, testTex2D); MyShader.GetMyUsampler2D(mat).Should().Be(testTex2D, x);

                MyShader.SetMySampler3D(mat, testTex3D); MyShader.GetMySampler3D(mat).Should().Be(testTex3D, x);
                MyShader.SetMyIsampler3D(mat, testTex3D); MyShader.GetMyIsampler3D(mat).Should().Be(testTex3D, x);
                MyShader.SetMyUsampler3D(mat, testTex3D); MyShader.GetMyUsampler3D(mat).Should().Be(testTex3D, x);

                MyShader.SetMySampler2DArray(mat, testTex2DArray); MyShader.GetMySampler2DArray(mat).Should().Be(testTex2DArray, x);
                MyShader.SetMyIsampler2DArray(mat, testTex2DArray); MyShader.GetMyIsampler2DArray(mat).Should().Be(testTex2DArray, x);
                MyShader.SetMyUsampler2DArray(mat, testTex2DArray); MyShader.GetMyUsampler2DArray(mat).Should().Be(testTex2DArray, x);

                MyShader.SetMySamplerCube(mat, cubemap); MyShader.GetMySamplerCube(mat).Should().Be(cubemap, x);
                MyShader.SetMySamplerCubeArray(mat, cubemapArray); MyShader.GetMySamplerCubeArray(mat).Should().Be(cubemapArray, x);
                MyShader.SetMySamplerExternalOES(mat, externalTexture); MyShader.GetMySamplerExternalOES(mat).Should().Be(externalTexture, x);
            }

            void TestReset()
            {
                var x = TEST_NAME(nameof(TestReset));

                MyShader.ResetMyInt(mat);
                MyShader.ResetMyUint(mat);
                MyShader.ResetMyBool(mat);
                MyShader.ResetMyFloat(mat);

                MyShader.ResetMyMat2(mat);
                MyShader.ResetMyMat3(mat);
                MyShader.ResetMyMat4(mat);

                MyShader.ResetMyVec2(mat);
                MyShader.ResetMyVec3(mat);
                MyShader.ResetMyVec4(mat);

                MyShader.ResetMyIvec2(mat);
                MyShader.ResetMyIvec3(mat);
                MyShader.ResetMyIvec4(mat);

                MyShader.ResetMyUvec2(mat);
                MyShader.ResetMyUvec3(mat);
                MyShader.ResetMyUvec4(mat);

                MyShader.ResetMyBvec2(mat);
                MyShader.ResetMyBvec3(mat);
                MyShader.ResetMyBvec4(mat);

                MyShader.ResetMyCol3(mat);
                MyShader.ResetMyCol4(mat);

                MyShader.ResetMyIntAsRange(mat);
                MyShader.ResetMyFloatAsRange(mat);

                MyShader.ResetMyEnumAsInt1(mat);
                MyShader.ResetMyEnumAsInt2(mat);
                MyShader.ResetMyEnumAsEnum(mat);

                MyShader.ResetMySampler2D(mat);
                MyShader.ResetMyIsampler2D(mat);
                MyShader.ResetMyUsampler2D(mat);

                MyShader.ResetMySampler3D(mat);
                MyShader.ResetMyIsampler3D(mat);
                MyShader.ResetMyUsampler3D(mat);

                MyShader.ResetMySampler2DArray(mat);
                MyShader.ResetMyIsampler2DArray(mat);
                MyShader.ResetMyUsampler2DArray(mat);

                MyShader.ResetMySamplerCube(mat);
                MyShader.ResetMySamplerCubeArray(mat);
                MyShader.ResetMySamplerExternalOES(mat);

                TestDefaultGet(x);
            }
        }

        static void RunEmptyShaderTest(ShaderMaterial mat)
        {
            var x = TEST_NAME(nameof(RunEmptyShaderTest));
            mat.Shader.ResourcePath.Should().Be(EmptyShader.ResPath, x);
            NestedTypes(typeof(EmptyShader)).Should().BeEmpty(x);
            DeclaredFields(typeof(EmptyShader)).Should().BeEmpty(x);
            DeclaredMethods(typeof(EmptyShader)).Should().BeEmpty(x);
        }

        static void RunMyShaderAsResourceTest(ShaderMaterial mat)
        {
            var x = TEST_NAME(nameof(RunMyShaderWithDefaultsTest));
            mat.Shader.ResourcePath.Should().Be(MyShaderAsResource.ResPath, x);
            NestedTypes(typeof(MyShaderAsResource)).Should().BeEquivalentTo(NestedTypes(typeof(MyShader)), x);
            DeclaredMethods(typeof(MyShaderAsResource)).Should().BeEquivalentTo(DeclaredMethods(typeof(MyShader)), x);
            DeclaredFields(typeof(MyShaderAsResource.Params)).Should().BeEquivalentTo(DeclaredFields(typeof(MyShader.Params)), x);
            DeclaredFields(typeof(MyShaderAsResource.Default)).Should().BeEquivalentTo(DeclaredFields(typeof(MyShader.Default)), x);

            TestParams();
            TestDefaults();
            TestDefaultGet();
            TestSet();
            TestReset();

            void TestParams()
            {
                var x = TEST_NAME(nameof(TestParams));

                MyShaderAsResource.Params.MyInt.Should().Be((StringName)"my_int", x);
                MyShaderAsResource.Params.MyUint.Should().Be((StringName)"my_uint", x);
                MyShaderAsResource.Params.MyBool.Should().Be((StringName)"my_bool", x);
                MyShaderAsResource.Params.MyFloat.Should().Be((StringName)"my_float", x);

                MyShaderAsResource.Params.MyMat2.Should().Be((StringName)"my_mat2", x);
                MyShaderAsResource.Params.MyMat3.Should().Be((StringName)"my_mat3", x);
                MyShaderAsResource.Params.MyMat4.Should().Be((StringName)"my_mat4", x);

                MyShaderAsResource.Params.MyVec2.Should().Be((StringName)"my_vec2", x);
                MyShaderAsResource.Params.MyVec3.Should().Be((StringName)"my_vec3", x);
                MyShaderAsResource.Params.MyVec4.Should().Be((StringName)"my_vec4", x);

                MyShaderAsResource.Params.MyIvec2.Should().Be((StringName)"my_ivec2", x);
                MyShaderAsResource.Params.MyIvec3.Should().Be((StringName)"my_ivec3", x);
                MyShaderAsResource.Params.MyIvec4.Should().Be((StringName)"my_ivec4", x);

                MyShaderAsResource.Params.MyUvec2.Should().Be((StringName)"my_uvec2", x);
                MyShaderAsResource.Params.MyUvec3.Should().Be((StringName)"my_uvec3", x);
                MyShaderAsResource.Params.MyUvec4.Should().Be((StringName)"my_uvec4", x);

                MyShaderAsResource.Params.MyBvec2.Should().Be((StringName)"my_bvec2", x);
                MyShaderAsResource.Params.MyBvec3.Should().Be((StringName)"my_bvec3", x);
                MyShaderAsResource.Params.MyBvec4.Should().Be((StringName)"my_bvec4", x);

                MyShaderAsResource.Params.MyCol3.Should().Be((StringName)"my_col3", x);
                MyShaderAsResource.Params.MyCol4.Should().Be((StringName)"my_col4", x);

                MyShaderAsResource.Params.MyIntAsRange.Should().Be((StringName)"my_int_as_range", x);
                MyShaderAsResource.Params.MyFloatAsRange.Should().Be((StringName)"my_float_as_range", x);

                MyShaderAsResource.Params.MyEnumAsInt1.Should().Be((StringName)"my_enum_as_int1", x);
                MyShaderAsResource.Params.MyEnumAsInt2.Should().Be((StringName)"my_enum_as_int2", x);
                MyShaderAsResource.Params.MyEnumAsEnum.Should().Be((StringName)"my_enum_as_enum", x);

                MyShaderAsResource.Params.MySampler2D.Should().Be((StringName)"my_sampler2D", x);
                MyShaderAsResource.Params.MyIsampler2D.Should().Be((StringName)"my_isampler2D", x);
                MyShaderAsResource.Params.MyUsampler2D.Should().Be((StringName)"my_usampler2D", x);

                MyShaderAsResource.Params.MySampler3D.Should().Be((StringName)"my_sampler3D", x);
                MyShaderAsResource.Params.MyIsampler3D.Should().Be((StringName)"my_isampler3D", x);
                MyShaderAsResource.Params.MyUsampler3D.Should().Be((StringName)"my_usampler3D", x);

                MyShaderAsResource.Params.MySampler2DArray.Should().Be((StringName)"my_sampler2DArray", x);
                MyShaderAsResource.Params.MyIsampler2DArray.Should().Be((StringName)"my_isampler2DArray", x);
                MyShaderAsResource.Params.MyUsampler2DArray.Should().Be((StringName)"my_usampler2DArray", x);

                MyShaderAsResource.Params.MySamplerCube.Should().Be((StringName)"my_samplerCube", x);
                MyShaderAsResource.Params.MySamplerCubeArray.Should().Be((StringName)"my_samplerCubeArray", x);
                MyShaderAsResource.Params.MySamplerExternalOES.Should().Be((StringName)"my_samplerExternalOES", x);

                MyShaderAsResource.Params.All.Should().BeEquivalentTo(
                [
                    MyShaderAsResource.Params.MyInt,
                    MyShaderAsResource.Params.MyUint,
                    MyShaderAsResource.Params.MyBool,
                    MyShaderAsResource.Params.MyFloat,

                    MyShaderAsResource.Params.MyMat2,
                    MyShaderAsResource.Params.MyMat3,
                    MyShaderAsResource.Params.MyMat4,

                    MyShaderAsResource.Params.MyVec2,
                    MyShaderAsResource.Params.MyVec3,
                    MyShaderAsResource.Params.MyVec4,

                    MyShaderAsResource.Params.MyIvec2,
                    MyShaderAsResource.Params.MyIvec3,
                    MyShaderAsResource.Params.MyIvec4,

                    MyShaderAsResource.Params.MyUvec2,
                    MyShaderAsResource.Params.MyUvec3,
                    MyShaderAsResource.Params.MyUvec4,

                    MyShaderAsResource.Params.MyBvec2,
                    MyShaderAsResource.Params.MyBvec3,
                    MyShaderAsResource.Params.MyBvec4,

                    MyShaderAsResource.Params.MyCol3,
                    MyShaderAsResource.Params.MyCol4,

                    MyShaderAsResource.Params.MyIntAsRange,
                    MyShaderAsResource.Params.MyFloatAsRange,

                    MyShaderAsResource.Params.MyEnumAsInt1,
                    MyShaderAsResource.Params.MyEnumAsInt2,
                    MyShaderAsResource.Params.MyEnumAsEnum,

                    MyShaderAsResource.Params.MySampler2D,
                    MyShaderAsResource.Params.MyIsampler2D,
                    MyShaderAsResource.Params.MyUsampler2D,

                    MyShaderAsResource.Params.MySampler3D,
                    MyShaderAsResource.Params.MyIsampler3D,
                    MyShaderAsResource.Params.MyUsampler3D,

                    MyShaderAsResource.Params.MySampler2DArray,
                    MyShaderAsResource.Params.MyIsampler2DArray,
                    MyShaderAsResource.Params.MyUsampler2DArray,

                    MyShaderAsResource.Params.MySamplerCube,
                    MyShaderAsResource.Params.MySamplerCubeArray,
                    MyShaderAsResource.Params.MySamplerExternalOES,
                ], x);
            }

            void TestDefaults()
            {
                var x = TEST_NAME(nameof(TestDefaults));

                MyShaderAsResource.Default.MyInt.Should().Be(0, x);
                MyShaderAsResource.Default.MyUint.Should().Be(0, x);
                MyShaderAsResource.Default.MyBool.Should().Be(false, x);
                MyShaderAsResource.Default.MyFloat.Should().Be(.0f, x);

                // Shader will set matrices to Identity if no default provided
                MyShaderAsResource.Default.MyMat2.Should().Be(Transform2D.Identity, x);
                MyShaderAsResource.Default.MyMat3.Should().Be(Basis.Identity, x);
                MyShaderAsResource.Default.MyMat4.Should().Be(Transform3D.Identity, x);

                MyShaderAsResource.Default.MyVec2.Should().Be(Vector2.Zero, x);
                MyShaderAsResource.Default.MyVec3.Should().Be(Vector3.Zero, x);
                MyShaderAsResource.Default.MyVec4.Should().Be(Vector4.Zero, x);

                MyShaderAsResource.Default.MyIvec2.Should().Be(Vector2I.Zero, x);
                MyShaderAsResource.Default.MyIvec3.Should().Be(Vector3I.Zero, x);
                MyShaderAsResource.Default.MyIvec4.Should().Be(Vector4I.Zero, x);

                MyShaderAsResource.Default.MyUvec2.Should().Be(Vector2I.Zero, x);
                MyShaderAsResource.Default.MyUvec3.Should().Be(Vector3I.Zero, x);
                MyShaderAsResource.Default.MyUvec4.Should().Be(Vector4I.Zero, x);

                MyShaderAsResource.Default.MyBvec2.Should().Be(0, x);
                MyShaderAsResource.Default.MyBvec3.Should().Be(0, x);
                MyShaderAsResource.Default.MyBvec4.Should().Be(0, x);

                // Shader will set colors to 0,0,0,1 if no default provided
                MyShaderAsResource.Default.MyCol3.Should().Be(new Color(0, 0, 0, 1), x);
                MyShaderAsResource.Default.MyCol4.Should().Be(new Color(0, 0, 0, 1), x);

                MyShaderAsResource.Default.MyIntAsRange.Should().Be(0, x);
                MyShaderAsResource.Default.MyFloatAsRange.Should().Be(.0f, x);

                MyShaderAsResource.Default.MyEnumAsInt1.Should().Be(0, x);
                MyShaderAsResource.Default.MyEnumAsInt2.Should().Be(0, x);
                ((object)MyShaderAsResource.Default.MyEnumAsEnum).Should().Be((MyShaderEnum)0, x);

                MyShaderAsResource.Default.MySampler2D.Should().Be(null, x);
                MyShaderAsResource.Default.MyIsampler2D.Should().Be(null, x);
                MyShaderAsResource.Default.MyUsampler2D.Should().Be(null, x);

                MyShaderAsResource.Default.MySampler3D.Should().Be(null, x);
                MyShaderAsResource.Default.MyIsampler3D.Should().Be(null, x);
                MyShaderAsResource.Default.MyUsampler3D.Should().Be(null, x);

                MyShaderAsResource.Default.MySampler2DArray.Should().Be(null, x);
                MyShaderAsResource.Default.MyIsampler2DArray.Should().Be(null, x);
                MyShaderAsResource.Default.MyUsampler2DArray.Should().Be(null, x);

                MyShaderAsResource.Default.MySamplerCube.Should().Be(null, x);
                MyShaderAsResource.Default.MySamplerCubeArray.Should().Be(null, x);
                MyShaderAsResource.Default.MySamplerExternalOES.Should().Be(null, x);
            }

            void TestDefaultGet(string x = null)
            {
                x ??= TEST_NAME(nameof(TestDefaultGet));

                MyShaderAsResource.GetMyInt(mat).Should().Be(MyShaderAsResource.Default.MyInt, x);
                MyShaderAsResource.GetMyUint(mat).Should().Be(MyShaderAsResource.Default.MyUint, x);
                MyShaderAsResource.GetMyBool(mat).Should().Be(MyShaderAsResource.Default.MyBool, x);
                MyShaderAsResource.GetMyFloat(mat).Should().Be(MyShaderAsResource.Default.MyFloat, x);

                MyShaderAsResource.GetMyMat2(mat).Should().Be(MyShaderAsResource.Default.MyMat2, x);
                MyShaderAsResource.GetMyMat3(mat).Should().Be(MyShaderAsResource.Default.MyMat3, x);
                MyShaderAsResource.GetMyMat4(mat).Should().Be(MyShaderAsResource.Default.MyMat4, x);

                MyShaderAsResource.GetMyVec2(mat).Should().Be(MyShaderAsResource.Default.MyVec2, x);
                MyShaderAsResource.GetMyVec3(mat).Should().Be(MyShaderAsResource.Default.MyVec3, x);
                MyShaderAsResource.GetMyVec4(mat).Should().Be(MyShaderAsResource.Default.MyVec4, x);

                MyShaderAsResource.GetMyIvec2(mat).Should().Be(MyShaderAsResource.Default.MyIvec2, x);
                MyShaderAsResource.GetMyIvec3(mat).Should().Be(MyShaderAsResource.Default.MyIvec3, x);
                MyShaderAsResource.GetMyIvec4(mat).Should().Be(MyShaderAsResource.Default.MyIvec4, x);

                MyShaderAsResource.GetMyUvec2(mat).Should().Be(MyShaderAsResource.Default.MyUvec2, x);
                MyShaderAsResource.GetMyUvec3(mat).Should().Be(MyShaderAsResource.Default.MyUvec3, x);
                MyShaderAsResource.GetMyUvec4(mat).Should().Be(MyShaderAsResource.Default.MyUvec4, x);

                MyShaderAsResource.GetMyBvec2(mat).Should().Be(MyShaderAsResource.Default.MyBvec2, x);
                MyShaderAsResource.GetMyBvec3(mat).Should().Be(MyShaderAsResource.Default.MyBvec3, x);
                MyShaderAsResource.GetMyBvec4(mat).Should().Be(MyShaderAsResource.Default.MyBvec4, x);

                MyShaderAsResource.GetMyCol3(mat).Should().Be(MyShaderAsResource.Default.MyCol3, x);
                MyShaderAsResource.GetMyCol4(mat).Should().Be(MyShaderAsResource.Default.MyCol4, x);

                MyShaderAsResource.GetMyIntAsRange(mat).Should().Be(MyShaderAsResource.Default.MyIntAsRange, x);
                MyShaderAsResource.GetMyFloatAsRange(mat).Should().Be(MyShaderAsResource.Default.MyFloatAsRange, x);

                MyShaderAsResource.GetMyEnumAsInt1(mat).Should().Be(MyShaderAsResource.Default.MyEnumAsInt1, x);
                MyShaderAsResource.GetMyEnumAsInt2(mat).Should().Be(MyShaderAsResource.Default.MyEnumAsInt2, x);
                MyShaderAsResource.GetMyEnumAsEnum(mat).Should().Be(MyShaderAsResource.Default.MyEnumAsEnum, x);

                MyShaderAsResource.GetMySampler2D(mat).Should().Be(MyShaderAsResource.Default.MySampler2D, x);
                MyShaderAsResource.GetMyIsampler2D(mat).Should().Be(MyShaderAsResource.Default.MyIsampler2D, x);
                MyShaderAsResource.GetMyUsampler2D(mat).Should().Be(MyShaderAsResource.Default.MyUsampler2D, x);

                MyShaderAsResource.GetMySampler3D(mat).Should().Be(MyShaderAsResource.Default.MySampler3D, x);
                MyShaderAsResource.GetMyIsampler3D(mat).Should().Be(MyShaderAsResource.Default.MyIsampler3D, x);
                MyShaderAsResource.GetMyUsampler3D(mat).Should().Be(MyShaderAsResource.Default.MyUsampler3D, x);

                MyShaderAsResource.GetMySampler2DArray(mat).Should().Be(MyShaderAsResource.Default.MySampler2DArray, x);
                MyShaderAsResource.GetMyIsampler2DArray(mat).Should().Be(MyShaderAsResource.Default.MyIsampler2DArray, x);
                MyShaderAsResource.GetMyUsampler2DArray(mat).Should().Be(MyShaderAsResource.Default.MyUsampler2DArray, x);

                MyShaderAsResource.GetMySamplerCube(mat).Should().Be(MyShaderAsResource.Default.MySamplerCube, x);
                MyShaderAsResource.GetMySamplerCubeArray(mat).Should().Be(MyShaderAsResource.Default.MySamplerCubeArray, x);
                MyShaderAsResource.GetMySamplerExternalOES(mat).Should().Be(MyShaderAsResource.Default.MySamplerExternalOES, x);
            }

            void TestSet()
            {
                var x = TEST_NAME(nameof(TestSet));

                MyShaderAsResource.SetMyInt(mat, 1); MyShaderAsResource.GetMyInt(mat).Should().Be(1, x);
                MyShaderAsResource.SetMyUint(mat, 1); MyShaderAsResource.GetMyUint(mat).Should().Be(1, x);
                MyShaderAsResource.SetMyBool(mat, true); MyShaderAsResource.GetMyBool(mat).Should().Be(true, x);
                MyShaderAsResource.SetMyFloat(mat, .1f); MyShaderAsResource.GetMyFloat(mat).Should().Be(.1f, x);

                MyShaderAsResource.SetMyMat2(mat, new Transform2D()); MyShaderAsResource.GetMyMat2(mat).Should().Be(new Transform2D(), x);
                MyShaderAsResource.SetMyMat3(mat, new Basis()); MyShaderAsResource.GetMyMat3(mat).Should().Be(new Basis(), x);
                MyShaderAsResource.SetMyMat4(mat, new Transform3D()); MyShaderAsResource.GetMyMat4(mat).Should().Be(new Transform3D(), x);

                MyShaderAsResource.SetMyVec2(mat, Vector2.One); MyShaderAsResource.GetMyVec2(mat).Should().Be(Vector2.One, x);
                MyShaderAsResource.SetMyVec3(mat, Vector3.One); MyShaderAsResource.GetMyVec3(mat).Should().Be(Vector3.One, x);
                MyShaderAsResource.SetMyVec4(mat, Vector4.One); MyShaderAsResource.GetMyVec4(mat).Should().Be(Vector4.One, x);

                MyShaderAsResource.SetMyIvec2(mat, Vector2I.One); MyShaderAsResource.GetMyIvec2(mat).Should().Be(Vector2I.One, x);
                MyShaderAsResource.SetMyIvec3(mat, Vector3I.One); MyShaderAsResource.GetMyIvec3(mat).Should().Be(Vector3I.One, x);
                MyShaderAsResource.SetMyIvec4(mat, Vector4I.One); MyShaderAsResource.GetMyIvec4(mat).Should().Be(Vector4I.One, x);

                MyShaderAsResource.SetMyUvec2(mat, Vector2I.One); MyShaderAsResource.GetMyUvec2(mat).Should().Be(Vector2I.One, x);
                MyShaderAsResource.SetMyUvec3(mat, Vector3I.One); MyShaderAsResource.GetMyUvec3(mat).Should().Be(Vector3I.One, x);
                MyShaderAsResource.SetMyUvec4(mat, Vector4I.One); MyShaderAsResource.GetMyUvec4(mat).Should().Be(Vector4I.One, x);

                MyShaderAsResource.SetMyBvec2(mat, 1); MyShaderAsResource.GetMyBvec2(mat).Should().Be(1, x);
                MyShaderAsResource.SetMyBvec3(mat, 1); MyShaderAsResource.GetMyBvec3(mat).Should().Be(1, x);
                MyShaderAsResource.SetMyBvec4(mat, 1); MyShaderAsResource.GetMyBvec4(mat).Should().Be(1, x);

                MyShaderAsResource.SetMyCol3(mat, Colors.Red); MyShaderAsResource.GetMyCol3(mat).Should().Be(Colors.Red, x);
                MyShaderAsResource.SetMyCol4(mat, Colors.Red); MyShaderAsResource.GetMyCol4(mat).Should().Be(Colors.Red, x);

                MyShaderAsResource.SetMyIntAsRange(mat, 1); MyShaderAsResource.GetMyIntAsRange(mat).Should().Be(1, x);
                MyShaderAsResource.SetMyFloatAsRange(mat, .1f); MyShaderAsResource.GetMyFloatAsRange(mat).Should().Be(.1f, x);

                MyShaderAsResource.SetMyEnumAsInt1(mat, 1); MyShaderAsResource.GetMyEnumAsInt1(mat).Should().Be(1, x);
                MyShaderAsResource.SetMyEnumAsInt2(mat, 1); MyShaderAsResource.GetMyEnumAsInt2(mat).Should().Be(1, x);
                MyShaderAsResource.SetMyEnumAsEnum(mat, MyShaderEnum.B); MyShaderAsResource.GetMyEnumAsEnum(mat).Should().Be(MyShaderEnum.B, x);

                var testTex2D = GD.Load<Texture2D>("res://Assets/icon.svg");
                var testTex3D = new ImageTexture3D();
                var testTex2DArray = new Texture2DArray();
                var cubemap = new Cubemap();
                var cubemapArray = new CubemapArray();
                var externalTexture = new ExternalTexture();

                MyShaderAsResource.SetMySampler2D(mat, testTex2D); MyShaderAsResource.GetMySampler2D(mat).Should().Be(testTex2D, x);
                MyShaderAsResource.SetMyIsampler2D(mat, testTex2D); MyShaderAsResource.GetMyIsampler2D(mat).Should().Be(testTex2D, x);
                MyShaderAsResource.SetMyUsampler2D(mat, testTex2D); MyShaderAsResource.GetMyUsampler2D(mat).Should().Be(testTex2D, x);

                MyShaderAsResource.SetMySampler3D(mat, testTex3D); MyShaderAsResource.GetMySampler3D(mat).Should().Be(testTex3D, x);
                MyShaderAsResource.SetMyIsampler3D(mat, testTex3D); MyShaderAsResource.GetMyIsampler3D(mat).Should().Be(testTex3D, x);
                MyShaderAsResource.SetMyUsampler3D(mat, testTex3D); MyShaderAsResource.GetMyUsampler3D(mat).Should().Be(testTex3D, x);

                MyShaderAsResource.SetMySampler2DArray(mat, testTex2DArray); MyShaderAsResource.GetMySampler2DArray(mat).Should().Be(testTex2DArray, x);
                MyShaderAsResource.SetMyIsampler2DArray(mat, testTex2DArray); MyShaderAsResource.GetMyIsampler2DArray(mat).Should().Be(testTex2DArray, x);
                MyShaderAsResource.SetMyUsampler2DArray(mat, testTex2DArray); MyShaderAsResource.GetMyUsampler2DArray(mat).Should().Be(testTex2DArray, x);

                MyShaderAsResource.SetMySamplerCube(mat, cubemap); MyShaderAsResource.GetMySamplerCube(mat).Should().Be(cubemap, x);
                MyShaderAsResource.SetMySamplerCubeArray(mat, cubemapArray); MyShaderAsResource.GetMySamplerCubeArray(mat).Should().Be(cubemapArray, x);
                MyShaderAsResource.SetMySamplerExternalOES(mat, externalTexture); MyShaderAsResource.GetMySamplerExternalOES(mat).Should().Be(externalTexture, x);
            }

            void TestReset()
            {
                var x = TEST_NAME(nameof(TestReset));

                MyShaderAsResource.ResetMyInt(mat);
                MyShaderAsResource.ResetMyUint(mat);
                MyShaderAsResource.ResetMyBool(mat);
                MyShaderAsResource.ResetMyFloat(mat);

                MyShaderAsResource.ResetMyMat2(mat);
                MyShaderAsResource.ResetMyMat3(mat);
                MyShaderAsResource.ResetMyMat4(mat);

                MyShaderAsResource.ResetMyVec2(mat);
                MyShaderAsResource.ResetMyVec3(mat);
                MyShaderAsResource.ResetMyVec4(mat);

                MyShaderAsResource.ResetMyIvec2(mat);
                MyShaderAsResource.ResetMyIvec3(mat);
                MyShaderAsResource.ResetMyIvec4(mat);

                MyShaderAsResource.ResetMyUvec2(mat);
                MyShaderAsResource.ResetMyUvec3(mat);
                MyShaderAsResource.ResetMyUvec4(mat);

                MyShaderAsResource.ResetMyBvec2(mat);
                MyShaderAsResource.ResetMyBvec3(mat);
                MyShaderAsResource.ResetMyBvec4(mat);

                MyShaderAsResource.ResetMyCol3(mat);
                MyShaderAsResource.ResetMyCol4(mat);

                MyShaderAsResource.ResetMyIntAsRange(mat);
                MyShaderAsResource.ResetMyFloatAsRange(mat);

                MyShaderAsResource.ResetMyEnumAsInt1(mat);
                MyShaderAsResource.ResetMyEnumAsInt2(mat);
                MyShaderAsResource.ResetMyEnumAsEnum(mat);

                MyShaderAsResource.ResetMySampler2D(mat);
                MyShaderAsResource.ResetMyIsampler2D(mat);
                MyShaderAsResource.ResetMyUsampler2D(mat);

                MyShaderAsResource.ResetMySampler3D(mat);
                MyShaderAsResource.ResetMyIsampler3D(mat);
                MyShaderAsResource.ResetMyUsampler3D(mat);

                MyShaderAsResource.ResetMySampler2DArray(mat);
                MyShaderAsResource.ResetMyIsampler2DArray(mat);
                MyShaderAsResource.ResetMyUsampler2DArray(mat);

                MyShaderAsResource.ResetMySamplerCube(mat);
                MyShaderAsResource.ResetMySamplerCubeArray(mat);
                MyShaderAsResource.ResetMySamplerExternalOES(mat);

                TestDefaultGet(x);
            }
        }

        static void RunMyShaderWithDefaultsTest(ShaderMaterial mat)
        {
            // Defaults are not set on shader material by default
            MyShaderWithDefaults.Reset(mat);

            var x = TEST_NAME(nameof(RunMyShaderWithDefaultsTest));
            mat.Shader.ResourcePath.Should().Be(MyShaderWithDefaults.ResPath, x);
            NestedTypes(typeof(MyShaderWithDefaults)).Should().BeEquivalentTo(["Params", "Default"], x);
            DeclaredMethods(typeof(MyShaderWithDefaults)).Count().Should().Be(MyShaderWithDefaults.Params.All.Length * 3 + 1, x); // Get/Set/Reset + Reset
            DeclaredFields(typeof(MyShaderWithDefaults.Params)).Count().Should().Be(MyShaderWithDefaults.Params.All.Length + 1, x); // +1 => All
            DeclaredFields(typeof(MyShaderWithDefaults.Default)).Count().Should().Be(MyShaderWithDefaults.Params.All.Length, x);

            TestParams();
            TestDefaults();
            TestDefaultGet();
            TestSet();
            TestReset();

            void TestParams()
            {
                var x = TEST_NAME(nameof(TestParams));

                MyShaderWithDefaults.Params.MyInt.Should().Be((StringName)"my_int", x);
                MyShaderWithDefaults.Params.MyUint.Should().Be((StringName)"my_uint", x);
                MyShaderWithDefaults.Params.MyBool.Should().Be((StringName)"my_bool", x);
                MyShaderWithDefaults.Params.MyFloat.Should().Be((StringName)"my_float", x);

                MyShaderWithDefaults.Params.MyMat2.Should().Be((StringName)"my_mat2", x);
                MyShaderWithDefaults.Params.MyMat3.Should().Be((StringName)"my_mat3", x);
                MyShaderWithDefaults.Params.MyMat4.Should().Be((StringName)"my_mat4", x);

                MyShaderWithDefaults.Params.MyVec2.Should().Be((StringName)"my_vec2", x);
                MyShaderWithDefaults.Params.MyVec3.Should().Be((StringName)"my_vec3", x);
                MyShaderWithDefaults.Params.MyVec4.Should().Be((StringName)"my_vec4", x);

                MyShaderWithDefaults.Params.MyIvec2.Should().Be((StringName)"my_ivec2", x);
                MyShaderWithDefaults.Params.MyIvec3.Should().Be((StringName)"my_ivec3", x);
                MyShaderWithDefaults.Params.MyIvec4.Should().Be((StringName)"my_ivec4", x);

                MyShaderWithDefaults.Params.MyUvec2.Should().Be((StringName)"my_uvec2", x);
                MyShaderWithDefaults.Params.MyUvec3.Should().Be((StringName)"my_uvec3", x);
                MyShaderWithDefaults.Params.MyUvec4.Should().Be((StringName)"my_uvec4", x);

                MyShaderWithDefaults.Params.MyBvec2.Should().Be((StringName)"my_bvec2", x);
                MyShaderWithDefaults.Params.MyBvec3.Should().Be((StringName)"my_bvec3", x);
                MyShaderWithDefaults.Params.MyBvec4.Should().Be((StringName)"my_bvec4", x);

                MyShaderWithDefaults.Params.MyCol3.Should().Be((StringName)"my_col3", x);
                MyShaderWithDefaults.Params.MyCol4.Should().Be((StringName)"my_col4", x);

                MyShaderWithDefaults.Params.MyIntAsRange.Should().Be((StringName)"my_int_as_range", x);
                MyShaderWithDefaults.Params.MyFloatAsRange.Should().Be((StringName)"my_float_as_range", x);

                MyShaderWithDefaults.Params.MyEnumAsInt1.Should().Be((StringName)"my_enum_as_int1", x);
                MyShaderWithDefaults.Params.MyEnumAsInt60.Should().Be((StringName)"my_enum_as_int60", x);
                MyShaderWithDefaults.Params.MyEnumAsEnum1.Should().Be((StringName)"my_enum_as_enum1", x);
                MyShaderWithDefaults.Params.MyEnumAsEnum60.Should().Be((StringName)"my_enum_as_enum60", x);

                MyShaderWithDefaults.Params.MyVec2WithScalarDefault.Should().Be((StringName)"my_vec2_WithScalarDefault", x);
                MyShaderWithDefaults.Params.MyVec3WithScalarDefault.Should().Be((StringName)"my_vec3_WithScalarDefault", x);
                MyShaderWithDefaults.Params.MyVec4WithScalarDefault.Should().Be((StringName)"my_vec4_WithScalarDefault", x);

                MyShaderWithDefaults.Params.MyIvec2WithScalarDefault.Should().Be((StringName)"my_ivec2_WithScalarDefault", x);
                MyShaderWithDefaults.Params.MyIvec3WithScalarDefault.Should().Be((StringName)"my_ivec3_WithScalarDefault", x);
                MyShaderWithDefaults.Params.MyIvec4WithScalarDefault.Should().Be((StringName)"my_ivec4_WithScalarDefault", x);

                MyShaderWithDefaults.Params.MyUvec2WithScalarDefault.Should().Be((StringName)"my_uvec2_WithScalarDefault", x);
                MyShaderWithDefaults.Params.MyUvec3WithScalarDefault.Should().Be((StringName)"my_uvec3_WithScalarDefault", x);
                MyShaderWithDefaults.Params.MyUvec4WithScalarDefault.Should().Be((StringName)"my_uvec4_WithScalarDefault", x);

                MyShaderWithDefaults.Params.MyBvec2WithScalarDefault.Should().Be((StringName)"my_bvec2_WithScalarDefault", x);
                MyShaderWithDefaults.Params.MyBvec3WithScalarDefault.Should().Be((StringName)"my_bvec3_WithScalarDefault", x);
                MyShaderWithDefaults.Params.MyBvec4WithScalarDefault.Should().Be((StringName)"my_bvec4_WithScalarDefault", x);

                MyShaderWithDefaults.Params.MyCol3WithScalarDefault.Should().Be((StringName)"my_col3_WithScalarDefault", x);
                MyShaderWithDefaults.Params.MyCol4WithScalarDefault.Should().Be((StringName)"my_col4_WithScalarDefault", x);

                MyShaderWithDefaults.Params.All.Should().BeEquivalentTo(
                [
                    MyShaderWithDefaults.Params.MyInt,
                    MyShaderWithDefaults.Params.MyUint,
                    MyShaderWithDefaults.Params.MyBool,
                    MyShaderWithDefaults.Params.MyFloat,

                    MyShaderWithDefaults.Params.MyMat2,
                    MyShaderWithDefaults.Params.MyMat3,
                    MyShaderWithDefaults.Params.MyMat4,

                    MyShaderWithDefaults.Params.MyVec2,
                    MyShaderWithDefaults.Params.MyVec3,
                    MyShaderWithDefaults.Params.MyVec4,

                    MyShaderWithDefaults.Params.MyIvec2,
                    MyShaderWithDefaults.Params.MyIvec3,
                    MyShaderWithDefaults.Params.MyIvec4,

                    MyShaderWithDefaults.Params.MyUvec2,
                    MyShaderWithDefaults.Params.MyUvec3,
                    MyShaderWithDefaults.Params.MyUvec4,

                    MyShaderWithDefaults.Params.MyBvec2,
                    MyShaderWithDefaults.Params.MyBvec3,
                    MyShaderWithDefaults.Params.MyBvec4,

                    MyShaderWithDefaults.Params.MyCol3,
                    MyShaderWithDefaults.Params.MyCol4,

                    MyShaderWithDefaults.Params.MyIntAsRange,
                    MyShaderWithDefaults.Params.MyFloatAsRange,

                    MyShaderWithDefaults.Params.MyEnumAsInt1,
                    MyShaderWithDefaults.Params.MyEnumAsInt60,
                    MyShaderWithDefaults.Params.MyEnumAsEnum1,
                    MyShaderWithDefaults.Params.MyEnumAsEnum60,

                    MyShaderWithDefaults.Params.MyVec2WithScalarDefault,
                    MyShaderWithDefaults.Params.MyVec3WithScalarDefault,
                    MyShaderWithDefaults.Params.MyVec4WithScalarDefault,

                    MyShaderWithDefaults.Params.MyIvec2WithScalarDefault,
                    MyShaderWithDefaults.Params.MyIvec3WithScalarDefault,
                    MyShaderWithDefaults.Params.MyIvec4WithScalarDefault,

                    MyShaderWithDefaults.Params.MyUvec2WithScalarDefault,
                    MyShaderWithDefaults.Params.MyUvec3WithScalarDefault,
                    MyShaderWithDefaults.Params.MyUvec4WithScalarDefault,

                    MyShaderWithDefaults.Params.MyBvec2WithScalarDefault,
                    MyShaderWithDefaults.Params.MyBvec3WithScalarDefault,
                    MyShaderWithDefaults.Params.MyBvec4WithScalarDefault,

                    MyShaderWithDefaults.Params.MyCol3WithScalarDefault,
                    MyShaderWithDefaults.Params.MyCol4WithScalarDefault,
                ], x);
            }

            void TestDefaults()
            {
                var x = TEST_NAME(nameof(TestDefaults));

                MyShaderWithDefaults.Default.MyInt.Should().Be(1, x);
                MyShaderWithDefaults.Default.MyUint.Should().Be(1, x);
                MyShaderWithDefaults.Default.MyBool.Should().Be(true, x);
                MyShaderWithDefaults.Default.MyFloat.Should().Be(.1f, x);

                MyShaderWithDefaults.Default.MyMat2.Should().Be(Transform2D.Identity, x);
                MyShaderWithDefaults.Default.MyMat3.Should().Be(Basis.Identity, x);
                MyShaderWithDefaults.Default.MyMat4.Should().Be(Transform3D.Identity, x);

                MyShaderWithDefaults.Default.MyVec2.Should().Be(Vector2.One, x);
                MyShaderWithDefaults.Default.MyVec3.Should().Be(Vector3.One, x);
                MyShaderWithDefaults.Default.MyVec4.Should().Be(Vector4.One, x);

                MyShaderWithDefaults.Default.MyIvec2.Should().Be(Vector2I.One, x);
                MyShaderWithDefaults.Default.MyIvec3.Should().Be(Vector3I.One, x);
                MyShaderWithDefaults.Default.MyIvec4.Should().Be(Vector4I.One, x);

                MyShaderWithDefaults.Default.MyUvec2.Should().Be(Vector2I.One, x);
                MyShaderWithDefaults.Default.MyUvec3.Should().Be(Vector3I.One, x);
                MyShaderWithDefaults.Default.MyUvec4.Should().Be(Vector4I.One, x);

                MyShaderWithDefaults.Default.MyBvec2.Should().Be(0b11, x);
                MyShaderWithDefaults.Default.MyBvec3.Should().Be(0b111, x);
                MyShaderWithDefaults.Default.MyBvec4.Should().Be(0b1111, x);

                MyShaderWithDefaults.Default.MyCol3.Should().Be(new Color(1, 1, 1, 1), x);
                MyShaderWithDefaults.Default.MyCol4.Should().Be(new Color(1, 1, 1, 1), x);

                MyShaderWithDefaults.Default.MyIntAsRange.Should().Be(1, x);
                MyShaderWithDefaults.Default.MyFloatAsRange.Should().Be(.1f, x);

                MyShaderWithDefaults.Default.MyEnumAsInt1.Should().Be(1, x);
                MyShaderWithDefaults.Default.MyEnumAsInt60.Should().Be(60, x);
                MyShaderWithDefaults.Default.MyEnumAsEnum1.Should().Be((MyShaderEnum)1, x);
                MyShaderWithDefaults.Default.MyEnumAsEnum60.Should().Be(MyShaderEnum.B, x);

                MyShaderWithDefaults.Default.MyVec2WithScalarDefault.Should().Be(Vector2.One, x);
                MyShaderWithDefaults.Default.MyVec3WithScalarDefault.Should().Be(Vector3.One, x);
                MyShaderWithDefaults.Default.MyVec4WithScalarDefault.Should().Be(Vector4.One, x);

                MyShaderWithDefaults.Default.MyIvec2WithScalarDefault.Should().Be(Vector2I.One, x);
                MyShaderWithDefaults.Default.MyIvec3WithScalarDefault.Should().Be(Vector3I.One, x);
                MyShaderWithDefaults.Default.MyIvec4WithScalarDefault.Should().Be(Vector4I.One, x);

                MyShaderWithDefaults.Default.MyUvec2WithScalarDefault.Should().Be(Vector2I.One, x);
                MyShaderWithDefaults.Default.MyUvec3WithScalarDefault.Should().Be(Vector3I.One, x);
                MyShaderWithDefaults.Default.MyUvec4WithScalarDefault.Should().Be(Vector4I.One, x);

                MyShaderWithDefaults.Default.MyBvec2WithScalarDefault.Should().Be(0b11, x);
                MyShaderWithDefaults.Default.MyBvec3WithScalarDefault.Should().Be(0b111, x);
                MyShaderWithDefaults.Default.MyBvec4WithScalarDefault.Should().Be(0b1111, x);

                MyShaderWithDefaults.Default.MyCol3WithScalarDefault.Should().Be(new Color(1, 1, 1, 1), x);
                MyShaderWithDefaults.Default.MyCol4WithScalarDefault.Should().Be(new Color(1, 1, 1, 1), x);
            }

            void TestDefaultGet(string x = null)
            {
                x ??= TEST_NAME(nameof(TestDefaultGet));

                MyShaderWithDefaults.GetMyInt(mat).Should().Be(MyShaderWithDefaults.Default.MyInt, x);
                MyShaderWithDefaults.GetMyUint(mat).Should().Be(MyShaderWithDefaults.Default.MyUint, x);
                MyShaderWithDefaults.GetMyBool(mat).Should().Be(MyShaderWithDefaults.Default.MyBool, x);
                MyShaderWithDefaults.GetMyFloat(mat).Should().Be(MyShaderWithDefaults.Default.MyFloat, x);

                MyShaderWithDefaults.GetMyMat2(mat).Should().Be(MyShaderWithDefaults.Default.MyMat2, x);
                MyShaderWithDefaults.GetMyMat3(mat).Should().Be(MyShaderWithDefaults.Default.MyMat3, x);
                MyShaderWithDefaults.GetMyMat4(mat).Should().Be(MyShaderWithDefaults.Default.MyMat4, x);

                MyShaderWithDefaults.GetMyVec2(mat).Should().Be(MyShaderWithDefaults.Default.MyVec2, x);
                MyShaderWithDefaults.GetMyVec3(mat).Should().Be(MyShaderWithDefaults.Default.MyVec3, x);
                MyShaderWithDefaults.GetMyVec4(mat).Should().Be(MyShaderWithDefaults.Default.MyVec4, x);

                MyShaderWithDefaults.GetMyIvec2(mat).Should().Be(MyShaderWithDefaults.Default.MyIvec2, x);
                MyShaderWithDefaults.GetMyIvec3(mat).Should().Be(MyShaderWithDefaults.Default.MyIvec3, x);
                MyShaderWithDefaults.GetMyIvec4(mat).Should().Be(MyShaderWithDefaults.Default.MyIvec4, x);

                MyShaderWithDefaults.GetMyUvec2(mat).Should().Be(MyShaderWithDefaults.Default.MyUvec2, x);
                MyShaderWithDefaults.GetMyUvec3(mat).Should().Be(MyShaderWithDefaults.Default.MyUvec3, x);
                MyShaderWithDefaults.GetMyUvec4(mat).Should().Be(MyShaderWithDefaults.Default.MyUvec4, x);

                MyShaderWithDefaults.GetMyBvec2(mat).Should().Be(MyShaderWithDefaults.Default.MyBvec2, x);
                MyShaderWithDefaults.GetMyBvec3(mat).Should().Be(MyShaderWithDefaults.Default.MyBvec3, x);
                MyShaderWithDefaults.GetMyBvec4(mat).Should().Be(MyShaderWithDefaults.Default.MyBvec4, x);

                MyShaderWithDefaults.GetMyCol3(mat).Should().Be(MyShaderWithDefaults.Default.MyCol3, x);
                MyShaderWithDefaults.GetMyCol4(mat).Should().Be(MyShaderWithDefaults.Default.MyCol4, x);

                MyShaderWithDefaults.GetMyIntAsRange(mat).Should().Be(MyShaderWithDefaults.Default.MyIntAsRange, x);
                MyShaderWithDefaults.GetMyFloatAsRange(mat).Should().Be(MyShaderWithDefaults.Default.MyFloatAsRange, x);

                MyShaderWithDefaults.GetMyEnumAsInt1(mat).Should().Be(MyShaderWithDefaults.Default.MyEnumAsInt1, x);
                MyShaderWithDefaults.GetMyEnumAsInt60(mat).Should().Be(MyShaderWithDefaults.Default.MyEnumAsInt60, x);
                MyShaderWithDefaults.GetMyEnumAsEnum1(mat).Should().Be(MyShaderWithDefaults.Default.MyEnumAsEnum1, x);
                MyShaderWithDefaults.GetMyEnumAsEnum60(mat).Should().Be(MyShaderWithDefaults.Default.MyEnumAsEnum60, x);

                MyShaderWithDefaults.GetMyVec2WithScalarDefault(mat).Should().Be(MyShaderWithDefaults.Default.MyVec2WithScalarDefault, x);
                MyShaderWithDefaults.GetMyVec3WithScalarDefault(mat).Should().Be(MyShaderWithDefaults.Default.MyVec3WithScalarDefault, x);
                MyShaderWithDefaults.GetMyVec4WithScalarDefault(mat).Should().Be(MyShaderWithDefaults.Default.MyVec4WithScalarDefault, x);

                MyShaderWithDefaults.GetMyIvec2WithScalarDefault(mat).Should().Be(MyShaderWithDefaults.Default.MyIvec2WithScalarDefault, x);
                MyShaderWithDefaults.GetMyIvec3WithScalarDefault(mat).Should().Be(MyShaderWithDefaults.Default.MyIvec3WithScalarDefault, x);
                MyShaderWithDefaults.GetMyIvec4WithScalarDefault(mat).Should().Be(MyShaderWithDefaults.Default.MyIvec4WithScalarDefault, x);

                MyShaderWithDefaults.GetMyUvec2WithScalarDefault(mat).Should().Be(MyShaderWithDefaults.Default.MyUvec2WithScalarDefault, x);
                MyShaderWithDefaults.GetMyUvec3WithScalarDefault(mat).Should().Be(MyShaderWithDefaults.Default.MyUvec3WithScalarDefault, x);
                MyShaderWithDefaults.GetMyUvec4WithScalarDefault(mat).Should().Be(MyShaderWithDefaults.Default.MyUvec4WithScalarDefault, x);

                MyShaderWithDefaults.GetMyBvec2WithScalarDefault(mat).Should().Be(MyShaderWithDefaults.Default.MyBvec2WithScalarDefault, x);
                MyShaderWithDefaults.GetMyBvec3WithScalarDefault(mat).Should().Be(MyShaderWithDefaults.Default.MyBvec3WithScalarDefault, x);
                MyShaderWithDefaults.GetMyBvec4WithScalarDefault(mat).Should().Be(MyShaderWithDefaults.Default.MyBvec4WithScalarDefault, x);

                MyShaderWithDefaults.GetMyCol3WithScalarDefault(mat).Should().Be(MyShaderWithDefaults.Default.MyCol3WithScalarDefault, x);
                MyShaderWithDefaults.GetMyCol4WithScalarDefault(mat).Should().Be(MyShaderWithDefaults.Default.MyCol4WithScalarDefault, x);
            }

            void TestSet()
            {
                var x = TEST_NAME(nameof(TestSet));

                MyShaderWithDefaults.SetMyInt(mat, default); MyShaderWithDefaults.GetMyInt(mat).Should().Be(0, x);
                MyShaderWithDefaults.SetMyUint(mat, default); MyShaderWithDefaults.GetMyUint(mat).Should().Be(0, x);
                MyShaderWithDefaults.SetMyBool(mat, default); MyShaderWithDefaults.GetMyBool(mat).Should().Be(false, x);
                MyShaderWithDefaults.SetMyFloat(mat, default); MyShaderWithDefaults.GetMyFloat(mat).Should().Be(.0f, x);

                MyShaderWithDefaults.SetMyMat2(mat, default); MyShaderWithDefaults.GetMyMat2(mat).Should().Be(new Transform2D(), x);
                MyShaderWithDefaults.SetMyMat3(mat, default); MyShaderWithDefaults.GetMyMat3(mat).Should().Be(new Basis(), x);
                MyShaderWithDefaults.SetMyMat4(mat, default); MyShaderWithDefaults.GetMyMat4(mat).Should().Be(new Transform3D(), x);

                MyShaderWithDefaults.SetMyVec2(mat, default); MyShaderWithDefaults.GetMyVec2(mat).Should().Be(Vector2.Zero, x);
                MyShaderWithDefaults.SetMyVec3(mat, default); MyShaderWithDefaults.GetMyVec3(mat).Should().Be(Vector3.Zero, x);
                MyShaderWithDefaults.SetMyVec4(mat, default); MyShaderWithDefaults.GetMyVec4(mat).Should().Be(Vector4.Zero, x);

                MyShaderWithDefaults.SetMyIvec2(mat, default); MyShaderWithDefaults.GetMyIvec2(mat).Should().Be(Vector2I.Zero, x);
                MyShaderWithDefaults.SetMyIvec3(mat, default); MyShaderWithDefaults.GetMyIvec3(mat).Should().Be(Vector3I.Zero, x);
                MyShaderWithDefaults.SetMyIvec4(mat, default); MyShaderWithDefaults.GetMyIvec4(mat).Should().Be(Vector4I.Zero, x);

                MyShaderWithDefaults.SetMyUvec2(mat, default); MyShaderWithDefaults.GetMyUvec2(mat).Should().Be(Vector2I.Zero, x);
                MyShaderWithDefaults.SetMyUvec3(mat, default); MyShaderWithDefaults.GetMyUvec3(mat).Should().Be(Vector3I.Zero, x);
                MyShaderWithDefaults.SetMyUvec4(mat, default); MyShaderWithDefaults.GetMyUvec4(mat).Should().Be(Vector4I.Zero, x);

                MyShaderWithDefaults.SetMyBvec2(mat, default); MyShaderWithDefaults.GetMyBvec2(mat).Should().Be(0, x);
                MyShaderWithDefaults.SetMyBvec3(mat, default); MyShaderWithDefaults.GetMyBvec3(mat).Should().Be(0, x);
                MyShaderWithDefaults.SetMyBvec4(mat, default); MyShaderWithDefaults.GetMyBvec4(mat).Should().Be(0, x);

                MyShaderWithDefaults.SetMyCol3(mat, default); MyShaderWithDefaults.GetMyCol3(mat).Should().Be(new Color(), x);
                MyShaderWithDefaults.SetMyCol4(mat, default); MyShaderWithDefaults.GetMyCol4(mat).Should().Be(new Color(), x);

                MyShaderWithDefaults.SetMyIntAsRange(mat, default); MyShaderWithDefaults.GetMyIntAsRange(mat).Should().Be(0, x);
                MyShaderWithDefaults.SetMyFloatAsRange(mat, default); MyShaderWithDefaults.GetMyFloatAsRange(mat).Should().Be(.0f, x);

                MyShaderWithDefaults.SetMyEnumAsInt1(mat, default); MyShaderWithDefaults.GetMyEnumAsInt1(mat).Should().Be(0, x);
                MyShaderWithDefaults.SetMyEnumAsInt60(mat, default); MyShaderWithDefaults.GetMyEnumAsInt60(mat).Should().Be(0, x);
                MyShaderWithDefaults.SetMyEnumAsEnum1(mat, default); MyShaderWithDefaults.GetMyEnumAsEnum1(mat).Should().Be(0, x);
                MyShaderWithDefaults.SetMyEnumAsEnum60(mat, default); MyShaderWithDefaults.GetMyEnumAsEnum60(mat).Should().Be(0, x);

                MyShaderWithDefaults.SetMyVec2WithScalarDefault(mat, default); MyShaderWithDefaults.GetMyVec2WithScalarDefault(mat).Should().Be(Vector2.Zero, x);
                MyShaderWithDefaults.SetMyVec3WithScalarDefault(mat, default); MyShaderWithDefaults.GetMyVec3WithScalarDefault(mat).Should().Be(Vector3.Zero, x);
                MyShaderWithDefaults.SetMyVec4WithScalarDefault(mat, default); MyShaderWithDefaults.GetMyVec4WithScalarDefault(mat).Should().Be(Vector4.Zero, x);

                MyShaderWithDefaults.SetMyIvec2WithScalarDefault(mat, default); MyShaderWithDefaults.GetMyIvec2WithScalarDefault(mat).Should().Be(Vector2I.Zero, x);
                MyShaderWithDefaults.SetMyIvec3WithScalarDefault(mat, default); MyShaderWithDefaults.GetMyIvec3WithScalarDefault(mat).Should().Be(Vector3I.Zero, x);
                MyShaderWithDefaults.SetMyIvec4WithScalarDefault(mat, default); MyShaderWithDefaults.GetMyIvec4WithScalarDefault(mat).Should().Be(Vector4I.Zero, x);

                MyShaderWithDefaults.SetMyUvec2WithScalarDefault(mat, default); MyShaderWithDefaults.GetMyUvec2WithScalarDefault(mat).Should().Be(Vector2I.Zero, x);
                MyShaderWithDefaults.SetMyUvec3WithScalarDefault(mat, default); MyShaderWithDefaults.GetMyUvec3WithScalarDefault(mat).Should().Be(Vector3I.Zero, x);
                MyShaderWithDefaults.SetMyUvec4WithScalarDefault(mat, default); MyShaderWithDefaults.GetMyUvec4WithScalarDefault(mat).Should().Be(Vector4I.Zero, x);

                MyShaderWithDefaults.SetMyBvec2WithScalarDefault(mat, default); MyShaderWithDefaults.GetMyBvec2WithScalarDefault(mat).Should().Be(0, x);
                MyShaderWithDefaults.SetMyBvec3WithScalarDefault(mat, default); MyShaderWithDefaults.GetMyBvec3WithScalarDefault(mat).Should().Be(0, x);
                MyShaderWithDefaults.SetMyBvec4WithScalarDefault(mat, default); MyShaderWithDefaults.GetMyBvec4WithScalarDefault(mat).Should().Be(0, x);

                MyShaderWithDefaults.SetMyCol3WithScalarDefault(mat, default); MyShaderWithDefaults.GetMyCol3WithScalarDefault(mat).Should().Be(new Color(), x);
                MyShaderWithDefaults.SetMyCol4WithScalarDefault(mat, default); MyShaderWithDefaults.GetMyCol4WithScalarDefault(mat).Should().Be(new Color(), x);
            }

            void TestReset()
            {
                var x = TEST_NAME(nameof(TestReset));

                MyShaderWithDefaults.ResetMyInt(mat);
                MyShaderWithDefaults.ResetMyUint(mat);
                MyShaderWithDefaults.ResetMyBool(mat);
                MyShaderWithDefaults.ResetMyFloat(mat);

                MyShaderWithDefaults.ResetMyMat2(mat);
                MyShaderWithDefaults.ResetMyMat3(mat);
                MyShaderWithDefaults.ResetMyMat4(mat);

                MyShaderWithDefaults.ResetMyVec2(mat);
                MyShaderWithDefaults.ResetMyVec3(mat);
                MyShaderWithDefaults.ResetMyVec4(mat);

                MyShaderWithDefaults.ResetMyIvec2(mat);
                MyShaderWithDefaults.ResetMyIvec3(mat);
                MyShaderWithDefaults.ResetMyIvec4(mat);

                MyShaderWithDefaults.ResetMyUvec2(mat);
                MyShaderWithDefaults.ResetMyUvec3(mat);
                MyShaderWithDefaults.ResetMyUvec4(mat);

                MyShaderWithDefaults.ResetMyBvec2(mat);
                MyShaderWithDefaults.ResetMyBvec3(mat);
                MyShaderWithDefaults.ResetMyBvec4(mat);

                MyShaderWithDefaults.ResetMyCol3(mat);
                MyShaderWithDefaults.ResetMyCol4(mat);

                MyShaderWithDefaults.ResetMyIntAsRange(mat);
                MyShaderWithDefaults.ResetMyFloatAsRange(mat);

                MyShaderWithDefaults.ResetMyEnumAsInt1(mat);
                MyShaderWithDefaults.ResetMyEnumAsInt60(mat);
                MyShaderWithDefaults.ResetMyEnumAsEnum1(mat);
                MyShaderWithDefaults.ResetMyEnumAsEnum60(mat);

                MyShaderWithDefaults.ResetMyVec2WithScalarDefault(mat);
                MyShaderWithDefaults.ResetMyVec3WithScalarDefault(mat);
                MyShaderWithDefaults.ResetMyVec4WithScalarDefault(mat);

                MyShaderWithDefaults.ResetMyIvec2WithScalarDefault(mat);
                MyShaderWithDefaults.ResetMyIvec3WithScalarDefault(mat);
                MyShaderWithDefaults.ResetMyIvec4WithScalarDefault(mat);

                MyShaderWithDefaults.ResetMyUvec2WithScalarDefault(mat);
                MyShaderWithDefaults.ResetMyUvec3WithScalarDefault(mat);
                MyShaderWithDefaults.ResetMyUvec4WithScalarDefault(mat);

                MyShaderWithDefaults.ResetMyBvec2WithScalarDefault(mat);
                MyShaderWithDefaults.ResetMyBvec3WithScalarDefault(mat);
                MyShaderWithDefaults.ResetMyBvec4WithScalarDefault(mat);

                MyShaderWithDefaults.ResetMyCol3WithScalarDefault(mat);
                MyShaderWithDefaults.ResetMyCol4WithScalarDefault(mat);

                TestDefaultGet(x);
            }
        }

        static void RunMyShaderWithDefaultsAsResourceTest(ShaderMaterial mat)
        {
            // Defaults are set by material shader resource
            //MyShaderWithDefaults.Reset(mat);

            var x = TEST_NAME(nameof(RunMyShaderWithDefaultsAsResourceTest));
            mat.Shader.ResourcePath.Should().Be(MyShaderWithDefaultsAsResource.ResPath);
            NestedTypes(typeof(MyShaderWithDefaultsAsResource)).Should().BeEquivalentTo(NestedTypes(typeof(MyShaderWithDefaults)), x);
            DeclaredMethods(typeof(MyShaderWithDefaultsAsResource)).Should().BeEquivalentTo(DeclaredMethods(typeof(MyShaderWithDefaults)), x);
            DeclaredFields(typeof(MyShaderWithDefaultsAsResource.Params)).Should().BeEquivalentTo(DeclaredFields(typeof(MyShaderWithDefaults.Params)), x);
            DeclaredFields(typeof(MyShaderWithDefaultsAsResource.Default)).Should().BeEquivalentTo(DeclaredFields(typeof(MyShaderWithDefaults.Default)), x);

            TestParams();
            TestDefaults();
            TestDefaultGet();
            TestSet();
            TestReset();

            void TestParams()
            {
                var x = TEST_NAME(nameof(TestParams));

                MyShaderWithDefaultsAsResource.Params.MyInt.Should().Be((StringName)"my_int", x);
                MyShaderWithDefaultsAsResource.Params.MyUint.Should().Be((StringName)"my_uint", x);
                MyShaderWithDefaultsAsResource.Params.MyBool.Should().Be((StringName)"my_bool", x);
                MyShaderWithDefaultsAsResource.Params.MyFloat.Should().Be((StringName)"my_float", x);

                MyShaderWithDefaultsAsResource.Params.MyMat2.Should().Be((StringName)"my_mat2", x);
                MyShaderWithDefaultsAsResource.Params.MyMat3.Should().Be((StringName)"my_mat3", x);
                MyShaderWithDefaultsAsResource.Params.MyMat4.Should().Be((StringName)"my_mat4", x);

                MyShaderWithDefaultsAsResource.Params.MyVec2.Should().Be((StringName)"my_vec2", x);
                MyShaderWithDefaultsAsResource.Params.MyVec3.Should().Be((StringName)"my_vec3", x);
                MyShaderWithDefaultsAsResource.Params.MyVec4.Should().Be((StringName)"my_vec4", x);

                MyShaderWithDefaultsAsResource.Params.MyIvec2.Should().Be((StringName)"my_ivec2", x);
                MyShaderWithDefaultsAsResource.Params.MyIvec3.Should().Be((StringName)"my_ivec3", x);
                MyShaderWithDefaultsAsResource.Params.MyIvec4.Should().Be((StringName)"my_ivec4", x);

                MyShaderWithDefaultsAsResource.Params.MyUvec2.Should().Be((StringName)"my_uvec2", x);
                MyShaderWithDefaultsAsResource.Params.MyUvec3.Should().Be((StringName)"my_uvec3", x);
                MyShaderWithDefaultsAsResource.Params.MyUvec4.Should().Be((StringName)"my_uvec4", x);

                MyShaderWithDefaultsAsResource.Params.MyBvec2.Should().Be((StringName)"my_bvec2", x);
                MyShaderWithDefaultsAsResource.Params.MyBvec3.Should().Be((StringName)"my_bvec3", x);
                MyShaderWithDefaultsAsResource.Params.MyBvec4.Should().Be((StringName)"my_bvec4", x);

                MyShaderWithDefaultsAsResource.Params.MyCol3.Should().Be((StringName)"my_col3", x);
                MyShaderWithDefaultsAsResource.Params.MyCol4.Should().Be((StringName)"my_col4", x);

                MyShaderWithDefaultsAsResource.Params.MyIntAsRange.Should().Be((StringName)"my_int_as_range", x);
                MyShaderWithDefaultsAsResource.Params.MyFloatAsRange.Should().Be((StringName)"my_float_as_range", x);

                MyShaderWithDefaultsAsResource.Params.MyEnumAsInt1.Should().Be((StringName)"my_enum_as_int1", x);
                MyShaderWithDefaultsAsResource.Params.MyEnumAsInt60.Should().Be((StringName)"my_enum_as_int60", x);
                MyShaderWithDefaultsAsResource.Params.MyEnumAsEnum1.Should().Be((StringName)"my_enum_as_enum1", x);
                MyShaderWithDefaultsAsResource.Params.MyEnumAsEnum60.Should().Be((StringName)"my_enum_as_enum60", x);

                MyShaderWithDefaultsAsResource.Params.MyVec2WithScalarDefault.Should().Be((StringName)"my_vec2_WithScalarDefault", x);
                MyShaderWithDefaultsAsResource.Params.MyVec3WithScalarDefault.Should().Be((StringName)"my_vec3_WithScalarDefault", x);
                MyShaderWithDefaultsAsResource.Params.MyVec4WithScalarDefault.Should().Be((StringName)"my_vec4_WithScalarDefault", x);

                MyShaderWithDefaultsAsResource.Params.MyIvec2WithScalarDefault.Should().Be((StringName)"my_ivec2_WithScalarDefault", x);
                MyShaderWithDefaultsAsResource.Params.MyIvec3WithScalarDefault.Should().Be((StringName)"my_ivec3_WithScalarDefault", x);
                MyShaderWithDefaultsAsResource.Params.MyIvec4WithScalarDefault.Should().Be((StringName)"my_ivec4_WithScalarDefault", x);

                MyShaderWithDefaultsAsResource.Params.MyUvec2WithScalarDefault.Should().Be((StringName)"my_uvec2_WithScalarDefault", x);
                MyShaderWithDefaultsAsResource.Params.MyUvec3WithScalarDefault.Should().Be((StringName)"my_uvec3_WithScalarDefault", x);
                MyShaderWithDefaultsAsResource.Params.MyUvec4WithScalarDefault.Should().Be((StringName)"my_uvec4_WithScalarDefault", x);

                MyShaderWithDefaultsAsResource.Params.MyBvec2WithScalarDefault.Should().Be((StringName)"my_bvec2_WithScalarDefault", x);
                MyShaderWithDefaultsAsResource.Params.MyBvec3WithScalarDefault.Should().Be((StringName)"my_bvec3_WithScalarDefault", x);
                MyShaderWithDefaultsAsResource.Params.MyBvec4WithScalarDefault.Should().Be((StringName)"my_bvec4_WithScalarDefault", x);

                MyShaderWithDefaultsAsResource.Params.MyCol3WithScalarDefault.Should().Be((StringName)"my_col3_WithScalarDefault", x);
                MyShaderWithDefaultsAsResource.Params.MyCol4WithScalarDefault.Should().Be((StringName)"my_col4_WithScalarDefault", x);

                MyShaderWithDefaultsAsResource.Params.All.Should().BeEquivalentTo(
                [
                    MyShaderWithDefaultsAsResource.Params.MyInt,
                    MyShaderWithDefaultsAsResource.Params.MyUint,
                    MyShaderWithDefaultsAsResource.Params.MyBool,
                    MyShaderWithDefaultsAsResource.Params.MyFloat,

                    MyShaderWithDefaultsAsResource.Params.MyMat2,
                    MyShaderWithDefaultsAsResource.Params.MyMat3,
                    MyShaderWithDefaultsAsResource.Params.MyMat4,

                    MyShaderWithDefaultsAsResource.Params.MyVec2,
                    MyShaderWithDefaultsAsResource.Params.MyVec3,
                    MyShaderWithDefaultsAsResource.Params.MyVec4,

                    MyShaderWithDefaultsAsResource.Params.MyIvec2,
                    MyShaderWithDefaultsAsResource.Params.MyIvec3,
                    MyShaderWithDefaultsAsResource.Params.MyIvec4,

                    MyShaderWithDefaultsAsResource.Params.MyUvec2,
                    MyShaderWithDefaultsAsResource.Params.MyUvec3,
                    MyShaderWithDefaultsAsResource.Params.MyUvec4,

                    MyShaderWithDefaultsAsResource.Params.MyBvec2,
                    MyShaderWithDefaultsAsResource.Params.MyBvec3,
                    MyShaderWithDefaultsAsResource.Params.MyBvec4,

                    MyShaderWithDefaultsAsResource.Params.MyCol3,
                    MyShaderWithDefaultsAsResource.Params.MyCol4,

                    MyShaderWithDefaultsAsResource.Params.MyIntAsRange,
                    MyShaderWithDefaultsAsResource.Params.MyFloatAsRange,

                    MyShaderWithDefaultsAsResource.Params.MyEnumAsInt1,
                    MyShaderWithDefaultsAsResource.Params.MyEnumAsInt60,
                    MyShaderWithDefaultsAsResource.Params.MyEnumAsEnum1,
                    MyShaderWithDefaultsAsResource.Params.MyEnumAsEnum60,

                    MyShaderWithDefaultsAsResource.Params.MyVec2WithScalarDefault,
                    MyShaderWithDefaultsAsResource.Params.MyVec3WithScalarDefault,
                    MyShaderWithDefaultsAsResource.Params.MyVec4WithScalarDefault,

                    MyShaderWithDefaultsAsResource.Params.MyIvec2WithScalarDefault,
                    MyShaderWithDefaultsAsResource.Params.MyIvec3WithScalarDefault,
                    MyShaderWithDefaultsAsResource.Params.MyIvec4WithScalarDefault,

                    MyShaderWithDefaultsAsResource.Params.MyUvec2WithScalarDefault,
                    MyShaderWithDefaultsAsResource.Params.MyUvec3WithScalarDefault,
                    MyShaderWithDefaultsAsResource.Params.MyUvec4WithScalarDefault,

                    MyShaderWithDefaultsAsResource.Params.MyBvec2WithScalarDefault,
                    MyShaderWithDefaultsAsResource.Params.MyBvec3WithScalarDefault,
                    MyShaderWithDefaultsAsResource.Params.MyBvec4WithScalarDefault,

                    MyShaderWithDefaultsAsResource.Params.MyCol3WithScalarDefault,
                    MyShaderWithDefaultsAsResource.Params.MyCol4WithScalarDefault,
                ], x);
            }

            void TestDefaults()
            {
                var x = TEST_NAME(nameof(TestDefaults));

                MyShaderWithDefaultsAsResource.Default.MyInt.Should().Be(1, x);
                MyShaderWithDefaultsAsResource.Default.MyUint.Should().Be(1, x);
                MyShaderWithDefaultsAsResource.Default.MyBool.Should().Be(true, x);
                MyShaderWithDefaultsAsResource.Default.MyFloat.Should().Be(.1f, x);

                MyShaderWithDefaultsAsResource.Default.MyMat2.Should().Be(Transform2D.Identity, x);
                MyShaderWithDefaultsAsResource.Default.MyMat3.Should().Be(Basis.Identity, x);
                MyShaderWithDefaultsAsResource.Default.MyMat4.Should().Be(Transform3D.Identity, x);

                MyShaderWithDefaultsAsResource.Default.MyVec2.Should().Be(Vector2.One, x);
                MyShaderWithDefaultsAsResource.Default.MyVec3.Should().Be(Vector3.One, x);
                MyShaderWithDefaultsAsResource.Default.MyVec4.Should().Be(Vector4.One, x);

                MyShaderWithDefaultsAsResource.Default.MyIvec2.Should().Be(Vector2I.One, x);
                MyShaderWithDefaultsAsResource.Default.MyIvec3.Should().Be(Vector3I.One, x);
                MyShaderWithDefaultsAsResource.Default.MyIvec4.Should().Be(Vector4I.One, x);

                MyShaderWithDefaultsAsResource.Default.MyUvec2.Should().Be(Vector2I.One, x);
                MyShaderWithDefaultsAsResource.Default.MyUvec3.Should().Be(Vector3I.One, x);
                MyShaderWithDefaultsAsResource.Default.MyUvec4.Should().Be(Vector4I.One, x);

                MyShaderWithDefaultsAsResource.Default.MyBvec2.Should().Be(0b11, x);
                MyShaderWithDefaultsAsResource.Default.MyBvec3.Should().Be(0b111, x);
                MyShaderWithDefaultsAsResource.Default.MyBvec4.Should().Be(0b1111, x);

                MyShaderWithDefaultsAsResource.Default.MyCol3.Should().Be(new Color(1, 1, 1, 1), x);
                MyShaderWithDefaultsAsResource.Default.MyCol4.Should().Be(new Color(1, 1, 1, 1), x);

                MyShaderWithDefaultsAsResource.Default.MyIntAsRange.Should().Be(1, x);
                MyShaderWithDefaultsAsResource.Default.MyFloatAsRange.Should().Be(.1f, x);

                MyShaderWithDefaultsAsResource.Default.MyEnumAsInt1.Should().Be(1, x);
                MyShaderWithDefaultsAsResource.Default.MyEnumAsInt60.Should().Be(60, x);
                MyShaderWithDefaultsAsResource.Default.MyEnumAsEnum1.Should().Be((MyShaderEnum)1, x);
                MyShaderWithDefaultsAsResource.Default.MyEnumAsEnum60.Should().Be(MyShaderEnum.B, x);

                MyShaderWithDefaultsAsResource.Default.MyVec2WithScalarDefault.Should().Be(Vector2.One, x);
                MyShaderWithDefaultsAsResource.Default.MyVec3WithScalarDefault.Should().Be(Vector3.One, x);
                MyShaderWithDefaultsAsResource.Default.MyVec4WithScalarDefault.Should().Be(Vector4.One, x);

                MyShaderWithDefaultsAsResource.Default.MyIvec2WithScalarDefault.Should().Be(Vector2I.One, x);
                MyShaderWithDefaultsAsResource.Default.MyIvec3WithScalarDefault.Should().Be(Vector3I.One, x);
                MyShaderWithDefaultsAsResource.Default.MyIvec4WithScalarDefault.Should().Be(Vector4I.One, x);

                MyShaderWithDefaultsAsResource.Default.MyUvec2WithScalarDefault.Should().Be(Vector2I.One, x);
                MyShaderWithDefaultsAsResource.Default.MyUvec3WithScalarDefault.Should().Be(Vector3I.One, x);
                MyShaderWithDefaultsAsResource.Default.MyUvec4WithScalarDefault.Should().Be(Vector4I.One, x);

                MyShaderWithDefaultsAsResource.Default.MyBvec2WithScalarDefault.Should().Be(0b11, x);
                MyShaderWithDefaultsAsResource.Default.MyBvec3WithScalarDefault.Should().Be(0b111, x);
                MyShaderWithDefaultsAsResource.Default.MyBvec4WithScalarDefault.Should().Be(0b1111, x);

                MyShaderWithDefaultsAsResource.Default.MyCol3WithScalarDefault.Should().Be(new Color(1, 1, 1, 1), x);
                MyShaderWithDefaultsAsResource.Default.MyCol4WithScalarDefault.Should().Be(new Color(1, 1, 1, 1), x);
            }

            void TestDefaultGet(string x = null)
            {
                x ??= TEST_NAME(nameof(TestDefaultGet));

                MyShaderWithDefaultsAsResource.GetMyInt(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyInt, x);
                MyShaderWithDefaultsAsResource.GetMyUint(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyUint, x);
                MyShaderWithDefaultsAsResource.GetMyBool(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyBool, x);
                MyShaderWithDefaultsAsResource.GetMyFloat(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyFloat, x);

                MyShaderWithDefaultsAsResource.GetMyMat2(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyMat2, x);
                MyShaderWithDefaultsAsResource.GetMyMat3(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyMat3, x);
                MyShaderWithDefaultsAsResource.GetMyMat4(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyMat4, x);

                MyShaderWithDefaultsAsResource.GetMyVec2(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyVec2, x);
                MyShaderWithDefaultsAsResource.GetMyVec3(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyVec3, x);
                MyShaderWithDefaultsAsResource.GetMyVec4(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyVec4, x);

                MyShaderWithDefaultsAsResource.GetMyIvec2(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyIvec2, x);
                MyShaderWithDefaultsAsResource.GetMyIvec3(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyIvec3, x);
                MyShaderWithDefaultsAsResource.GetMyIvec4(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyIvec4, x);

                MyShaderWithDefaultsAsResource.GetMyUvec2(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyUvec2, x);
                MyShaderWithDefaultsAsResource.GetMyUvec3(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyUvec3, x);
                MyShaderWithDefaultsAsResource.GetMyUvec4(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyUvec4, x);

                MyShaderWithDefaultsAsResource.GetMyBvec2(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyBvec2, x);
                MyShaderWithDefaultsAsResource.GetMyBvec3(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyBvec3, x);
                MyShaderWithDefaultsAsResource.GetMyBvec4(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyBvec4, x);

                MyShaderWithDefaultsAsResource.GetMyCol3(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyCol3, x);
                MyShaderWithDefaultsAsResource.GetMyCol4(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyCol4, x);

                MyShaderWithDefaultsAsResource.GetMyIntAsRange(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyIntAsRange, x);
                MyShaderWithDefaultsAsResource.GetMyFloatAsRange(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyFloatAsRange, x);

                MyShaderWithDefaultsAsResource.GetMyEnumAsInt1(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyEnumAsInt1, x);
                MyShaderWithDefaultsAsResource.GetMyEnumAsInt60(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyEnumAsInt60, x);
                MyShaderWithDefaultsAsResource.GetMyEnumAsEnum1(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyEnumAsEnum1, x);
                MyShaderWithDefaultsAsResource.GetMyEnumAsEnum60(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyEnumAsEnum60, x);

                MyShaderWithDefaultsAsResource.GetMyVec2WithScalarDefault(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyVec2WithScalarDefault, x);
                MyShaderWithDefaultsAsResource.GetMyVec3WithScalarDefault(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyVec3WithScalarDefault, x);
                MyShaderWithDefaultsAsResource.GetMyVec4WithScalarDefault(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyVec4WithScalarDefault, x);

                MyShaderWithDefaultsAsResource.GetMyIvec2WithScalarDefault(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyIvec2WithScalarDefault, x);
                MyShaderWithDefaultsAsResource.GetMyIvec3WithScalarDefault(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyIvec3WithScalarDefault, x);
                MyShaderWithDefaultsAsResource.GetMyIvec4WithScalarDefault(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyIvec4WithScalarDefault, x);

                MyShaderWithDefaultsAsResource.GetMyUvec2WithScalarDefault(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyUvec2WithScalarDefault, x);
                MyShaderWithDefaultsAsResource.GetMyUvec3WithScalarDefault(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyUvec3WithScalarDefault, x);
                MyShaderWithDefaultsAsResource.GetMyUvec4WithScalarDefault(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyUvec4WithScalarDefault, x);

                MyShaderWithDefaultsAsResource.GetMyBvec2WithScalarDefault(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyBvec2WithScalarDefault, x);
                MyShaderWithDefaultsAsResource.GetMyBvec3WithScalarDefault(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyBvec3WithScalarDefault, x);
                MyShaderWithDefaultsAsResource.GetMyBvec4WithScalarDefault(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyBvec4WithScalarDefault, x);

                MyShaderWithDefaultsAsResource.GetMyCol3WithScalarDefault(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyCol3WithScalarDefault, x);
                MyShaderWithDefaultsAsResource.GetMyCol4WithScalarDefault(mat).Should().Be(MyShaderWithDefaultsAsResource.Default.MyCol4WithScalarDefault, x);
            }

            void TestSet()
            {
                var x = TEST_NAME(nameof(TestSet));

                MyShaderWithDefaultsAsResource.SetMyInt(mat, default); MyShaderWithDefaultsAsResource.GetMyInt(mat).Should().Be(0, x);
                MyShaderWithDefaultsAsResource.SetMyUint(mat, default); MyShaderWithDefaultsAsResource.GetMyUint(mat).Should().Be(0, x);
                MyShaderWithDefaultsAsResource.SetMyBool(mat, default); MyShaderWithDefaultsAsResource.GetMyBool(mat).Should().Be(false, x);
                MyShaderWithDefaultsAsResource.SetMyFloat(mat, default); MyShaderWithDefaultsAsResource.GetMyFloat(mat).Should().Be(.0f, x);

                MyShaderWithDefaultsAsResource.SetMyMat2(mat, default); MyShaderWithDefaultsAsResource.GetMyMat2(mat).Should().Be(new Transform2D(), x);
                MyShaderWithDefaultsAsResource.SetMyMat3(mat, default); MyShaderWithDefaultsAsResource.GetMyMat3(mat).Should().Be(new Basis(), x);
                MyShaderWithDefaultsAsResource.SetMyMat4(mat, default); MyShaderWithDefaultsAsResource.GetMyMat4(mat).Should().Be(new Transform3D(), x);

                MyShaderWithDefaultsAsResource.SetMyVec2(mat, default); MyShaderWithDefaultsAsResource.GetMyVec2(mat).Should().Be(Vector2.Zero, x);
                MyShaderWithDefaultsAsResource.SetMyVec3(mat, default); MyShaderWithDefaultsAsResource.GetMyVec3(mat).Should().Be(Vector3.Zero, x);
                MyShaderWithDefaultsAsResource.SetMyVec4(mat, default); MyShaderWithDefaultsAsResource.GetMyVec4(mat).Should().Be(Vector4.Zero, x);

                MyShaderWithDefaultsAsResource.SetMyIvec2(mat, default); MyShaderWithDefaultsAsResource.GetMyIvec2(mat).Should().Be(Vector2I.Zero, x);
                MyShaderWithDefaultsAsResource.SetMyIvec3(mat, default); MyShaderWithDefaultsAsResource.GetMyIvec3(mat).Should().Be(Vector3I.Zero, x);
                MyShaderWithDefaultsAsResource.SetMyIvec4(mat, default); MyShaderWithDefaultsAsResource.GetMyIvec4(mat).Should().Be(Vector4I.Zero, x);

                MyShaderWithDefaultsAsResource.SetMyUvec2(mat, default); MyShaderWithDefaultsAsResource.GetMyUvec2(mat).Should().Be(Vector2I.Zero, x);
                MyShaderWithDefaultsAsResource.SetMyUvec3(mat, default); MyShaderWithDefaultsAsResource.GetMyUvec3(mat).Should().Be(Vector3I.Zero, x);
                MyShaderWithDefaultsAsResource.SetMyUvec4(mat, default); MyShaderWithDefaultsAsResource.GetMyUvec4(mat).Should().Be(Vector4I.Zero, x);

                MyShaderWithDefaultsAsResource.SetMyBvec2(mat, default); MyShaderWithDefaultsAsResource.GetMyBvec2(mat).Should().Be(0, x);
                MyShaderWithDefaultsAsResource.SetMyBvec3(mat, default); MyShaderWithDefaultsAsResource.GetMyBvec3(mat).Should().Be(0, x);
                MyShaderWithDefaultsAsResource.SetMyBvec4(mat, default); MyShaderWithDefaultsAsResource.GetMyBvec4(mat).Should().Be(0, x);

                MyShaderWithDefaultsAsResource.SetMyCol3(mat, default); MyShaderWithDefaultsAsResource.GetMyCol3(mat).Should().Be(new Color(), x);
                MyShaderWithDefaultsAsResource.SetMyCol4(mat, default); MyShaderWithDefaultsAsResource.GetMyCol4(mat).Should().Be(new Color(), x);

                MyShaderWithDefaultsAsResource.SetMyIntAsRange(mat, default); MyShaderWithDefaultsAsResource.GetMyIntAsRange(mat).Should().Be(0, x);
                MyShaderWithDefaultsAsResource.SetMyFloatAsRange(mat, default); MyShaderWithDefaultsAsResource.GetMyFloatAsRange(mat).Should().Be(.0f, x);

                MyShaderWithDefaultsAsResource.SetMyEnumAsInt1(mat, default); MyShaderWithDefaultsAsResource.GetMyEnumAsInt1(mat).Should().Be(0, x);
                MyShaderWithDefaultsAsResource.SetMyEnumAsInt60(mat, default); MyShaderWithDefaultsAsResource.GetMyEnumAsInt60(mat).Should().Be(0, x);
                MyShaderWithDefaultsAsResource.SetMyEnumAsEnum1(mat, default); MyShaderWithDefaultsAsResource.GetMyEnumAsEnum1(mat).Should().Be(0, x);
                MyShaderWithDefaultsAsResource.SetMyEnumAsEnum60(mat, default); MyShaderWithDefaultsAsResource.GetMyEnumAsEnum60(mat).Should().Be(0, x);

                MyShaderWithDefaultsAsResource.SetMyVec2WithScalarDefault(mat, default); MyShaderWithDefaultsAsResource.GetMyVec2WithScalarDefault(mat).Should().Be(Vector2.Zero, x);
                MyShaderWithDefaultsAsResource.SetMyVec3WithScalarDefault(mat, default); MyShaderWithDefaultsAsResource.GetMyVec3WithScalarDefault(mat).Should().Be(Vector3.Zero, x);
                MyShaderWithDefaultsAsResource.SetMyVec4WithScalarDefault(mat, default); MyShaderWithDefaultsAsResource.GetMyVec4WithScalarDefault(mat).Should().Be(Vector4.Zero, x);

                MyShaderWithDefaultsAsResource.SetMyIvec2WithScalarDefault(mat, default); MyShaderWithDefaultsAsResource.GetMyIvec2WithScalarDefault(mat).Should().Be(Vector2I.Zero, x);
                MyShaderWithDefaultsAsResource.SetMyIvec3WithScalarDefault(mat, default); MyShaderWithDefaultsAsResource.GetMyIvec3WithScalarDefault(mat).Should().Be(Vector3I.Zero, x);
                MyShaderWithDefaultsAsResource.SetMyIvec4WithScalarDefault(mat, default); MyShaderWithDefaultsAsResource.GetMyIvec4WithScalarDefault(mat).Should().Be(Vector4I.Zero, x);

                MyShaderWithDefaultsAsResource.SetMyUvec2WithScalarDefault(mat, default); MyShaderWithDefaultsAsResource.GetMyUvec2WithScalarDefault(mat).Should().Be(Vector2I.Zero, x);
                MyShaderWithDefaultsAsResource.SetMyUvec3WithScalarDefault(mat, default); MyShaderWithDefaultsAsResource.GetMyUvec3WithScalarDefault(mat).Should().Be(Vector3I.Zero, x);
                MyShaderWithDefaultsAsResource.SetMyUvec4WithScalarDefault(mat, default); MyShaderWithDefaultsAsResource.GetMyUvec4WithScalarDefault(mat).Should().Be(Vector4I.Zero, x);

                MyShaderWithDefaultsAsResource.SetMyBvec2WithScalarDefault(mat, default); MyShaderWithDefaultsAsResource.GetMyBvec2WithScalarDefault(mat).Should().Be(0, x);
                MyShaderWithDefaultsAsResource.SetMyBvec3WithScalarDefault(mat, default); MyShaderWithDefaultsAsResource.GetMyBvec3WithScalarDefault(mat).Should().Be(0, x);
                MyShaderWithDefaultsAsResource.SetMyBvec4WithScalarDefault(mat, default); MyShaderWithDefaultsAsResource.GetMyBvec4WithScalarDefault(mat).Should().Be(0, x);

                MyShaderWithDefaultsAsResource.SetMyCol3WithScalarDefault(mat, default); MyShaderWithDefaultsAsResource.GetMyCol3WithScalarDefault(mat).Should().Be(new Color(), x);
                MyShaderWithDefaultsAsResource.SetMyCol4WithScalarDefault(mat, default); MyShaderWithDefaultsAsResource.GetMyCol4WithScalarDefault(mat).Should().Be(new Color(), x);
            }

            void TestReset()
            {
                var x = TEST_NAME(nameof(TestReset));

                MyShaderWithDefaultsAsResource.ResetMyInt(mat);
                MyShaderWithDefaultsAsResource.ResetMyUint(mat);
                MyShaderWithDefaultsAsResource.ResetMyBool(mat);
                MyShaderWithDefaultsAsResource.ResetMyFloat(mat);

                MyShaderWithDefaultsAsResource.ResetMyMat2(mat);
                MyShaderWithDefaultsAsResource.ResetMyMat3(mat);
                MyShaderWithDefaultsAsResource.ResetMyMat4(mat);

                MyShaderWithDefaultsAsResource.ResetMyVec2(mat);
                MyShaderWithDefaultsAsResource.ResetMyVec3(mat);
                MyShaderWithDefaultsAsResource.ResetMyVec4(mat);

                MyShaderWithDefaultsAsResource.ResetMyIvec2(mat);
                MyShaderWithDefaultsAsResource.ResetMyIvec3(mat);
                MyShaderWithDefaultsAsResource.ResetMyIvec4(mat);

                MyShaderWithDefaultsAsResource.ResetMyUvec2(mat);
                MyShaderWithDefaultsAsResource.ResetMyUvec3(mat);
                MyShaderWithDefaultsAsResource.ResetMyUvec4(mat);

                MyShaderWithDefaultsAsResource.ResetMyBvec2(mat);
                MyShaderWithDefaultsAsResource.ResetMyBvec3(mat);
                MyShaderWithDefaultsAsResource.ResetMyBvec4(mat);

                MyShaderWithDefaultsAsResource.ResetMyCol3(mat);
                MyShaderWithDefaultsAsResource.ResetMyCol4(mat);

                MyShaderWithDefaultsAsResource.ResetMyIntAsRange(mat);
                MyShaderWithDefaultsAsResource.ResetMyFloatAsRange(mat);

                MyShaderWithDefaultsAsResource.ResetMyEnumAsInt1(mat);
                MyShaderWithDefaultsAsResource.ResetMyEnumAsInt60(mat);
                MyShaderWithDefaultsAsResource.ResetMyEnumAsEnum1(mat);
                MyShaderWithDefaultsAsResource.ResetMyEnumAsEnum60(mat);

                MyShaderWithDefaultsAsResource.ResetMyVec2WithScalarDefault(mat);
                MyShaderWithDefaultsAsResource.ResetMyVec3WithScalarDefault(mat);
                MyShaderWithDefaultsAsResource.ResetMyVec4WithScalarDefault(mat);

                MyShaderWithDefaultsAsResource.ResetMyIvec2WithScalarDefault(mat);
                MyShaderWithDefaultsAsResource.ResetMyIvec3WithScalarDefault(mat);
                MyShaderWithDefaultsAsResource.ResetMyIvec4WithScalarDefault(mat);

                MyShaderWithDefaultsAsResource.ResetMyUvec2WithScalarDefault(mat);
                MyShaderWithDefaultsAsResource.ResetMyUvec3WithScalarDefault(mat);
                MyShaderWithDefaultsAsResource.ResetMyUvec4WithScalarDefault(mat);

                MyShaderWithDefaultsAsResource.ResetMyBvec2WithScalarDefault(mat);
                MyShaderWithDefaultsAsResource.ResetMyBvec3WithScalarDefault(mat);
                MyShaderWithDefaultsAsResource.ResetMyBvec4WithScalarDefault(mat);

                MyShaderWithDefaultsAsResource.ResetMyCol3WithScalarDefault(mat);
                MyShaderWithDefaultsAsResource.ResetMyCol4WithScalarDefault(mat);

                TestDefaultGet(x);
            }
        }

        static ShaderMaterial RAW(string path)
            => new() { Shader = GD.Load<Shader>(path) };

        static ShaderMaterial RES(Type t)
            => GD.Load<ShaderMaterial>(Path.ChangeExtension(t.GetCustomAttribute<ScriptPathAttribute>().Path, "tres"));

        static string TEST_NAME(string x)
            => $"TEST '{x}'";

        static IEnumerable<string> NestedTypes(Type t)
            => t.GetNestedTypes(PublicStatic).Select(x => x.Name).Except(NestedGodotTypes);

        static IEnumerable<string> DeclaredFields(Type t)
            => t.GetFields(PublicStatic).Select(x => x.Name);

        static IEnumerable<string> DeclaredMethods(Type t)
        {
            var x = t.GetMethods(PublicStatic).Select(x => x.Name).ToArray();
            x.First().Should().Be("get_ResPath");
            return x.Skip(1);
        }
    }
}
