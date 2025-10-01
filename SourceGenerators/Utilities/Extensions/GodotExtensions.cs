﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace GodotSharp.SourceGenerators;

internal static class GD
{
    private const string GodotProjectFile = "project.godot";

    private static string _resRoot = null;
    private static string GetProjectRoot(string path)
    {
        return _resRoot is null || !path.StartsWith(_resRoot)
            ? _resRoot = GetProjectRoot(path) : _resRoot;

        static string GetProjectRoot(string path)
        {
            var dir = Path.GetDirectoryName(path);

            while (dir is not null)
            {
                if (File.Exists($"{dir}/{GodotProjectFile}"))
                    return dir;

                dir = Path.GetDirectoryName(dir);
            }

            throw new Exception($"Could not find {GodotProjectFile} in path {Path.GetDirectoryName(path)}");
        }
    }

    public static string GetProjectFile(string path, string projectDir = null)
        => Path.Combine(projectDir ?? GetProjectRoot(path), GodotProjectFile);

    public static string GetResourcePath(string path, string projectDir = null)
        => $"res://{path[(projectDir ?? GetProjectRoot(path)).Length..].Replace("\\", "/").TrimStart('/')}";

    public static string ROOT(SyntaxNode node, AnalyzerConfigOptions options = null)
        => options?.TryGetGodotProjectDir() ?? GetProjectRoot(node.SyntaxTree.FilePath);

    public static string FILE(string resPath, SyntaxNode node, AnalyzerConfigOptions options = null, string ext = "tres")
    {
        var csFile = node.SyntaxTree.FilePath;
        var gdRoot = options?.TryGetGodotProjectDir() ?? GetProjectRoot(csFile);

        if (!Path.HasExtension(resPath))
            resPath = Path.ChangeExtension(resPath, ext);

        var absPath = Path.Combine(gdRoot, resPath.Replace("res://", ""));
        if (File.Exists(absPath)) return absPath;

        var relPath = Path.Combine(Path.GetDirectoryName(csFile), resPath);
        if (File.Exists(relPath)) return relPath;

        //
        throw new Exception($"Could not find {resPath}\n - {absPath}\n - {relPath}");
    }

    public static string TSCN(SyntaxNode node, AnalyzerConfigOptions options = null) => Res("tscn", node, options);
    public static string TRES(SyntaxNode node, AnalyzerConfigOptions options = null) => Res("tres", node, options);
    private static string Res(string ext, SyntaxNode node, AnalyzerConfigOptions options = null)
    {
        var csPath = node.SyntaxTree.FilePath;

        var csClass = node.FirstAncestorOrSelf<ClassDeclarationSyntax>().Identifier.ValueText;
        if (Path.GetFileNameWithoutExtension(csPath) != csClass) return null;

        var resPath = Path.ChangeExtension(csPath, ext);
        if (!File.Exists(resPath)) return null;

        resPath = GetResourcePath(resPath, options?.TryGetGodotProjectDir());
        return resPath;
    }

    public static string Get(this string path, params string[] ext)
    {
        foreach (var _ext in ext)
        {
            var source = Path.ChangeExtension(path, _ext);
            if (File.Exists(source)) return source;
        }

        throw new Exception($"Could not find [{string.Join(", ", ext)}] for {path}");
    }

    public static (string SystemPath, DiagnosticDetail Error) GetRealPath(string resPath, SyntaxNode node, AnalyzerConfigOptions options = null, string ext = null)
    {
        try { return (FILE(resPath, node, options, ext), null); }
        catch (Exception e) { return (null, Diagnostics.FileNotFound(resPath, e.Message)); }
    }
}
