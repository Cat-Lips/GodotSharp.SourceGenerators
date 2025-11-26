using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.ShaderGlobalsExtensions;

internal class ShaderGlobalsDataModel(INamedTypeSymbol symbol, string gdRoot) : ClassDataModel(symbol)
{
    public record ShaderGlobal(string Name, string Type, string Default, string RawName);

    public IList<ShaderGlobal> ShaderGlobals { get; } = [..
        ShaderGlobalsScraper
            .GetShaderGlobals(gdRoot)
            .Select(Convert)];

    protected override string Str()
        => string.Join("\n", ShaderGlobals);

    #region Convert

    private static ShaderGlobal Convert(ShaderGlobalsScraper.ShaderGlobal raw)
    {
        var csType = ConvertType(raw.Type);
        var csValue = ConvertValue(raw.Default) ?? "default";
        return new(raw.Name.ToPascalCase(), csType, csValue, raw.Name);

        string ConvertType(string type) => type switch
        {
            "bvec2" => "int",
            "bvec3" => "int",
            "bvec4" => "int",

            "ivec2" => "Vector2I",
            "ivec3" => "Vector3I",
            "ivec4" => "Vector4I",

            "uvec2" => "Vector2I",
            "uvec3" => "Vector3I",
            "uvec4" => "Vector4I",

            "vec2" => "Vector2",
            "vec3" => "Vector3",
            "vec4" => "Vector4",

            "color" => "Color",

            "rect2" => "Rect2",
            "rect2i" => "Rect2I",

            "mat2" => "Vector4",
            "mat3" => "Basis",
            "mat4" => "Projection",

            "transform_2d" => "Transform2D",
            "transform" => "Transform3D",

            "sampler2D" => "Texture2D",
            "sampler2DArray" => "Texture2DArray",
            "sampler3D" => "Texture3D",
            "samplerCube" => "Cubemap",
            "samplerExternalOES" => "ExternalTexture",

            _ => type,
        };

        string ConvertValue(string v)
        {
            return v is null ? null : TryAsRes() ?? TryAsCtor() ?? SafeValue(v);

            string TryAsRes()
                => v.StartsWith("res://") ? @$"GD.Load<{csType}>(""{v}"")" : null;

            string TryAsCtor()
            {
                if (v.EndsWith(")"))
                {
                    var args = v.Split('(').Last().TrimEnd(')').Split(',').Select(SafeValue);
                    return $"new {csType}({string.Join(",", args)})";
                }

                return null;
            }

            static string SafeValue(string v)
                => v.Contains('.') ? $"{v}f" : v;
        }
    }

    #endregion
}
