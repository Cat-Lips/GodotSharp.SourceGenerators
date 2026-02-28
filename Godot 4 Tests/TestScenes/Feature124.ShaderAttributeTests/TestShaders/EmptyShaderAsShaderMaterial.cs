using Godot;

namespace GodotTests.TestScenes.SUT_ShaderAttribute;

[Shader(nameof(EmptyShader), generate_tests: true)]
public partial class EmptyShaderAsShaderMaterial : ShaderMaterial;
