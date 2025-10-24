using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators;

internal static class MiniCsScraper
{
    public static string GetType(Compilation compilation, string file)
    {
        Log.Debug($">>> GetType {file}");
        var name = Path.GetFileNameWithoutExtension(file);
        var type = compilation.GetFullName(name, file);
        Log.Debug($"<<< {type ?? "<null>"}");
        return type;
    }
}
