namespace GodotSharp.SourceGenerators.InputMapExtensions
{
    internal static class Diagnostics
    {
        public static DiagnosticDetail GodotAttributeNotFound(string tscn) => new() { Title = "Scene file not found", Message = $"Could not find scene file: {tscn}" };
    }
}
