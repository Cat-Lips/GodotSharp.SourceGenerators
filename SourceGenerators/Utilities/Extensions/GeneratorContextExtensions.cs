//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.Diagnostics;
//using GeneratorContext = Microsoft.CodeAnalysis.IncrementalGeneratorInitializationContext;

//namespace GodotSharp.SourceGenerators
//{
//    public static class GeneratorContextExtensions
//    {
//        // CompilerVisibleProperty: GodotProjectDir & GodotProjectDirBase64
//        private const string GodotProjectDir = "GodotProjectDir";
//        private const string GodotProjectDirBase64 = "GodotProjectDirBase64";

//        public static string GetGodotProjectDir(this GeneratorContext context)
//            => context.GetGlobalOption(GodotProjectDir, GodotProjectDirBase64);
//        // ?? throw new InvalidOperationException("Property 'GodotProjectDir' is null or empty");

//        private static string GetGlobalOption(this GeneratorContext context, params string[] aliases)
//        {
//            var options = context.GlobalOptions();
//            if (options is null) return null;

//            foreach (var name in aliases)
//            {
//                if (options.TryGetValue($"build_property.{name}", out var value) && !string.IsNullOrWhiteSpace(value))
//                    return value;
//            }

//            return null;
//        }

//        private static AnalyzerConfigOptions GlobalOptions(this GeneratorContext context)
//        {
//            AnalyzerConfigOptions ret = null;
//            context.AnalyzerConfigOptionsProvider.Select((x, ct) => ret = x.GlobalOptions);
//            return ret;
//        }
//    }
//}
