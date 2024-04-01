using Microsoft.CodeAnalysis.Diagnostics;

namespace GodotSharp.SourceGenerators
{
    public static class GeneratorExtensions
    {
        public static string TryGetGodotProjectDir(this AnalyzerConfigOptions options)
        {
            const string GodotProjectDir = "build_property.GodotProjectDir";
            const string GodotProjectDirBase64 = "build_property.GodotProjectDirBase64";

            return options.TryGetValue(GodotProjectDir, out var value) ? value.TrimEnd('\\')
                : options.TryGetValue(GodotProjectDirBase64, out value) ? value.TrimEnd('\\') : null;
        }
    }
}
