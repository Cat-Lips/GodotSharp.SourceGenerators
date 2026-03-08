using Godot;

namespace GodotTests.TestScenes.SUT_ShaderAttribute;

[Shader(generate_tests: true)]
public partial class MyShaderWithDefaults;

[Shader(generate_tests: true)]
public static partial class MyShaderWithDefaultsAsStatic;

[Shader(generate_tests: true)]
public partial class MyShaderWithDefaultsAsResource : Resource;

// NO TEST //

[Shader]
public partial class MyShaderWithDefaults_NO_TEST;

[Shader]
public static partial class MyShaderWithDefaultsAsStatic_NO_TEST;

[Shader]
public partial class MyShaderWithDefaultsAsResource_NO_TEST : Resource;

[Shader]
public partial class MyShaderWithDefaultsAsShaderMaterial_NO_TEST : ShaderMaterial;
