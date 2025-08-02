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
    /// A Roslyn source generator that produces strongly typed localization keys from a translation file.
    /// </summary>
    [Generator]
    public sealed class LocalizationKeysSourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            // Nothing to initialize.
        }

        public void Execute(GeneratorExecutionContext context)
        {
            // Find all classes with the LocalizationKeysAttribute.
            foreach (var syntaxTree in context.Compilation.SyntaxTrees)
            {
                var semanticModel = context.Compilation.GetSemanticModel(syntaxTree);
                var root = syntaxTree.GetRoot(context.CancellationToken);
                var classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

                foreach (var classDecl in classDeclarations)
                {
                    var classSymbol = semanticModel.GetDeclaredSymbol(classDecl, context.CancellationToken);
                    if (classSymbol == null)
                        continue;

                    foreach (var attr in classSymbol.GetAttributes())
                    {
                        // We match on the full name because the attribute may not be in the default namespace.
                        var attrName = attr.AttributeClass?.ToDisplayString();
                        if (attrName == "Godot.LocalizationKeysAttribute")
                        {
                            ProcessClass(context, classDecl, classSymbol, attr);
                            break;
                        }
                    }
                }
            }
        }

        private void ProcessClass(GeneratorExecutionContext context, ClassDeclarationSyntax classDecl, INamedTypeSymbol classSymbol, AttributeData attr)
        {
            // Extract attribute arguments
            var filePathArg = attr.ConstructorArguments.Length > 0 ? attr.ConstructorArguments[0].Value as string : null;
            var dataTypeArg = attr.ConstructorArguments.Length > 1 ? attr.ConstructorArguments[1].Value as string : null;
            var classPathArg = attr.ConstructorArguments.Length > 2 ? attr.ConstructorArguments[2].Value as string : null;

            if (string.IsNullOrEmpty(filePathArg))
            {
                return;
            }

            var resolvedPath = ResolvePath(filePathArg, classPathArg);
            if (resolvedPath == null || !File.Exists(resolvedPath))
            {
                // Report diagnostic if file not found
                var descriptor = new DiagnosticDescriptor(
                    id: "LOC001",
                    title: "Localization file not found",
                    messageFormat: $"Localization file '{filePathArg}' not found (resolved path: '{resolvedPath}').",
                    category: "Localization",
                    DiagnosticSeverity.Error,
                    isEnabledByDefault: true);
                context.ReportDiagnostic(Diagnostic.Create(descriptor, classDecl.GetLocation()));
                return;
            }

            // Read and parse keys
            var keys = ParseKeys(resolvedPath);
            if (keys.Count == 0)
            {
                return;
            }

            var rootNode = BuildTree(keys);
            var dataType = string.IsNullOrEmpty(dataTypeArg) ? "StringName" : dataTypeArg;
            var generatedSource = GenerateClassSource(classSymbol, rootNode, dataType);

            // Add generated source
            var hintName = $"{classSymbol.Name}_LocalizationKeys.g.cs";
            context.AddSource(hintName, generatedSource);
        }

        /// <summary>
        /// Resolve file path relative to the class file location. Supports res:// paths.
        /// </summary>
        private static string ResolvePath(string filePath, string classPath)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;

            // If absolute, return as is
            if (Path.IsPathRooted(filePath) && !filePath.StartsWith("res://", StringComparison.OrdinalIgnoreCase))
            {
                return Path.GetFullPath(filePath);
            }

            // Determine base directory from class path
            var baseDir = !string.IsNullOrEmpty(classPath) ? Path.GetDirectoryName(classPath) : null;
            if (string.IsNullOrEmpty(baseDir))
            {
                baseDir = Directory.GetCurrentDirectory();
            }

            // Handle res:// prefix
            if (filePath.StartsWith("res://", StringComparison.OrdinalIgnoreCase))
            {
                var relative = filePath.Substring("res://".Length);
                var projectRoot = FindGodotProjectRoot(baseDir);
                if (projectRoot != null)
                {
                    var candidate = Path.Combine(projectRoot, relative.Replace('/', Path.DirectorySeparatorChar));
                    return candidate;
                }
                // Fall back to treating as relative to class path
                return Path.Combine(baseDir, relative.Replace('/', Path.DirectorySeparatorChar));
            }

            // Otherwise treat as relative
            return Path.GetFullPath(Path.Combine(baseDir, filePath.Replace('/', Path.DirectorySeparatorChar)));
        }

        /// <summary>
        /// Walks up directories to locate a file named project.godot.
        /// </summary>
        private static string FindGodotProjectRoot(string startDir)
        {
            var dir = startDir;
            while (!string.IsNullOrEmpty(dir))
            {
                var projectFile = Path.Combine(dir, "project.godot");
                if (File.Exists(projectFile))
                {
                    return dir;
                }
                dir = Directory.GetParent(dir)?.FullName;
            }
            return null;
        }

        /// <summary>
        /// Parse keys from a CSV-like translation file. Assumes first column contains the key.
        /// </summary>
        private static List<string> ParseKeys(string filePath)
        {
            var list = new List<string>();
            foreach (var rawLine in File.ReadLines(filePath))
            {
                var line = rawLine.Trim();
                if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                    continue;
                // Split by comma; naive splitting (no quoted values). We only need first column.
                var parts = line.Split(new[] { ',' }, StringSplitOptions.None);
                if (parts.Length == 0)
                    continue;
                var key = parts[0].Trim();
                if (string.IsNullOrEmpty(key))
                    continue;
                // Skip header row if first cell is id or key
                if (string.Equals(key, "id", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(key, "key", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                list.Add(key);
            }
            return list;
        }

        /// <summary>
        /// Represents a node in the key hierarchy tree.
        /// </summary>
        private sealed class KeyNode
        {
            public string Name;
            public string FullKey;
            public Dictionary<string, KeyNode> Children = new Dictionary<string, KeyNode>();
        }

        private static KeyNode BuildTree(List<string> keys)
        {
            var root = new KeyNode { Name = string.Empty, FullKey = string.Empty };
            foreach (var key in keys)
            {
                var normalized = key.Replace("\\", "/").Replace(".", "/");
                var parts = normalized.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                var current = root;
                for (int i = 0; i < parts.Length; i++)
                {
                    var part = parts[i];
                    if (!current.Children.TryGetValue(part, out var child))
                    {
                        child = new KeyNode { Name = part, FullKey = string.Join("/", parts.Take(i + 1)) };
                        current.Children.Add(part, child);
                    }
                    current = child;
                }
            }
            return root;
        }

        /// <summary>
        /// Generates the C# source for the class containing the localization keys.
        /// </summary>
        private static string GenerateClassSource(INamedTypeSymbol classSymbol, KeyNode rootNode, string dataType)
        {
            var sb = new StringBuilder();
            // Add namespace if exists
            if (!string.IsNullOrEmpty(classSymbol.ContainingNamespace?.ToDisplayString()))
            {
                sb.AppendLine($"namespace {classSymbol.ContainingNamespace.ToDisplayString()}");
                sb.AppendLine("{");
            }
            sb.AppendLine($"partial class {classSymbol.Name}");
            sb.AppendLine("{");
            foreach (var child in rootNode.Children.Values)
            {
                GenerateNode(sb, child, 1, dataType);
            }
            sb.AppendLine("}");
            if (!string.IsNullOrEmpty(classSymbol.ContainingNamespace?.ToDisplayString()))
            {
                sb.AppendLine("}");
            }
            return sb.ToString();
        }

        private static void GenerateNode(StringBuilder sb, KeyNode node, int indent, string dataType)
        {
            var indentStr = new string(' ', indent * 4);
            var safeName = ToSafeName(node.Name);
            if (node.Children.Count == 0)
            {
                // Leaf node - generate field
                if (dataType == "string")
                {
                    sb.AppendLine($"{indentStr}public static readonly string {safeName} = \"{node.FullKey}\";");
                }
                else
                {
                    // Default to StringName
                    sb.AppendLine($"{indentStr}public static readonly Godot.StringName {safeName} = new Godot.StringName(\"{node.FullKey}\");");
                }
            }
            else
            {
                // Generate nested static class and then fields inside
                sb.AppendLine($"{indentStr}public static class {safeName}");
                sb.AppendLine($"{indentStr}{{");
                foreach (var child in node.Children.Values)
                {
                    GenerateNode(sb, child, indent + 1, dataType);
                }
                sb.AppendLine($"{indentStr}}}");
            }
        }

        /// <summary>
        /// Converts a string into a safe C# identifier.  Non-alphanumeric characters are replaced with underscores.
        /// If the first character is not a letter or underscore, an underscore is prefixed.  Words are capitalized (PascalCase).
        /// </summary>
        private static string ToSafeName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return "";
            // Replace invalid chars with space then title case
            var chars = name.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                var c = chars[i];
                if (!char.IsLetterOrDigit(c))
                {
                    chars[i] = ' ';
                }
            }
            var text = new string(chars);
            // PascalCase
            var words = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var pascal = new StringBuilder();
            foreach (var w in words)
            {
                if (w.Length > 0)
                {
                    pascal.Append(char.ToUpperInvariant(w[0]));
                    if (w.Length > 1)
                        pascal.Append(w.Substring(1));
                }
            }
            var result = pascal.ToString();
            if (string.IsNullOrEmpty(result))
                result = "_";
            // If first character is not valid identifier start, prefix underscore
            if (!(result[0] == '_' || char.IsLetter(result[0])))
            {
                result = "_" + result;
            }
            return result;
        }
    }
}