using Godot;

namespace GodotTests.TestScenes.SUT_ShaderAttribute;

[Shader(generate_tests: true)]
public partial class EmptyShader;

[Shader(generate_tests: true)]
public static partial class EmptyShaderAsStatic;

[Shader(generate_tests: true)]
public partial class EmptyShaderAsResource : Resource;

// NO TEST //

[Shader]
public partial class EmptyShader_NO_TEST;

[Shader]
public static partial class EmptyShaderAsStatic_NO_TEST;

[Shader]
public partial class EmptyShaderAsResource_NO_TEST : Resource;

[Shader]
public partial class EmptyShaderAsShaderMaterial_NO_TEST : ShaderMaterial;
