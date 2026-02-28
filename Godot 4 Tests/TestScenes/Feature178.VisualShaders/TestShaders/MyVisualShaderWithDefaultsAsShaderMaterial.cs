using Godot;

namespace GodotTests.TestScenes.SUT_ShaderAttribute;

[Shader(nameof(MyVisualShaderWithDefaults), generate_tests: true)]
public partial class MyVisualShaderWithDefaultsAsShaderMaterial : ShaderMaterial;
