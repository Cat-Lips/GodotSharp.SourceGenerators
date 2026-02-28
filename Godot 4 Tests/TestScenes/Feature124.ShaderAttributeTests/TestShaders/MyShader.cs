using Godot;

namespace GodotTests.TestScenes.SUT_ShaderAttribute;

[Shader(generate_tests: true)]
public partial class MyShader;

[Shader(generate_tests: true)]
public static partial class MyShaderAsStatic;

[Shader(generate_tests: true)]
public partial class MyShaderAsResource : Resource;

// NO TEST //

[Shader]
public partial class MyShader_NO_TEST;

[Shader]
public static partial class MyShaderAsStatic_NO_TEST;

[Shader]
public partial class MyShaderAsResource_NO_TEST : Resource;

[Shader]
public partial class MyShaderAsShaderMaterial_NO_TEST : ShaderMaterial;
