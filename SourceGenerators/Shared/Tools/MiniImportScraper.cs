using System.Text.RegularExpressions;

namespace GodotSharp.SourceGenerators;

internal static class MiniImportScraper
{
    private const string TypeRegexStr = @"^type=""(?<Type>.+)""";
    private static readonly Regex TypeRegex = new(TypeRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    private const string FilesRegexStr = @"^files=\[""(?<Files>.+)""\]";
    private static readonly Regex FilesRegex = new(FilesRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    private const string SplitFilesRegexStr = @""", """;
    private static readonly Regex SplitFilesRegex = new(SplitFilesRegexStr, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    public static string GetType(string importFile, out string[] exports)
    {
        importFile = importFile.AddSuffix(".import");

        Log.Debug($">>> GetType {importFile}");
        ParseFile(importFile, out var type, out exports);
        Log.Debug($"<<< {type ?? "<null>"}{exports?.Format("|", x => $" [Exports: {x}]")}");

        return type;

        static void ParseFile(string file, out string type, out string[] exports)
        {
            const string REMAP = "[remap]";
            const string DEPS = "[deps]";
            const string PARAMS = "[params]";

            type = null;
            exports = null;
            string section = null;
            foreach (var line in File.ReadLines(file))
            {
                Log.Debug($"Line: {line}");

                switch (section)
                {
                    case null:
                        if (line is REMAP) section = REMAP;
                        continue;
                    case REMAP:
                        if (line is DEPS) section = DEPS;
                        else if (type is null) TryMatchType(line, ref type);
                        continue;
                    case DEPS:
                        if (line is PARAMS) return;
                        else if (exports is null) TryMatchExports(line, ref exports, file);
                        continue;
                }
            }

            static void TryMatchType(string line, ref string type)
            {
                var match = TypeRegex.Match(line);
                if (match.Success)
                {
                    Log.Debug($" - TypeRegex {TypeRegex.GetGroupsAsStr(match)}");
                    type = match.Groups["Type"].Value;
                }
            }

            static void TryMatchExports(string line, ref string[] exports, string file)
            {
                var match = FilesRegex.Match(line);
                if (match.Success)
                {
                    Log.Debug($" - FilesRegex {FilesRegex.GetGroupsAsStr(match)}");
                    exports = SplitFilesRegex.Split(match.Groups["Files"].Value);
                    Log.Debug($" - SplitFilesRegex {string.Join("|", exports)}");
                    exports = SameDirFiles(file, exports);
                    Log.Debug($" - SameDirFiles {string.Join("|", exports ?? [])}");
                }

                static string[] SameDirFiles(string file, string[] files)
                {
                    var dir = Path.GetDirectoryName(file);
                    files = [.. files.Where(x => dir.EndsWith(Path.GetDirectoryName(x.TrimPrefix("res://"))))];
                    return files.Length is 0 ? null : files;
                }
            }
        }
    }
}
