using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.ShaderGlobalsExtensions;

internal class ShaderGlobalsDataModel : ClassDataModel
{
    public abstract record ShaderGlobalDefault
    {
        public abstract string GetDefault(string type);
    }

    public record ShaderGlobalResourceDefault(string ResourcePath) : ShaderGlobalDefault
    {
        private static readonly Regex ResourcePathRegex = new(@"^""res://.+""$");
        public static bool TryParse(string @default, out ShaderGlobalDefault outDefault)
        {
            if (ResourcePathRegex.IsMatch(@default))
            {
                outDefault = new ShaderGlobalResourceDefault(@default);
                return true;
            }

            outDefault = null;
            return false;
        }

        public override string GetDefault(string type) => $"GD.Load<{type}>({ResourcePath})";
    }

    public record ShaderGlobalConstructorDefault(string Parameters) : ShaderGlobalDefault
    {
        private static readonly Regex ConstructorRegex = new(@"^.+\((?<Parameters>.+)\)$");
        public static bool TryParse(string @default, out ShaderGlobalDefault outDefault)
        {
            var match = ConstructorRegex.Match(@default);
            if (match.Success)
            {
                outDefault = new ShaderGlobalConstructorDefault(match.Groups["Parameters"].Value);
                return true;
            }

            outDefault = null;
            return false;
        }

        public override string GetDefault(string type) => $"new {type}({string.Join(",", Parameters
            .Split(',')
            .Select(ConvertFloatingLiteral))})";
    }

    public record ShaderGlobalLiteralDefault(string Literal) : ShaderGlobalDefault
    {
        public override string GetDefault(string type) => ConvertFloatingLiteral(Literal);
    }

    // We need to append 'f' to float literals.
    private static string ConvertFloatingLiteral(string literal) => literal.Contains('.') ? literal + "f" : literal;

    public record ShaderGlobal(string Type, string Name, string Default, string RawName);

    public IList<ShaderGlobal> ShaderGlobals { get; }

    public ShaderGlobalsDataModel(INamedTypeSymbol symbol, string csPath, string gdRoot) : base(symbol)
    {
        ShaderGlobals = ShaderGlobalsScraper
            .GetShaderGlobals(csPath, gdRoot)
            .Select(Convert)
            .ToArray();

        static ShaderGlobal Convert(ShaderGlobalsScraper.ShaderGlobal global)
        {
            var csType = ConvertType(global.Type);
            return new(csType, global.Name.ToPascalCase(), ConvertDefault(global.Default)?.GetDefault(csType) ?? "default", global.Name);
        }

        static ShaderGlobalDefault ConvertDefault(string @default)
            => @default is null or "" or "\"\""
                ? null
                : ShaderGlobalResourceDefault.TryParse(@default, out var outDefault)
                    ? outDefault
                    : ShaderGlobalConstructorDefault.TryParse(@default, out outDefault)
                        ? outDefault
                        : new ShaderGlobalLiteralDefault(@default);

        static string ConvertType(string type)
            => type switch
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
    }

    protected override string Str()
    {
        return string.Join("\n", ShaderGlobals());

        IEnumerable<string> ShaderGlobals()
        {
            foreach (var (type, name, @default, rawType) in this.ShaderGlobals)
                yield return $"Type: {type}, Name: {name}, Default: {@default}, RawType: {rawType}";
        }
    }
}
