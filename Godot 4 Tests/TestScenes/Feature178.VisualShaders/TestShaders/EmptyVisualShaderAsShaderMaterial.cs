using Godot;

namespace GodotTests.TestScenes.SUT_ShaderAttribute;

[Shader(nameof(EmptyVisualShader), generate_tests: true)]
public partial class EmptyVisualShaderAsShaderMaterial : ShaderMaterial;
