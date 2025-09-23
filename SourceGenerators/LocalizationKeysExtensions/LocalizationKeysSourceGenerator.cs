using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GodotSharp.SourceGenerators.LocalizationKeysExtensions
{
  /// <summary>
    /// Roslyn incremental generator that reads a CSV localization file and
    /// emits strongly typed constants for all keys.  Keys with nested
    /// components separated by slashes ("/") will be represented as nested
    /// static classes.  Names are sanitized to valid C# identifiers using
    /// logic similar to the existing GodotSharp.SourceGenerators implementation
    /// (unsafe characters are replaced with underscores and names starting
    /// with invalid characters are prefixed with an underscore)【533002778006099†L25-L30】.
    /// </summary>
    [Generator]
    public sealed class LocalizationKeysSourceGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // B1: Collect all Class has attribute LocalizationKeys
            var classDeclarations = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (SyntaxNode node, CancellationToken _) =>
                        node is ClassDeclarationSyntax cds && cds.AttributeLists.Count > 0,
                    transform: static (GeneratorSyntaxContext ctx, CancellationToken _) =>
                    {
                        var classSyntax = (ClassDeclarationSyntax)ctx.Node;
                        foreach (var attrList in classSyntax.AttributeLists)
                        {
                            foreach (var attr in attrList.Attributes)
                            {
                                var name = attr.Name.ToString();
                                if (name.Contains("LocalizationKeys"))
                                    return classSyntax;
                            }
                        }
                        return null;
                    })
                .Where(static c => c != null);

            // B2: collect all .csv from AdditionalFiles (optinal)
            var csvFiles = context.AdditionalTextsProvider
                .Where(file => file.Path.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                .Collect();

            // B3: combine compilation + classes + csvFiles
            var combo = context.CompilationProvider.Combine(classDeclarations.Collect()).Combine(csvFiles);

            context.RegisterSourceOutput(combo, (spc, source) =>
            {
                var compilation = source.Left.Left;
                var classes = source.Left.Right;
                var csvList = source.Right;

                foreach (var classDecl in classes)
                {
                    var model = compilation.GetSemanticModel(classDecl.SyntaxTree);
                    var classSymbol = model.GetDeclaredSymbol(classDecl);
                    if (classSymbol is null) continue;

                    var attrData = classSymbol.GetAttributes().FirstOrDefault(a =>
                        a.AttributeClass?.Name == nameof(Godot.LocalizationKeysAttribute) ||
                        a.AttributeClass?.ToDisplayString() == "Godot.LocalizationKeysAttribute");

                    if (attrData is null) continue;

                    var filePath = attrData.ConstructorArguments[0].Value as string;
                    var dataType = attrData.ConstructorArguments.Length > 1 ? attrData.ConstructorArguments[1].Value as string : "StringName";
                    var classPath = attrData.ConstructorArguments.Length > 2 ? attrData.ConstructorArguments[2].Value as string : null;

                    var resolvedPath = ResolvePath(filePath, classPath);

                    IEnumerable<string> keys;
                    try
                    {
                        // Find in AdditionalFiles
                        var csv = csvList.FirstOrDefault(f =>
                            Path.GetFullPath(f.Path).Equals(Path.GetFullPath(resolvedPath), StringComparison.OrdinalIgnoreCase));

                        if (csv != null)
                        {
                            keys = ParseTranslationKeys(csv.Path);
                        }
                        else if (File.Exists(resolvedPath))
                        {
                            // fallback: read direct from system file
                            keys = ParseTranslationKeys(resolvedPath);
                        }
                        else
                        {
                            var descriptor = new DiagnosticDescriptor(
                                id: "LOC001",
                                title: "Localization file not found",
                                messageFormat: $"Localization file '{resolvedPath}' not found.",
                                category: "Localization",
                                defaultSeverity: DiagnosticSeverity.Error,
                                isEnabledByDefault: true);
                            spc.ReportDiagnostic(Diagnostic.Create(descriptor, classDecl.Identifier.GetLocation()));
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        var descriptor = new DiagnosticDescriptor(
                            id: "LOC002",
                            title: "Localization parse error",
                            messageFormat: $"Failed to parse localization file '{resolvedPath}': {ex.Message}",
                            category: "Localization",
                            defaultSeverity: DiagnosticSeverity.Error,
                            isEnabledByDefault: true);
                        spc.ReportDiagnostic(Diagnostic.Create(descriptor, classDecl.Identifier.GetLocation()));
                        continue;
                    }

                    var code = GenerateClassCode(classSymbol, keys, dataType);
                    spc.AddSource($"{classSymbol.Name}_LocalizationKeys.g.cs", code);
                }
            });
        }

        /// <summary>
        /// Reads the first column of a CSV/translation file.  Empty lines and
        /// lines starting with '#' are ignored.  If the first column is
        /// "id" or "key", it is treated as a header row and skipped.
        /// </summary>
        private static IEnumerable<string> ParseTranslationKeys(string filePath)
        {
            foreach (var line in File.ReadLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                var trimmed = line.Trim();
                if (trimmed.StartsWith("#"))
                    continue;
                var parts = trimmed.Split(',');
                if (parts.Length == 0)
                    continue;
                var key = parts[0].Trim();
                if (string.IsNullOrEmpty(key))
                    continue;
                if (key.Equals("id", StringComparison.OrdinalIgnoreCase) || key.Equals("key", StringComparison.OrdinalIgnoreCase))
                    continue;
                yield return key;
            }
        }

        /// <summary>
        /// Constructs a tree of localization keys.  Each node represents a
        /// path component separated by '/'.  Leaf nodes store the full key.
        /// </summary>
        private static KeyNode BuildKeyTree(IEnumerable<string> keys)
        {
            var root = new KeyNode();
            foreach (var key in keys)
            {
                // Normalize separators to '/'.  Godot supports '/' in CSV
                // keys, but we also handle '.' or '\\' for convenience.
                var normalized = key.Replace('\\', '/').Replace('.', '/');
                // Use array overload of Split to support older C#/frameworks
                var parts = normalized.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                var current = root;
                for (int i = 0; i < parts.Length; i++)
                {
                    var part = parts[i];
                    if (!current.Children.TryGetValue(part, out var child))
                    {
                        child = new KeyNode();
                        current.Children[part] = child;
                    }
                    current = child;
                    if (i == parts.Length - 1)
                    {
                        current.FullKey = key;
                    }
                }
            }
            return root;
        }

        /// <summary>
        /// Generates the source code for a partial class containing strongly
        /// typed localization keys.
        /// </summary>
        private static string GenerateClassCode(INamedTypeSymbol classSymbol, IEnumerable<string> keys, string dataType)
        {
            var ns = classSymbol.ContainingNamespace?.IsGlobalNamespace == true ? null : classSymbol.ContainingNamespace.ToDisplayString();
            var className = classSymbol.Name;
            var root = BuildKeyTree(keys);
            var builder = new StringBuilder();
            builder.AppendLine("// <auto‑generated> Localization Keys Generator</auto‑generated>");
            builder.AppendLine("using Godot;");
            builder.AppendLine();
            if (!string.IsNullOrEmpty(ns))
            {
                builder.AppendLine($"namespace {ns}");
                builder.AppendLine("{");
            }
            builder.AppendLine($"partial class {className}");
            builder.AppendLine("{");
            // Write all top level members.
            foreach (var kv in root.Children)
            {
                WriteNode(builder, kv.Key, kv.Value, dataType, 1);
            }
            builder.AppendLine("}");
            if (!string.IsNullOrEmpty(ns))
            {
                builder.AppendLine("}");
            }
            return builder.ToString();
        }

        /// <summary>
        /// Writes a node or leaf into the generated code.
        /// </summary>
        private static void WriteNode(StringBuilder builder, string partName, KeyNode node, string dataType, int indent)
        {
            var safeName = ToSafeName(partName);
            var indentStr = new string(' ', indent * 4);
            if (node.IsLeaf)
            {
                // Leaf nodes become fields.
                builder.AppendLine($"{indentStr}/// <summary>The strongly typed localization key for \"{node.FullKey}\".</summary>");
                if (string.Equals(dataType, "StringName", StringComparison.Ordinal))
                {
                    builder.AppendLine($"{indentStr}public static readonly StringName {safeName} = new(\"{node.FullKey}\");");
                }
                else
                {
                    builder.AppendLine($"{indentStr}public static readonly string {safeName} = \"{node.FullKey}\";");
                }
            }
            else
            {
                // Inner nodes become nested classes.
                builder.AppendLine($"{indentStr}public static class {safeName}");
                builder.AppendLine($"{indentStr}{{");
                foreach (var kv in node.Children)
                {
                    WriteNode(builder, kv.Key, kv.Value, dataType, indent + 1);
                }
                builder.AppendLine($"{indentStr}}}");
            }
        }

        /// <summary>
        /// Sanitizes an arbitrary string into a valid C# identifier by
        /// splitting words, removing unsafe characters and prefixing with an
        /// underscore when necessary.  This logic mirrors the implementation
        /// used by the official Godot source generators【533002778006099†L25-L30】.
        /// </summary>
        private static string ToSafeName(string source)
        {
            // Convert to title case using word boundaries on spaces, underscores,
            // hyphens and camel case.
            var title = ToTitleCase(source);
            // Remove spaces entirely.
            var noSpaces = title.Replace(" ", string.Empty);
            // Replace any non‑word characters with underscores.
            var sanitized = UnsafeCharsRegex().Replace(noSpaces, "_");
            // If the first character is not a letter or underscore, prefix an underscore.
            return UnsafeFirstCharRegex().IsMatch(sanitized) ? $"_{sanitized}" : sanitized;
        }

        // Precompiled regexes for sanitizing identifiers.
        private static System.Text.RegularExpressions.Regex UnsafeCharsRegex() => _unsafeChars ??= new System.Text.RegularExpressions.Regex("[^\\w]+", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.ExplicitCapture);
        private static System.Text.RegularExpressions.Regex UnsafeFirstCharRegex() => _unsafeFirst ??= new System.Text.RegularExpressions.Regex("^[^a-zA-Z_]+", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.ExplicitCapture);
        private static System.Text.RegularExpressions.Regex SplitRegex() => _split ??= new System.Text.RegularExpressions.Regex("[ _-]+|(?<=[a-z])(?=[A-Z])", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.ExplicitCapture);
        private static System.Text.RegularExpressions.Regex _unsafeChars;
        private static System.Text.RegularExpressions.Regex _unsafeFirst;
        private static System.Text.RegularExpressions.Regex _split;

        private static string ToTitleCase(string source)
        {
            var words = SplitRegex().Replace(source, " ").ToLowerInvariant();
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(words);
        }

        /// <summary>
        /// Represents a node in the localization key tree.
        /// </summary>
        private sealed class KeyNode
        {
            public readonly Dictionary<string, KeyNode> Children = new(StringComparer.Ordinal);
            public string FullKey { get; set; }
            public bool IsLeaf => Children.Count == 0;
        }

        /// <summary>
        /// Resolve a localization file path.  Supports absolute paths,
        /// relative paths (interpreted relative to the location of the
        /// annotated class), and Godot's "res://" paths.  When
        /// encountering a "res://" prefix, this method will attempt to
        /// locate the project root by searching upward for a
        /// "project.godot" file.  If found, the prefix is replaced with
        /// the project root.  Otherwise, the prefix is stripped and the
        /// remainder is treated as a relative path.
        /// </summary>
        private static string ResolvePath(string filePath, string classPath)
        {
            if (string.IsNullOrEmpty(filePath))
                return filePath;
            var trimmed = filePath.Replace("\\", "/");
            // Handle Godot's res:// prefix.
            const string resPrefix = "res://";
            if (trimmed.StartsWith(resPrefix, StringComparison.OrdinalIgnoreCase))
            {
                var relative = trimmed.Substring(resPrefix.Length).TrimStart('/');
                // Attempt to find the project root (directory containing project.godot).
                var projectDir = classPath != null ? FindGodotProjectRoot(Path.GetDirectoryName(classPath)) : null;
                if (projectDir != null)
                {
                    return Path.Combine(projectDir, relative.Replace('/', Path.DirectorySeparatorChar));
                }
                // Fallback: treat path after res:// as relative to class directory.
                if (classPath != null)
                {
                    var dir = Path.GetDirectoryName(classPath);
                    return Path.Combine(dir, relative.Replace('/', Path.DirectorySeparatorChar));
                }
                return relative;
            }
            // If the path is already rooted, return as is.
            if (Path.IsPathRooted(filePath))
            {
                return filePath;
            }
            // Otherwise, combine with class directory if available.
            if (classPath != null)
            {
                var dir = Path.GetDirectoryName(classPath);
                return Path.Combine(dir, filePath);
            }
            return filePath;
        }

        /// <summary>
        /// Ascend from a starting directory to locate the Godot project root.
        /// The root is identified by the presence of a "project.godot" file.
        /// Returns null if no project root is found.
        /// </summary>
        private static string FindGodotProjectRoot(string startDir)
        {
            var dir = startDir;
            while (!string.IsNullOrEmpty(dir))
            {
                try
                {
                    var candidate = Path.Combine(dir, "project.godot");
                    if (File.Exists(candidate))
                        return dir;
                    var parent = Directory.GetParent(dir);
                    if (parent == null)
                        break;
                    dir = parent.FullName;
                }
                catch
                {
                    break;
                }
            }
            return null;
        }
    }
}