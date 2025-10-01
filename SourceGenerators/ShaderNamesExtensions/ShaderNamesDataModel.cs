using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.ShaderNamesExtensions;

internal class ShaderNamesDataModel : ClassDataModel
{
    public record ShaderName(string SafeName, string RawName, string Type, string Default, bool IsEnum);

    public readonly string ResPath;
    public readonly ShaderName[] ShaderNames;

    public ShaderNamesDataModel(Compilation compilation, INamedTypeSymbol symbol, string shader) : base(symbol)
    {
        ResPath = GD.GetResourcePath(shader);
        ShaderNames = [.. Convert(ShaderScraper.Uniforms(shader))];

        IEnumerable<ShaderName> Convert(IEnumerable<ShaderUniform> source)
        {
            foreach (var x in source)
            {
                var csType = GetType(x, out var isEnum, out var isColor);
                var csDflt = GetDefault(x, csType, isEnum, isColor);
                yield return new(x.Name.ToPascalCase(), x.Name, csType, csDflt, isEnum);
            }

            string GetType(ShaderUniform x, out bool isEnum, out bool isColor)
            {
                isEnum = false;
                isColor = false;
                return x.Type switch
                {
                    "int" => TryGetEnum(ref isEnum) ?? "int",
                    "uint" => "int",
                    "bool" => "bool",
                    "float" => "float",

                    "mat2" => "Transform2D",
                    "mat3" => "Basis",
                    "mat4" => "Transform3D", // TODO: Projection?

                    "vec2" => "Vector2",
                    "vec3" => IsColor(ref isColor) ? "Color" : "Vector3",
                    "vec4" => IsColor(ref isColor) ? "Color" : "Vector4", // TODO: Rect2, Plane, Quaternion?

                    "ivec2" => "Vector2I",
                    "ivec3" => "Vector3I",
                    "ivec4" => "Vector4I",

                    "uvec2" => "Vector2I",
                    "uvec3" => "Vector3I",
                    "uvec4" => "Vector4I",

                    "bvec2" => "int",
                    "bvec3" => "int",
                    "bvec4" => "int",

                    "sampler2D" => "Texture2D",
                    "isampler2D" => "Texture2D",
                    "usampler2D" => "Texture2D",

                    "sampler2DArray" => "Texture2DArray",
                    "isampler2DArray" => "Texture2DArray",
                    "usampler2DArray" => "Texture2DArray",

                    "sampler3D" => "Texture3D",
                    "isampler3D" => "Texture3D",
                    "usampler3D" => "Texture3D",

                    "samplerCube" => "Cubemap",
                    "samplerCubeArray" => "CubemapArray",
                    "samplerExternalOES" => "ExternalTexture",

                    _ => "Variant", // unknown type!
                };

                bool IsColor(ref bool isColor)
                    => isColor = x.Hint is "source_color";

                string TryGetEnum(ref bool isEnum)
                {
                    var enumType = TryGetEnum();
                    isEnum = enumType is not null;
                    return enumType;

                    string TryGetEnum()
                    {
                        return x.Comment is not null && x.Hint?.StartsWith("hint_enum") is true
                            ? compilation.TryGetEnum(x.Comment) : null;
                    }
                }
            }

            string GetDefault(ShaderUniform x, string csType, bool isEnum, bool isColor)
            {
                return x.Default is null ? GetIdentity() : GetDefault();

                string GetIdentity() => x.Type switch
                {
                    // Shader will set matrices to Identity if no default provided
                    "mat2" => "Transform2D.Identity",
                    "mat3" => "Basis.Identity",
                    "mat4" => "Transform3D.Identity",

                    // Shader will set colors to 0,0,0,1 if no default provided
                    "vec3" => isColor ? "Colors.Black" : null,
                    "vec4" => isColor ? "Colors.Black" : null,

                    _ => null, // no default
                };

                string GetDefault() => x.Type switch
                {
                    "int" => isEnum ? FormatEnum() : x.Default,
                    "uint" => x.Default.TrimEnd('u'),
                    "bool" => x.Default,
                    "float" => $"{x.Default}f",

                    // FIXME:  Too many ways to initialise these - skip for now
                    "mat2" => "Transform2D.Identity",
                    "mat3" => "Basis.Identity",
                    "mat4" => "Transform3D.Identity",

                    "vec2" => FormatFloatCtor(2),
                    "vec3" => FormatFloatCtor(3),
                    "vec4" => FormatFloatCtor(4),

                    "ivec2" => FormatCtor(2),
                    "ivec3" => FormatCtor(3),
                    "ivec4" => FormatCtor(4),

                    "uvec2" => FormatIntCtor(2),
                    "uvec3" => FormatIntCtor(3),
                    "uvec4" => FormatIntCtor(4),

                    "bvec2" => FormatMask(2),
                    "bvec3" => FormatMask(3),
                    "bvec4" => FormatMask(4),

                    _ => null, // unknown type!
                };

                string FormatEnum() => compilation.TryGetEnumValue(csType, x.Default);
                string FormatIntCtor(int count) => FormatCtor(count, x => x.TrimEnd('u'));
                string FormatFloatCtor(int count) => FormatCtor(count, x => $"{x}f");

                string FormatCtor(int count, Func<string, string> transform = null)
                {
                    return x.Args is null ? x.Default :
                        $"new {csType}({FormatArgs(count, transform ?? (arg => arg))})";
                }

                string FormatMask(int count)
                {
                    return x.Args is null ? x.Default : $"0b{FormatArgs(count, Transform, sep: "")}";

                    string Transform(string arg) => arg switch
                    {
                        "true" => "1",
                        "false" => "0",
                        _ => null,
                    };
                }

                string FormatArgs(int count, Func<string, string> transform, string sep = ", ")
                {
                    var args = x.Args.Split(',').Select(a => transform(a.Trim())).ToArray();
                    if (args.Length is 1) args = [.. Enumerable.Repeat(args.Single(), count)];
                    return string.Join(sep, args);
                }
            }
        }
    }

    protected override string Str()
    {
        return string.Join("\n", Parts());

        IEnumerable<string> Parts()
        {
            yield return $"{nameof(ResPath)}: {ResPath}";
            yield return $"{nameof(ShaderNames)} ({ShaderNames.Length}):";
            foreach (var name in ShaderNames) yield return $" - {name}";
        }
    }
}
