using Godot;

namespace GodotTests.TestScenes.SUT_ShaderAttribute;

[Shader(nameof(MyShader), generate_tests: true)]
public partial class MyShaderAsShaderMaterial : ShaderMaterial;
