using Godot;

namespace GodotTests.TestScenes.SUT_ShaderAttribute;

[Shader(nameof(MyShaderWithDefaults), generate_tests: true)]
public partial class MyShaderWithDefaultsAsShaderMaterial : ShaderMaterial;
