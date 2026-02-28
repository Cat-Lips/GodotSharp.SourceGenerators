using Godot;

namespace GodotTests.TestScenes.SUT_ShaderAttribute;

[Shader(generate_tests: true)]
public partial class MyVisualShader;

[Shader(generate_tests: true)]
public static partial class MyVisualShaderAsStatic;

[Shader(generate_tests: true)]
public partial class MyVisualShaderAsResource : Resource;

// NO TEST //

[Shader]
public partial class MyVisualShader_NO_TEST;

[Shader]
public static partial class MyVisualShaderAsStatic_NO_TEST;

[Shader]
public partial class MyVisualShaderAsResource_NO_TEST : Resource;

[Shader]
public partial class MyVisualShaderAsShaderMaterial_NO_TEST : ShaderMaterial;
