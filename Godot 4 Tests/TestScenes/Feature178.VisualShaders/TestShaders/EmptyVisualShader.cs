using Godot;

namespace GodotTests.TestScenes.SUT_ShaderAttribute;

[Shader(generate_tests: true)]
public partial class EmptyVisualShader;

[Shader(generate_tests: true)]
public static partial class EmptyVisualShaderAsStatic;

[Shader(generate_tests: true)]
public partial class EmptyVisualShaderAsResource : Resource;

// NO TEST //

[Shader]
public partial class EmptyVisualShader_NO_TEST;

[Shader]
public static partial class EmptyVisualShaderAsStatic_NO_TEST;

[Shader]
public partial class EmptyVisualShaderAsResource_NO_TEST : Resource;

[Shader]
public partial class EmptyVisualShaderAsShaderMaterial_NO_TEST : ShaderMaterial;
