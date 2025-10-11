namespace GodotSharp.SourceGenerators;

internal static class Diagnostics
{
    public static DiagnosticDetail FileNotFound(string path, string msg = null) => new() { Title = "File not found", Message = msg ?? $"Could not find file: {path}" };
}
