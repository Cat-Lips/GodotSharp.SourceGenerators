shader_type spatial;

uniform int my_int = 1;
uniform uint my_uint = 1u;
uniform bool my_bool = true;
uniform float my_float = 0.1;

uniform mat2 my_mat2 = mat2(1.0);
uniform mat3 my_mat3 = mat3(1.0);
uniform mat4 my_mat4 = mat4(1.0);

uniform vec2 my_vec2 = vec2(1.0, 1.0);
uniform vec3 my_vec3 = vec3(1.0, 1.0, 1.0);
uniform vec4 my_vec4 = vec4(1.0, 1.0, 1.0, 1.0);

uniform ivec2 my_ivec2 = ivec2(1, 1);
uniform ivec3 my_ivec3 = ivec3(1, 1, 1);
uniform ivec4 my_ivec4 = ivec4(1, 1, 1, 1);

uniform uvec2 my_uvec2 = uvec2(1u, 1u);
uniform uvec3 my_uvec3 = uvec3(1u, 1u, 1u);
uniform uvec4 my_uvec4 = uvec4(1u, 1u, 1u, 1u);

uniform bvec2 my_bvec2 = bvec2(true, true);
uniform bvec3 my_bvec3 = bvec3(true, true, true);
uniform bvec4 my_bvec4 = bvec4(true, true, true, true);

uniform vec3 my_col3 : source_color = vec3(1.0, 1.0, 1.0);
uniform vec4 my_col4 : source_color = vec4(1.0, 1.0, 1.0, 1.0);

uniform int my_int_as_range : hint_range(0, 10) = 1;
uniform float my_float_as_range : hint_range(0.0, 1.0) = 0.1;

uniform int my_enum_as_int1 : hint_enum("Slow:30", "Average:60", "Very Fast:200") = 1;
uniform int my_enum_as_int60 : hint_enum("Slow:30", "Average:60", "Very Fast:200") = 60; // InvalidEnum
uniform int my_enum_as_enum1 : hint_enum("Slow:30", "Average:60", "Very Fast:200") = 1; // MyShaderEnum
uniform int my_enum_as_enum60 : hint_enum("Slow:30", "Average:60", "Very Fast:200") = 60; // MyShaderEnum

uniform vec2 my_vec2_WithScalarDefault = vec2(1.0);
uniform vec3 my_vec3_WithScalarDefault = vec3(1.0);
uniform vec4 my_vec4_WithScalarDefault = vec4(1.0);

uniform ivec2 my_ivec2_WithScalarDefault = ivec2(1);
uniform ivec3 my_ivec3_WithScalarDefault = ivec3(1);
uniform ivec4 my_ivec4_WithScalarDefault = ivec4(1);

uniform uvec2 my_uvec2_WithScalarDefault = uvec2(1u);
uniform uvec3 my_uvec3_WithScalarDefault = uvec3(1u);
uniform uvec4 my_uvec4_WithScalarDefault = uvec4(1u);

uniform bvec2 my_bvec2_WithScalarDefault = bvec2(true);
uniform bvec3 my_bvec3_WithScalarDefault = bvec3(true);
uniform bvec4 my_bvec4_WithScalarDefault = bvec4(true);

uniform vec3 my_col3_WithScalarDefault : source_color = vec3(1.0);
uniform vec4 my_col4_WithScalarDefault : source_color = vec4(1.0);

void vertex() {
}

void fragment() {
}
