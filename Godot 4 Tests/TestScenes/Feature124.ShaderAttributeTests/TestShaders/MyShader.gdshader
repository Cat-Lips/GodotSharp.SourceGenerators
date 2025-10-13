shader_type spatial;

uniform int my_int;
uniform uint my_uint;
uniform bool my_bool;
uniform float my_float;

//uniform int my_int_array[1];
//uniform uint my_uint_array[2];
//uniform bool my_bool_array[3];
//uniform float my_float_array[4];

uniform mat2 my_mat2;
uniform mat3 my_mat3;
uniform mat4 my_mat4; // TODO: Projection?

uniform vec2 my_vec2;
uniform vec3 my_vec3;
uniform vec4 my_vec4;

uniform ivec2 my_ivec2;
uniform ivec3 my_ivec3;
uniform ivec4 my_ivec4;

uniform uvec2 my_uvec2;
uniform uvec3 my_uvec3;
uniform uvec4 my_uvec4;

uniform bvec2 my_bvec2;
uniform bvec3 my_bvec3;
uniform bvec4 my_bvec4; // TODO: Rect2, Plane, Quaternion?

uniform vec3 my_col3 : source_color;
uniform vec4 my_col4 : source_color;

uniform int my_int_as_range : hint_range(0, 10);
uniform float my_float_as_range : hint_range(0.0, 1.0);

uniform int my_enum_as_int1 : hint_enum("Slow:30", "Average:60", "Very Fast:200");
uniform int my_enum_as_int2 : hint_enum("Slow:30", "Average:60", "Very Fast:200"); // InvalidEnum
uniform int my_enum_as_enum : hint_enum("Slow:30", "Average:60", "Very Fast:200"); // MyShaderEnum

uniform sampler2D my_sampler2D;
uniform isampler2D my_isampler2D;
uniform usampler2D my_usampler2D;

uniform sampler3D my_sampler3D;
uniform isampler3D my_isampler3D;
uniform usampler3D my_usampler3D;

uniform sampler2DArray my_sampler2DArray;
uniform isampler2DArray my_isampler2DArray;
uniform usampler2DArray my_usampler2DArray;

uniform samplerCube my_samplerCube;
uniform samplerCubeArray my_samplerCubeArray;
//uniform samplerExternalOES my_samplerExternalOES;

void vertex() {
}

void fragment() {
}
