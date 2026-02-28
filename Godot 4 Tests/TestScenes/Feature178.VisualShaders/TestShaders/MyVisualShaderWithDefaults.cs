using Godot;

namespace GodotTests.TestScenes.SUT_ShaderAttribute;

[Shader(generate_tests: true)]
public partial class MyVisualShaderWithDefaults;

[Shader(generate_tests: true)]
public static partial class MyVisualShaderWithDefaultsAsStatic;

[Shader(generate_tests: true)]
public partial class MyVisualShaderWithDefaultsAsResource : Resource;

// NO TEST //

[Shader]
public partial class MyVisualShaderWithDefaults_NO_TEST;

[Shader]
public static partial class MyVisualShaderWithDefaultsAsStatic_NO_TEST;

[Shader]
public partial class MyVisualShaderWithDefaultsAsResource_NO_TEST : Resource;

[Shader]
public partial class MyVisualShaderWithDefaultsAsShaderMaterial_NO_TEST : ShaderMaterial;
