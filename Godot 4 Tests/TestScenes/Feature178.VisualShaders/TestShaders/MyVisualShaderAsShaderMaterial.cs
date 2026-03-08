using Godot;

namespace GodotTests.TestScenes.SUT_ShaderAttribute;

[Shader(nameof(MyVisualShader), generate_tests: true)]
public partial class MyVisualShaderAsShaderMaterial : ShaderMaterial;
